using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;

namespace ProcessManager
{
    internal class ProcessListView : ListView
    {
        public ProcessListView(ProcessListDataSource processList) : base(processList)
        { }
        public override bool ProcessKey(KeyEvent kb)
        {
            return base.ProcessKey(kb);
        }

        public override bool ProcessHotKey(KeyEvent keyEvent)
        {
            return base.ProcessHotKey(keyEvent);
        }
    }
}
