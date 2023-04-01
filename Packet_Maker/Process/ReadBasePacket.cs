using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.Json;
using System.Text.Json.Nodes;

namespace Packet_Maker.Process
{
    public class ReadBasePacket
    {
        private string m_basePacketPath = "";
        private JsonArray m_jsonArray;
        public JsonArray JsonPackets { get { return m_jsonArray; } }
        public ReadBasePacket()
        {
            var packetConfig= File.ReadAllText(m_basePacketPath+ "Packet.json");
            JsonNode node =JsonNode.Parse(packetConfig);
            m_jsonArray = node.AsArray();
        }
    }
}
