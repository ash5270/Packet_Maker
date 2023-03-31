using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packet_Maker.Menu
{
    public class PopupMenu : BaseMenu
    {
        public PopupMenu(string name, MenuManger manger) : base(name, manger)
        {
        }

        public override void InputCommad(ConsoleKey input)
        {
            base.InputCommad(input);
        }

        public override void StartProcess()
        {

        }

        public override string Print()
        {
            return m_title;
        }

        private string m_title = "\t\t\tComplete\n";

    }


    public enum PacketID : Int32
    {

    }

    public abstract class Packet
    {
        public abstract PacketID GetID();
        public abstract Int32 GetSize();
    }
    
    public class P_ {0} : Packet
    {
        public override PacketID GetID()
        {
            return PI_{ 0};
        }

        public override Int32 GetSize()
        {
            return {1};
        }
        //member values
        {2}
           
        //member function
        {3}
    }
}
