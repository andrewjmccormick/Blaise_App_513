

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


namespace Blaise_App {

    /// <summary>
    /// Application Logic
    /// </summary>
    internal partial class AppController {
        public delegate void dg_casesloaded();
        public event dg_casesloaded ev_repaint_buttons;

        //comment
        //comment

   



        #region Constructors
        private static AppController _instance;
        

        
        
        
        private AppController(MainWindow window) {
            this.Window = window;
            this.InstrumentManager = new InstrumentManager();
            //this.InstrumentManager.OnInstrumentInfoLoaded += InstrumentManager_RunDataTransfer;
            this.InstrumentManager.OnInstrumentInfoLoaded += InstrumentManager_OnInstrumentInfoLoaded;
            //this.InstrumentManager.OnInstrumentInstalled += InstrumentManager_OnInstrumentInstalled;
            //this.InstrumentManager.OnConfigLoaded += InstrumentManager_OnConfigLoaded;
            // Add eventhandlers for application commands
           // this.Window.CommandBindings.Add(new CommandBinding(ApplicationCommands.ShowSurveyDetails, cmdShowSurveyDetails_Executed));
            this.Window.CommandBindings.Add(new CommandBinding(ApplicationCommands.StartSurvey, cmdStartSurvey_Executed));
          
            //            this.Window.CommandBindings.Add(new CommandBinding(ApplicationCommands.UploadSurveyData, cmdUploadSurveyData_Executed));
            //            this.Window.CommandBindings.Add(new CommandBinding(ApplicationCommands.InstallSurvey, cmdInstallSurvey_Executed));
            //            this.Window.CommandBindings.Add(new CommandBinding(ApplicationCommands.RemoveSurvey, cmdRemoveSurvey_Executed));
            //            this.Window.CommandBindings.Add(new CommandBinding(ApplicationCommands.DownloadCases, cmdDownloadCases_Executed));
          //  this.Window.CommandBindings.Add(new CommandBinding(ApplicationCommands.Refresh, cmdRefresh_Executed));
          //  this.Window.CommandBindings.Add(new CommandBinding(ApplicationCommands.ShowSettings, cmdShowSettings_Executed));
            this.Window.CommandBindings.Add(new CommandBinding(System.Windows.Input.ApplicationCommands.Close, cmdClose_Executed));
            this.ev_repaint_buttons += new dg_casesloaded(Window.surveysMain.EventMessage);
          

        }

        internal static AppController Instance {
            get { return _instance; }
        }
        internal static AppController Create(MainWindow window) {
            _instance = new AppController(window);
            return _instance;
        }
        #endregion

        internal InstrumentManager InstrumentManager { get; private set; }
        internal MainWindow Window { private set; get; } // Keep track of the Main Window
        internal Dictionary<string, string> RuntimeParameters = new Dictionary<string, string>();
        public IInstrumentInfo _selectedInstrument; // In Listview selected instrument
        public IInstrumentInfo _surveyinstrument; // IInstrumentinfor of the Progress questionnaire
        public IInstrumentInfo _progressinstrument; // IInstrumentinfor of the Progress questionnaire
        public IInstrumentInfo _currentwagesinstrument; // IInstrumentinfor of the Progress questionnaire
        public string _currentwageslocation; //records which environment Progress in installed on
        //public IInstrumentInfo _archivedwagesinstrument; // IInstrumentinfor of the Progress questionnaire
        //public string _archivedwageslocation; //records which environment Progress in installed on
        public IInstrumentInfo _expensesinstrument; // IInstrumentinfor of the Progress questionnaire
        public string _expenseslocation; //records which environment Progress in installed on
        public IInstrumentInfo _annualleaveinstrument; // IInstrumentinfor of the Progress questionnaire
        public string _annualleavelocation; //records which environment Progress in installed on

