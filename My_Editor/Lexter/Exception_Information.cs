using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace My_Editor
{
    public class Exception_Information
    {
        //Начало сообщения об ошыбке
        public int Offset { get; set; }

        //длина сообщения об ошыбке
        public int Length { get; set; }

        //описание ошыбки
        public string Description { get; set; }

        public Exception_Information() { }
        public Exception_Information(int Offset, int Length, string Description)
        {
            this.Description = Description;
            this.Offset = Offset;
            this.Length = Length;
        }
    }
}
