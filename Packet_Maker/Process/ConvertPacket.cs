using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace Packet_Maker.Process
{
    public class ConvertPacket
    {
        public ConvertPacket() { }
        private bool ConvertClass(string? str,StreamWriter writer)
        {
            if (str == null)
                return false;
            if (!Regex.IsMatch(str, "class") || str == "")
                return false;

            string pattern = @"(class @\w+ : @\w+)";
            var result = Regex.Match(str, pattern).Value;

            //클래스 패턴 매칭
            if (Regex.Match(str, pattern).Value == str && str != "")
            {
                pattern = @"(?<=@)\w+";
                var className = Regex.Matches(str, pattern);

                string re = $"class {className[0].Value} : {className[1].Value} {{";

                Console.WriteLine(re);
                writer.WriteLine(re);
                return true;
            }
            else
                return false;
        }
        
        //변수 패턴 매칭
        private bool ConvertValue(string? str,StreamWriter writer)
        {
            if (str == null)
                return false;

            string pattern = @"(#\w+ @\w+)";
            if (!Regex.IsMatch(str, pattern))
                return false;
                
            pattern = @"(?<=#)\w+";
            var valueType = Regex.Match(str, pattern).Value;



            pattern = @"(?<=@)\w+";
            var valueName =Regex.Match(str, pattern).Value;
            string re = string.Format("{0} {1};", "uin16_t", valueName);
            writer.WriteLine(re);
            Console.WriteLine(re);
            return false;
        }

        private bool ConvertFunc(string? str, StreamWriter writer)
        {
           
            return false;
        }

        public async void Start()
        {
            App.SetInputMode(false);

            var file = File.Open("test.pml", System.IO.FileMode.Open);
            StreamReader reader = new StreamReader(file);

            var cppfile = File.Open("packet.cpp", System.IO.FileMode.OpenOrCreate);
            StreamWriter writer = new StreamWriter(cppfile);

            string? str = "";
            while (str != "!end")
            {
                str = await reader.ReadLineAsync();
                if (ConvertClass(str, writer))
                {

                }
                else if (ConvertValue(str, writer))
                {

                }else if(ConvertFunc(str, writer))
                {

                }
            }

            writer.Close();
            cppfile.Close();
            reader.Close();
            file.Close();
        }
    }
}
