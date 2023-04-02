using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packet_Maker.Menu
{
    public class MainMenu : BaseMenu
    {
        public MainMenu(string name,MenuManger manger) : base(name,manger)
        {

        }

        public override string Print()
        {
            return m_title;
        }

        public override void InputCommad(ConsoleKey input)
        {
            switch (input)
            {
                case ConsoleKey.D1:
                    m_menuManger.NextMenu(Menu_Type.StartMenu);
                    break;
                case ConsoleKey.D2:
                    var path = OptionConfigManager.ConfigData.output_dir;
                    if (path == "")
                        path = Directory.GetCurrentDirectory();

                    System.Diagnostics.Process.Start(
                        new System.Diagnostics.ProcessStartInfo("explorer",path));
                    m_menuManger.ResetPrint();
                    break;
                case ConsoleKey.D3:
                case ConsoleKey.Q:
                    App.Stop();
                    break;
                default:
                    m_menuManger.ResetPrint();
                    break;
            }
        }

        private string m_title =
            "\t\t\t1.   Start Make Packet...\n" +
            "\t\t\t2.   Directory Open...\n" +
            "\t\t\t3.   exit...\n";
    }
}
