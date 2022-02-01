using System;
using System.Windows;


namespace Lab1
{
    class Program : Application
    {
        [STAThread]
        static void Main()
        {
            Program Progr = new Program();
            Progr.Run(new MainWindow());
        }
    }
}

