using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;

namespace ProcessManager
{
    internal class ProcessDetailsTextView : TextView
    {
        public ProcessDetailsTextView(Rect frame) : base(frame)
        { }
        public ProcessDetailsTextView() : base()
        { }
        public event Action YieldFocus;
        public override bool ProcessKey(KeyEvent kb)
        {
            switch(kb.Key)
            {
                case Key.Esc:
                    YieldFocus();
                    break;
            }
            return base.ProcessKey(kb);
        }
    }
}
