using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;

namespace Blaise_App {

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {

        /// <summary>
        /// Handle exception as a last resort
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e) {
            Trace.WriteLine(e.Exception.Message);
            try {
                var s = "Blaise StarterKit";
                var ev = "Unhandled exception";
                if (!EventLog.SourceExists(s)) {
                    EventLog.CreateEventSource(s, "Application");
                }
                EventLog.WriteEntry(s, ev, EventLogEntryType.Error);
                EventLog.WriteEntry(s, e.Exception.Message);
                if (e.Exception.InnerException != null) {
                    EventLog.WriteEntry(s, e.Exception.InnerException.Message);
                }
            } catch { }
            try {
                var mbr = MessageBox.Show("The App encountered an unexpected error.\nDo you want to continue the application?\n\n"+e.Exception.Message,
                    "Blaise StarterKit", MessageBoxButton.YesNo, MessageBoxImage.Question);
                e.Handled = true;
                if (mbr == MessageBoxResult.No || mbr == MessageBoxResult.Cancel)
                    this.Shutdown(1);
            } catch {
                Trace.WriteLine("Application will be closed. Exception could not be handled");
            }

        }
        private void Application_Exit(object sender, ExitEventArgs e)
        {
           
            if (System.IO.File.Exists("c:\\b4menu\\WPFManimenuBlaise4.exe"))
            {
                Process.Start("c:\\b4menu\\WPFManimenuBlaise4.exe");
            }
            if (System.IO.File.Exists("c:\\Blaise5Controller513\\Blaise5Controller513.exe"))
            {
                Process.Start("c:\\Blaise5Controller513\\Blaise5Controller513.exe");
            }
            if (System.IO.File.Exists("C:\\Users\\1375455\\source\\repos\\WPFManimenuBlaise4\\WPFManimenuBlaise4\\bin\\x86\\Release\\WPFManimenuBlaise4.exe"))
            {
                Process.Start("C:\\Users\\1375455\\source\\repos\\WPFManimenuBlaise4\\WPFManimenuBlaise4\\bin\\x86\\Release\\WPFManimenuBlaise4.exe");
            }
            


        }

    }
}