        public string _progresslocation; //records which environment Progress in installed on
        public IInstrumentInfo _performanceinstrument; // IInstrumentinfor of the Progress questionnaire
        public string _performancelocation; //records which environment Performance in installed on
        public IInstrumentInfo _traineeappinstrument; // IInstrumentinfor of the Progress questionnaire
        public string _traineeapplocation; //records which environment TraineeApp in installed on
        public IInstrumentInfo _shiftdetailsinstrument; // IInstrumentinfor of the Progress questionnaire
        public string _shiftdetailslocation; //records which environment ShiftDetails in installed on
        public IInstrumentInfo _NIPSMenuinstrument; // IInstrumentinfor of the NIPS surveys
        public IInstrumentInfo _allocationsinstrument; // IInstrumentinfor of the First Offer of Work
        public string _allocationslocation; //records which environment Allocations in installed on
        public bool _runSetData = true; //only run SetData when you have to 
        public InstrumentInfoLoadedEventArgs InstalledonSurveys = new InstrumentInfoLoadedEventArgs();
        public InstrumentInfoLoadedEventArgs InstalledonRespond = new InstrumentInfoLoadedEventArgs();

        public string _primkey; // In Listview selected instrument
        public string _serno; // In Listview selected instrument
        public string _hhno; // In Listview selected instrument
        public string _intno; // In Listview selected instrument
        public string _site; // In Listview selected instrument
        public bool _casesloadedSurveys = false; //Computed to true when cases are loaded in 
        public bool _casesloadedRespond = false; //Computed to true when cases are loaded in 
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
        public bool? _connectedtoSurveys; //check if SURVEYS environment is available locally
        public bool? _connectedtoRespond; //check if SURVEYS environment is available locally
        public string currentenvironment = "RESEARCH";
        public bool InstalledOnSurveysComputed = false;
        public bool InstalledOnRespondComputed = false;
        public DateTime _wageyearstartdate;

        //internal bool Refresh() {
        //    return AppController.Instance.Connect("C:\\B5SURVEYS", "SURVEYS.NISRA.GOV.UK");
        //}

        private void cmdClose_Executed(object sender, ExecutedRoutedEventArgs e) {
            this.Window.Close();
        }

  

//==================================================================================================================================================================================================================




        private void cmdStartSurvey_Executed(object sender, ExecutedRoutedEventArgs e) {
            e.Handled = true;
           // _selectedInstrument = GetSelectedInstrument(e).Instrument ;
            if (_selectedInstrument != null) {
                ILoadBalancer loadbalancer = DataEntryManager.GetLoadBalancer(_selectedInstrument.RunMode);
                loadbalancer.ServerParkObtained += Loadbalancer_ServerParkObtained;
                if (string.IsNullOrEmpty(_selectedInstrument.Host))
                    _selectedInstrument.Host = "localhost";
                loadbalancer.RequestServerPark(string.Format("http://{0}:{1}", _selectedInstrument.Host, _selectedInstrument.Port), _selectedInstrument.ServerPark);
            }
        }

  


        //private void cmdRefresh_Executed(object sender, ExecutedRoutedEventArgs e) {
        //    e.Handled = true;
        //    Refresh();
        //}

        //private void cmdShowSettings_Executed(object sender, ExecutedRoutedEventArgs e) {
        //    var dialog = new SettingsWindow() { Owner = this.Window };
        //    if (dialog.ShowDialog() == true) {
        //        if (!Refresh()) {
        //            MessageBox.Show(this.Window, "Could not connect", this.Window.Title, MessageBoxButton.OK, MessageBoxImage.Error);
        //        }
        //    }
        //}



       
        //private void InstrumentManager_OnConfigLoaded(object sender, InstrumentManagerConfigLoadedArgs e) {
        //    if (string.IsNullOrEmpty(Properties.Settings.Default.DeployFolder)) {
        //        // set default folder if app user settings is empty
        //        Properties.Settings.Default.DeployFolder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "B5Surveys", "StarterKit");
        //    }
        //    // change the default deploy folder to the one in app user settings
        //    e.Configuration["DeployFolder"] = Properties.Settings.Default.DeployFolder;
        //    System.Diagnostics.Debug.WriteLine("DeployFolder: " + e.Configuration["DeployFolder"]);
        //}

