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
            
            _noofsyncevents = 0;
            _synceventscounter = 0;
            AllLoopereventscompleted = 0;
            string logstr = "";
            string LogDetails = "";
            if (LooperEnvironment == "SURVEYS")
            {
                LogDetails = Properties.Settings.Default.DeployFolder + "\\logfile.txt";
                AppController.Globals.LooperEnvironment = "SURVEYS";
                //MessageBox.Show(string.Format("In Looper :{0}, {1}, {2}", LooperEnvironment, AppController.Globals.LooperEnvironment, LogDetails));
            }
            else if (LooperEnvironment == "RESPOND")
            {
                LogDetails = Properties.Settings.Default.DeployFolderRespond + "\\logfile.txt";
                AppController.Globals.LooperEnvironment = "RESPOND";
                //MessageBox.Show(string.Format("In Looper :{0}, {1}, {2}", LooperEnvironment, AppController.Globals.LooperEnvironment, LogDetails));
            }
            //MessageBox.Show(string.Format("No. of surveys :{0}", data.Count()));
            if (data.Count() > 0)
               
            {
                //================================================================================
                // This section counts how many events are going to happen
                //================================================================================
                //if (fullsync == true)
                //{
                //    IEnumerable<IInstrumentInfo> Ax =
                //    data.Where(a => a.ServerInstallDate.Ticks == 0);
                //    foreach (IInstrumentInfo A in Ax)
                //    {
                //        _noofsyncevents = _noofsyncevents + 1;
                //        //MessageBox.Show(string.Format("Noofsyncevents:{0}  Synceventcounter:{1}  Where: Remove", _noofsyncevents, _synceventscounter));
                //    }
                //    IEnumerable<IInstrumentInfo> Bx =
                //           data.Where(b => b.InstrumentState == InstrumentState.InstalledOnServer);
                //    foreach (IInstrumentInfo B in Bx)
                //    {
                //        _noofsyncevents = _noofsyncevents + 1;
                //        //MessageBox.Show(string.Format("Noofsyncevents:{0}  Synceventcounter:{1}  Where: Install", _noofsyncevents, _synceventscounter));
                //    }
                //}
                IEnumerable<IInstrumentInfo> Cx =
                data.Where(c => c.InstrumentState == InstrumentState.InstalledOnClient); // && c.ServerInstallDate.Ticks != 0);
                foreach (IInstrumentInfo C in Cx)
                {
                    long? count = InstrumentManager.GetLocalDataCount(C);
                    if (count.HasValue && count > 0)
                    {
                        _noofsyncevents = _noofsyncevents + 1;
                        //MessageBox.Show(string.Format("Noofsyncevents:{0}  Synceventcounter:{1}   Where: Upload", _noofsyncevents, _synceventscounter));
                    }
                }
                IEnumerable<IInstrumentInfo> Dx =
                data.Where(d => d.InstrumentState == InstrumentState.InstalledOnClient);// && d.ServerInstallDate.Ticks != 0);
                foreach (IInstrumentInfo D in Dx)
                {
                    _noofsyncevents = _noofsyncevents + 1;
                    //MessageBox.Show(string.Format("Noofsyncevents:{0}  Synceventcounter:{1}  Where: Download", _noofsyncevents, _synceventscounter));
                }




                //================================================================================
                // This section uploads data from installed surveys
                //================================================================================
                String Username = GetUsername();
                IEnumerable<IInstrumentInfo> surveysdatatoupload =
                //Changed this next line to allow uploads attempts of all surveys installed locally - even if server connection not there
                data.Where(s => s.InstrumentState == InstrumentState.InstalledOnClient); // && s.ServerInstallDate.Ticks != 0);

                foreach (IInstrumentInfo surveydatatoupload in surveysdatatoupload)
                {
                    long? count = InstrumentManager.GetLocalDataCount(surveydatatoupload);
                    if (count.HasValue && count > 0)
                    {


                        if (((surveydatatoupload.Name.Length >= 8) && (surveydatatoupload.Name.ToUpper().Substring(0, 8) == "PROGRESS")) | ((surveydatatoupload.Name.Length >= 11) && (surveydatatoupload.Name.ToUpper().Substring(0, 11) == "PERFORMANCE")) | ((surveydatatoupload.Name.Length >= 4) && (surveydatatoupload.Name.ToUpper().Substring(0, 4) == "NIPS")) | ((surveydatatoupload.Name.Length >= 10) && (surveydatatoupload.Name.ToUpper().Substring(0, 10) == "TRAINEEAPP")) | ((surveydatatoupload.Name.Length >= 8) && (surveydatatoupload.Name.ToUpper().Substring(0, 8) == "SHIFTDET")))
                        {

                            Console.WriteLine("In upload data section cases to upload");

                            try
                            {
                                InstrumentManager.UploadData(surveydatatoupload);
                                DateTime LogTime = DateTime.Now;
                                logstr = logstr + " " + LogTime + " " + surveydatatoupload.Name + " data successfully uploaded (App)" + Environment.NewLine;
                            }
                            catch
                            {
                                //MessageBox.Show(string.Format("There was an error uploading data for the survey: {0} \n Please contact the office with this error message", surveydatatoupload.Name));
                                DateTime LogTime = DateTime.Now;
                                logstr = logstr + " " + LogTime + " " + surveydatatoupload.Name + " data FAILED to uploaded (App)" + Environment.NewLine;
                            }

                            _synceventscounter = _synceventscounter + 1;
                            //MessageBox.Show(string.Format("Noofsyncevents:{0}  Synceventcounter:{1}  Where: Upload", _noofsyncevents, _synceventscounter));
                            if (_noofsyncevents == _synceventscounter)
                            { AllLoopereventscompleted = 1; }
                        }
                        else
                        {
                            Console.WriteLine("In upload data section cases to upload");
                            try
                            {
                                string RecordFilterupload = "intno=" + Username;
                                InstrumentManager.UploadData(surveydatatoupload, RecordFilterupload);
                                DateTime LogTime = DateTime.Now;
                                logstr = logstr + " " + LogTime + " " + surveydatatoupload.Name + " data successfully uploaded (App)" + Environment.NewLine;
                            }
                            catch
                            {
                                //MessageBox.Show(string.Format("There was an error uploading data for the survey: {0} \n Please contact the office with this error message", surveydatatoupload.Name));
                                DateTime LogTime = DateTime.Now;
                                logstr = logstr + " " + LogTime + " " + surveydatatoupload.Name + " data FAILED to uploaded (App)" + Environment.NewLine;

                            }
                            _synceventscounter = _synceventscounter + 1;

                            if (_noofsyncevents == _synceventscounter)
                            { AllLoopereventscompleted = 1; }
                        }

                    }
                }
                //================================================================================
                // This section downloads cases to existing surveys surveys
                //================================================================================

                IEnumerable<IInstrumentInfo> SurveysCasestoDownload =
                data.Where(t => t.InstrumentState == InstrumentState.InstalledOnClient);// && t.ServerInstallDate.Ticks != 0);
                foreach (IInstrumentInfo SurveyCasestoDownload in SurveysCasestoDownload)
                {
                    if (((SurveyCasestoDownload.Name.Length >= 8) && (SurveyCasestoDownload.Name.ToUpper().Substring(0, 8) == "PROGRESS")) | ((SurveyCasestoDownload.Name.Length >= 11) && (SurveyCasestoDownload.Name.ToUpper().Substring(0, 11) == "PERFORMANCE")) | ((SurveyCasestoDownload.Name.Length >= 10) && (SurveyCasestoDownload.Name.ToUpper().Substring(0, 10) == "TRAINEEAPP")))
                    {

                        try
                        {
                            string RecordFilterProgress = "FT_Num=" + Username;
                            InstrumentManager.DownloadCases(SurveyCasestoDownload, RecordFilterProgress, false);
                        }
                        catch
                        {
                            // MessageBox.Show(string.Format("There was an error downloading cases for the survey: {0} \n Please contact the office with this error message", SurveyCasestoDownload.Name));

                        }
                        _synceventscounter = _synceventscounter + 1;
                        //MessageBox.Show(string.Format("Noofsyncevents:{0}  Synceventcounter:{1}  Where: Download Progress", _noofsyncevents, _synceventscounter));
                        if (_noofsyncevents == _synceventscounter)
                        { AllLoopereventscompleted = 1; }
                    }
                    else if ((SurveyCasestoDownload.Name.Length >= 4) && (SurveyCasestoDownload.Name.ToUpper().Substring(0, 4) == "NIPS"))
                    {
                        try
                        {
                            string RecordFilter = "admin1.authno=" + Username + "or admin1.authno=9999";
                            InstrumentManager.DownloadCases(SurveyCasestoDownload, RecordFilter, false);
                        }
                        catch
                        {
                            //MessageBox.Show(string.Format("There was an error downloading cases for the survey: {0} \n Please contact the office with this error message", SurveyCasestoDownload.Name));

                        }

                        _synceventscounter = _synceventscounter + 1;
                        //MessageBox.Show(string.Format("Noofsyncevents:{0}  Synceventcounter:{1}  Where: Download surv", _noofsyncevents, _synceventscounter));
                        if (_noofsyncevents == _synceventscounter)
                        { AllLoopereventscompleted = 1; }
                    }
                    else if ((SurveyCasestoDownload.Name.Length >= 8) && (SurveyCasestoDownload.Name.ToUpper().Substring(0, 8) == "SHIFTDET"))
                    {
                        try
                        {
                            string RecordFilter = "TeamL=" + Username;
                            InstrumentManager.DownloadCases(SurveyCasestoDownload, RecordFilter, false);
                        }
                        catch
                        {
                            // MessageBox.Show(string.Format("There was an error downloading cases for the survey: {0} \n Please contact the office with this error message", SurveyCasestoDownload.Name));

                        }

                        _synceventscounter = _synceventscounter + 1;
                        //MessageBox.Show(string.Format("Noofsyncevents:{0}  Synceventcounter:{1}  Where: Download surv", _noofsyncevents, _synceventscounter));
                        if (_noofsyncevents == _synceventscounter)
                        { AllLoopereventscompleted = 1; }
                    }
                    else
                    {
                        try
                        {
                            string RecordFilter = "intno=" + Username + "or intno=9999";
                            InstrumentManager.DownloadCases(SurveyCasestoDownload, RecordFilter, false);
                        }
                        catch
                        {
                            //MessageBox.Show(string.Format("There was an error downloading cases for the survey: {0} \n Please contact the office with this error message", SurveyCasestoDownload.Name));

                        }
                        _synceventscounter = _synceventscounter + 1;
                        //MessageBox.Show(string.Format("Noofsyncevents:{0}  Synceventcounter:{1}  Where: Download surv", _noofsyncevents, _synceventscounter));
                        if (_noofsyncevents == _synceventscounter)
                        { AllLoopereventscompleted = 1; }
                    }
                }

                //if (fullsync == true)
                //{
                //    //=================================================================================
                //    // This section removes surveys from the Client it they don't exist on the Server
                //    //=================================================================================
                //    //Enumerate the available Instruments that the App can see


                //    IEnumerable<IInstrumentInfo> SurveystoRemove =
                //    data.Where(z => z.ServerInstallDate.Ticks == 0);
                //    foreach (IInstrumentInfo SurveytoRemove in SurveystoRemove)
                //    {
                //        try
                //        {
                //            InstrumentManager.RemoveInstrument(SurveytoRemove);
                //            DateTime LogTime = DateTime.Now;
                //            logstr = logstr + " " + LogTime + " " + SurveytoRemove.Name + " successfully removed (App)" + Environment.NewLine;
                //        }
                //        catch
                //        {
                //            // MessageBox.Show(string.Format("There was an error removing the survey: {0} \n Please contact the office with this error message", SurveytoRemove.Name));
                //            DateTime LogTime = DateTime.Now;
                //            logstr = logstr + " " + LogTime + " " + SurveytoRemove.Name + " FAILED to remove (App)" + Environment.NewLine;
                //        }
                //        _synceventscounter = _synceventscounter + 1;

                //        if (_noofsyncevents == _synceventscounter)
                //        { AllLoopereventscompleted = 1; }
                //    }

                //    //================================================================================
                //    // This section installs available surveys
                //    //================================================================================
                //    IEnumerable<IInstrumentInfo> SurveystoInstall =
                //        data.Where(u => u.InstrumentState == InstrumentState.InstalledOnServer);
                //    foreach (IInstrumentInfo SurveytoInstall in SurveystoInstall)
                //    {
                //        try
                //        {
                //            InstrumentManager.DownloadAndInstallInstrument(SurveytoInstall, false);
                //            DateTime LogTime = DateTime.Now;
                //            logstr = logstr + " " + LogTime + " " + SurveytoInstall.Name + " successfully installed (App)" + Environment.NewLine;

                //        }
                //        catch
                //        {
                //            // MessageBox.Show(string.Format("There was an error installing the survey: {0} \n Please contact the office with this error message", SurveytoInstall.Name));
                //            DateTime LogTime = DateTime.Now;
                //            logstr = logstr + " " + LogTime + " " + SurveytoInstall.Name + " FAILED to install (App)" + Environment.NewLine;

                //        }

                //    }
                //    InstrumentManager.OnInstrumentInstalled += InstrumentManagerInstance_OnInstrumentInstalled;
                //}
                if (_noofsyncevents == 0)
                {
                    AllLoopereventscompleted = 1;
                }

            }// end (data.Count() > 0)
            else if (data.Count() == 0)
            {
                MessageBox.Show(string.Format("No instruments"));
                DateTime LogTime = DateTime.Now;
                logstr = logstr + " " + LogTime + " No events to synchronise." + Environment.NewLine;
                AllLoopereventscompleted = 1;
                //if (_noofsyncevents == _synceventscounter)
                //{ AllLoopereventscompleted = 1; }
            }
            //Refresh();
            File.AppendAllText(LogDetails, logstr);
            //MessageBox.Show(string.Format("{0}, {1}", LogDetails, logstr));
        } //end of Looper

        private void InstrumentManagerInstance_OnInstrumentInstalled(object sender, InstrumentInstalledEventArgs e)
        {
            IInstrumentInfo installed = e.Instrument;
            String Username = AppController.Instance.GetUsername();
            //*****************************************************************************************************************************
            try
            {
                string FileLocation = " ";
                if (AppController.Globals.LooperEnvironment == "SURVEYS")
                {
                    FileLocation = Properties.Settings.Default.DeployFolder + "\\" + Username + "\\surveys\\" + installed.Name + "\\remote.config2";
                }
                else if (AppController.Globals.LooperEnvironment == "RESPOND")
                {
                    FileLocation = Properties.Settings.Default.DeployFolderRespond + "\\" + Username + "\\surveys\\" + installed.Name + "\\remote.config2";
                }

                string str = File.ReadAllText(FileLocation);
                System.IO.File.Delete(FileLocation);
                str = str.Replace(">http<", ">https<");
                File.WriteAllText(FileLocation, str);
            }
            catch { }
            //*****************************************************************************************************************************

           
            if (((installed.Name.Length >= 8) && (installed.Name.ToUpper().Substring(0, 8) == "PROGRESS")) | ((installed.Name.Length >= 11) && (installed.Name.ToUpper().Substring(0, 11) == "PERFORMANCE")) | ((installed.Name.Length >= 10) && (installed.Name.ToUpper().Substring(0, 10) == "TRAINEEAPP")))
            {
                Console.WriteLine("In download cases section");
                try
                {
                    string RecordFilterProgress = "FT_Num=" + Username;
                    InstrumentManager.DownloadCases(installed, RecordFilterProgress, false);
                }
                catch
                {
                    //MessageBox.Show(string.Format("There was an error downloading cases for the survey: {0} \n Please contact the office with this error message", installed.Name));

                }
                _synceventscounter = _synceventscounter + 1;
                //MessageBox.Show(string.Format("Noofsyncevents:{0}  Synceventcounter:{1}  Where: Install Progress {2}", _noofsyncevents, _synceventscounter, installed.Name));
                if (_noofsyncevents == _synceventscounter)
                { AllLoopereventscompleted = 1; }
            }
            else if ((installed.Name.Length >= 4) && (installed.Name.ToUpper().Substring(0, 4) == "NIPS"))
            {
                try
                {
                    string RecordFilter = "admin1.authno=" + Username + "or admin1.authno=9999";
                    InstrumentManager.DownloadCases(installed, RecordFilter, false);
                }
                catch
                {
                    // MessageBox.Show(string.Format("There was an error downloading cases for the survey: {0} \n Please contact the office with this error message", installed.Name));

                }

                _synceventscounter = _synceventscounter + 1;
                //MessageBox.Show(string.Format("Noofsyncevents:{0}  Synceventcounter:{1}  Where: Download surv", _noofsyncevents, _synceventscounter));
                if (_noofsyncevents == _synceventscounter)
                { AllLoopereventscompleted = 1; }
            }
            else if ((installed.Name.Length >= 8) && (installed.Name.ToUpper().Substring(0, 8) == "SHIFTDET"))
            {
                try
                {
                    string RecordFilter = "TeamL=" + Username;
                    InstrumentManager.DownloadCases(installed, RecordFilter, false);
                }
                catch
                {
                    //MessageBox.Show(string.Format("There was an error downloading cases for the survey: {0} \n Please contact the office with this error message", installed.Name));

                }

                _synceventscounter = _synceventscounter + 1;
                //MessageBox.Show(string.Format("Noofsyncevents:{0}  Synceventcounter:{1}  Where: Download surv", _noofsyncevents, _synceventscounter));
                if (_noofsyncevents == _synceventscounter)
                { AllLoopereventscompleted = 1; }
            }
            else
            {
                Console.WriteLine("In download cases section");
                try
                {
                    string RecordFilter = "intno=" + Username + " or intno=9999";
                    InstrumentManager.DownloadCases(installed, RecordFilter, false);
                }
                catch
                {
                    //MessageBox.Show(string.Format("There was an error downloading cases for the survey: {0} \n Please contact the office with this error message", installed.Name));

                }

                _synceventscounter = _synceventscounter + 1;
                //MessageBox.Show(string.Format("Noofsyncevents:{0}  Synceventcounter:{1}  Where: Install surv {2}", _noofsyncevents, _synceventscounter,installed.Name));
                if (_noofsyncevents == _synceventscounter)
                { AllLoopereventscompleted = 1; }
            }
        }

        public int AllLoopereventscompleted
        {
            get { return _allLoopereventscompleted; }
            set
            {
                _allLoopereventscompleted = value;
                if (_allLoopereventscompleted == 1)
                {


                    if (AppController.Globals.LooperEnvironment == "SURVEYS")
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

                    AppController.Instance.SetData(AppController.Instance.InstalledonSurveys.Instruments, true, "C:\\B5SURVEYS");


                    if ((AppController.Globals.LooperEnvironment == "SURVEYS") && (AppController._instance._connectedtoRespond == false))
                    {
                        //MessageBox.Show(this.Window, string.Format("{0}", syncstr), this.Window.Title, MessageBoxButton.OK);
                        ev_repaint_buttons();
                        _synctime = DateTime.Now;
                    }
                    else if ((AppController.Globals.LooperEnvironment == "SURVEYS") && (AppController._instance._connectedtoRespond == true))
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
