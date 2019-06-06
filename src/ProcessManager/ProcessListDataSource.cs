using NStack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Terminal.Gui;
using Rune = System.Rune;

namespace ProcessManager
{
    internal class ProcessListDataSource : IListDataSource
    {
        private IList<Process> _processes;
        private int _index = 0;
        public ProcessListDataSource(IList<Process> processes)
        {
            _processes = processes;
        }
        public int Count => _processes.Count;

        public bool IsMarked(int _) => false;
        public bool AllowsMarking { get { return false; } }
        public void Render(ListView container, ConsoleDriver driver, bool selected, int item, int col, int line, int width)
        {
            container.Move(col, line);
            var process = _processes[item];
            string output; 
            try
            {
                output = ($"{process.Id,7} {process.ProcessName,-10} {process.MainModule.FileName}");
            }
            catch (Exception)
            {
                output = ($"{process.Id,7} {process.ProcessName,-10} [Elevated process - cannot determine path]");
            }
            RenderUstr(driver, (string)output, col, line, width);
        }

        void RenderUstr(ConsoleDriver driver, ustring ustr, int col, int line, int width)
        {
            int byteLen = ustr.Length;
            int used = 0;
            for (int i = 0; i < byteLen;)
            {
                (var rune, var size) = Utf8.DecodeRune(ustr, i, i - byteLen);
                var count = Rune.ColumnWidth(rune);
                if (used + count >= width)
                    break;
                driver.AddRune(rune);
                used += count;
                i += size;
            }
            for (; used < width; used++)
            {
                driver.AddRune(' ');
            }
        }

        public void SetMark(int item, bool value)
        {
            throw new NotImplementedException();
        }
    }
}
