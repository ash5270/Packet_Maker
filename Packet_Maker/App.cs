using Packet_Maker.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packet_Maker
{
    //http://patorjk.com/software/taag/#p=display&f=Slant&t=Packet%20Maker
    public static class App
    {
        private static MenuManger menuManger;
        private static bool m_isUpdate=false;
        public static void Init()
        {
            menuManger = new MenuManger();
        }

        public static void Start()
        {
            m_isUpdate= true;
    
        }
        public static void Update()
        {
            while (m_isUpdate)
            {
                Console.Write("\t\t\t->\t");
                //키 입력
                var key= Console.ReadKey();
                menuManger.GetCommand(key.Key);
                
            }
        }

        public static void Stop()
        {
            m_isUpdate = false;
        }
    }
}
