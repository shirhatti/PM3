using Microsoft.Diagnostics.Tools.RuntimeClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Terminal.Gui;

namespace ProcessManager
{
    internal class WindowHelper
    {
        internal static View Create()
        {
            var top = new Toplevel()
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill()
            };
            var processesWindow = new Window("Processes")
            {
                X = 0,
                Y = 0,
                Width = Dim.Percent(50),
                Height = Dim.Percent(50)
            };
            var detailsWindow = new Window("Details")
            {
                X = 0,
                Y = Pos.Percent(50),
                Width = Dim.Percent(50),
                Height = Dim.Fill() -1
            };
            var processesListView = new ListView(new ProcessListDataSource(GetProcessList()))
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill(),
            };
            processesListView.CanFocus = true;
            processesWindow.Add(processesListView);
            var countersWindows = new Window("Counters")
            {
                X = Pos.Percent(50),
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill() - 1
            };
            var helpLabel = new View()
            {
                X = 0,
                Y = Pos.AnchorEnd(1),
                Width = Dim.Fill(),
                Height = 1,
                CanFocus = true
            };
            top.Add(processesWindow, countersWindows, detailsWindow, helpLabel);

            //Application.MainLoop.AddTimeout(TimeSpan.FromSeconds(5), (eventLoop) =>
            //{
            //    List<string> list = GetProcessList();
            //    var lv = new ListView(list)
            //    {
            //        X = 0,
            //        Y = 0,
            //        Width = Dim.Fill(),
            //        Height = Dim.Fill()
            //    };
            //    eventLoop.Invoke(() =>
            //    {
            //        processesWindow.RemoveAll();
            //        processesWindow.Add(lv);
            //    });
            //    return true;
            //});
            return top;
        }

        private static List<string> GetProcessListAsListOfString()
        {
            var processes = GetProcessList();
            var list = new List<string>();
            foreach (var process in processes)
            {
                try
                {
                    list.Add($"{process.Id,10} {process.ProcessName,-10} {process.MainModule.FileName}");
                }
                catch (Exception)
                {
                    list.Add($"{process.Id,10} {process.ProcessName,-10} [Elevated process - cannot determine path]");
                }
            }

            return list;
        }

        private static IList<Process> GetProcessList()
        {
            return EventPipeClient.ListAvailablePorts()
                .Select(GetProcessById)
                .Where(process => process != null)
                .OrderBy(process => process.ProcessName)
                .ThenBy(process => process.Id)
                .ToList();
        }

        private static Process GetProcessById(int processId)
        {
            try
            {
                return Process.GetProcessById(processId);
            }
            catch (ArgumentException)
            {
                return null;
            }
        }
    }
}