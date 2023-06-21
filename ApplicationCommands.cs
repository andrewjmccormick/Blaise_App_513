using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Blaise_App {

    /// <summary>
    /// Custom Application commands 
    /// </summary>
    public static class ApplicationCommands {

        static ApplicationCommands() {
            ShowSettings = new RoutedUICommand("Settings", "ShowSettingsCommand", (typeof(Window)));
            Refresh = new RoutedUICommand("Refresh", "RefreshCommand", (typeof(Window)));
            Refresh.InputGestures.Add(new KeyGesture(Key.F5, ModifierKeys.None));
            //InstallSurvey = new RoutedUICommand("Install", "InstallSurveyCommand", (typeof(Window)));
            //RemoveSurvey = new RoutedUICommand("remove", "RemoveSurveyCommand", (typeof(Window)));
            StartSurvey = new RoutedUICommand("Start", "StartSurveyCommand", (typeof(Window)));
     

            StartSurvey.InputGestures.Add(new KeyGesture(Key.Enter, ModifierKeys.None));
            ShowSurveyDetails = new RoutedUICommand("SurveyDetails", "ShowSurveyDetailsCommand", (typeof(Window)));
            //DownloadCases = new RoutedUICommand("Download Cases", "DownloadCasesCommand", (typeof(Window)));
            //UploadSurveyData = new RoutedUICommand("Upload Data", "UploadSurveyDataCommand", (typeof(Window)));
        }


        public static RoutedUICommand ShowSettings { get; private set; }
        //public static RoutedUICommand InstallSurvey { get; private set; }
        //public static RoutedUICommand RemoveSurvey { get; private set; }
        public static RoutedUICommand StartSurvey { get; private set; }
    

        public static RoutedUICommand ShowSurveyDetails { get; private set; }
        public static RoutedUICommand Refresh { get; private set; }
        //public static RoutedUICommand DownloadCases { get; private set; }
        //public static RoutedUICommand UploadSurveyData { get; private set; }

    }
}
