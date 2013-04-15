using System;
using System.Linq;
using System.Collections.Generic;

namespace ProtoBuilder.Model {
    public class Packet {
        //  Basic properties
        public string Name { get; set; }
        public string Description { get; set; }
        public PacketType PacketType { get; set; }
        //  Segments collection
        public List<Segment> Segments { get; set; }
        public uint Size {
            get { return Segments == null ? 0 : (uint) Segments.Sum(t => t.Size); }
        }
        public static List<PacketType> GetPacketTypes() {
            return Enum.GetValues(typeof (PacketType)).Cast<PacketType>().ToList();
        }
    }

    public enum PacketType { Request, Response }
}
