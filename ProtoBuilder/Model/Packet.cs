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

        public int  MoveSegment2PrevPosition(Segment segment) {
            var orderId = -1;
            foreach (var seg in Segments.OrderBy(t=>t.OrderId)) {
                orderId++;
                seg.OrderId = orderId;
            }
            var oldIndex = Segments[Segments.IndexOf(segment)].OrderId;
            if (oldIndex == 0) return oldIndex;
            Segments.First(t => t.OrderId == oldIndex - 1).OrderId = oldIndex;
            segment.OrderId = oldIndex - 1;
            return segment.OrderId;
        }

        public int MoveSegment2NextPosition(Segment segment) {
            var orderId = -1;
            foreach (var seg in Segments.OrderBy(t => t.OrderId)) {
                orderId++;
                seg.OrderId = orderId;
            }
            var oldIndex = Segments[Segments.IndexOf(segment)].OrderId;
            if (oldIndex == Segments.Count - 1) return oldIndex;
            Segments.First(t => t.OrderId == oldIndex + 1).OrderId = oldIndex;
            segment.OrderId = oldIndex + 1;
            return segment.OrderId;
        }
    }

    public enum PacketType { Request, Response }
}
