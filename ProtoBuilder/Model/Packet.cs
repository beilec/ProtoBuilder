using System.Linq;
using System.Collections.Generic;

namespace ProtoBuilder.Model {
    public class Packet {
        public string Name { get; set; }
        public List<Segment> Segments { get; set; }
        public uint Size {
            get { return Segments == null ? 0 : (uint) Segments.Sum(t => t.Size); }
        }
    }
}
