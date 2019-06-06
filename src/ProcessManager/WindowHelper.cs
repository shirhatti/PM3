using System;
using Terminal.Gui;

namespace ProcessManager
{
    internal class WindowHelper
    {
        internal static View Create()
        {
            return new Window ("MyApp") {
	            X = 0,
	            Y = 1, // Leave one row for the toplevel menu

                // By using Dim.Fill(), it will automatically resize without manual intervention
	            Width = Dim.Fill (),
	            Height = Dim.Fill ()
	        };
        }
    }
}