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
        public static ConfigData? data;
        public static JsonElement LanguageConfig;
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
                LanguageConfig = JsonDocument.Parse(language).RootElement;

             
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
