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
//using System.Drawing;









namespace Blaise_App {
    /// <summary>
    /// Interaction logic for SurveyDetails.xaml
    /// </summary>
    public partial class SurveyDetails : UserControl
    {

        private GridViewColumnHeader listViewSortCol = null;
        private SortAdorner listViewSortAdorner = null;
        public delegate void dg_synchronised();
        public event dg_synchronised ev_repaintsynchronising_buttons;

        public SurveyDetails()
        {
            InitializeComponent();
            
            lvsurveyDetails.ItemsSource = AppController.Instance.FullDataSet;
            lvsurveyDetails.Items.SortDescriptions.Add(new SortDescription("Distance", ListSortDirection.Ascending));
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvsurveyDetails.ItemsSource);

            view.Filter = UserFilter;
            

            this.ev_repaintsynchronising_buttons += new dg_synchronised(AppController.Instance.Window.surveysMain.EventSynchronisingMessage);

            ObservableCollection<IFullRecordInterface> temp = new ObservableCollection<IFullRecordInterface>(AppController.Instance.FullDataSet);
            temp.Add(new FullRecord(AppController.Instance._selectedInstrument, " ", " ", " ", " ", "View All", " ", 0, 0, " ", " ", " ", DateTime.ParseExact("01/01/2001", "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None), DateTime.ParseExact("01/01/2001", "d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None)," ", " ", 0, " ", " ",
                    " ", " ", " ", " ", 0, 0, " ", " "," "));
            var res = temp
                        .GroupBy(x => x.SurveyShort)
                        .Select(g => g.OrderByDescending(x => x.SurveyShort).First());

            cmbSurveySelection.ItemsSource = res;
            RadioButton rb = Still_to_do;
           
          
            Still_to_do.IsChecked = true;

         



        }
        public List<string> stringsearchoutcome = new List<string> { " " };
        public List<string> stringsearchdropdown = new List<string> { "View All" };
        public List<string> stringsearchpractice = new List<string> { "Active" };

        private bool UserFilter(object item)
        {
            string xoutcome = (item as FullRecord).Outcome;
            string xdropdown = (item as FullRecord).SurveyShort;
            string xpractice = (item as FullRecord).Practice;



            if ((stringsearchoutcome.Contains(xoutcome)) && (stringsearchdropdown.Contains(xdropdown)) && (stringsearchpractice.Contains(xpractice)))
                return true;
            if ((stringsearchoutcome.Contains(xoutcome)) && (stringsearchdropdown.Contains("View All")) && (stringsearchpractice.Contains(xpractice)))
                return true;


            else
                return false;


            //return ((item as FullRecord).Outcome.IndexOf(filtervar, StringComparison.OrdinalIgnoreCase) >= 0);
            //return ((item as FullRecord).Outcome.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void txtFilter_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(lvsurveyDetails.ItemsSource).Refresh();
        }



        private void All_Cases_Click(object sender, RoutedEventArgs e)
        {

          
            stringsearchoutcome.Clear();
            stringsearchoutcome.Add(" ");
            stringsearchoutcome.Add("Complete");
            stringsearchoutcome.Add("Reallocation");
            stringsearchoutcome.Add("Non-Contact");
            stringsearchoutcome.Add("Refusal");
            stringsearchoutcome.Add("Other Non-Response");
            stringsearchoutcome.Add("Unknown Eligibility");
            stringsearchoutcome.Add("Ineligible");
            //stringsearchoutcome.Add("All Cases");
            CollectionViewSource.GetDefaultView(lvsurveyDetails.ItemsSource).Refresh();
        }
        private void Still_To_Do_Click(object sender, RoutedEventArgs e)
        {

            stringsearchoutcome.Clear();
            stringsearchoutcome.Add(" ");
            CollectionViewSource.GetDefaultView(lvsurveyDetails.ItemsSource).Refresh();
        }

        private void Complete_Click(object sender, RoutedEventArgs e)
        {

            stringsearchoutcome.Clear();
            stringsearchoutcome.Add("Complete");
            CollectionViewSource.GetDefaultView(lvsurveyDetails.ItemsSource).Refresh();
        }
        private void Reallocations_Click(object sender, RoutedEventArgs e)
        {
            stringsearchoutcome.Clear();
            stringsearchoutcome.Add("Reallocation");
            CollectionViewSource.GetDefaultView(lvsurveyDetails.ItemsSource).Refresh();
        }

        private void Non_Response_Click(object sender, RoutedEventArgs e)
        {
            stringsearchoutcome.Clear();
            stringsearchoutcome.Add("Non-Contact");
            stringsearchoutcome.Add("Refusal");
            stringsearchoutcome.Add("Other Non-Response");
            stringsearchoutcome.Add("Unknown Eligibility");
            stringsearchoutcome.Add("Ineligible");
            CollectionViewSource.GetDefaultView(lvsurveyDetails.ItemsSource).Refresh();
        }