        internal void Loadbalancer_ServerParkObtained(object sender, StatNeth.Blaise.API.DataEntry.ServerPark.ServerParkObtainedEventArgs e) {
            // Most likely this is called from an other thread then the UI thread, due to asynchrone communications
            if (e != null && e.Park != null) {
                var surveyWindow = new SurveyWindow() { Owner = this.Window, WindowState = WindowState.Maximized, Icon = this.Window.Icon, WindowStartupLocation = this.Window.WindowStartupLocation };
                surveyWindow.Owner = this.Window;

                // Once we obtained the serverpark, we can get the DataEntryController. This controller runs the interview
                IDataEntryController controller = DataEntryManager.GetDataEntryController(e.Park, surveyWindow, surveyWindow.mainGrid);
                surveyWindow.DataEntryController = controller;

                if (((_selectedInstrument.Name.Length >= 8) && (_selectedInstrument.Name.ToUpper().Substring(0, 8) == "PROGRESS"))| ((_selectedInstrument.Name.Length >= 11) && (_selectedInstrument.Name.ToUpper().Substring(0, 11) == "PERFORMANCE")) | ((_selectedInstrument.Name.Length >= 10) && (_selectedInstrument.Name.ToUpper().Substring(0, 10) == "TRAINEEAPP")))
                {
                    
                    String Username = GetUsername();
                    
                    RuntimeParameters.Add("Fields", "FT_Num=" + Username);
                }
                else if ((_selectedInstrument.Name.Length >= 8) && (_selectedInstrument.Name.ToUpper().Substring(0, 8) == "SHIFTDET"))
                {

                    String Username = GetUsername();

                    RuntimeParameters.Add("Fields", "TeamL=" + Username);
                }
                else if ((_selectedInstrument.Name.Length >= 4) && (_selectedInstrument.Name.ToUpper().Substring(0, 4) == "NIPS"))
                {

                    String Username = GetUsername();

                    RuntimeParameters.Add("Fields", "admin1.authno=" + Username);
                }
                else if (((_selectedInstrument.Name.Length >= 11) && (_selectedInstrument.Name.ToUpper().Substring(0, 11) == "ALLOCATIONS"))| ((_selectedInstrument.Name.Length >= 7) && (_selectedInstrument.Name.ToUpper().Substring(0, 7) == "B5WAGES")) | ((_selectedInstrument.Name.Length >= 13) && (_selectedInstrument.Name.ToUpper().Substring(0, 13) == "EXPENSESCLAIM")))
                {

                    String Username = GetUsername();

                    RuntimeParameters.Add("Fields", "intno=" + Username);
                }
                //else { RuntimeParameters.Add("KeyValue", _serno + "," + _hhno + "," + _intno); }

                // Be sure to execute all the user interface stuff on the main UI thread
                this.Window.Dispatcher.Invoke(() => {
                    this.Window.WindowState = WindowState.Maximized; //Needed to get the correct screen dimensions
                    // Start the interview using the runtime parameters that we have filled already during the start of this Activity
                    controller.StartInterview(_selectedInstrument.Id, _selectedInstrument.Name, RuntimeParameters,
                            System.Threading.Thread.CurrentThread.CurrentCulture.Name, (int)this.Window.ActualWidth, (int)this.Window.ActualHeight); //Pass screen dimensions in order to pick correct layoutset
                    surveyWindow.ShowDialog() ;
                    _selectedInstrument = null;
                    RuntimeParameters.Clear();
                     
                });
            }
        }

