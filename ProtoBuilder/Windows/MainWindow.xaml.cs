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
                var segments = new List<Segment>();
                for (var j = 0; j < 3; j++) {
                    segments.Add(new Segment {
                        OrderId = 0,
                        Name = string.Format("Сегмент {0} пакета {1}", j, i),
                        Size = 4
                    });
                }
                packets.Add(new Packet {
                    Name = string.Format("Packet {0}", i),
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
