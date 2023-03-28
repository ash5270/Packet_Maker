using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Packet_Maker.Menu
{
    public enum Menu_Type : int
    {
        //main 
        Main = 0,
        EXIT =99,

        //MainMenu
        StartMenu=1,
        DirOpen = 2,

        //StartMakePacket Menu
        ConvertStart=10,
        SelectFile =11,
        SelectOutDir = 12,
        SelectLanguage = 13,
    }

    public class MenuManger
    {

        private int m_curMenuIndex;
        private Stack<int> m_menuIndexStack;
        private Dictionary<Menu_Type, BaseMenu> m_menuDic;


        public MenuManger()
        {
            m_menuIndexStack= new Stack<int>(); 
            m_menuDic = new Dictionary<Menu_Type, BaseMenu>();
            m_curMenuIndex = 0;
            //
            m_menuDic.Add(Menu_Type.Main, new MainMenu("main",this));
            m_menuDic.Add(Menu_Type.StartMenu, new StartMakePacketMenu("start",this));
            m_menuDic.Add(Menu_Type.ConvertStart,new ConvertPacketMenu("convert",this));
            //
            CurrentPrint();
        }


        public void NextMenu(int index)
        {
            m_menuIndexStack.Push(m_curMenuIndex);
            m_curMenuIndex = index;
        }

        public void NextMenu(Menu_Type type)
        {
            m_menuIndexStack.Push(m_curMenuIndex);
            m_curMenuIndex = (int)type;

            CurrentPrint();
            m_menuDic[(Menu_Type)m_curMenuIndex].StartProcess();
        }

        //메뉴에서 입력 받았을때 입력 받은 값 
        public void GetCommand(ConsoleKey command)
        {
            if (command == ConsoleKey.Q&&(m_curMenuIndex!= (int)Menu_Type.Main&&
                m_curMenuIndex!=(int)Menu_Type.ConvertStart))
            {
                PrevMenu();
            }
            else
            {
                m_menuDic[(Menu_Type)m_curMenuIndex].InputCommad(command);
            }
        }

        private void PrevMenu()
        {
            m_curMenuIndex = m_menuIndexStack.Peek();
            m_menuIndexStack.Pop();

            CurrentPrint();
        }

        public void ResetPrint()
        {
            CurrentPrint();
        }

        private void CurrentPrint()
        {
            //한번 초기화
            Console.Clear();
            //메인 타이틀 출력
            Console.WriteLine(BaseMenu.MainTitle);
            //현재 메뉴 출력
            Console.WriteLine("\t\t\t   ---- " + m_menuDic[(Menu_Type)m_curMenuIndex].MenuName.ToUpper()+" ----\n");
            Console.WriteLine(m_menuDic[(Menu_Type)m_curMenuIndex].Print());
        }

        public void Print()
        {
            CurrentPrint();
        }
    }
}