        internal void Loadbalancer_ServerParkObtainedSpecific(object sender, StatNeth.Blaise.API.DataEntry.ServerPark.ServerParkObtainedEventArgs e)
        {
            // Most likely this is called from an other thread then the UI thread, due to asynchrone communications
            if (e != null && e.Park != null)
            {
                var surveyWindow = new SurveyWindow() { Owner = this.Window, WindowState = WindowState.Maximized, Icon = this.Window.Icon, WindowStartupLocation = this.Window.WindowStartupLocation };
                surveyWindow.Owner = this.Window;

                // Once we obtained the serverpark, we can get the DataEntryController. This controller runs the interview
                IDataEntryController controller = DataEntryManager.GetDataEntryController(e.Park, surveyWindow, surveyWindow.mainGrid);
                surveyWindow.DataEntryController = controller;

                RuntimeParameters.Add("KeyValue", _primkey);
                //if ((_selectedInstrument.Name == "Progress") | (_selectedInstrument.Name == "Performance"))
                //{
                //    DateTime datestr = _intdate;
                //    String Username = GetUsername();
                //    string strintdate = _intdate.ToString("ddMMyyyy");
                //    //MessageBox.Show(this.Window, string.Format("Date: {0} string date:{1}", _intdate,strintdate), this.Window.Title, MessageBoxButton.OK, MessageBoxImage.Information);
                //    //                    RuntimeParameters.Add("KeyValue", _intno + "," + strintdate);
                //    RuntimeParameters.Add("KeyValue", _primkey);
                //}
                //else if (_selectedInstrument.Name.Substring(0, 4) == "NIPS")
                //{
                //    DateTime datestr = _intdate;

                //    String Username = GetUsername();
                //    string strintdate = _intdate.ToString("ddMMyyyy");
                //    //MessageBox.Show(this.Window, string.Format("Date: {0} string date:{1}", _intdate,strintdate), this.Window.Title, MessageBoxButton.OK, MessageBoxImage.Information);
                //    RuntimeParameters.Add("KeyValue", _primkey);
                //}
                //else
                //{
                //    RuntimeParameters.Add("KeyValue", _primkey);

                ////RuntimeParameters.Add("KeyValue", _serno + "," + _hhno + "," + _intno);
                // }


                // Be sure to execute all the user interface stuff on the main UI thread

                
                this.Window.Dispatcher.Invoke(() =>
                {
                    this.Window.WindowState = WindowState.Maximized;

                    //Needed to get the correct screen dimensions
                    // Start the interview using the runtime parameters that we have filled already during the start of this Activity
                    controller.StartInterview(_selectedInstrument.Id, _selectedInstrument.Name, RuntimeParameters,
                            System.Threading.Thread.CurrentThread.CurrentCulture.Name, (int)this.Window.ActualWidth, (int)this.Window.ActualHeight); //Pass screen dimensions in order to pick correct layoutset
                    surveyWindow.ShowDialog();
                    _selectedInstrument = null;
                    RuntimeParameters.Clear();
                    _serno = null;
                    _hhno = null;
                    _intno = null;
                    _site = null;
                    });
                

            }
        }



        //private void InstrumentManager_OnInstrumentInstalled(object sender,InstrumentInstalledEventArgs e) {
        //    // watch out, be sure to use UI Thread
        //    this.Window.Dispatcher.Invoke(() => {
        //        if (e.Success) {
        //            Refresh();
        //            MessageBox.Show(this.Window, string.Format("Instrument {0} installed", e.Instrument.Name), this.Window.Title, MessageBoxButton.OK, MessageBoxImage.Information);
        //        } else {
        //            MessageBox.Show(this.Window, string.Format("Could not install instrument {0}:\n{1}", e.Instrument.Name, e.ErrorMessage), this.Window.Title, MessageBoxButton.OK, MessageBoxImage.Error);
        //        }
        //    }
        //    );
        //}



        internal string GetUsername()
        {
            string path = @"c:\id.bat";
            string usertxt = " ";
            if (System.IO.File.Exists(path))
            {
                Console.WriteLine("File exists");

                string line;
                // Read the text file contianing survey name and Guid of surveys to be deleted.  

                System.IO.StreamReader file =
                new System.IO.StreamReader(@"c:\id.bat");
                while ((line = file.ReadLine()) != null)
                {
                    string[] parts = line.Split('T');
                    usertxt = parts[3];
                }
                file.Close();

            }
            else
            {
                Console.WriteLine("File does not exists");
                usertxt = " ";
            }
            Console.WriteLine("User name {0}", usertxt);
            return usertxt;
        }


