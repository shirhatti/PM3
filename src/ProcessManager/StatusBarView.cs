using System;
using System.Collections.Generic;
using System.Text;
using Terminal.Gui;

namespace ProcessManager
{
    internal class StatusBarView : View
    {
        public StatusBarView() : base()
        { }

        public static StatusBarView Create()
        {
            var statusBar = new StatusBarView()
            {
                X = 0,
                Y = Pos.AnchorEnd(1),
                Width = Dim.Fill(),
                Height = 1
            };
            statusBar.Add(
                new Label(" ^C - Quit | ^R - Refresh process list | Enter - Show details | Esc - Return to process list")
            );
            return statusBar;
        }
    }
}
