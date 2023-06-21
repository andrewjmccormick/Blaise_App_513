using StatNeth.Blaise.API.DataEntry;
using StatNeth.Blaise.API.DataEntryWpf;
using System;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using System.Security;
using MetaAPI = StatNeth.Blaise.API.Meta;
using DataRecordAPI = StatNeth.Blaise.API.DataRecord;
using DataLinkAPI = StatNeth.Blaise.API.DataLink;
using System.Threading;
using System.Data;
using System.Windows.Controls;
using System.Windows.Media;
using System.Device.Location;
using System.Configuration;
using System.Threading.Tasks;
using System.Diagnostics;


namespace Blaise_App
{
    internal partial class AppController
    {

        public static class Globals
        {
            public static bool vardelete = true;
            public static IInstrumentInfo selected_instrument = null;
            public static string LooperEnvironment = " ";
        }

        public void Looper(IEnumerable<IInstrumentInfo> data, bool fullsync, string LooperEnvironment)
        {

            try
            {
                var proc = new Process();
                string Filelocation1 = "C:\\Users\\1150276\\source\\repos\\Blaise5Controller513\\bin\\x64\\Release\\Blaise5Controller513.exe";
                string Filelocation2 = "c:\\Blaise5Controller513\\Blaise5Controller513.exe";
                if (System.IO.File.Exists(Filelocation1))
                {
                    proc.StartInfo.FileName = Filelocation1;
                    proc.Start();
                    proc.WaitForExit();
                    var exitCode = proc.ExitCode;
                    proc.Close();
                }
                if (System.IO.File.Exists(Filelocation2))
                {
                    proc.StartInfo.FileName = Filelocation2;
                    proc.Start();
                    proc.WaitForExit();
                    var exitCode = proc.ExitCode;
                    proc.Close();
                }

            }
            catch { }
            AllLoopereventscompleted = 1;
        }



        public int AllLoopereventscompleted
        {
            get { return _allLoopereventscompleted; }
            set
            {
                _allLoopereventscompleted = value;
                if (_allLoopereventscompleted == 1)
                {


                    if (AppController.Globals.LooperEnvironment == "RESEARCH")
                    {
                        //MessageBox.Show(string.Format("Completed {0}, Connected to Surveys: {1}, Connected to Respond {2}", AppController.Globals.LooperEnvironment, AppController._instance._connectedtoSurveys, AppController._instance._connectedtoRespond));
                        string SyncLocation = Properties.Settings.Default.DeployFolder + "\\synctime.txt";
                        string SyncLocationFull = Properties.Settings.Default.DeployFolder + "\\synctimeFull.txt";

                        if (System.IO.File.Exists(SyncLocation))
                        {
                            System.IO.File.Delete(SyncLocation);
                        }
                        if (System.IO.File.Exists(SyncLocationFull))
                        {
                            System.IO.File.Delete(SyncLocationFull);
                        }
                        DateTime Syncdetails = DateTime.Now;
                        string strDate = Syncdetails.ToShortDateString();
                        string strTime = Syncdetails.ToShortTimeString();

                        string syncstr = "Last sync: " + strDate + " at " + strTime;
                        _syncronisationdetails = syncstr;
                        File.WriteAllText(SyncLocation, syncstr);
                        File.WriteAllText(SyncLocationFull, Syncdetails.ToString());
                    }
                    else if (AppController.Globals.LooperEnvironment == "RESPOND")
                    {
                        string SyncLocation = Properties.Settings.Default.DeployFolderRespond + "\\synctime.txt";
                        string SyncLocationFull = Properties.Settings.Default.DeployFolderRespond + "\\synctimeFull.txt";

                        if (System.IO.File.Exists(SyncLocation))
                        {
                            System.IO.File.Delete(SyncLocation);
                        }
                        if (System.IO.File.Exists(SyncLocationFull))
                        {
                            System.IO.File.Delete(SyncLocationFull);
                        }
                        DateTime Syncdetails = DateTime.Now;
                        string strDate = Syncdetails.ToShortDateString();
                        string strTime = Syncdetails.ToShortTimeString();

                        string syncstr = "Last sync: " + strDate + " at " + strTime;
                        _syncronisationdetails = syncstr;
                        File.WriteAllText(SyncLocation, syncstr);
                        File.WriteAllText(SyncLocationFull, Syncdetails.ToString());
                    }
                    //If only Surveys.nisra.gov.uk exists then go ahead and repaint buttons

                    AppController.Instance.SetData(AppController.Instance.InstalledonSurveys.Instruments, true, "C:\\B5RESEARCH");


                    if ((AppController.Globals.LooperEnvironment == "RESEARCH") && (AppController._instance._connectedtoRespond == false))
                    {
                        //MessageBox.Show(this.Window, string.Format("{0}", syncstr), this.Window.Title, MessageBoxButton.OK);
                        ev_repaint_buttons();
                        _synctime = DateTime.Now;
                    }
                    else if ((AppController.Globals.LooperEnvironment == "RESEARCH") && (AppController._instance._connectedtoRespond == true))
                    {
                        //MessageBox.Show(string.Format("Running Looper now for Respond {0}", AppController.Globals.LooperEnvironment), this.Window.Title, MessageBoxButton.OK);
                        //MessageBox.Show(string.Format("Running Looper now for Respond {0}", AppController.Globals.LooperEnvironment));

                        bool connected = AppController.Instance.Connect("C:\\B5Respond", "respond.nisra.gov.uk", false, true);
                        //bool connected = AppController.Instance.Connect("C:\\B5Surveys", "surveys.nisra.gov.uk", false);
                        if (connected == true)
                        {
                            AppController.Instance.Looper(AppController.Instance.InstalledonRespond.Instruments, false, "RESPOND");
                        }
                    }
                    //If you get here then either there is no Surveys environment or you've already synced it - so go ahead and repaint buttons
                    else if (AppController.Globals.LooperEnvironment == "RESPOND")
                    {
                        //MessageBox.Show(this.Window, string.Format("{0}", syncstr), this.Window.Title, MessageBoxButton.OK);
                        ev_repaint_buttons();
                        _synctime = DateTime.Now;
                    }

                }
            }
        }
        private int _allLoopereventscompleted;
    }
}
