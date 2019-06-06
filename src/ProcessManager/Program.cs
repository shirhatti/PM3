using System;
using Terminal.Gui;

namespace ProcessManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Application.Init();
            Console.CancelKeyPress += delegate (object sender, ConsoleCancelEventArgs e)
            {
                e.Cancel = true;
                Application.RequestStop();
            };
            var top = Application.Top;
            top.Add(WindowHelper.Create());
            Application.Run();
        }
    }
}