        internal SecureString GetPassword()
        {
            string path = @"c:\zpass.bat";
            string passtxt = " ";
            if (System.IO.File.Exists(path))
            {
                Console.WriteLine("File exists");

                string line;
                // Read the text file contianing survey name and Guid of surveys to be deleted.  

                System.IO.StreamReader file =
                new System.IO.StreamReader(@"c:\zpass.bat");
                while ((line = file.ReadLine()) != null)
                {
                    string[] parts = line.Split('=');
                    passtxt = parts[1];
                }
                file.Close();

            }
            char[] passwordChars = passtxt.ToCharArray();
            SecureString password = new SecureString();
            foreach (char c in passwordChars)
                password.AppendChar(c);
            return password;
        }

        internal ClientPreferredBinding GetBinding()
        {
            ClientPreferredBinding result = ClientPreferredBinding.Https;
            return result;
        }


        internal DateTime GetWageYearStartDate()
        {
            string path = @"c:\surveys\wageyearstartdate.txt";
            string wagedatedetails = " ";
            if (System.IO.File.Exists(path))
            {
                System.IO.StreamReader file = new System.IO.StreamReader(path);
                wagedatedetails = file.ReadLine();
                file.Close();
                //Reads the properly formatted date and time of the last scynchronise
                try
                {
                    AppController.Instance._wageyearstartdate = DateTime.Parse(wagedatedetails);
                }
                catch
                {
                    AppController.Instance._wageyearstartdate = new DateTime(2022, 1, 1);
                }
            }
            else
            {
                //MessageBox.Show(string.Format("Cant read from {0}", wagedatedetails));
                AppController.Instance._wageyearstartdate = new DateTime(2022, 1, 1);
                wagedatedetails = "01/01/2022 ";
            }
            //Console.WriteLine("User name {0}", wagedatedetails);
            DateTime datefield = DateTime.Parse(wagedatedetails);
            return datefield;
        }

        internal bool Connect(string Location, string HostAddress, bool RunSetData, bool NeedTimer)
        {
            ClientPreferredBinding binding = GetBinding();
            GPSApp_Load();
            String Username = GetUsername();
            SecureString Password = GetPassword();
            GetWageYearStartDate();
            //MessageBox.Show(string.Format("Wage start date {0}", Convert.ToString(_wageyearstartdate)));
            try
            {
                bool isConnected = false;
                if (InstrumentManager.Connect(HostAddress, 8033, Username, Password, binding, 30, "tls12", Location))
                {
                    isConnected = true;
                    //MessageBox.Show(string.Format("Am I connected to {0} ==== {1}", Location, isConnected));
                    if (Location.ToUpper() == "C:\\B5RESEARCH")
                    {
                        //MessageBox.Show(string.Format("I am in {0}",Location));
                        AppController.Instance._runSetData = RunSetData;
                        AppController.Instance._connectedtoSurveys = true;
                        AppController.Instance.currentenvironment = "C:\\B5RESEARCH";
                    }
                    else if (Location.ToUpper() == "C:\\B5RESPOND")
                    {
                        //MessageBox.Show(string.Format("I am in {0}", Location));
                        AppController.Instance._runSetData = RunSetData;
                        AppController.Instance._connectedtoRespond = true;
                        AppController.Instance.currentenvironment = "C:\\B5RESPOND";
                    }
                    AppController.Instance.InstrumentManager.GetAvailableInstruments();
                }
                //bool isConnected = InstrumentManager.Connect(HostAddress, 8033, Username, Password, binding, 30,"tls12",Location);
                //MessageBox.Show(string.Format("Am I connected to {0} ==== {1}", Location, isConnected));

                return isConnected;
            }
            catch (Exception)
            {
                if (Location.ToUpper() == "C:\\B5RESEARCH")
                {
                    //MessageBox.Show(string.Format("I am NOT in {0}", Location));
                    AppController.Instance._connectedtoSurveys = false;
                }
                else if (Location.ToUpper() == "C:\\B5RESPOND")
                {
                    //MessageBox.Show(string.Format("I am NOT in {0}", Location));
                    AppController.Instance._connectedtoRespond = false;
                    //If there is no conenction to respond then immediately enable to buttons again and reset the counters
                    ev_repaint_buttons();
                    _casesloadedSurveys = false;
                    _casesloadedRespond = false;
                }
                return false;
            }
        }

     
        internal bool ConnectLocally(string Location)
        {
            string servername = "";
            if (Location.ToUpper() == "C:\\B5RESEARCH")
            { servername = "research.nisra.gov.uk"; }
            else if (Location.ToUpper() == "C:\\B5RESPOND")
            { servername = "respond.nisra.gov.uk"; }

            GPSApp_Load();
            String Username = GetUsername();
            SecureString Password = GetPassword();
            try
            {
              
                bool isConnected = InstrumentManager.ConnectLocally(Username, servername ,Password, Location);
                //MessageBox.Show(string.Format("{0} reporting as connected.", Location));
                AppController.Instance.InstrumentManager.GetAvailableInstruments();
                if (isConnected == true)
                {
                    if (Location.ToUpper() == "C:\\B5RESEARCH")
                    {
                        AppController.Instance._connectedtoSurveys = true;
                    }
                    else if (Location.ToUpper() == "C:\\B5RESPOND")
                    {
                        AppController.Instance._connectedtoRespond = true;
                    }
                }
                else if (isConnected == false)
                {
                    if (Location.ToUpper() == "C:\\B5RESEARCH")
                    {
                        AppController.Instance._connectedtoSurveys = false;
                    }
                    else if (Location.ToUpper() == "C:\\B5RESPOND")
                    {
                        AppController.Instance._connectedtoRespond = false;
                    }
                }

                return isConnected;

            }
            catch (Exception)
            {
                //MessageBox.Show(this.Window, string.Format("There is a problem logging with the provided user name and password. Please contact the office."), this.Window.Title, MessageBoxButton.OK);
                return false;
            }

        }