        private void cmbSurveySelection_DropDownClosed(object sender, EventArgs e)
        {
            stringsearchdropdown.Clear();
            //string x = cmbIntSelection.Text;
            if (cmbSurveySelection.SelectedItem is null)
            {
                stringsearchdropdown.Add("View All");
            }
            else
            {
                stringsearchdropdown.Add((cmbSurveySelection.SelectedItem as FullRecord).SurveyShort);
            }

            //string xx = (cmbIntSelection.SelectedItem as ProgressRecord).Intno;
            //MessageBox.Show(string.Format("Primary Key= {0}", xx));
            CollectionViewSource.GetDefaultView(lvsurveyDetails.ItemsSource).Refresh();
        }
        private void View_Practice_Only(object sender, RoutedEventArgs e)
        {
            stringsearchpractice.Clear();
            stringsearchpractice.Add("Practice");
            CollectionViewSource.GetDefaultView(lvsurveyDetails.ItemsSource).Refresh();

        }
        private void View_Active_Only(object sender, RoutedEventArgs e)
        {
            stringsearchpractice.Clear();
            stringsearchpractice.Add("Active");
            CollectionViewSource.GetDefaultView(lvsurveyDetails.ItemsSource).Refresh();

        }

        private void lvUsersColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = (sender as GridViewColumnHeader);
            string sortBy = column.Tag.ToString();
            if (listViewSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(listViewSortCol).Remove(listViewSortAdorner);
                lvsurveyDetails.Items.SortDescriptions.Clear();
            }

            ListSortDirection newDir = ListSortDirection.Ascending;
            if (listViewSortCol == column && listViewSortAdorner.Direction == newDir)
                newDir = ListSortDirection.Descending;

