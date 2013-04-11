using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ProtoBuilder.Controllers;
using ProtoBuilder.Model;

namespace ProtoBuilder.Windows {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow {
        public ProtocolController ProtocolController { get; set; }

        public MainWindow() {
            InitializeComponent();
            ProtocolController = new ProtocolController();
            GenTestData();

            Loaded += OnLoaded;

            CbProtocols.SelectionChanged += CbProtocolsOnSelectionChanged;
            LbPackets.SelectionChanged += LbPacketsOnSelectionChanged;
        }

        private void OnLoaded(object sender, RoutedEventArgs e) {
            CbProtocols.ItemsSource = ProtocolController.Protocols;
        }

        private void CbProtocolsOnSelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (e.AddedItems.Count == 1) {
                var protocol = (Protocol) e.AddedItems[0];
                LbPackets.ItemsSource = null;
                LbPackets.ItemsSource = protocol.Packets;
                LbSegments.ItemsSource = null;
            }
            else {
                LbPackets.ItemsSource = null;
            }
        }

        private void LbPacketsOnSelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (e.AddedItems.Count == 1) {
                var packet = (Packet) e.AddedItems[0];
                LbSegments.ItemsSource = null;
                LbSegments.ItemsSource = packet.Segments;
            }
            else {
                LbSegments.ItemsSource = null;
            }
        }

        private void GenTestData() {
            var protocols = new List<Protocol>();
            var packets = new List<Packet>();
            for (var i = 0; i < 3; i++) {
                var segments = new List<Segment> {
                                                     new Segment {
                                                                     OrderId = 0,
                                                                     Name = string.Format("Сегмент byte"),
                                                                     Type = DataType.Byte,
                                                                     Size = 4
                                                                 },
                                                     new Segment {
                                                                     OrderId = 0,
                                                                     Name = string.Format("Сегмент Int16"),
                                                                     Type = DataType.Int16,
                                                                     Size = 4
                                                                 },
                                                     new Segment {
                                                                     OrderId = 0,
                                                                     Name = string.Format("Сегмент Int32"),
                                                                     Type = DataType.Int32,
                                                                     Size = 4
                                                                 },
                                                     new Segment {
                                                                     OrderId = 0,
                                                                     Name = string.Format("Сегмент Int64"),
                                                                     Type = DataType.Int64,
                                                                     Size = 4
                                                                 },
                                                     new Segment {
                                                                     OrderId = 0,
                                                                     Name = string.Format("Сегмент UInt16"),
                                                                     Type = DataType.UInt16,
                                                                     Size = 4
                                                                 },
                                                     new Segment {
                                                                     OrderId = 0,
                                                                     Name = string.Format("Сегмент UInt32"),
                                                                     Type = DataType.UInt32,
                                                                     Size = 4
                                                                 },
                                                     new Segment {
                                                                     OrderId = 0,
                                                                     Name = string.Format("Сегмент UInt64"),
                                                                     Type = DataType.UInt64,
                                                                     Size = 4
                                                                 },
                                                     new Segment {
                                                                     OrderId = 0,
                                                                     Name = string.Format("Сегмент Double"),
                                                                     Type = DataType.Double,
                                                                     Size = 4
                                                                 },
                                                     new Segment {
                                                                     OrderId = 0,
                                                                     Name = string.Format("Сегмент DateTime"),
                                                                     Type = DataType.DateTime,
                                                                     Size = 4
                                                                 },
                                                     new Segment {
                                                                     OrderId = 0,
                                                                     Name = string.Format("Сегмент String"),
                                                                     Type = DataType.String,
                                                                     Size = 4
                                                                 },
                                                     new Segment {
                                                                     OrderId = 0,
                                                                     Name = string.Format("Сегмент BytesArray"),
                                                                     Type = DataType.Bytes,
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
    }
}
