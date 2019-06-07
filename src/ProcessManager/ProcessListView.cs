using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Terminal.Gui;

namespace ProcessManager
{
    internal class ProcessListView : ListView
    {
        public event Action<Process> Select;
        public ProcessListView(ProcessListDataSource source) :base(source)
        {
        }

        public override bool ProcessKey(KeyEvent kb)
        {
            switch(kb.Key)
            {
                case Key.ControlR:
                    Reload();
                    break;
                case Key.Enter:
                    Select(((ProcessListDataSource)Source)[SelectedItem]);
                    break;
            }
            return base.ProcessKey(kb);
        }

        private void Reload()
        {
            Source = new ProcessListDataSource(ProcessHelper.GetProcessList());
            SetNeedsDisplay();
        }
    }
}