            listViewSortCol = column;
            listViewSortAdorner = new SortAdorner(listViewSortCol, newDir);
            AdornerLayer.GetAdornerLayer(listViewSortCol).Add(listViewSortAdorner);
            lvsurveyDetails.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
        }

        private void SurveyDetails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedrecord = GetSelectedRecord();
        }


        private IFullRecordInterface GetSelectedRecord()
        {
            return lvsurveyDetails.SelectedItem as IFullRecordInterface;
        }

        private string ConstructWindowsPassword()
        {
            string path = @"c:\id.bat";
            string usertxt = " ";
            string userpassword = " ";
            if (System.IO.File.Exists(path))
            {
                string line;
                // Read the text file contianing survey name and Guid of surveys to be deleted.  

                System.IO.StreamReader file =
                new System.IO.StreamReader(path);
                while ((line = file.ReadLine()) != null)
                {
                    string[] parts = line.Split('T');
                    usertxt = parts[3];
                }
                file.Close();
                userpassword = "Int" + usertxt + "a";
            }
            return userpassword;
        }





        private void RunSurvey_Click(object sender, RoutedEventArgs e)
        {

          
            var SelectedCase = GetSelectedRecord();
            //This next statement ensure that the program knows which environment is being used
            //MessageBox.Show(string.Format("Status. Current = {0} Required =  {1}", AppController.Instance.currentenvironment, SelectedCase.Environment));
            
            string InterviewerPassword = ConstructWindowsPassword();
            if (SelectedCase != null)
            {
                AppController.Instance.currentenvironment = SelectedCase.Environment;
                AppController.Instance.ConnectLocallyr(SelectedCase.Environment, false);
                //Turn the button's IsCancel property on only if a case has been selected. This closes the survey cases list window.
                //tester.IsCancel = true;
                AppController.Instance._selectedInstrument = SelectedCase.Instrument;
                //AppController.Instance._serno = SelectedCase.Serno;
                //AppController.Instance._hhno = SelectedCase.Hhno;
                //AppController.Instance._intno = SelectedCase.Intno;
                AppController.Instance._primkey = SelectedCase.Primkey;

                //MessageBox.Show(string.Format("I can press this {0}",SelectedCase.Primkey ));
               
                ILoadBalancer loadbalancer = DataEntryManager.GetLoadBalancer(SelectedCase.Instrument.RunMode);
                //MessageBox.Show(string.Format("RunMode {0}", SelectedCase.Instrument.RunMode.ToString()));
                loadbalancer.ServerParkObtained += AppController.Instance.Loadbalancer_ServerParkObtainedSpecific;

                //this statement puts a message up if the interviewer is showing as being more than a quarter of a mile away from the adress location (assuming GPS is available and the address has a POINTER location).
                if (SelectedCase.Distance > 0.25 && AppController.Instance._txtLat != 0 && AppController.Instance._txtLong != 0 && SelectedCase.GridX != 0 && SelectedCase.GridY != 0)
                {
                    var result = MessageBox.Show(string.Format("It doesn't look like you are at {0}. \n\n Are you sure you want to open this case?",SelectedCase.AddStrt), "Location Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Information);
                    //MessageBox.Show(string.Format("Primary Key= {0}, Serial Number = {1}, Hhno = {2}, intno = {3} ", primkey, serno, hhno, intno));
                    if (result == MessageBoxResult.Yes)
                    {
                        if (SelectedCase.Outcome == "Complete")
                        {
                            string RealIntpass = ConstructWindowsPassword();
                            var pword = new IntPassword() { Owner = AppController.Instance.Window, WindowState = AppController.Instance.Window.WindowState, Icon = AppController.Instance.Window.Icon, WindowStartupLocation = AppController.Instance.Window.WindowStartupLocation, Title = "Password Confirmation" };
                            pword.ShowDialog();
                            if (AppController.Instance._intPass == RealIntpass)
                            {
                                if (string.IsNullOrEmpty(SelectedCase.Instrument.Host))
                                    SelectedCase.Instrument.Host = "localhost";
                                loadbalancer.RequestServerPark(string.Format("http://{0}:{1}", SelectedCase.Instrument.Host, SelectedCase.Instrument.Port), SelectedCase.Instrument.ServerPark);
                            }
                            else
                            {
                                MessageBox.Show("Incorrect password", "Location Confirmation", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            AppController.Instance._intPass = " ";
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(SelectedCase.Instrument.Host))
                                SelectedCase.Instrument.Host = "localhost";
                            loadbalancer.RequestServerPark(string.Format("http://{0}:{1}", SelectedCase.Instrument.Host, SelectedCase.Instrument.Port), SelectedCase.Instrument.ServerPark);
                        }
                    }
                }
                else
                {
                    if (SelectedCase.Outcome == "Complete")
                    {
                        string RealIntpass = ConstructWindowsPassword();
                        var pword = new IntPassword() { Owner = AppController.Instance.Window, ResizeMode = ResizeMode.NoResize, WindowStartupLocation= WindowStartupLocation.CenterScreen, Icon = AppController.Instance.Window.Icon, Title = "Enter your Password" };
                        pword.ShowDialog();
                        if (AppController.Instance._intPass == RealIntpass)
                        {
                            if (string.IsNullOrEmpty(SelectedCase.Instrument.Host))
                                SelectedCase.Instrument.Host = "localhost";
                            loadbalancer.RequestServerPark(string.Format("http://{0}:{1}", SelectedCase.Instrument.Host, SelectedCase.Instrument.Port), SelectedCase.Instrument.ServerPark);
                        }
                        else if (AppController.Instance._intPass == "Cancel")
                        {
                        }
                        else
                        {
                            MessageBox.Show("Incorrect password", "Location Confirmation", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        AppController.Instance._intPass = " ";
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(SelectedCase.Instrument.Host))
                            SelectedCase.Instrument.Host = "localhost";
                        //MessageBox.Show(string.Format("host= {0}, port = {1}, Server Park = {2}", SelectedCase.Instrument.Host, SelectedCase.Instrument.Port, SelectedCase.Instrument.ServerPark));
                        loadbalancer.RequestServerPark(string.Format("http://{0}:{1}", SelectedCase.Instrument.Host, SelectedCase.Instrument.Port), SelectedCase.Instrument.ServerPark);
                    }

                }
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



    public class SortAdorner : Adorner
    {
        private static Geometry ascGeometry =
            Geometry.Parse("M 0 4 L 3.5 0 L 7 4 Z");
        private static Geometry descGeometry =
            Geometry.Parse("M 0 0 L 3.5 4 L 7 0 Z");

        public ListSortDirection Direction { get; private set; }

        public SortAdorner(UIElement element, ListSortDirection dir)
            : base(element)
        {
            this.Direction = dir;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (AdornedElement.RenderSize.Width < 20)
                return;

            TranslateTransform transform = new TranslateTransform
                (
                    AdornedElement.RenderSize.Width - 15,
                    (AdornedElement.RenderSize.Height - 5) / 2
                );
            drawingContext.PushTransform(transform);

            Geometry geometry = ascGeometry;
            if (this.Direction == ListSortDirection.Descending)
                geometry = descGeometry;
            drawingContext.DrawGeometry(Brushes.Black, null, geometry);

            drawingContext.Pop();
        }
    }




    

   











} //ends namespace
