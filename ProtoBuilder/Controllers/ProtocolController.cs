using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Win32;
using ProtoBuilder.Model;
using ProtoBuilder.Windows;

namespace ProtoBuilder.Controllers {
    [XmlRoot("ProtocolsStorageFile", Namespace = "name.pdev.ProtoBuilder.Protocols", IsNullable = false)]
    [Serializable]
    public class ProtocolController : Controller {
        [XmlElement(Namespace = "name.pdev.ProtoBuilder.Protocols", ElementName = "Protocol")]
        public List<Protocol> Protocols { get; set; }
        [XmlIgnore]
        public string FileName { get; set; }

        public ProtocolController() {
            Protocols = new List<Protocol>();
        }

        public Segment AddSegment(Packet currentPacket) {
            var segment = new Segment {
                Name = "",
                Description = "",
                OrderId = currentPacket.Segments.Count + 1,
                Type = new DataTypeView { Type = DataType.Byte },
                Size = DataTypeView.SizeOfType(DataType.Byte)
            };

            currentPacket.Segments.Add(segment);
            return segment;
        }

        public static void SaveProtocols(ProtocolController storage) {
            if (string.IsNullOrEmpty(storage.FileName)) {
                var dlg = new SaveFileDialog {
                    Title = "Save protocols to XML Storage",
                    FileName = "Protocols",
                    DefaultExt = ".xml",
                    Filter = "Protocols XML Storage (.xml)|*.xml"
                };
                var result = (bool) dlg.ShowDialog();
                if (!result) return;
                storage.FileName = dlg.FileName;
            }
            var xml = new XmlSerializer(typeof(ProtocolController));
            using (var writer = new StreamWriter(storage.FileName)) {
                xml.Serialize(writer, storage);
            }
        }

        public static ProtocolController LoadProtocols() {
            ProtocolController protocolController;
            var dlg = new OpenFileDialog {
                Title = "Open Protocols XML Storage",
                FileName = "Protocols",
                DefaultExt = ".xml",
                Filter = "Protocols XML Storage (.xml)|*.xml"
            };
            var result = (bool) dlg.ShowDialog();
            if (!result) return null;
            var xml = new XmlSerializer(typeof(ProtocolController));
            using (var reader = new StreamReader(dlg.FileName)) {
                protocolController = (ProtocolController) xml.Deserialize(reader);
                protocolController.FileName = dlg.FileName;
            }

            return protocolController;
        }

        public Packet AddPacket(Protocol currentProtocol) {
            var packet = new Packet {
                                        Name = "",
                                        Description = "",
                                        Segments = new List<Segment>()
                                    };

            var win = new PacketEditWindow(packet);
            var showDialog = win.ShowDialog();
            if (showDialog != null && (bool) showDialog) {
                currentProtocol.Packets.Add(packet);
                return packet;
            }

            return null;
        }

        public void ShowEditProtocolWindow(Protocol protocol) {
            var window = new ProtocolPropertiesWindow(protocol);
            window.ShowDialog();
        }

        public void CreateProtocol() {
            var protocol = new Protocol {
                Name = "New protocol",
                Packets = new List<Packet>()
            };
            Protocols = Protocols ?? new List<Protocol>();
            var window = new ProtocolPropertiesWindow(protocol);
            var showDialog = window.ShowDialog();
            if (showDialog != null && (bool) showDialog)
                Protocols.Add(protocol);
        }
    }
}