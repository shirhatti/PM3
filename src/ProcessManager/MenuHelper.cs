using System;
using Terminal.Gui;

namespace ProcessManager
{
    internal class MenuHelper
    {
        internal static View Create()
        {
            return new MenuBar (new MenuBarItem [] {
                new MenuBarItem ("_Edit", new MenuItem [] {
                    new MenuItem ("_Copy", "", null),
                    new MenuItem ("C_ut", "", null),
                    new MenuItem ("_Paste", "", null)
                })
            });
        }
    }
}