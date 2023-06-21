using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using StatNeth.Blaise.API.DataEntry;
using MetaAPI = StatNeth.Blaise.API.Meta;
using DataRecordAPI = StatNeth.Blaise.API.DataRecord;
using DataLinkAPI = StatNeth.Blaise.API.DataLink;
using System.Collections.ObjectModel;
using StatNeth.Blaise.API.DataEntryWpf;
using System.ComponentModel;
using System.Diagnostics;

namespace Blaise_App
{

    /// <summary>
    /// Interaction logic for Surveys.xaml
    /// </summary>
    public partial class Surveys : UserControl
    {
        public delegate void dg_synchronised();
        public event dg_synchronised ev_repaintsynchronising_buttons;

        public Surveys()
        {
            
            InitializeComponent();
            //AppController.Instance.SetData(AppController.Instance.InstalledSurveys.Instruments);
            //Show_cases.IsEnabled = true;
            this.ev_repaintsynchronising_buttons += new dg_synchronised(EventSynchronisingMessage);



            //surveysMain.ItemsSource = FullDataSet;
        }

        public void EventMessage()
        {
            this.Dispatcher.Invoke(() =>
            {
                //this next reinstates the close button after initial app load and after syncing
                AppController.Instance.Window.WindowStyle = WindowStyle.SingleBorderWindow;

                if (AppController.Instance._surveyinstrument != null)
                {
                    //AppController.Instance.Refresh();
                    Button btn2 = Show_cases;
                    btn2.IsEnabled = true;
                    btn2.Visibility = Visibility.Visible;
                    btn2.Content = "View Survey Cases";
                    btn2.Background = Brushes.CornflowerBlue;
                }
                else
                {
                    Button btn2 = Show_cases;
                   
                    btn2.IsEnabled = false;
                    btn2.Visibility = Visibility.Collapsed;
                                       //btn2.Content = "Not available";
                }
                if ((AppController.Instance._progressinstrument != null) |(AppController.Instance._performanceinstrument != null) | (AppController.Instance._traineeappinstrument != null))
                {
                    Button btn2 = Show_FieldTrainer;
                    btn2.IsEnabled = true;
                    //btn2.Content = "View Team Progress \n\n Field Trainers only";
                    btn2.Visibility = Visibility.Visible;
                    btn2.Content = "Field Trainers only";
                    btn2.Background = Brushes.CornflowerBlue;
                }
                else
                {
                    Button btn2 = Show_FieldTrainer;
                    btn2.IsEnabled = false;
                    btn2.Visibility = Visibility.Collapsed;
                    //btn2.Content = "Not available";
                }
                //if (AppController.Instance._performanceinstrument != null)
                //{
                //    Button btn2 = Show_performance;
                //    btn2.IsEnabled = true;
                //    btn2.Content = "View Team Performance \n\n Field Trainers only";
                //    btn2.Background = Brushes.CornflowerBlue;
                //}
                //else
                //{
                //    Button btn2 = Show_performance;
                //    btn2.IsEnabled = false;
                //    btn2.Content = "Not available";
                //}
                if (AppController.Instance._NIPSMenuinstrument != null)
                {
                    Button btn2 = Show_NIPSMenu;
                    btn2.IsEnabled = true;
                    btn2.Visibility = Visibility.Visible;
                    btn2.Content = "Open NIPS Menu";
                    btn2.Background = Brushes.CornflowerBlue;
                }
                else
                {
                    Button btn2 = Show_NIPSMenu;
                    btn2.IsEnabled = false;
                    btn2.Visibility = Visibility.Collapsed;
                    //btn2.Content = "Not available";
                }
                if (AppController.Instance._allocationsinstrument != null)
                {
                    //AppController.Instance.Refresh();
                    Button btn2 = First_offer_button;
                    btn2.IsEnabled = true;
                    btn2.Visibility = Visibility.Visible;
                    btn2.Content = "First Offer of Work";
                    btn2.Background = Brushes.DarkGoldenrod ;
                }
                else
                {
                    Button btn2 = First_offer_button;

                    btn2.IsEnabled = false;
                    btn2.Visibility = Visibility.Collapsed;
                    //btn2.Content = "Not available";
                }
                DateTime WageDate = new DateTime(2022, 3, 21);
                DateTime Now = DateTime.Now;
                String UserName = AppController.Instance.GetUsername();
                if (((AppController.Instance._currentwagesinstrument != null) && (Now >= WageDate))| ((AppController.Instance._currentwagesinstrument != null) && (UserName == "8888")))
                {
                    //AppController.Instance.Refresh();
                    Button btn2 = Show_WAGESMenu;
                    btn2.IsEnabled = true;
                    btn2.Visibility = Visibility.Visible;
                    btn2.Content = "Wages";
                    btn2.Background = Brushes.CornflowerBlue;
                }
                else
                {
                    Button btn2 = Show_WAGESMenu;

                    btn2.IsEnabled = false;
                    btn2.Visibility = Visibility.Collapsed;
                    //btn2.Content = "Not available";
                }

                string SyncLocation = "";
                string SyncLocationFull = "";
                if (AppController.Instance._connectedtoSurveys == true)
                {
                    SyncLocation = Properties.Settings.Default.DeployFolder + "\\synctime.txt";
                    SyncLocationFull = Properties.Settings.Default.DeployFolder + "\\synctimeFull.txt";
                }
                else if (AppController.Instance._connectedtoRespond == true)
                {
                    SyncLocation = Properties.Settings.Default.DeployFolderRespond + "\\synctime.txt";
                    SyncLocationFull = Properties.Settings.Default.DeployFolderRespond + "\\synctimeFull.txt";
                }
                    if (System.IO.File.Exists(SyncLocation))
                {
                    System.IO.StreamReader file = new System.IO.StreamReader(SyncLocation);
                    string syncdetails = file.ReadLine();
                    file.Close();
                    //Reads the properly formatted date and time of the last scynchronise
                    AppController.Instance._syncronisationdetails = syncdetails;

                    if (System.IO.File.Exists(SyncLocationFull))
                    {
                        System.IO.StreamReader filefull = new System.IO.StreamReader(SyncLocationFull);
                        string syncdetailsfull = filefull.ReadLine();
                        filefull.Close();
                        //Creates a DateTime based on the time stamp of the last backup - ensures backup not done where menu has been closed and re-opened in quick succession
                        AppController.Instance._synctime = DateTime.Parse(syncdetailsfull);
                    }
                        Button btn3 = Synchronise_button;
                    btn3.IsEnabled = true;
                    btn3.Content = "Manual Synchronise with HQ" +
                                   "\nThis may take a few minutes" +
                                   "\n(" + AppController.Instance._syncronisationdetails + ")";
                    btn3.Background = Brushes.CornflowerBlue;

                }
                else
                {
                    Button btn3 = Synchronise_button;
                    btn3.IsEnabled = true;
                    btn3.Content = "Synchronise";
                    btn3.Background = Brushes.CornflowerBlue;
                }
                //Button shw_cases = Window.surveysMain.Show_cases;
                //MessageBox.Show(string.Format("We are loaded"));

            });
        }
        public void EventSynchronisingMessage()
        {
            this.Dispatcher.Invoke(() =>
        {
            //MessageBox.Show(string.Format("Syncing"));
            //this next line removes the close button during the loading of the app
            //AppController.Instance.Window.WindowStyle = WindowStyle.None;
            Button btna = Show_cases;
            btna.IsEnabled = false;
            btna.Content = "Synchronising with HQ...";
            btna.Background = Brushes.Red;

            if ((AppController.Instance._progressinstrument != null) | (AppController.Instance._performanceinstrument != null))
            {
                Button btnb = Show_FieldTrainer;
                btnb.IsEnabled = false;
                btnb.Content = "Synchronising with HQ...";
            }
            //if (AppController.Instance._performanceinstrument != null)
            //{
            //    Button btnb = Show_performance;
            //    btnb.IsEnabled = false;
            //    btnb.Content = "Synchronising with HQ...";
            //}
            if (AppController.Instance._NIPSMenuinstrument != null)
            {
                Button btnb = Show_NIPSMenu;
                btnb.IsEnabled = false;
                btnb.Content = "Synchronising with HQ...";
            }

            if (AppController.Instance._currentwagesinstrument != null)
            {
                Button btnb = Show_WAGESMenu;
                btnb.IsEnabled = false;
                btnb.Content = "Synchronising with HQ...";
            }
            if (AppController.Instance._allocationsinstrument != null)
            {
                Button btnb = First_offer_button;
                btnb.IsEnabled = false;
                btnb.Content = "Synchronising with HQ...";
            }


            Button btnc = Synchronise_button;
            btnc.IsEnabled = false;
            btnc.Content = "Synchronising with HQ...";

        });
        }



