using System.Collections.Generic;

namespace ProtoBuilder.Model {
    public class Protocol {
        public string Name { get; set; }
        public List<Packet> Packets { get; set; }
    }
}
