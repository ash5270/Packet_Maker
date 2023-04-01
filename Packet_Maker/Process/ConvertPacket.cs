using Packet_Maker.Data;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Packet_Maker.Process
{
    public class ConvertPacket
    {
        private ReadBaseLanguage m_baseReadData;
        private ReadBasePacket m_packetReadData;

        List<PacketData> m_packetDatas;

        public ConvertPacket()
        {
            m_baseReadData = new ReadBaseLanguage();
            m_packetReadData = new ReadBasePacket();
            //디시리얼라이즈 한 데이터 저장
            m_packetDatas=new List<PacketData>();
        }

        public void Start()
        {
            App.SetInputMode(false);
            //packet data load
            m_baseReadData.ReadAll();

            foreach(var packets in m_packetReadData.JsonPackets)
            {
                var packet = packets["Packet"];
                var data= JsonSerializer.Deserialize<PacketData>(packet.ToJsonString());
                m_packetDatas.Add(data);
            }

            SumSyntax(m_packetDatas[0],"cs");
        }
        
        //패킷 하나 만들기
        private string SumSyntax(PacketData packet,string language)
        {
            string str =  m_baseReadData.CsBase;
            string member= MemberSyntax(packet.values,language);
            string serialize = SerializeSyntax(packet.values);
            string deserialize= DeserializeSyntax(packet.values);

            string result = string.Format(str, packet.name, packet.name, 2, member, serialize, deserialize);
            return result;
        }

        private  string MemberSyntax(List<ValueData> values,string language)
        {
            string result="";
            foreach(var val in values)
            {
                var type = OptionConfigManager.LanguageConfig.GetProperty(language).GetProperty(val.type).GetString();
                if(language=="cs")
                    result += string.Format("public {0} {1};\n", type , val.name);
                else
                    result += string.Format("{0} {1};\n", type, val.name);
            }

            return result;
        }

        private string SerializeSyntax(List<ValueData> values)
        {
            string result = "";
            foreach (var val in values)
            {
                var type = OptionConfigManager.LanguageConfig.GetProperty("cs").GetProperty(val.type).GetString();
                result += string.Format("buffer.write({0});\n", val.name);
            }

            return result;
        }
        private string DeserializeSyntax(List<ValueData> values)
        {
            string result = "";
            foreach (var val in values)
            {
                var type = OptionConfigManager.LanguageConfig.GetProperty("cs").GetProperty(val.type).GetString();
                result += string.Format("buffer.read({0});\n", val.name);
            }

            return result;
        }
    }
}