        private void Show_cases_Click(object sender, RoutedEventArgs e)
        {
            var c = new SurveyDetails() { DataContext = AppController.Instance.FullDataSet };
            //var c = new SurveyDetails();
            //            var c = new SurveyDetails();
            var d = new Window() { Owner = AppController.Instance.Window, ResizeMode = ResizeMode.NoResize, WindowState = AppController.Instance.Window.WindowState, Icon = AppController.Instance.Window.Icon, WindowStartupLocation = AppController.Instance.Window.WindowStartupLocation, Content = c, Title = "Browse cases" };
            d.ShowDialog();
        }

        private void Show_NIPSMenu_Click(object sender, RoutedEventArgs e)
        {
            var c = new NIPSMenu();// { DataContext = AppController.Instance.NIPSMenuDataSet };
            //var c = new NIPSMenu();
            //            var c = new NIPSMenu();
            var d = new Window() { Owner = AppController.Instance.Window, ResizeMode = ResizeMode.NoResize, WindowState = AppController.Instance.Window.WindowState, Icon = AppController.Instance.Window.Icon, WindowStartupLocation = AppController.Instance.Window.WindowStartupLocation, Content = c, Title = "NIPS Menu" };
            d.ShowDialog();

        }

        private void Show_WAGESMenu_Click(object sender, RoutedEventArgs e)
        {
            var c = new WAGESMenu();// { DataContext = AppController.Instance.WAGESMenuDataSet };
            //var c = new NIPSMenu();
            //            var c = new NIPSMenu();
            var d = new Window() { Owner = AppController.Instance.Window, ResizeMode = ResizeMode.NoResize, WindowState = AppController.Instance.Window.WindowState, Icon = AppController.Instance.Window.Icon, WindowStartupLocation = AppController.Instance.Window.WindowStartupLocation, Content = c, Title = "Wages Menu" };
            d.ShowDialog();

        }


