using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parser
{
    [Serializable]
    public class InterfaceItem
    {
        public InterfaceType interface_type;//enum
        public List<string> id_list;
        public InterfaceMode interface_mode;//enum
        public SubTypeIndication subtype;
        public bool hasValue;

        public int value; //значение (если есть)

        private List<string> interface_item_string;

        private void Init(List<string> interface_item_string)
        {
            this.interface_item_string = new List<string>(interface_item_string);
            this.interface_item_string.RemoveAll(Globals.isSpaceOrEmpty);
        }

        public InterfaceItem(List<string> interface_item_string)
        {
            Init(interface_item_string);
            setInterfaceType();
            SetIdList();
            setMode();
            setSubTypeIndication();
            if (hasValue)
                SetValue();
        }

        private void SetValue()
        {
            Globals.DeleteBeforeWord(":=", ref interface_item_string);
            value = (int)Globals.Calculate(ref interface_item_string);
        }

        private void setSubTypeIndication()
        {
            subtype = new SubTypeIndication(interface_item_string);
            hasValue = interface_item_string.Contains(":=");
        }

        private void setMode()
        {
            string s_first_word = interface_item_string.ElementAt(0);
            SetModeFromWord(s_first_word);
            if (!interface_mode.Equals(InterfaceMode._not_defined))
                interface_item_string.RemoveAt(0);
            else
            {
                interface_mode = InterfaceMode._in;
                if (interface_type.Equals(InterfaceType.not_defined))
                    interface_type = InterfaceType.constant;
            }
        }

        private void SetModeFromWord(string s_first_word)
        {
            switch (s_first_word)
            {
                case "in":
                    interface_mode = InterfaceMode._in;
                    break;
                case "out":
                    interface_mode = InterfaceMode._out;
                    break;
                case "inout":
                    interface_mode = InterfaceMode._inout;
                    break;
                case "buffer":
                    interface_mode = InterfaceMode._buffer;
                    break;
                default:
                    interface_mode = InterfaceMode._not_defined;
                    break;
            }
        }

        private void SetIdList()
        {
            try
            {
                int index_of_double_point = interface_item_string.IndexOf(":");
                List<string> temp = new List<string>(interface_item_string);
                temp.RemoveRange(index_of_double_point, temp.Count - index_of_double_point);
                id_list = new List<string>(Globals.MakeStringFromList(temp).Split(new char[] { ' ', ',', ')', '(' },
                    StringSplitOptions.RemoveEmptyEntries));
                interface_item_string.RemoveRange(0, index_of_double_point + 1);
            }
            catch (ArgumentOutOfRangeException e) { }
        }

        private void setInterfaceType()
        {
            int index_of_first_word;
            string s_first_word = Globals.GetFirstWord(interface_item_string,
                out index_of_first_word);
            SetInterfaceTypeFromWord(s_first_word);
            if (!interface_type.Equals(InterfaceType.not_defined))
                while (index_of_first_word >= 0)
                    interface_item_string.RemoveAt(index_of_first_word--);
        }

        private void SetInterfaceTypeFromWord(string s_first_word)
        {
            switch (s_first_word)
            {
                case "constant":
                    interface_type = InterfaceType.constant;
                    break;
                case "file":
                    interface_type = InterfaceType.file;
                    break;
                case "signal":
                    interface_type = InterfaceType.signal;
                    break;
                case "var":
                    interface_type = InterfaceType.var;
                    break;
                default:
                    interface_type = InterfaceType.not_defined;
                    break;
            }
        }
    }
}
