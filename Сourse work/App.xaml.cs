using System;
using System.Windows;
using DenisovArt_Kurs.ProgramClasses;

namespace DenisovArt_Kurs
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            LogSaver.AppStartup();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            LogSaver.AppExit();
        }


        private void Application_UnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            LogSaver.CatchUnhandledException(sender, e);
        }
    }
}
