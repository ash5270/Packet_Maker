using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packet_Maker.Process
{
    public class ReadBaseLanguage
    {
        private string? m_baseLanguagePath = "";
        public string? CsBase;
        public string? CppBase;
        
        public string? CsPacketID;
        public string? CppPacketID;

        public ReadBaseLanguage() 
        {
            m_baseLanguagePath = OptionConfigManager.ConfigData.packet_base_file_path;
        }

        public void ReadAll()
        {
            CsBase = File.ReadAllText(m_baseLanguagePath + "PacketBaseCs.txt");
            CppBase = File.ReadAllText(m_baseLanguagePath + "PacketBaseCpp.txt");

            CsPacketID = File.ReadAllText(m_baseLanguagePath + "PacketIDCs.txt");
            CppPacketID = File.ReadAllText(m_baseLanguagePath + "PacketIDCpp.txt");
        }
    }
}
