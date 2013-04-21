using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using ProtoBuilder.Controllers;
using ProtoBuilder.Model;

namespace ProtoBuilder.Windows {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow {
        public ProtocolController ProtocolController { get; set; }
        private Protocol CurrentProtocol { get; set; }
        private Packet CurrentPacket { get; set; }
        private Segment CurrentSegment { get; set; }
        private DispatcherTimer SegmentListRefreshTimer { get; set; }
        public ListBox DragSource { get; protected set; }

        public MainWindow() {
            InitializeComponent();
            ProtocolController = new ProtocolController();
            //GenTestData();

            Loaded += OnLoaded;
            
            CbProtocols.SelectionChanged += CbProtocolsOnSelectionChanged;
            LbPackets.SelectionChanged += LbPacketsOnSelectionChanged;
            LbSegments.SelectionChanged += LbSegmentsOnSelectionChanged;
            LbPackets.MouseDoubleClick += LbPacketsOnMouseDoubleClick;
            LbPackets.KeyUp += LbPacketsOnKeyUp;

            //  Говно-код, но иначе не получается получить доступ
            //  к ComboBox внутри ContentControl.ContentTemplate
            //  для инициализации SelectedItem
            CbDataType.ItemsSource = Segment.GetDataTypes();
            TxtSegmentName.TextChanged += TxtSegmentNameOnTextChanged;
            TxtSegmentName.LostFocus += TxtSegmentNameOnLostFocus;
            TxtSegmentDesc.TextChanged += TxtSegmentDescOnTextChanged;
            TxtSegmentDesc.LostFocus += TxtSegmentDescOnLostFocus;
            CbDataType.SelectionChanged += CbDataTypeOnSelectionChanged;
            CbDataType.DropDownClosed += CbDataTypeOnDropDownClosed;
            TxtSegmentSize.TextChanged += TxtSizeOnTextChanged;
            TxtSegmentSize.LostFocus += TxtSegmentSizeOnLostFocus;
            CbPacketType.SelectionChanged += CbPacketTypeOnSelectionChanged;
            CbDynamicSize.DropDownClosed += CbDynamicSizeOnDropDownClosed;
            InitTimer();
            //  Конец говно-кода
        }

        private void CbPacketTypeOnSelectionChanged(object sender, SelectionChangedEventArgs e) {
            CurrentPacket.PacketType = (PacketType) CbPacketType.SelectedItem;
            var index = LbPackets.SelectedIndex;
            var segIndex = LbSegments.SelectedIndex;
            LbPackets.ItemsSource = null;
            LbPackets.ItemsSource = CurrentProtocol.Packets;
            LbPackets.SelectedIndex = index;
            LbSegments.SelectedIndex = segIndex;
        }

        private void LbPacketsOnKeyUp(object sender, KeyEventArgs e) {
            if (LbPackets.SelectedIndex > -1 && e.Key == Key.F2)
                EditPacket();
        }

        private void LbPacketsOnMouseDoubleClick(object sender, MouseButtonEventArgs e) {
            if (LbPackets.SelectedIndex > -1)
                EditPacket();
        }

        private void EditPacket() {
            var win = new PacketEditWindow(CurrentPacket);
            var showDialog = win.ShowDialog();
            if (showDialog == null || !((bool) showDialog)) return;
            var index = LbPackets.SelectedIndex;
            LbPackets.ItemsSource = null;
            LbPackets.ItemsSource = CurrentProtocol.Packets;
            LbPackets.SelectedIndex = index;
        }

        private void CbDataTypeOnDropDownClosed(object sender, EventArgs eventArgs) {
            RefreshSegmentList();
        }

        private void TxtSegmentSizeOnLostFocus(object sender, RoutedEventArgs routedEventArgs) {
            RefreshSegmentList();
        }

        private void TxtSegmentDescOnLostFocus(object sender, RoutedEventArgs routedEventArgs) {
            RefreshSegmentList();
        }

        private void TxtSegmentNameOnLostFocus(object sender, RoutedEventArgs routedEventArgs) {
            RefreshSegmentList();
        }