        private void Show_FieldTrainer_Click(object sender, RoutedEventArgs e)
        {
            var c = new FieldTrainerMenu() { DataContext = AppController.Instance.ProgressDataSet };
            //var c = new SurveyDetails();
            //            var c = new SurveyDetails();
            var d = new Window() { Owner = AppController.Instance.Window, ResizeMode = ResizeMode.NoResize, WindowState = AppController.Instance.Window.WindowState, Icon = AppController.Instance.Window.Icon, WindowStartupLocation = AppController.Instance.Window.WindowStartupLocation, Content = c, Title = "Field Trainer Menu" };
            d.ShowDialog();
        }

        //private void Show_Performance_Click(object sender, RoutedEventArgs e)
        //{
        //    var c = new PerformanceDetails() { DataContext = AppController.Instance.PerformanceDataSet };
        //    //var c = new SurveyDetails();
        //    //            var c = new SurveyDetails();
        //    var d = new Window() { Owner = AppController.Instance.Window, ResizeMode = ResizeMode.NoResize, WindowState = AppController.Instance.Window.WindowState, Icon = AppController.Instance.Window.Icon, WindowStartupLocation = AppController.Instance.Window.WindowStartupLocation, Content = c, Title = "Browse Communications with your team" };
        //    d.ShowDialog();
        //}

        private void Synchronise_button_Click(object sender, RoutedEventArgs e)
        {

            if (AppController.Instance._connectedtoSurveys == true)
            {
                bool connected = AppController.Instance.Connect("C:\\B5Research", "research.nisra.gov.uk", false, false);
                //bool connected = AppController.Instance.Connect("C:\\B5Surveys", "surveys.nisra.gov.uk", false);
                if (connected == true)
                {
                    if (System.IO.File.Exists("c:\\Blaise5Controller513\\Blaise5Controller513.exe"))
                    {
                        //Check for any instances of Blaise5Controller running in the background and only invoke this is there isn't
                        Process[] p = Process.GetProcessesByName("Blaise5Controller513");
                        if (p.Count() == 0)
                        {
                             ev_repaintsynchronising_buttons();
                             Task.Delay(1000).ContinueWith(t => Runsync("RESEARCH"));
                        }
                    }
                    else
                    {
                        ev_repaintsynchronising_buttons();
                        Task.Delay(1000).ContinueWith(t => Runsync("REESEARCH"));
                    }
                }
            }
            else if (AppController.Instance._connectedtoRespond == true)
            {
                bool connected = AppController.Instance.Connect("C:\\B5Respond", "respond.nisra.gov.uk", false, false);
                //bool connected = AppController.Instance.Connect("C:\\B5Surveys", "surveys.nisra.gov.uk", false);
                if (connected == true)
                {
                    if (System.IO.File.Exists("c:\\Blaise5Controller511\\Blaise5Controller511.exe"))
                    {
                        //Check for any instances of Blaise5Controller running in the background and only invoke this is there isn't
                        Process[] p = Process.GetProcessesByName("Blaise5Controller511");
                        if (p.Count() == 0)
                        {
                             ev_repaintsynchronising_buttons();
                             Task.Delay(1000).ContinueWith(t => Runsync("RESPOND"));
                        }
                    }
                    else
                    {
                        ev_repaintsynchronising_buttons();
                        Task.Delay(1000).ContinueWith(t => Runsync("RESPOND"));
                    }

                }
            }
            else
            {
                this.Dispatcher.Invoke(() =>
                {
                    AppController.Instance.Window.WindowStyle = WindowStyle.SingleBorderWindow;
                });
            }


        }

