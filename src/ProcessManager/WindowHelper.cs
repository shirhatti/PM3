using Microsoft.Diagnostics.Tools.RuntimeClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
            var detailsTextView = new ProcessDetailsTextView()
            {
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill(),
                ReadOnly = true,
                CanFocus = true
            };
            detailsWindow.Add(detailsTextView);
            var processesListView = new ProcessListView(new ProcessListDataSource(ProcessHelper.GetProcessList()))
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
            var countersTextView = CountersTextView.Create();
            countersWindows.Add(countersTextView);
            Object timeoutToken = new Object(); 
            detailsTextView.YieldFocus += delegate ()
            {
                countersTextView.Stop();
                countersTextView.Text = "";
                top.SetFocus(processesWindow);
                Application.MainLoop.RemoveTimeout(timeoutToken);
            };
            processesListView.Select += delegate (Process process)
            {
                detailsTextView.Text = process.FormatDetailsAsString();
                countersTextView.Start(process);
                detailsTextView.SetNeedsDisplay();
                top.SetFocus(detailsTextView);
                timeoutToken = Application.MainLoop.AddTimeout(TimeSpan.FromSeconds(1), (eventLoop) =>
                {
                    countersTextView.Update();
                    return true;
                });
            };
            top.Add(processesWindow,
                    countersWindows,
                    detailsWindow,
                    StatusBarView.Create());

            return top;
        }
    }
}