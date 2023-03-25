using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;



namespace Packet_Maker.Process
{
    public class ConvertPacket
    {
        public ConvertPacket() { }

        public void Start()
        {
            var file = File.Open("test.txt",System.IO.FileMode.Open);
            byte[] b = new byte[1024];
            file.Read(b);
            Console.WriteLine(Encoding.UTF8.GetString(b));
        }

    }
}
