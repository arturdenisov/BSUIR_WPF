using System;
using System.Windows;
using System.Xaml;

namespace Lab2_2
{
    class Program : Application
    {
        [STAThread]
        static void Main()
        {
            Application Program = new Application();
            Program.Run(new InterfaceCalculator());
        }
    }
}

