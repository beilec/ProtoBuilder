using System.Collections.Generic;

namespace ProtoBuilder.Model {
    public class Protocol {
        //  Basic properties
        public string Name { get; set; }
        public int ConcurentConnections { get; set; }
        public int Port { get; set; }
        //  Server side properties
        public int ServerListenerTimeout { get; set; }
        public bool NoTimeout { get; set; }
        public int ServerSendTimeout { get; set; }
        public int ServerReceiveTimeout { get; set; }
        public int ServerKeepAliveTimeout { get; set; }
        //  Client side properties
        public int ClientSendTimeout { get; set; }
        public int ClientReceiveTimeout { get; set; }
        //  Packets collection
        public List<Packet> Packets { get; set; }
    }
}
