using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_editor
{
    internal class ChangeMemory
    {
        public bool TypeOfChange { get; set; }    //True - add, false - delete
        public string ChangeText { get; set; }
        public int Position { get; set; }

        public ChangeMemory(bool Type, string changeText, int position)
        {
            TypeOfChange = Type;
            ChangeText = changeText;
            Position = position;
        }

    }
}
