using Packet_Maker.Data;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Packet_Maker.Process
{
    public class ConvertPacket
    {
        private ReadBaseLanguage m_baseReadData;
        private ReadPacket m_packetReadData;
        private WritePacketFile m_writePacketFile;

        List<PacketData> m_packetDatas;

        public ConvertPacket()
        {
           
        }

        public void Start()
        {
            m_baseReadData = new ReadBaseLanguage();
            m_packetReadData = new ReadPacket();
            m_writePacketFile = new WritePacketFile();
            //디시리얼라이즈 한 데이터 저장
            m_packetDatas = new List<PacketData>();

            App.SetInputMode(false);
            //packet data load
            m_baseReadData.ReadAll();

            foreach (var packets in m_packetReadData.JsonPackets)
            {
                var packet = packets["Packet"];
                var data = JsonSerializer.Deserialize<PacketData>(packet.ToJsonString());
                m_packetDatas.Add(data);
            }

            MakeCPP(m_packetDatas);
            MakeCS(m_packetDatas);
        }
        //cpp 파일 string 생성
        private void MakeCPP(List<PacketData> packets)
        {
            //패킷 ID
            string id_str = SumPacketID(m_baseReadData.CppPacketID);
            m_writePacketFile.WriteFile(id_str, "PacketID.cpp", @"Cpp\");
            //패킷 Data
            string packet_str = "";
            foreach (var packet in packets)
            {
                packet_str += SumPacketData(packet, m_baseReadData.CppBase, "cpp");
            }
            m_writePacketFile.WriteFile(packet_str, "Packet.cpp", @"Cpp\");
        }
        //cs 파일 string 생성
        private void MakeCS(List<PacketData> packets)
        {
            //패킷 ID
            string id_str = SumPacketID(m_baseReadData.CsPacketID);
            m_writePacketFile.WriteFile(id_str, "PacketID.cs", @"Cs\");
            //패킷 Data
            string packet_str = "";
            foreach (var packet in packets)
            {
                packet_str += SumPacketData(packet, m_baseReadData.CsBase, "cs");
            }
            m_writePacketFile.WriteFile(packet_str, "Packet.cs", @"Cs\");
        }

        //패킷 아이디 파일 string 값 합침
        public string SumPacketID(string baseFile)
        {
            string str = baseFile;
            string id = IDSyntaxCreate(m_packetDatas);
            string result = string.Format(str, id);
            return result;
        }

        //패킷 파일 string 값 합침
        private string SumPacketData(PacketData packet, string baseFile, string language)
        {
            string str = baseFile;
            string member = MemberSyntaxCreate(packet.values, language);
            string serialize = SerializeSyntaxCreate(packet.values, language);
            string deserialize = DeserializeSyntaxCreate(packet.values, language);
            string size = SizeSyntaxCreate(packet.values, language);
            string result = string.Format(str, packet.name, packet.name, size, member, serialize, deserialize);
            return result;
        }

        //packet 문자열 구문 생성
        private string IDSyntaxCreate(List<PacketData> packets)
        {
            string result = "";
            foreach (var packet in packets) {

                result += string.Format("{0}={1},\n", packet.name, packet.ID);
            }

            return result;
        }

        private string MemberSyntaxCreate(List<ValueData> values, string language)
        {
            string result = "";
            foreach (var val in values)
            {
                var type = OptionConfigManager.LanguageConfig.GetProperty(language).GetProperty(val.type).GetString();
                if (language == "cs")
                    result += string.Format("public {0} {1};\n\t", type, val.name);
                else
                    result += string.Format("{0} {1};\n\t", type, val.name);
            }
            return result;
        }

        private string SizeSyntaxCreate(List<ValueData> values, string language)
        {
            string result = "";
            foreach (var val in values)
            {
                var type = OptionConfigManager.LanguageConfig.GetProperty(language).GetProperty(val.type).GetString();
                if(val.type=="string")
                {
                    var len_type  = OptionConfigManager.LanguageConfig.
                        GetProperty(language).GetProperty("short");
                    if(language=="cs")
                        result += string.Format("sizeof{0}+{1}.Length +", len_type, val.name);
                    else
                        result += string.Format("sizeof{0}+{1}.length() +", len_type, val.name);
                }
                else
                    result += string.Format("sizeof({0})+", type);
            }

            var lastOp= result.LastIndexOf('+');
            result = result.Remove(lastOp, 1);

            return result;
        }

        private string SerializeSyntaxCreate(List<ValueData> values, string language)
        {
            string result = "";
            foreach (var val in values)
            {
                var type = OptionConfigManager.LanguageConfig.GetProperty(language).GetProperty(val.type).GetString();
                result += string.Format("buffer.Write({0});\n\t\t", val.name);
            }

            return result;
        }
        private string DeserializeSyntaxCreate(List<ValueData> values, string language)
        {
            string result = "";
            foreach (var val in values)
            {
                var type = OptionConfigManager.LanguageConfig.GetProperty(language).GetProperty(val.type).GetString();
                result += string.Format("buffer.Read({0});\n\t\t", val.name);
            }

            return result;
        }
    }
}
