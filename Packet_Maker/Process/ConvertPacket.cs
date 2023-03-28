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
        private bool ConvertClass(string? str, StreamWriter writer, string language)
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
                string re = "";
                if (language == "cpp")
                    re = $"class {className[0].Value} :public {className[1].Value} {{";
                else
                    re = $"public class {className[0].Value} : {className[1].Value} {{";

                writer.WriteLine(re);
                return true;
            }
            else
                return false;
        }

        //변수 패턴 매칭
        private bool ConvertValue(string? str, StreamWriter writer, string language)
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
            return false;
        }

        private bool ConvertFunc(string? str, StreamWriter writer, string language)
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
            var args = Regex.Match(str, pattern).Value;
            args = args.Remove(0, 1);
            args = args.Remove(args.Length - 1, 1);
            //ref,type,name
            List<Tuple<bool, string, string>> argumentTypes = new List<Tuple<bool, string, string>>();

            var strs = args.Split(',');
            for (int i = 0; i < strs.Length; i++)
            {
                bool isRef = false;
                string type = "";
                string name = "";
                if (strs[i].IndexOf("ref") != -1)
                    isRef = true;
                if (strs[i].IndexOf("#") != -1)
                {
                    int start = strs[i].IndexOf("#");
                    int end = strs[i].IndexOf(" ", start);
                    if (end == -1)
                        end = strs[i].Length;
                    type = strs[i].Substring(start + 1, end - start - 1);
                    type = OptionConfigManager.LanguageConfig.GetProperty(language).GetProperty(type).GetString();
                }
                if (strs[i].IndexOf("@") != -1)
                {
                    int start = strs[i].IndexOf("@");
                    name = strs[i].Substring(start + 1, strs[i].Length - start - 1);
                }

                argumentTypes.Add(new Tuple<bool, string, string>(isRef, type, name));

            }

            string result = "";


            //loog 돌려서 두번
            if (language == "cs")
            {
                result += "public ";
                result += isOverride ? "override " : "";
                result += funcReturnType + " ";

                result += funcName + "(";

                for (int i = 0; i < argumentTypes.Count; i++)
                {
                    string refCheck = argumentTypes[i].Item1 ? "ref " : "";
                    result += refCheck + argumentTypes[i].Item2 + " " + argumentTypes[i].Item3 + ",";
                }
                result = result.Remove(result.Length - 1, 1);
                result += "){\n";
            }

            else if (language == "cpp")
            {

                result += funcReturnType + " ";

                result += funcName + "(";

                for (int i = 0; i < argumentTypes.Count; i++)
                {
                    string refCheck = argumentTypes[i].Item1 ? "&" : "";
                    result += argumentTypes[i].Item2 + refCheck + " " + argumentTypes[i].Item3 + ",";
                }
                result = result.Remove(result.Length - 1, 1);
                result += isOverride ? ")override{\n" : "){\n";
            }

            writer.Write(result);


            return true;
        }

        public bool ConvertFuncIn(string str, StreamWriter writer, string language)
        {
            if (str == "" || str == "{" || str == "}")
                return false;
            string pattern = @"#\w+ @\w+";
            string result = "";
            if (!Regex.IsMatch(str, pattern))
            {
                if (Regex.IsMatch(str, @"return @\w+"))
                {
                    result += "\treturn ";
                    var value = Regex.Match(str, @"(?<=@)\w+").Value;
                    result += value + ";}";
                    writer.WriteLine(result);
                    return false;
                }
                if (Regex.IsMatch(str, @"return"))
                {
                    writer.WriteLine("}");
                    return false;
                }
                else
                    result = str + ";";
            }
            else
            {
                pattern = @"(?<=#)\w+";
                var valueType = Regex.Match(str, pattern).Value;
                valueType = OptionConfigManager.LanguageConfig.GetProperty(language).GetProperty(valueType).GetString();
                pattern = @"(?<=@)\w+";
                var valueName = Regex.Match(str, pattern).Value;

                result = string.Format("\t{0} {1};", valueType, valueName);
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

            Dictionary<string, StreamWriter> writers = new Dictionary<string, StreamWriter>();

            writers.Add("cpp", cppWriter);
            writers.Add("cs", csWriter);

            bool isConvertStart = false;

            string? str = "";
            int count = 0;
            while (str != "~end")
            {
                str = await reader.ReadLineAsync();
                if(isConvertStart&&str=="")
                {
                    foreach (var dir in writers)
                    {
                        if (dir.Key == "cpp")
                            dir.Value.WriteLine("};\n\n");
                        else if(dir.Key =="cs")
                            dir.Value.WriteLine("}\n\n");
                    }
                    isConvertStart= false;
                    continue;
                }
                if (str == "~start" || str == "~end" || str == "")
                    continue;
                isConvertStart = true;
                if (str.IndexOf("class") != -1)
                {
                    foreach (var dir in writers)
                    {
                        ConvertClass(str, dir.Value, dir.Key);
                    }
                }
                else if (Regex.IsMatch(str, @"^(?!.*(#\w+).*(\$\w+)\(.*\))(#\w+ @\w+)$"))
                {
                    foreach (var dir in writers)
                    {
                        ConvertValue(str, dir.Value, dir.Key);
                    }
                }
                else if (Regex.IsMatch(str, @"(#\w+).*(\$\w+)\(.*\)"))
                {
                    foreach (var dir in writers)
                    {
                        ConvertFunc(str, dir.Value, dir.Key);
                    }
                }
                else
                {
                    foreach (var dir in writers)
                    {
                        ConvertFuncIn(str, dir.Value, dir.Key);
                    }
                }

                Console.Write('*');
            }
            foreach (var dir in writers)
            {
                dir.Value.Close();
            }
            cppfile.Close();
            csfile.Close();
            reader.Close();
            file.Close();
        }
    }
}