        internal bool ConnectLocallyr(string Location,bool RunSetData)
        {

            string servername = "";
            if (Location.ToUpper() == "C:\\B5RESEARCH")
            { servername = "research.nisra.gov.uk"; }
            else if (Location.ToUpper() == "C:\\B5RESPOND")
            { servername = "respond.nisra.gov.uk"; }

            GPSApp_Load();
            String Username = GetUsername();
            SecureString Password = GetPassword();
            try
            {
                bool isConnected = false;


                if (InstrumentManager.ConnectLocally(Username, servername, Password, Location))
                    {
                        isConnected = true;
                    //MessageBox.Show(string.Format("{0} reporting as connected.", Location));
                    if (Location.ToUpper() == "C:\\B5RESEARCH")
                    {
                        //MessageBox.Show(string.Format("I am in {0}", Location));
                        AppController.Instance._runSetData = RunSetData;
                        AppController.Instance._connectedtoSurveys = true;
                        AppController.Instance.currentenvironment = "C:\\B5RESEARCH";
                    }
                    else if (Location.ToUpper() == "C:\\B5RESPOND")
                    {
                        //MessageBox.Show(string.Format("I am in {0}", Location));
                        AppController.Instance._runSetData = RunSetData;
                        AppController.Instance._connectedtoRespond = true;
                        AppController.Instance.currentenvironment = "C:\\B5RESPOND";
                    }



                    //AppController.Instance._runSetData = RunSetData;
                    AppController.Instance.InstrumentManager.GetAvailableInstruments();
                }
                return isConnected;

            }
            catch (Exception)
            {
                //MessageBox.Show(this.Window, string.Format("There is a problem logging with the provided user name and password. Please contact the office."), this.Window.Title, MessageBoxButton.OK);
                return false;
            }

        }

        private void GetInstruments()
        {
            
            InstrumentManager.GetAvailableInstruments();
        }

