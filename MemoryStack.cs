using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_editor
{
    internal class MemoryStack
    {
        public Stack <ChangeMemory> MainStack { get; set; }    //For Undo
        public Stack <ChangeMemory> BackStack { get; set; }    //For Redo

        public MemoryStack ()
        {
            MainStack = new Stack <ChangeMemory> ();
            BackStack = new Stack <ChangeMemory> ();
        }

        public void MainStackAdd (bool type, string text, int pos)
        {
            MainStack.Push (new ChangeMemory (type, text, pos));
        }

        public ChangeMemory MainStackPull()
        {
            if (MainStack != null && MainStack.Count!=0)
                return MainStack.Pop ();
            else
                return null;
        }

        public void BackStackAdd (ChangeMemory memory)
        {
            BackStack.Push (memory);
        }

        public ChangeMemory BackStackPull()
        {
            if (BackStack != null && BackStack.Count!=0)
                return BackStack.Pop ();
            else
                return null;
        }

        public void MainStackClear() => MainStack.Clear ();

        public void BackStackClear() => BackStack.Clear();
    }
}
