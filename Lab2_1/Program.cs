using System;
using System.Windows;

namespace Lab2_1
{
    class Program : Application
    {
        [STAThread]
        static void Main()
        {
            Application program = new Application();
            program.Run(new MainWindow());
        }
    }
}
