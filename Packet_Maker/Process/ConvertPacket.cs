using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
using System.ComponentModel.Design;
using System.Diagnostics.CodeAnalysis;

namespace Packet_Maker.Process
{
    public class ConvertPacket
    {
        public ConvertPacket() { }
        private bool ConvertClass(string? str, Dictionary<string, StreamWriter> writers)
        {
            if (str == null)
                return false;
            if (!Regex.IsMatch(str, "class") || str == "")
                return false;

            string pattern = @"(class.*@\w+.*:.*@\w+)";
            var result = Regex.Match(str, pattern).Value;

            //클래스 패턴 매칭
            if (Regex.Match(str, pattern).Value == str && str != "")
            {
                //loop
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
        private bool ConvertValue(string? str, Dictionary<string, StreamWriter> writers)
        {
            if (str == null) return false;

            string pattern = @"^(?!.*(#\w+).*(\$\w+)\(.*\))(#\w+ @\w+)$";
            if (!Regex.IsMatch(str, pattern))
                return false;


            pattern = @"(?<=#)\w+";
            var valueType = Regex.Match(str, pattern).Value;

            //요만큼 loop
            valueType = OptionConfigManager.LanguageConfig.GetProperty(language).GetProperty(valueType).GetString();
            pattern = @"(?<=@)\w+";
            var valueName = Regex.Match(str, pattern).Value;
            string re = string.Format("{0} {1};", valueType, valueName);
            writer.WriteLine(re);



            Console.WriteLine(re);
            return false;
        }

        private bool ConvertFunc(string? str, Dictionary<string, StreamWriter> writers)
        {
            if (str == null) return false;
            //함수인지 체크
            string pattern = @"(#\w+).*(\$\w+)\(.*\)";
            if (!Regex.IsMatch(str, pattern))
                return false;

            bool isOverride = false;

            //override인지 체크
            pattern = @"(#\w+) (\$\w+)\(.*\)";
            if (Regex.IsMatch(str, pattern))
                isOverride = true;

            pattern = @"(?<=#)\w+";
            var funcReturnType = Regex.Match(str, pattern).Value;
            funcReturnType = OptionConfigManager.LanguageConfig.GetProperty(language).GetProperty(funcReturnType).GetString();
            pattern = @"(?<=\$)\w+";
            var funcName = Regex.Match(str, pattern).Value;

            //함수 인자 확인 
            //일단 ref 받는지 확인
            pattern = @"\(([^)]*)\)";
            var args= Regex.Match(str, pattern).Value;
            args= args.Remove(0,1);
            args=args.Remove(args.Length - 1,1);
            //ref,type,name
            List<Tuple<bool,string,string>> argumentTypes = new List<Tuple<bool, string, string>>();

            var strs= args.Split(',');
            for(int i=0; i<strs.Length; i++)
            {
                bool isRef = false;
                string type = "";
                string name = "";
                if (strs[i].IndexOf("ref") != -1)
                    isRef = true;
                if (strs[i].IndexOf("#")!=-1)
                {
                    int start = strs[i].IndexOf("#");
                    int end = strs[i].IndexOf(" ",start);
                    if (end == -1)
                        end = strs[i].Length;
                    type = strs[i].Substring(start+1, end - start-1);
                    type = OptionConfigManager.LanguageConfig.GetProperty(language).GetProperty(type).GetString();
                }

                if(strs[i].IndexOf("@") != -1)
                {
                    int start =strs[i].IndexOf("@");
                    name = strs[i].Substring(start+1, strs[i].Length-start-1);
                }

                argumentTypes.Add(new Tuple<bool, string, string>(isRef, type, name));
               
            }

            string result = "";


            //loog 돌려서 두번
            if(language=="cpp")
            {
                result += "public ";
                result += isOverride?"override ":"";
                result +=funcReturnType + " ";
              
                result += funcName + "(";

                for(int i=0; i< argumentTypes.Count; i++)
                {
                    string refCheck = argumentTypes[i].Item1 ? "ref " : "";
                    result += refCheck + argumentTypes[i].Item2 + " " + argumentTypes[i].Item3+",";
                }
                result=result.Remove(result.Length-1,1);
                result += ")\n";  
            }

            writer.Write(result);


            return true;
        }

        public bool ConvertFuncIn(string str, Dictionary<string, StreamWriter> writers)
        {
            if (str == "" || str == "{" || str=="}"||str=="\n")
                return false;
            string pattern = @"#\w+ @\w+";
            string result = "";
            if (!Regex.IsMatch(str, pattern))
            {
                if (Regex.IsMatch(str, @"return @\w+"))
                {
                    result += "\treturn ";
                    var value = Regex.Match(str, @"(?<=@)\w+").Value;
                    result += value +";";
                    writer.WriteLine(result);
                    return false;
                }
                if (Regex.IsMatch(str, @"return"))
                {
                    return false;
                }
                else
                    result =str+";";
            }
            else
            {
                pattern = @"(?<=#)\w+";
                var valueType = Regex.Match(str, pattern).Value;
                valueType = OptionConfigManager.LanguageConfig.GetProperty(language).GetProperty(valueType).GetString();
                pattern = @"(?<=@)\w+";
                var valueName = Regex.Match(str, pattern).Value;

                result = string.Format("\t{0} {1};",valueType,valueName);
            }

            writer.WriteLine(result);
            return true;
        }

        public async void Start()
        {
            App.SetInputMode(false);

            var file = File.Open("test.pml", System.IO.FileMode.Open);
            StreamReader reader = new StreamReader(file);

            var cppfile = File.Open("packet.cpp", System.IO.FileMode.OpenOrCreate);
            StreamWriter cppWriter = new StreamWriter(cppfile);

            var csfile = File.Open("packet.cs", System.IO.FileMode.OpenOrCreate);
            StreamWriter csWriter = new StreamWriter(csfile);


            Dictionary<string, StreamWriter> writers=new Dictionary<string, StreamWriter>();

            writers.Add("cpp", cppWriter);
            writers.Add("cs", csWriter);

            string? str = "";
            while (str != "~end")
            {
                str = await reader.ReadLineAsync();
                if (ConvertClass(str, csWriter))
                {

                }
                else if (ConvertValue(str, csWriter))
                {

                }
                else if (ConvertFunc(str, csWriter))
                {
                    str = await reader.ReadLineAsync();
                    writer.WriteLine("{");
                    while (ConvertFuncIn(str, csWriter))
                    {
                        str = await reader.ReadLineAsync();
                    }
                    writer.WriteLine("}");
                }
            }

            writer.Close();
            cppfile.Close();
            reader.Close();
            file.Close();
        }
    }
}
