using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.DataEntry;
using System.Diagnostics;
using StatNeth.Blaise.API.DataEntryWpf;
using System.ComponentModel;
using System.Data.SqlClient;




namespace Blaise_App
{
    /// <summary>
    /// Interaction logic for SurveyDetails.xaml
    /// </summary>
    public partial class TraineeAppDetails : UserControl
    {

        private GridViewColumnHeader listViewSortCol = null;
        private SortAdorner listViewSortAdorner = null;
        public delegate void dg_synchronised();
        public event dg_synchronised ev_repaintsynchronising_buttons;

        public TraineeAppDetails()
        {
            InitializeComponent();
            lvtraineeappDetails.ItemsSource = AppController.Instance.TraineeAppDataSet;

            ObservableCollection<ITraineeAppRecordInterface> temp = new ObservableCollection<ITraineeAppRecordInterface>(AppController.Instance.TraineeAppDataSet);
            temp.Add(new TraineeAppRecord(AppController.Instance._selectedInstrument, " ", "View All", DateTime.ParseExact("01/01/2001", "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None), " ", " "," "));
            var res = temp
                        .GroupBy(x => x.Intno)
                        .Select(g => g.OrderByDescending(x => x.Intno).First());

            cmbIntSelection.ItemsSource = res;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvtraineeappDetails.ItemsSource);
            lvtraineeappDetails.Items.SortDescriptions.Add(new SortDescription("IntDate", ListSortDirection.Descending));
            view.Filter = UserFilter;
            this.ev_repaintsynchronising_buttons += new dg_synchronised(AppController.Instance.Window.surveysMain.EventSynchronisingMessage);

        }

        private bool UserFilter(object item)
        {
            string x = (item as TraineeAppRecord).Intno;

            if ((stringsearch.Contains(x)) | (stringsearch.Contains("View All")))
                return true;
            else return false;
            //return ((item as FullRecord).Outcome.IndexOf(filtervar, StringComparison.OrdinalIgnoreCase) >= 0);
            //return ((item as FullRecord).Outcome.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }
        public List<string> stringsearch = new List<string> { "View All" };


        private void lvUsersColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = (sender as GridViewColumnHeader);
            string sortBy = column.Tag.ToString();
            if (listViewSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(listViewSortCol).Remove(listViewSortAdorner);
                lvtraineeappDetails.Items.SortDescriptions.Clear();
            }

            ListSortDirection newDir = ListSortDirection.Ascending;
            if (listViewSortCol == column && listViewSortAdorner.Direction == newDir)
                newDir = ListSortDirection.Descending;

            listViewSortCol = column;
            listViewSortAdorner = new SortAdorner(listViewSortCol, newDir);
            AdornerLayer.GetAdornerLayer(listViewSortCol).Add(listViewSortAdorner);
            lvtraineeappDetails.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
        }

        private void TraineeAppDetails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedrecord = GetSelectedRecord();
        }


        private ITraineeAppRecordInterface GetSelectedRecord()
        {
            return lvtraineeappDetails.SelectedItem as ITraineeAppRecordInterface;


        }

        private void RunSurvey_Click(object sender, RoutedEventArgs e)
        {
            var SelectedCase = GetSelectedRecord();

            if (SelectedCase != null)
            {
                AppController.Instance.ConnectLocallyr(SelectedCase.Environment, false);
                AppController.Instance.currentenvironment = SelectedCase.Environment;                //Turn the button's IsCancel property on only if a case has been selected. This closes the survey cases list window.
                //tester.IsCancel = true;
                AppController.Instance._selectedInstrument = SelectedCase.Instrument;
                //AppController.Instance._intno = SelectedCase.Intno;
                //AppController.Instance._intdate = SelectedCase.IntDate;
                AppController.Instance._primkey = SelectedCase.Primkey;


                //MessageBox.Show(string.Format("I can press this {0}",SelectedCase.Primkey ));


                ILoadBalancer loadbalancer = DataEntryManager.GetLoadBalancer(SelectedCase.Instrument.RunMode);
                loadbalancer.ServerParkObtained += AppController.Instance.Loadbalancer_ServerParkObtainedSpecific;
                if (string.IsNullOrEmpty(SelectedCase.Instrument.Host))
                    SelectedCase.Instrument.Host = "localhost";
                loadbalancer.RequestServerPark(string.Format("http://{0}:{1}", SelectedCase.Instrument.Host, SelectedCase.Instrument.Port), SelectedCase.Instrument.ServerPark);
            }
            else
            {
                MessageBox.Show("Please select a case", " ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RunNewSurvey_Click(object sender, RoutedEventArgs e)
        {
            var SelectedCase = AppController.Instance._traineeappinstrument;
            //This next statement ensure that the program knows which environment is being used
            AppController.Instance.ConnectLocallyr(AppController.Instance._traineeapplocation, false);
            AppController.Instance.currentenvironment = AppController.Instance._traineeapplocation;
            AppController.Instance._selectedInstrument = SelectedCase;

            ILoadBalancer loadbalancer = DataEntryManager.GetLoadBalancer(SelectedCase.RunMode);
            loadbalancer.ServerParkObtained += AppController.Instance.Loadbalancer_ServerParkObtained;
            if (string.IsNullOrEmpty(SelectedCase.Host))
                SelectedCase.Host = "localhost";


            loadbalancer.RequestServerPark(string.Format("http://{0}:{1}", SelectedCase.Host, SelectedCase.Port), SelectedCase.ServerPark);
        }

        //This next method refreshes the drop down list to make sure that any newly added interviewers appear on the drop down list
        private void cmbIntSelection_DropDownOpened(object sender, EventArgs e)
        {
            ObservableCollection<ITraineeAppRecordInterface> temp = new ObservableCollection<ITraineeAppRecordInterface>(AppController.Instance.TraineeAppDataSet);
            temp.Add(new TraineeAppRecord(AppController.Instance._selectedInstrument, " ", "View All", DateTime.ParseExact("01/01/2001", "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None), " ", " "," "));
            var res = temp
                        .GroupBy(x => x.Intno)
                        .Select(g => g.OrderByDescending(x => x.Intno).First());

            cmbIntSelection.ItemsSource = res;
        }
        private void cmbIntSelection_DropDownClosed(object sender, EventArgs e)
        {
            stringsearch.Clear();
            //string x = cmbIntSelection.Text;
            if (cmbIntSelection.SelectedItem is null)
            {
                stringsearch.Add("View All");
            }
            else
            {
                stringsearch.Add((cmbIntSelection.SelectedItem as TraineeAppRecord).Intno);
            }

            //string xx = (cmbIntSelection.SelectedItem as PerformanceRecord).Intno;
            //MessageBox.Show(string.Format("Primary Key= {0}", xx));
            CollectionViewSource.GetDefaultView(lvtraineeappDetails.ItemsSource).Refresh();
        }



        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
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
                            TimeSpan timesincelastsync = DateTime.Now - AppController.Instance._synctime;
                            //MessageBox.Show(string.Format("{0}", timesincelastsync.Hours) );
                            if (timesincelastsync.Hours > 10)
                            {
                                ev_repaintsynchronising_buttons();
                                Task.Delay(1000).ContinueWith(t => Runsync("RESEARCH"));
                            }

                        }
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
                            TimeSpan timesincelastsync = DateTime.Now - AppController.Instance._synctime;
                            //MessageBox.Show(string.Format("{0}", timesincelastsync.Hours) );
                            if (timesincelastsync.Hours > 10)
                            {
                                ev_repaintsynchronising_buttons();
                                Task.Delay(1000).ContinueWith(t => Runsync("RESPOND"));
                            }

                        }
                    }
                }
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
    }





} //ends namespace