        private void InitTimer() {
            SegmentListRefreshTimer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 0, 100) };
            SegmentListRefreshTimer.Tick += SegmentListRefreshTimerTick;
            SegmentListRefreshTimer.Stop();
        }

        private void SegmentListRefreshTimerTick(object sender, EventArgs e) {
            SegmentListRefreshTimer.Stop();
            var index = LbSegments.SelectedIndex;
            LbSegments.ItemsSource = null;
            LbSegments.ItemsSource = CurrentPacket.Segments;
            LbSegments.SelectedIndex = index;
        }

        private void CbDataTypeOnSelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (CurrentSegment == null) return;
            if (CbDataType.SelectedIndex == -1) return;
            CurrentSegment.Type = (DataTypeView) CbDataType.SelectedItem;
            var fixedSize = DataTypeView.SizeOfType(CurrentSegment.Type.Type);
            if (fixedSize > 0) {
                CurrentSegment.Size = fixedSize;
                TxtSegmentSize.Text = CurrentSegment.Size.ToString(CultureInfo.InvariantCulture);
                TxtSegmentSize.IsEnabled = false;
            }
            else {
                TxtSegmentSize.IsEnabled = true;
            }
        }

        private void TxtSizeOnTextChanged(object sender, TextChangedEventArgs e) {
            if (LbSegments.SelectedItems.Count == 0) return;

            int intVal;
            int.TryParse(TxtSegmentSize.Text, out intVal);
            CurrentSegment.Size = (uint) intVal;
        }

        private void TxtSegmentDescOnTextChanged(object sender, TextChangedEventArgs e) {
            if (LbSegments.SelectedItems.Count == 0) return;

            CurrentSegment.Description = TxtSegmentDesc.Text;
        }

        private void TxtSegmentNameOnTextChanged(object sender, TextChangedEventArgs e) {
            if (LbSegments.SelectedItems.Count == 0) return;

            CurrentSegment.Name = TxtSegmentName.Text;
        }

        private void RefreshSegmentList() {
            if (CurrentPacket == null) return;
            var index = LbSegments.SelectedIndex;
            LbSegments.ItemsSource = null;
            LbSegments.ItemsSource = CurrentPacket.Segments.OrderBy(t=>t.OrderId);
            LbSegments.SelectedIndex = index;
        }

        private void LbSegmentsOnSelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (e.AddedItems.Count > 0) {
                CbDynamicSize.ItemsSource = null;
                CbDynamicSize.ItemsSource = CurrentPacket.DynamicSizeSegments;
                CurrentSegment = (Segment) e.AddedItems[0];
                TxtSegmentName.Text = CurrentSegment.Name;
                TxtSegmentDesc.Text = CurrentSegment.Description;
                CbDynamicSize.Visibility = CurrentSegment.IsDynamicSize;
                BtnClearDynamicSize.Visibility = CurrentSegment.IsDynamicSize;
                var i = -1;
                foreach (DataTypeView item in CbDataType.ItemsSource) {
                    i++;
                    if (item.Type != CurrentSegment.Type.Type) continue;
                    CbDataType.SelectedIndex = i;
                    break;
                }
                i = -1;
                foreach (Segment item in CbDynamicSize.ItemsSource) {
                    i++;
                    if (item.Id != CurrentSegment.DynamicSizeSegment) continue;
                    CbDynamicSize.SelectedIndex = i;
                    break;
                }
                TxtSegmentSize.Text = CurrentSegment.Size.ToString(CultureInfo.InvariantCulture);
                TxtSegmentSize.IsEnabled = CurrentSegment.DynamicSizeSegment == Guid.Empty;
                BtnRemoveSegment.IsEnabled = true;
                BtnSegmentPrevPosition.IsEnabled = true;
                BtnSegmentNextPosition.IsEnabled = true;
            } else {
                TxtSegmentName.Text = "";
                TxtSegmentDesc.Text = "";
                CbDataType.SelectedIndex = -1;
                TxtSegmentSize.Text = "";
                BtnRemoveSegment.IsEnabled = false;
                BtnSegmentPrevPosition.IsEnabled = false;
                BtnSegmentNextPosition.IsEnabled = false;
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e) {
            CbProtocols.ItemsSource = ProtocolController.Protocols;
        }

        private void CbProtocolsOnSelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (e.AddedItems.Count == 1) {
                CurrentProtocol = (Protocol) e.AddedItems[0];
                LbPackets.ItemsSource = null;
                LbPackets.ItemsSource = CurrentProtocol.Packets.OrderBy(t=>t.Name);
                LbSegments.ItemsSource = null;
                BtnAddPacket.IsEnabled = true;
                BtnEditProtocol.IsEnabled = true;
                BtnRemoveProtocol.IsEnabled = true;
            }
            else {
                LbPackets.ItemsSource = null;
                BtnAddPacket.IsEnabled = false;
                BtnEditProtocol.IsEnabled = false;
                BtnRemoveProtocol.IsEnabled = false;
            }
        }

        private void LbPacketsOnSelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (e.AddedItems.Count > 0) {
                CurrentPacket = (Packet) e.AddedItems[0];
                RecalcPacketSize();
                CcPacket.Content = CurrentPacket;
                CbPacketType.ItemsSource = Packet.GetPacketTypes();
                var i = -1;
                foreach (var item in Packet.GetPacketTypes()) {
                    i++;
                    if (item != CurrentPacket.PacketType) continue;
                    CbPacketType.SelectedIndex = i;
                    break;
                }
                LbSegments.ItemsSource = null;
                LbSegments.ItemsSource = CurrentPacket.Segments.OrderBy(t=>t.OrderId);
                BtnAddSegment.IsEnabled = true;
                BtnRemovePacket.IsEnabled = true;
            }
            else {
                LbSegments.ItemsSource = null;
                BtnAddSegment.IsEnabled = false;
                BtnRemovePacket.IsEnabled = false;
            }
        }

        private void RecalcPacketSize() {
            if (CurrentPacket == null) return;
            TxtPacketSize.Text = CurrentPacket.Size.ToString(CultureInfo.InvariantCulture);
        }

        private void GenTestData() {
            var protocols = new List<Protocol>();
            var packets = new List<Packet>();
            for (var i = 0; i < 3; i++) {
                var segments = new List<Segment> {
                                                     new Segment {
                                                                     OrderId = 0,
                                                                     Name = string.Format("Сегмент byte"),
                                                                     Type = new DataTypeView { Type = DataType.Byte },
                                                                     Size = 4
                                                                 },
                                                     new Segment {
                                                                     OrderId = 0,
                                                                     Name = string.Format("Сегмент Int16"),
                                                                     Type = new DataTypeView { Type = DataType.Int16 },
                                                                     Size = 4
                                                                 },
                                                     new Segment {
                                                                     OrderId = 0,
                                                                     Name = string.Format("Сегмент Int32"),
                                                                     Type = new DataTypeView { Type = DataType.Int32 },
                                                                     Size = 4
                                                                 },
                                                     new Segment {
                                                                     OrderId = 0,
                                                                     Name = string.Format("Сегмент Int64"),
                                                                     Type = new DataTypeView { Type = DataType.Int64 },
                                                                     Size = 4
                                                                 },
                                                     new Segment {
                                                                     OrderId = 0,
                                                                     Name = string.Format("Сегмент UInt16"),
                                                                     Type = new DataTypeView { Type = DataType.UInt16 },
                                                                     Size = 4
                                                                 },
                                                     new Segment {
                                                                     OrderId = 0,
                                                                     Name = string.Format("Сегмент UInt32"),
                                                                     Type = new DataTypeView { Type = DataType.UInt32 },
                                                                     Size = 4
                                                                 },
                                                     new Segment {
                                                                     OrderId = 0,
                                                                     Name = string.Format("Сегмент UInt64"),
                                                                     Type = new DataTypeView { Type = DataType.UInt64 },
                                                                     Size = 4
                                                                 },
                                                     new Segment {
                                                                     OrderId = 0,
                                                                     Name = string.Format("Сегмент Double"),
                                                                     Type = new DataTypeView { Type = DataType.Double },
                                                                     Size = 4
                                                                 },
                                                     new Segment {
                                                                     OrderId = 0,
                                                                     Name = string.Format("Сегмент DateTime"),
                                                                     Type = new DataTypeView { Type = DataType.DateTime },
                                                                     Size = 4
                                                                 },
                                                     new Segment {
                                                                     OrderId = 0,
                                                                     Name = string.Format("Сегмент String"),
                                                                     Type = new DataTypeView { Type = DataType.String },
                                                                     Size = 4
                                                                 },
                                                     new Segment {
                                                                     OrderId = 0,
                                                                     Name = string.Format("Сегмент BytesArray"),
                                                                     Type = new DataTypeView { Type = DataType.Bytes },
                                                                     Size = 42
                                                                 },
                                                 };
                packets.Add(new Packet {
                    Name = string.Format("Packet {0}", i + 1),
                    Segments = segments
                });
            }
            protocols.Add(new Protocol {
                Name = "Test protocol 1",
                Packets = packets
            });
            protocols.Add(new Protocol {
                Name = "Test protocol 2",
                Packets = packets
            });
            ProtocolController.Protocols = protocols;
        }

        private void BtnAddSegment_OnClick(object sender, RoutedEventArgs e) {
            ProtocolController.AddSegment(CurrentPacket);
            CurrentPacket = ProtocolController.Protocols.First(t => t == CurrentProtocol).Packets.First(p => p == CurrentPacket);
            LbSegments.ItemsSource = null;
            LbSegments.ItemsSource = CurrentPacket.Segments;
            LbSegments.SelectedIndex = CurrentPacket.Segments.Count - 1;
            RecalcPacketSize();
        }

        private void BtnRemoveSegment_OnClick(object sender, RoutedEventArgs e) {
            foreach (Segment item in LbSegments.SelectedItems) {
                CurrentPacket.Segments.Remove(item);
            }
            LbSegments.ItemsSource = null;
            LbSegments.ItemsSource = CurrentPacket.Segments;
            RecalcPacketSize();
        }

        private void BtnAddPacket_OnClick(object sender, RoutedEventArgs e) {
            ProtocolController.AddPacket(CurrentProtocol);
            LbPackets.ItemsSource = null;
            LbPackets.ItemsSource = CurrentProtocol.Packets;
            if (CurrentProtocol.Packets != null)
                LbPackets.SelectedIndex = CurrentProtocol.Packets.Count - 1;
        }

        private void BtnRemovePacket_OnClick(object sender, RoutedEventArgs e) {
            foreach (Packet item in LbPackets.SelectedItems) {
                CurrentProtocol.Packets.Remove(item);
            }
            LbPackets.ItemsSource = null;
            LbPackets.ItemsSource = CurrentProtocol.Packets;
        }

        private void BtnRecalcPacketSize_OnClick(object sender, RoutedEventArgs e) {
            RecalcPacketSize();
        }

        private void TxtSegmentSize_OnTextChanged(object sender, TextChangedEventArgs e) {
            RecalcPacketSize();
        }

        private void BtnSegmentPrevPosition_OnClick(object sender, RoutedEventArgs e) {
            var index = CurrentPacket.MoveSegment2PrevPosition(CurrentSegment);
            LbSegments.ItemsSource = null;
            LbSegments.ItemsSource = CurrentPacket.Segments.OrderBy(t => t.OrderId);
            LbSegments.SelectedIndex = index;
        }

        private void BtnSegmentNextPosition_OnClick(object sender, RoutedEventArgs e) {
            var index = CurrentPacket.MoveSegment2NextPosition(CurrentSegment);
            LbSegments.ItemsSource = null;
            LbSegments.ItemsSource = CurrentPacket.Segments.OrderBy(t => t.OrderId);
            LbSegments.SelectedIndex = index;
        }

        private void BtnCreateProtocol_OnClick(object sender, RoutedEventArgs e) {
            var protocolsCount = ProtocolController.Protocols.Count;
            ProtocolController.CreateProtocol();
            if (ProtocolController.Protocols.Count <= protocolsCount) return;
            CbProtocols.ItemsSource = null;
            CbProtocols.ItemsSource = ProtocolController.Protocols;
            CbProtocols.SelectedIndex = CbProtocols.Items.Count - 1;
        }

        private void BtnEditProtocol_OnClick(object sender, RoutedEventArgs e) {
            ProtocolController.ShowEditProtocolWindow(CurrentProtocol);
        }

        private void BtnSaveProtocols_OnClick(object sender, RoutedEventArgs e) {
            ProtocolController.SaveProtocols(ProtocolController);
            BtnSaveProtocols.ToolTip = string.Format(@"Save {0}", ProtocolController.FileName);
        }

        private void BtnSaveAsProtocols_OnClick(object sender, RoutedEventArgs e) {
            ProtocolController.FileName = "";
            ProtocolController.SaveProtocols(ProtocolController);
            BtnSaveProtocols.ToolTip = string.Format(@"Save {0}", ProtocolController.FileName);
        }

        private void BtnLoadProtocols_OnClick(object sender, RoutedEventArgs e) {
            var protocolController = ProtocolController.LoadProtocols();
            if (protocolController == null) return;
            ProtocolController = protocolController;
            CbProtocols.ItemsSource = ProtocolController.Protocols;
            if (CbProtocols.Items.Count > 0)
                CbProtocols.SelectedIndex = 0;
            BtnSaveProtocols.ToolTip = string.Format(@"Save {0}", ProtocolController.FileName);
        }

        private void BtnRemoveProtocol_OnClick(object sender, RoutedEventArgs e) {
            if (CurrentProtocol == null) return;
            if (MessageBox.Show(string.Format(@"Удалить протокол {0}?", CurrentProtocol.Name), "Удаление протокола", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No) return;
            ProtocolController.Protocols.Remove(CurrentProtocol);
            CbProtocols.ItemsSource = null;
            CbProtocols.ItemsSource = ProtocolController.Protocols;
            if (CbProtocols.Items.Count > 0)
                CbProtocols.SelectedIndex = 0;
        }

        private void BtnAboutUs_OnClick(object sender, RoutedEventArgs e) {
            var dlg = new AboutUsWindow();
            dlg.ShowDialog();
        }

        private void CbDynamicSizeOnDropDownClosed(object sender, EventArgs eventArgs) {
            if (CbDynamicSize.SelectedIndex == -1) return;
            CurrentSegment.DynamicSizeSegment = ((Segment) CbDynamicSize.SelectedItem).Id;
            CurrentSegment.Size = 0;
            TxtSegmentSize.IsEnabled = false;
            RefreshSegmentList();
        }

        private void BtnClearDynamicSize_OnClick(object sender, RoutedEventArgs e) {
            CbDynamicSize.SelectedIndex = -1;
            CurrentSegment.DynamicSizeSegment = Guid.Empty;
            TxtSegmentSize.IsEnabled = true;
            RefreshSegmentList();
        }
    }
}
