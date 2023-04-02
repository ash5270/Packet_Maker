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
        public static ConfigData? ConfigData;
        public static JsonElement LanguageConfig;
        public static void Init()
        {
            using (var file = File.Open(@"Option\config.json", FileMode.Open))
            {
                var reader = new StreamReader(file);
                var json = reader.ReadToEnd();

                ConfigData = JsonSerializer.Deserialize<ConfigData>(json);
            }
            using (var language = File.Open("Option/language.json", FileMode.Open))
            {
                LanguageConfig = JsonDocument.Parse(language).RootElement;

            }
        }
    }
}
