using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise_App {

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            //this next line removes the close button during the loading of the app
            this.WindowStyle = WindowStyle.None;
            InitializeComponent();
            AppController.Create(this);
            AppControllerRespond.Create(this);
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e) {

            string VersionLocation = "C:\\surveys\\Blaise_App_Version.txt";
            string VersionLocationtext = "Version 3.0.8";
            if (System.IO.File.Exists(VersionLocation))
            {
                System.IO.File.Delete(VersionLocation);
            }
            File.WriteAllText(VersionLocation, VersionLocationtext);
            //if (!AppController.Instance.Connect("C:\\B5SURVEYS", "SURVEYSx.NISRA.GOV.UK", true, false))
            if (!AppController.Instance.Connect("C:\\B5RESEARCH", "RESEARCH.NISRA.GOV.UK", true, false))
            {
                
                AppController.Instance._connectedtoSurveys = false;
                if (!AppController.Instance.Connect("C:\\B5RESPOND", "RESPONDx.NISRA.GOV.UK", true, false))
                //if (!AppController.Instance.Connect("C:\\B5RESPOND", "RESPOND.NISRA.GOV.UK", true, false))
                    {
                        AppController.Instance._connectedtoRespond = false;
                    string intcheck = AppController.Instance.GetUsername();
                    if (intcheck == "8888")//((intcheck == "489") | (intcheck == "612") | (intcheck == "1043") | (intcheck == "8888"))
                    {
                        //MessageBox.Show(this, "Could not connect to HQ, only the locally installed surveys will be displayed", this.Title, MessageBoxButton.OK, MessageBoxImage.Warning);
                        MessageBox.Show(this, "Synchronisation not working properly - only your existing work will be available. Please contact the office.", this.Title, MessageBoxButton.OK, MessageBoxImage.Warning);
                        
                        //Switch tese next 2 lines on if we want to allow users to access local data files (ie NIPS issues at ports when they have no signal)
                        //if (!AppController.Instance.ConnectLocallyr("C:\\B5SURVEYS",true))
                        //{ MessageBox.Show(this, "Synchronisation not working properly - only your existing work will be available. Please contact the office.", this.Title, MessageBoxButton.OK, MessageBoxImage.Warning); }
                        
                    }
                    //AppController.Instance.InstrumentManager.GetAvailableInstruments();

                }
               

            }
          
            
            this.WindowStyle = WindowStyle.None;
        }

        //private string GetUsername()
        //{
        //    string path = @"c:\id.bat";
        //    string usertxt = " ";
        //    if (System.IO.File.Exists(path))
        //    {
        //        Console.WriteLine("File exists");

        //        string line;
        //        // Read the text file contianing survey name and Guid of surveys to be deleted.  

        //        System.IO.StreamReader file =
        //        new System.IO.StreamReader(@"c:\id.bat");
        //        while ((line = file.ReadLine()) != null)
        //        {
        //            string[] parts = line.Split('T');
        //            usertxt = parts[3];
        //        }
        //        file.Close();

        //    }
        //    else
        //    {
        //        Console.WriteLine("File does not exists");
        //        usertxt = " ";
        //    }
        //    Console.WriteLine("User name {0}", usertxt);
        //    return usertxt;
        //}
        
    }
}
