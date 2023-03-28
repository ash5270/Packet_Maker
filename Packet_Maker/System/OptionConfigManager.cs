//setting 
//기존에 저장했던 옵션/셋팅 정보 관리
//json으로 데이터 옵션 저장
//
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json;


namespace Packet_Maker
{
    public static class OptionConfigManager
    {
        public static Dictionary<string, LanguageConfig> LanguageConfigs
            = new Dictionary<string, LanguageConfig>();

        public static ConfigData? data;
        public static void Init()
        {
            try
            {
                var file = File.Open("config.json", FileMode.Open);
                var reader = new StreamReader(file);
                var json = reader.ReadToEnd();

                if (json == null) return;

                data = JsonSerializer.Deserialize<ConfigData>(json);

                if (data == null) return;

                var language = File.Open("language.json", FileMode.Open);
                var jsonLanguage = JsonDocument.Parse(language).RootElement;

                for (int i = 0; i < data.convert_language.Count(); i++)
                {
                    var curLan = jsonLanguage.GetProperty(data.convert_language[i]);
                    LanguageConfig curConfig = new LanguageConfig();
#pragma warning disable CS8601
                    //8bit - 1byte data
                    curConfig.S8BitBYTE = curLan.GetProperty("byte").GetString();
                    curConfig.U8BitBYTE = curLan.GetProperty("ubyte").GetString();
                    //16bit -2byte data
                    curConfig.U16BitINT = curLan.GetProperty("ushort").GetString();
                    curConfig.S16BitINT = curLan.GetProperty("short").GetString();
                    //32bit -4byte data
                    curConfig.S32BitINT = curLan.GetProperty("int").GetString();
                    curConfig.U32BitINT = curLan.GetProperty("uint").GetString();
                    //64bit-8byte data
                    curConfig.S64BitINT = curLan.GetProperty("long").GetString();
                    curConfig.U64BitINT = curLan.GetProperty("ulong").GetString();
                    //string
                    curConfig.STRING = curLan.GetProperty("string").GetString();
                    curConfig.WSTRING = curLan.GetProperty("wstring").GetString();
                    //32bit -4byte float
                    curConfig.S32BitFLOAT = curLan.GetProperty("float").GetString();
                    //64bit -8byte DOBULE
                    curConfig.S64BitDOBULE = curLan.GetProperty("double").GetString();
#pragma warning restore CS8601

                    LanguageConfigs.Add(data.convert_language[i], curConfig);
                }

            }catch (Exception e)
            {
                //Debug.Assert(true,e.ToString());
            }

        }

        public static void Start()
        { 
            
        }

        public static void Stop()
        {

        }

        private static void ReadConfigFile(string[] files)
        {
        
        }
    }
}