        private void Runsync(string Environment)
        {
            if (Environment == "RESEARCH")
            {
                //MessageBox.Show(string.Format("Connected to Surveys= {0} CurrentEnvironment =  {1}", AppController.Instance._connectedtoSurveys, AppController.Instance.currentenvironment));
                AppController.Instance.Looper(AppController.Instance.InstalledonSurveys.Instruments, false, "RESEARCH");
            }
            else if (Environment == "RESPOND")
            {
                //MessageBox.Show(string.Format("Connected to Respond= {0} CurrentEnvironment =  {1}", AppController.Instance._connectedtoRespond, AppController.Instance.currentenvironment));
                AppController.Instance.Looper(AppController.Instance.InstalledonRespond.Instruments, false, "RESPOND");
            }




        }
        private void First_offer_button_Click(object sender, RoutedEventArgs e)
        {
            var SelectedCase = AppController.Instance._allocationsinstrument;
            if (SelectedCase != null)
            {

                AppController.Instance._selectedInstrument = SelectedCase;
                //This next statement ensure that the program knows which environment is being used
                AppController.Instance.ConnectLocallyr(AppController.Instance._allocationslocation, false);
                AppController.Instance.currentenvironment = AppController.Instance._allocationslocation;
                //AppController.Instance._serno = SelectedCase.Serno;
                //AppController.Instance._intno = SelectedCase.Intno;
                //AppController.Instance._intdate = SelectedCase.Intdate;
                //AppController.Instance._site = SelectedCase.Site;
                //AppController.Instance._primkey = SelectedCase.Primkey;




                //MessageBox.Show(string.Format("I can press this {0}",SelectedCase.Primkey ));


                ILoadBalancer loadbalancer = DataEntryManager.GetLoadBalancer(SelectedCase.RunMode);
                loadbalancer.ServerParkObtained += AppController.Instance.Loadbalancer_ServerParkObtained;
                if (string.IsNullOrEmpty(SelectedCase.Host))
                    SelectedCase.Host = "localhost";
                loadbalancer.RequestServerPark(string.Format("http://{0}:{1}", SelectedCase.Host, SelectedCase.Port), SelectedCase.ServerPark);
            }
           
        }
 
        
    }
}










        //public void SetData(IEnumerable<IInstrumentInfo> data)
        //{
        //    if (data.Count() > 0)
        //    {
        //        FullDataSet.Clear();
        //        IEnumerable<IInstrumentInfo> InstalledInstruments =
        //            InstalledInstruments = data.Where(s => s.InstrumentState == InstrumentState.InstalledOnClient);
        //        foreach (IInstrumentInfo InstalledInstrument in InstalledInstruments)
        //        {
        //            string strDataModel = Properties.Settings.Default.DeployFolder + "\\surveys\\" + InstalledInstrument.Name + "\\" + InstalledInstrument.Name + ".bmix";
        //            string strDataInterface = Properties.Settings.Default.DeployFolder + "\\surveys\\" + InstalledInstrument.Name + "\\" + InstalledInstrument.Name + ".bdix";
        //            MetaAPI.IDatamodel dm = MetaAPI.MetaManager.GetDatamodel(strDataModel);
        //            DataLinkAPI.IDataLink dl = DataLinkAPI.DataLinkManager.GetDataLink(strDataInterface, dm);
        //            DataRecordAPI.IKey key = GetPrimaryKey(dm);
        //            DataLinkAPI.IDataSet ds = dl.Read(key, DataLinkAPI.ReadOrder.Ascending, 100, true);


        //            if (ds != null && ds.RecordCount > 0)
        //                // We need to clear the values out of DataSet each time so that the list of cases doesn't get duplicated 

        //            {
        //                // Typical way to iterate through an IDataSet:
        //                while (!ds.EndOfSet)
        //                {
        //                    DataRecordAPI.IDataRecord dr = ds.ActiveRecord;
        //                    FillDataRecord(dr, InstalledInstrument);
        //                    ds.MoveNext();
        //                }
        //            }

        //        }
        //    }
        //}

        //private void surveysMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    // synchronize actionBar
        //    var survey = GetSelectedSurvey();
        //    //actionBar.DataContext = survey;
        //    //if (survey == null)
        //    //{
        //    //    actionBar.Visibility = Visibility.Collapsed;
        //    //    return;
        //    //}
        //    //actionBar.Visibility = Visibility.Visible;
        //}




        //private void tester_Click(object sender, RoutedEventArgs e)
        //{
        //    var SelectedCase = GetSelectedSurvey();
        //    if (SelectedCase != null)
        //    {
        //        //Turn the button's IsCancel property on only if a case has been selected. This closes the survey cases list window.
        //        tester.IsCancel = true;
        //        AppController.Instance._selectedInstrument = SelectedCase.Instrument;
        //        AppController.Instance._serno = SelectedCase.Serno;
        //        AppController.Instance._hhno = SelectedCase.Hhno;
        //        AppController.Instance._intno = SelectedCase.Intno;

        //        //MessageBox.Show(string.Format("I can press this {0}",SelectedCase.Primkey ));


        //        ILoadBalancer loadbalancer = DataEntryManager.GetLoadBalancer(SelectedCase.Instrument.RunMode);
        //        loadbalancer.ServerParkObtained += AppController.Instance.Loadbalancer_ServerParkObtainedSpecific;
        //        if (string.IsNullOrEmpty(SelectedCase.Instrument.Host))
        //            SelectedCase.Instrument.Host = "localhost";
        //        loadbalancer.RequestServerPark(string.Format("http://{0}:{1}", SelectedCase.Instrument.Host, SelectedCase.Instrument.Port), SelectedCase.Instrument.ServerPark);
        //    }
        //    else
        //    {
        //        MessageBox.Show("Please selected a case", " ", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}


        //GridViewColumnHeader _lastHeaderClicked = null;
        //ListSortDirection _lastDirection = ListSortDirection.Ascending;

        //void GridViewColumnHeaderClickedHandler(object sender,
        //                                        RoutedEventArgs e)
        //{
        //    var headerClicked = e.OriginalSource as GridViewColumnHeader;
        //    ListSortDirection direction;

        //    if (headerClicked != null)
        //    {
        //        if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
        //        {
        //            if (headerClicked != _lastHeaderClicked)
        //            {
        //                direction = ListSortDirection.Ascending;
        //            }
        //            else
        //            {
        //                if (_lastDirection == ListSortDirection.Ascending)
        //                {
        //                    direction = ListSortDirection.Descending;
        //                }
        //                else
        //                {
        //                    direction = ListSortDirection.Ascending;
        //                }
        //            }

        //            var columnBinding = headerClicked.Column.DisplayMemberBinding as Binding;
        //            var sortBy = columnBinding?.Path.Path ?? headerClicked.Column.Header as string;

        //            Sort(sortBy, direction);

        //            if (direction == ListSortDirection.Ascending)
        //            {
        //                headerClicked.Column.HeaderTemplate =
        //                  Resources["HeaderTemplateArrowUp"] as DataTemplate;
        //            }
        //            else
        //            {
        //                headerClicked.Column.HeaderTemplate =
        //                  Resources["HeaderTemplateArrowDown"] as DataTemplate;
        //            }

        //            // Remove arrow from previously sorted header  
        //            if (_lastHeaderClicked != null && _lastHeaderClicked != headerClicked)
        //            {
        //                _lastHeaderClicked.Column.HeaderTemplate = null;
        //            }

        //            _lastHeaderClicked = headerClicked;
        //            _lastDirection = direction;
        //        }
        //    }
        //}

        //private void Sort(string sortBy, ListSortDirection direction)
        //{
        //    ICollectionView dataView =
        //      CollectionViewSource.GetDefaultView(surveysMain.ItemsSource);

        //    dataView.SortDescriptions.Clear();
        //    SortDescription sd = new SortDescription(sortBy, direction);
        //    dataView.SortDescriptions.Add(sd);
        //    dataView.Refresh();
        //}





   

