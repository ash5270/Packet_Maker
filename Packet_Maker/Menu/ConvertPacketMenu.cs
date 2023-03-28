using Packet_Maker.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packet_Maker.Menu
{
    public class ConvertPacketMenu : BaseMenu
    {
        private ConvertPacket m_convertPacket;
       
        public ConvertPacketMenu(string name, MenuManger manger) : base(name, manger)
        {
            m_convertPacket=new ConvertPacket();
        }

        public override void InputCommad(ConsoleKey input)
        {
            
        }

        public override void StartProcess()
        {
            m_convertPacket.Start();
            m_menuManger.PrevMenu();
            App.SetInputMode(true);
        }

        public override string Print()
        {
            return m_title;
        }

        private string m_title = "\t\t\t------------\n";
    }
}
