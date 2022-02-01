using System;
using System.IO;

namespace DenisovArt_Kurs.ProgramClasses
{
    static class LogSaver
    {
        public static void AppStartup()
        {
            EditTextFile("Open ");
        }

        public static void AppExit()
        {
            EditTextFile("Exit ");
        }

        public static void CatchUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            string message =
                "***************Exception. Description - BEGIN *********************\n "
                + "Sender: " + sender.ToString()
                + "\nDescription: \n "
                + e.Exception.ToString()
                + "\n***************Exception. Description - END   *********************\n"
                + "Error time: ";
            EditTextFile(message);
        }

        private static void EditTextFile(string loggedEvent)
        {
            string filePath = @"D:\Log_DenisovAY.txt";
            string logMessage = loggedEvent + DateTime.Now.ToString("h:mm:ss tt");
            if (!File.Exists(filePath))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(filePath))
                {
                    sw.WriteLine(logMessage);
                }
            }
            else
            {
                // Open the file to append a text from.
                using (StreamWriter sw = File.AppendText(filePath))
                {
                    sw.WriteLine(logMessage);
                }
            }
        }
    }
}
