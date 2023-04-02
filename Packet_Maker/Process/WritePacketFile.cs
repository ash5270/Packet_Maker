using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packet_Maker.Process
{
    //파일 작성 
    public class WritePacketFile
    {
        public WritePacketFile()
        {

        }

        public void WriteFile(string str, string file_name, string path)
        {
            using (var file = File.Open(path + file_name, FileMode.OpenOrCreate))
            {
                using (var stream = new StreamWriter(file))
                {
                    //여기서 파일 쓰기
                    stream.Write(str);
                }
            }
        }
    }
}
