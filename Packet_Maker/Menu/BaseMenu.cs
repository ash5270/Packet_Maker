using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packet_Maker.Menu
{
    public class BaseMenu
    {
        public BaseMenu(string name,MenuManger manger) 
        { MenuName = name; m_menuManger = manger; }

        protected MenuManger m_menuManger;
        public virtual string Print() { return ""; }
        public virtual void InputCommad(ConsoleKey input) { }

        public virtual void StartProcess() { }

        public string MenuName;

        public static string MainTitle ="\n\n\n"+
"\t    ____             __        __     __  ___      __            \r\n" +
"\t   / __ \\____ ______/ /_____  / /_   /  |/  /___ _/ /_____  _____\r\n" +
"\t  / /_/ / __ `/ ___/ //_/ _ \\/ __/  / /|_/ / __ `/ //_/ _ \\/ ___/\r\n" +
"\t / ____/ /_/ / /__/ ,< /  __/ /_   / /  / / /_/ / ,< /  __/ /    \r\n" +
"\t/_/    \\__,_/\\___/_/|_|\\___/\\__/  /_/  /_/\\__,_/_/|_|\\___/_/     \r\n" +
"\t                                                                  \n" +
"\t---------------------------------------------------------------\n";


    }
}
