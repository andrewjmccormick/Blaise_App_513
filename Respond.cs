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
    internal class AppControllerRespond
    {
        public delegate void dg_casesloaded();
        public event dg_casesloaded ev_repaint_buttons;

        #region Constructors
        private static AppControllerRespond _instance;


        private AppControllerRespond()
        {
            this.InstrumentManager = new InstrumentManager();
            //this.InstrumentManager.OnInstrumentInfoLoaded += InstrumentManager_OnInstrumentInfoLoadedRespond;

            //this.ev_repaint_buttons += new dg_casesloaded(Window.surveysMain.EventMessage);

        }

        internal static AppControllerRespond Instance
        {
            get { return _instance; }
        }
        internal static AppControllerRespond Create(MainWindow window)
        {
            _instance = new AppControllerRespond();
            return _instance;
        }
        #endregion
        internal InstrumentManager InstrumentManager { get; private set; }
        internal MainWindow Window { private set; get; } // Keep track of the Main Window
        internal Dictionary<string, string> RuntimeParameters = new Dictionary<string, string>();
        public IInstrumentInfo _selectedInstrument; // In Listview selected instrument
        public IInstrumentInfo _surveyinstrument; // IInstrumentinfor of the Progress questionnaire
        public IInstrumentInfo _progressinstrument; // IInstrumentinfor of the Progress questionnaire
        public IInstrumentInfo _performanceinstrument; // IInstrumentinfor of the Progress questionnaire
        public IInstrumentInfo _traineeappinstrument; // IInstrumentinfor of the Progress questionnaire
        public IInstrumentInfo _shiftdetailsinstrument; // IInstrumentinfor of the Progress questionnaire
        public IInstrumentInfo _NIPSMenuinstrument; // IInstrumentinfor of the NIPS surveys
        public IInstrumentInfo _allocationsinstrument; // IInstrumentinfor of the First Offer of Work

        public InstrumentInfoLoadedEventArgs InstalledSurveys = new InstrumentInfoLoadedEventArgs();

        public string _primkey; // In Listview selected instrument
        public string _serno; // In Listview selected instrument
        public string _hhno; // In Listview selected instrument
        public string _intno; // In Listview selected instrument
        public string _site; // In Listview selected instrument
        public bool _casesloaded; //Computed to 1 when cases are loaded in 
        public DateTime _intdate; // In Listview selected instrument
        public string _syncronisationdetails; //Details of the last time synchronisation was completed
        public DateTime _synctime; //Records last time a sync was done to enable no more than one automatic sync per hour 
        public double _txtLat; //Latitude of current location
        public double _txtLong; //Latitude of current location
        public int _noofsyncevents; //Counter of the number of events Looper will carry out
        public int _synceventscounter; //Counter of the number of events Looper will carry out
        public string _intPass; //copy of what interviewer types in the password box
        public DateTime _lastruntime; //stores last connect time to ensure that it doewsn't double load at launch of programme
        public bool _alreadyconnectedonce = false; //Check if a connection has already been made
        public bool _connectedlocally; //check if RESPOND environment is available locally


        internal bool Refresh()
        {
            return AppControllerRespond.Instance.Connect();
        }

        private DataRecordAPI.IKey GetPrimaryKey(MetaAPI.IDatamodel dm)
        {
            // Get an IKey interface for the primary key:
            DataRecordAPI.IKey result = DataRecordAPI.DataRecordManager.GetKey(dm, MetaAPI.Constants.KeyNames.Primary);
            return result;
        }

        internal bool Connect()
        {
            
            //Configuration
            ClientPreferredBinding binding = AppController.Instance.GetBinding();
            String Username = AppController.Instance.GetUsername();
            SecureString Password = AppController.Instance.GetPassword();
            try
            {
                bool isConnected = InstrumentManager.Connect("respond.nisra.gov.uk", 8033, Username, Password, binding, 30, "tls12", "c:\\B5Respond");
                //bool isConnected = InstrumentManager.Connect(Properties.Settings.Default.RemoteHost, Properties.Settings.Default.Port, Properties.Settings.Default.UserName, Properties.Settings.Default.Password.ToSecureString(), binding);
                AppControllerRespond.Instance.InstrumentManager.GetAvailableInstruments();
                
                return isConnected;
            }
            catch (Exception)
            {
                //MessageBox.Show(this.Window, string.Format("There is a problem logging with the provided user name and password. Please contact the office."), this.Window.Title, MessageBoxButton.OK);
                return false;
            }
        }

        private void Wait5Seconds()
        {
            Stopwatch sw = new Stopwatch(); // sw cotructor
            sw.Start(); // starts the stopwatch
            for (int i = 0; ; i++)
            {
                if (i % 100000 == 0) // if in 100000th iteration (could be any other large number
                                     // depending on how often you want the time to be checked) 
                {
                    sw.Stop(); // stop the time measurement
                    if (sw.ElapsedMilliseconds > 5000) // check if desired period of time has elapsed
                    {
                        break; // if more than 5000 milliseconds have passed, stop looping and return
                               // to the existing code
                    }
                    else
                    {
                        sw.Start(); // if less than 5000 milliseconds have elapsed, continue looping
                                    // and resume time measurement
                    }
                }
            }

        }

       

        //internal bool ConnectLocally()
        //{

        //    //Configuration
           
        //    String Username = AppController.Instance.GetUsername();
        //    SecureString Password = AppController.Instance.GetPassword();
           
        //    try
        //    {
        //        bool isConnected = InstrumentManager.ConnectLocally(Username, Password, "C:\\B5Respond");
        //        if (isConnected == true)
        //        {

        //            MessageBox.Show(string.Format("Respond Connected "));
        //            AppControllerRespond.Instance.InstrumentManager.GetAvailableInstruments();
        //            AppControllerRespond.Instance._connectedlocally = true;

        //        }
        //        return isConnected;
        //    }
        //    catch (Exception)
        //    {
        //        //MessageBox.Show(this.Window, string.Format("There is a problem logging with the provided user name and password. Please contact the office."), this.Window.Title, MessageBoxButton.OK);
        //        return false;
        //    }
        //}

        public static class Globals
        {
            public static bool vardelete = true;
            public static IInstrumentInfo selected_instrument = null;
        }

        //internal async void InstrumentManager_OnInstrumentInfoLoadedRespond(object sender, InstrumentInfoLoadedEventArgs e)
        //{
        //    // Loads up all the cases into 
        //    //There are issues with threading as the main UI thread is not the one doing the updating of the observable collections each time data upload is triggered


        //    InstalledSurveys = e;

        //    MessageBox.Show(string.Format("About to call SetData from RESPONDRESPOND. Number of Instruments: {0}", e.Instruments.Count()));


        //    await Task.Delay(1000);
        //    if (e != null)
        //    {
        //        //         Blaise_App.AppController.Instance.Window.Dispatcher.Invoke((Action)delegate
        //        //         {

        //        AppController.Instance.SetData(e.Instruments, false, "RESPOND");


        //        //    });

        //        //SetInstruments(e.Instruments);
        //        //InstalledSurveys = e;
        //    }
        //    //AppController.Instance.InstrumentInfoLoadedEventVariable = e;
        //    //this.Window.Dispatcher.Invoke(() => this.Window.surveysMain.SetData(e.Instruments,false));

        //    //this.Window.Dispatcher.Invoke(() => this.Window.surveysMain);


        //}






    }
}
