using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Packet_Maker.Data
{
    public class PacketData
    {
        public string name { get; set; }
        public int ID { get; set; }
        public List<ValueData> values { get; set; }
       
    }
}