        private async void InstrumentManager_OnInstrumentInfoLoaded(object sender, InstrumentInfoLoadedEventArgs e)
        {
            // Loads up all the cases into 

            //There are issues with threading as the main UI thread is not the one doing the updating of the observable collections each time data upload is triggered
            //only compute this once - it was getting re-computed as zero on come cases particurarly on tablet
            if ((currentenvironment == "C:\\B5RESEARCH") && (AppController.Instance.InstalledOnSurveysComputed == false))
            {
               
                InstalledonSurveys = e;
                AppController.Instance.InstalledOnSurveysComputed = true;

            }
            //only compute this once - it was getting re-computed as zero on come cases particurarly on tablet
            if ((currentenvironment == "C:\\B5RESPOND") && (AppController.Instance.InstalledOnRespondComputed == false))
            {
                //MessageBox.Show(string.Format("Instrument Count: {0}", e.Instruments.Count()));
                InstalledonRespond = e;
                AppController.Instance.InstalledOnRespondComputed = true;
            }
            //string environment = "C:\\B5SURVEYS";
            //if (currentenvironment == "C:\\B5RESPOND")
            //{ environment = "C:\\B5RESPOND"; }

            //MessageBox.Show(string.Format("About to call SetData from {0}. Number of Instruments: {1}", environment , e.Instruments.Count()));
            await Task.Delay(1000);
            if (e != null)
            {
                if (AppController.Instance._runSetData == true)
                {
                    Blaise_App.AppController.Instance.Window.Dispatcher.Invoke((Action)delegate
                    {
                        //MessageBox.Show(string.Format("Current Environment:{0}", currentenvironment));
                        SetData(e.Instruments, false, currentenvironment);
                    //MessageBox.Show(string.Format("No of instruments:{0}", e.Instruments.Count()));

                    });
                
                }
                AppController.Instance._runSetData = true;

            }
            //AppController.Instance.InstrumentInfoLoadedEventVariable = e;
            //this.Window.Dispatcher.Invoke(() => this.Window.surveysMain.SetData(e.Instruments,false));

            //this.Window.Dispatcher.Invoke(() => this.Window.surveysMain );


        }


        // The coordinate watcher.
        private GeoCoordinateWatcher Watcher = null;

        // Create and start the watcher.
        private void GPSApp_Load()
        {
            // Create the watcher.
         

              Watcher = new GeoCoordinateWatcher();
            
            // Catch the StatusChanged event.
            Watcher.StatusChanged += Watcher_StatusChanged;

            // Start the watcher.
            Watcher.Start();

        }

        // The watcher's status has change. See if it is ready.
        private void Watcher_StatusChanged(object sender,
            GeoPositionStatusChangedEventArgs e)
        {
            if (e.Status == GeoPositionStatus.Ready)
            { 
                this.Window.Dispatcher.Invoke(() => {
                
                    // Display the latitude and longitude.
                    if (Watcher.Position.Location.IsUnknown)
                    {
                        _txtLat = 0;
                        _txtLong = 0;
                        //MessageBox.Show(this.Window, string.Format("Lat:{0} Long:{1}", _txtLat, _txtLong), this.Window.Title, MessageBoxButton.OK);
                    }
                    else
                    {
                        GeoCoordinate location =
                           Watcher.Position.Location;
                        _txtLat = location.Latitude;
                        _txtLong = location.Longitude;
                        //MessageBox.Show(this.Window, string.Format("Lat:{0} Long:{1}", _txtLat, _txtLong), this.Window.Title, MessageBoxButton.OK);

                    }
                });
            }
        }


       
    

        //private void InstrumentManagerInstance_OnConfigLoaded(object sender, InstrumentManagerConfigLoadedArgs e)
        //{
        //    Console.WriteLine("getting config");
        //    //if (string.IsNullOrEmpty(Properties.Settings.Default.DeployFolder))
        //    //{
        //    //    Console.WriteLine("getting config");
        //        Properties.Settings.Default.DeployFolder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "B5Surveys");
        //    //}
        //    e.Configuration["DeployFolder"] = Properties.Settings.Default.DeployFolder;
        //}




    }//end of Class AppController
}//end of Namespace

  