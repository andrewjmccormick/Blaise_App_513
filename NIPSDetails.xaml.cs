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
using System.Security;










namespace Blaise_App
{
    /// <summary>
    /// Interaction logic for SurveyDetails.xaml
    /// </summary>
    public partial class NIPSDetails : UserControl
    {

        private GridViewColumnHeader listViewSortCol = null;
        private SortAdorner listViewSortAdorner = null;
        public delegate void dg_synchronised();
        public event dg_synchronised ev_repaintsynchronising_buttons;

        public NIPSDetails()
        {
            InitializeComponent();
            lvNIPSDetails.ItemsSource = AppController.Instance.NIPSDetailsDataSet;
            lvNIPSDetails.Items.SortDescriptions.Add(new SortDescription("Intdate", ListSortDirection.Descending));
            lvNIPSDetails.Items.SortDescriptions.Add(new SortDescription("TStart", ListSortDirection.Descending));

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvNIPSDetails.ItemsSource);

           // view.Filter = UserFilter;


            this.ev_repaintsynchronising_buttons += new dg_synchronised(AppController.Instance.Window.surveysMain.EventSynchronisingMessage);





        }
        //public List<string> stringsearchoutcome = new List<string> { " " };
        //public List<string> stringsearchdropdown = new List<string> { "View All" };
        //public List<string> stringsearchpractice = new List<string> { "Active" };

        //private bool UserFilter(object item)
        //{
        //    string xoutcome = (item as FullRecord).Outcome;
        //    string xdropdown = (item as FullRecord).SurveyShort;
        //    string xpractice = (item as FullRecord).Practice;



        //    if ((stringsearchoutcome.Contains(xoutcome)) && (stringsearchdropdown.Contains(xdropdown)) && (stringsearchpractice.Contains(xpractice)))
        //        return true;
        //    if ((stringsearchoutcome.Contains(xoutcome)) && (stringsearchdropdown.Contains("View All")) && (stringsearchpractice.Contains(xpractice)))
        //        return true;


        //    else
        //        return false;


        //    //return ((item as FullRecord).Outcome.IndexOf(filtervar, StringComparison.OrdinalIgnoreCase) >= 0);
        //    //return ((item as FullRecord).Outcome.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        //}

        //private void txtFilter_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        //{
        //    CollectionViewSource.GetDefaultView(lvsurveyDetails.ItemsSource).Refresh();
        //}



        //private void All_Cases_Click(object sender, RoutedEventArgs e)
        //{
        //    stringsearchoutcome.Clear();
        //    stringsearchoutcome.Add(" ");
        //    stringsearchoutcome.Add("Complete");
        //    stringsearchoutcome.Add("Reallocation");
        //    stringsearchoutcome.Add("Non-Contact");
        //    stringsearchoutcome.Add("Refusal");
        //    stringsearchoutcome.Add("Other Non-Response");
        //    stringsearchoutcome.Add("Unknown Eligibility");
        //    stringsearchoutcome.Add("Ineligible");
        //    //stringsearchoutcome.Add("All Cases");
        //    CollectionViewSource.GetDefaultView(lvsurveyDetails.ItemsSource).Refresh();
        //}
        //private void Still_To_Do_Click(object sender, RoutedEventArgs e)
        //{
        //    stringsearchoutcome.Clear();
        //    stringsearchoutcome.Add(" ");
        //    CollectionViewSource.GetDefaultView(lvsurveyDetails.ItemsSource).Refresh();
        //}

        //private void Complete_Click(object sender, RoutedEventArgs e)
        //{
        //    stringsearchoutcome.Clear();
        //    stringsearchoutcome.Add("Complete");
        //    CollectionViewSource.GetDefaultView(lvsurveyDetails.ItemsSource).Refresh();
        //}
        //private void Reallocations_Click(object sender, RoutedEventArgs e)
        //{
        //    stringsearchoutcome.Clear();
        //    stringsearchoutcome.Add("Reallocation");
        //    CollectionViewSource.GetDefaultView(lvsurveyDetails.ItemsSource).Refresh();
        //}

        //private void Non_Response_Click(object sender, RoutedEventArgs e)
        //{
        //    stringsearchoutcome.Clear();
        //    stringsearchoutcome.Add("Non-Contact");
        //    stringsearchoutcome.Add("Refusal");
        //    stringsearchoutcome.Add("Other Non-Response");
        //    stringsearchoutcome.Add("Unknown Eligibility");
        //    stringsearchoutcome.Add("Ineligible");
        //    CollectionViewSource.GetDefaultView(lvsurveyDetails.ItemsSource).Refresh();
        //}

        //private void cmbSurveySelection_DropDownClosed(object sender, EventArgs e)
        //{
        //    stringsearchdropdown.Clear();
        //    //string x = cmbIntSelection.Text;
        //    if (cmbSurveySelection.SelectedItem is null)
        //    {
        //        stringsearchdropdown.Add("View All");
        //    }
        //    else
        //    {
        //        stringsearchdropdown.Add((cmbSurveySelection.SelectedItem as FullRecord).SurveyShort);
        //    }

        //    //string xx = (cmbIntSelection.SelectedItem as ProgressRecord).Intno;
        //    //MessageBox.Show(string.Format("Primary Key= {0}", xx));
        //    CollectionViewSource.GetDefaultView(lvsurveyDetails.ItemsSource).Refresh();
        //}
        //private void View_Practice_Only(object sender, RoutedEventArgs e)
        //{
        //    stringsearchpractice.Clear();
        //    stringsearchpractice.Add("Practice");
        //    CollectionViewSource.GetDefaultView(lvsurveyDetails.ItemsSource).Refresh();

        //}
        //private void View_Active_Only(object sender, RoutedEventArgs e)
        //{
        //    stringsearchpractice.Clear();
        //    stringsearchpractice.Add("Active");
        //    CollectionViewSource.GetDefaultView(lvsurveyDetails.ItemsSource).Refresh();

        //}

        private void lvUsersColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = (sender as GridViewColumnHeader);
            string sortBy = column.Tag.ToString();
            if (listViewSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(listViewSortCol).Remove(listViewSortAdorner);
                lvNIPSDetails.Items.SortDescriptions.Clear();
            }

            ListSortDirection newDir = ListSortDirection.Ascending;
            if (listViewSortCol == column && listViewSortAdorner.Direction == newDir)
                newDir = ListSortDirection.Descending;

            listViewSortCol = column;
            listViewSortAdorner = new SortAdorner(listViewSortCol, newDir);
            AdornerLayer.GetAdornerLayer(listViewSortCol).Add(listViewSortAdorner);
            lvNIPSDetails.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
        }

        private void NIPSDetails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedrecord = GetSelectedRecord();
        }


        private INIPSDetailsInterface GetSelectedRecord()
        {
            return lvNIPSDetails.SelectedItem as INIPSDetailsInterface;
        }







        private void RunSurvey_Click(object sender, RoutedEventArgs e)
        {
            var SelectedCase = GetSelectedRecord();


            if (SelectedCase != null)
            {
                AppController.Instance.ConnectLocallyr(SelectedCase.Environment, false);
                AppController.Instance.currentenvironment = SelectedCase.Environment;
                AppController.Instance._selectedInstrument = SelectedCase.Instrument;
                //AppController.Instance._serno = SelectedCase.Serno;
                //AppController.Instance._intno = SelectedCase.Intno;
                //AppController.Instance._intdate = SelectedCase.Intdate;
                //AppController.Instance._site = SelectedCase.Site;
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
