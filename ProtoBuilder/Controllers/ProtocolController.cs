using System.Linq;
using System.Collections.Generic;
using ProtoBuilder.Model;
using ProtoBuilder.Windows;

namespace ProtoBuilder.Controllers {
    public class ProtocolController : Controller {
        public List<Protocol> Protocols { get; set; }

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
    }
}
