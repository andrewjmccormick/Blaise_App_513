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
    /// Interaction logic for Current_Wages_Menu.xaml
    /// </summary>
    public partial class Current_Wages_Menu : UserControl
    {

        private GridViewColumnHeader listViewSortCol = null;
        private SortAdorner listViewSortAdorner = null;
        public delegate void dg_synchronised();
        public event dg_synchronised ev_repaintsynchronising_buttons;

        public Current_Wages_Menu()
        {
            InitializeComponent();

            lvcurrentwages.ItemsSource = AppController.Instance.CurrentWagesDataSet;
            lvcurrentwages.Items.SortDescriptions.Add(new SortDescription("WageDate", ListSortDirection.Ascending));
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvcurrentwages.ItemsSource);

            //view.Filter = UserFilter;


            this.ev_repaintsynchronising_buttons += new dg_synchronised(AppController.Instance.Window.surveysMain.EventSynchronisingMessage);
        
               
        }

        private void txtFilter_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(lvcurrentwages.ItemsSource).Refresh();
        }

        private void lvUsersColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = (sender as GridViewColumnHeader);
            string sortBy = column.Tag.ToString();
            if (listViewSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(listViewSortCol).Remove(listViewSortAdorner);
                lvcurrentwages.Items.SortDescriptions.Clear();
            }

            ListSortDirection newDir = ListSortDirection.Ascending;
            if (listViewSortCol == column && listViewSortAdorner.Direction == newDir)
                newDir = ListSortDirection.Descending;

            listViewSortCol = column;
            listViewSortAdorner = new SortAdorner(listViewSortCol, newDir);
            AdornerLayer.GetAdornerLayer(listViewSortCol).Add(listViewSortAdorner);
            lvcurrentwages.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
        }

        private void Current_Wages_Menu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedrecord = GetSelectedRecord();
        }


        private ICurrentWagesInterface GetSelectedRecord()
        {
            return lvcurrentwages.SelectedItem as ICurrentWagesInterface;
        }





        private void RunSurvey_Click(object sender, RoutedEventArgs e)
        {



           // var SelectedCase = GetSelectedRecord();
            AppController.Instance.ConnectLocallyr(AppController.Instance._currentwageslocation, false);
            AppController.Instance.currentenvironment = AppController.Instance._currentwageslocation;
            //MessageBox.Show(string.Format("I can press this {0} ",SelectedCase.Instrument));
            //if (SelectedCase != null)
            //{
                //Turn the button's IsCancel property on only if a case has been selected. This closes the survey cases list window.
                //tester.IsCancel = true;
                AppController.Instance._selectedInstrument = AppController.Instance._currentwagesinstrument;

                //MessageBox.Show(string.Format("I can press this {0}",SelectedCase.Primkey ));

                ILoadBalancer loadbalancer = DataEntryManager.GetLoadBalancer(AppController.Instance._currentwagesinstrument.RunMode);
                loadbalancer.ServerParkObtained += AppController.Instance.Loadbalancer_ServerParkObtained;

                //this statement puts a message up if the interviewer is showing as being more than a quarter of a mile away from the adress location (assuming GPS is available and the address has a POINTER location).


                if (string.IsNullOrEmpty(AppController.Instance._currentwagesinstrument.Host))
                AppController.Instance._currentwagesinstrument.Host = "localhost";
                loadbalancer.RequestServerPark(string.Format("http://{0}:{1}", AppController.Instance._currentwagesinstrument.Host, AppController.Instance._currentwagesinstrument.Port), AppController.Instance._currentwagesinstrument.ServerPark);


            //}
            //else
            //{
            //    MessageBox.Show("Please selected a month", " ", MessageBoxButton.OK, MessageBoxImage.Error);
            //}

        }


        private void ViewCases_Click(object sender, RoutedEventArgs e)
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


   


}
