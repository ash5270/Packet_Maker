using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packet_Maker.Menu
{
    public class StartMakePacketMenu : BaseMenu
    {
        public StartMakePacketMenu(string name, MenuManger manger) : base(name, manger)
        {

        }

        public override void InputCommad(ConsoleKey input)
        {
            switch (input)
            {
                case ConsoleKey.D1:
                    m_menuManger.NextMenu(Menu_Type.ConvertStart);
                    break;
                case ConsoleKey.D2:
                    m_menuManger.NextMenu(Menu_Type.SelectFile);
                    break;
                case ConsoleKey.D3:
                    m_menuManger.NextMenu(Menu_Type.SelectOutDir);
                    break;
                case ConsoleKey.D4:
                    m_menuManger.NextMenu(Menu_Type.SelectLanguage);
                    break;
                default:
                    m_menuManger.ResetPrint();
                    break;
            }
        }

        public override string Print()
        {
            return m_title;
        }

        private string m_title =
            "\t\t\t1.   Starting Make...\n" +
            "\t\t\t2.   File Select...\n" +
            "\t\t\t3.   Select Output Dir...\n" +
            "\t\t\t4.   Select Language...\n" +
            "\t\t\tq.   back menu...\n";
    }
}
