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
using MetaAPI = StatNeth.Blaise.API.Meta;
using DataRecordAPI = StatNeth.Blaise.API.DataRecord;
using DataLinkAPI = StatNeth.Blaise.API.DataLink;










namespace Blaise_App
{
    /// <summary>
    /// Interaction logic for NIPSMenu.xaml
    /// </summary>
    public partial class NIPSMenu : UserControl
    {

        private GridViewColumnHeader listViewSortCol = null;
        private SortAdorner listViewSortAdorner = null;
        public delegate void dg_synchronised();
        public event dg_synchronised ev_repaintsynchronising_buttons;

        public NIPSMenu()
        {
            InitializeComponent();
            //DataContext = AppController.Instance.NIPSMenuDataSet;
            lvNIPSMenuDetails.ItemsSource = AppController.Instance.NIPSMenuDataSet;
            lvNIPSMenuDetails.Items.SortDescriptions.Add(new SortDescription("NIPSMonthNo", ListSortDirection.Descending));
       

            this.ev_repaintsynchronising_buttons += new dg_synchronised(AppController.Instance.Window.surveysMain.EventSynchronisingMessage);
            //lvNIPSMenuDetails.SelectedItem = AppController.Instance.NIPSMenuDataSet.FirstOrDefault();


            if (AppController.Instance._connectedtoSurveys == true)
            {
                //MessageBox.Show(string.Format("Connected to Surveys= {0} CurrentEnvironment =  {1}", AppController.Instance._connectedtoSurveys, AppController.Instance.currentenvironment));
                AppController.Instance.SetData(AppController.Instance.InstalledonSurveys.Instruments, true, "C:\\B5SURVEYS");
            }
            else if (AppController.Instance._connectedtoRespond == true)
            {
                //MessageBox.Show(string.Format("Connected to Respond= {0} CurrentEnvironment =  {1}", AppController.Instance._connectedtoRespond, AppController.Instance.currentenvironment));
                AppController.Instance.SetData(AppController.Instance.InstalledonRespond.Instruments, true, "C:\\B5RESPOND");
            }













           // if (AppController.Instance.currentenvironment == "C:\\B5SURVEYS")
           // {
           //     AppController.Instance.SetData(AppController.Instance.InstalledonSurveys.Instruments,true,"SURVEYS");
           // }
        }
  
  
     

        private void lvUsersColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = (sender as GridViewColumnHeader);
            string sortBy = column.Tag.ToString();
            if (listViewSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(listViewSortCol).Remove(listViewSortAdorner);
                lvNIPSMenuDetails.Items.SortDescriptions.Clear();
            }

            ListSortDirection newDir = ListSortDirection.Ascending;
            if (listViewSortCol == column && listViewSortAdorner.Direction == newDir)
                newDir = ListSortDirection.Descending;

            listViewSortCol = column;
            listViewSortAdorner = new SortAdorner(listViewSortCol, newDir);
            AdornerLayer.GetAdornerLayer(listViewSortCol).Add(listViewSortAdorner);
            lvNIPSMenuDetails.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
        }

        private void NIPSMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedrecord = GetSelectedRecord();

            //Create an IEnumerable IInstrumentInfo with only the eselected survey in it - which will then get populated 
            //InstrumentInfoLoadedEventArgs data = AppController.Instance.InstalledSurveys;
            //IEnumerable<IInstrumentInfo> f =
            //    data.Instruments.Where(g => g.Name == selectedrecord.Instrument.Name);
            //foreach (IInstrumentInfo A in f)
            //    MessageBox.Show(string.Format("I can press this {0} ",A.Name));
            //SetDataNIPS(AppController.Instance.InstalledSurveys.Instruments);

        }


        private INIPSMenuInterface GetSelectedRecord()
        {

            return lvNIPSMenuDetails.SelectedItem as INIPSMenuInterface;
        }

    





        private void RunSurvey_Click(object sender, RoutedEventArgs e)
        {
            var SelectedCase = GetSelectedRecord();
            AppController.Instance.ConnectLocallyr(SelectedCase.Environment, false);
            AppController.Instance.currentenvironment = SelectedCase.Environment;
            //MessageBox.Show(string.Format("I can press this {0} ",SelectedCase.Instrument));
            if (SelectedCase != null)
            {
                //Turn the button's IsCancel property on only if a case has been selected. This closes the survey cases list window.
                //tester.IsCancel = true;
                AppController.Instance._selectedInstrument = SelectedCase.Instrument;

                //MessageBox.Show(string.Format("I can press this {0}",SelectedCase.Primkey ));

                ILoadBalancer loadbalancer = DataEntryManager.GetLoadBalancer(SelectedCase.Instrument.RunMode);
                loadbalancer.ServerParkObtained += AppController.Instance.Loadbalancer_ServerParkObtained;

                //this statement puts a message up if the interviewer is showing as being more than a quarter of a mile away from the adress location (assuming GPS is available and the address has a POINTER location).

                
                if (string.IsNullOrEmpty(SelectedCase.Instrument.Host))
                    SelectedCase.Instrument.Host = "localhost";
                loadbalancer.RequestServerPark(string.Format("http://{0}:{1}", SelectedCase.Instrument.Host, SelectedCase.Instrument.Port), SelectedCase.Instrument.ServerPark);
                    
                
            }
            else
            {
                MessageBox.Show("Please selected a month", " ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ViewCases_Click(object sender, RoutedEventArgs e)
        {
            //var SelectedCase = GetSelectedRecord();

            ////Create an IEnumerable IInstrumentInfo with only the eselected survey in it - which will then get populated 
            //InstrumentInfoLoadedEventArgs data = AppController.Instance.InstalledSurveys;
            //IEnumerable<IInstrumentInfo> f =
            //    data.Instruments.Where(g => g.Name == SelectedCase.Instrument.Name);

            
            

            //AppController.Instance.SetData(AppController.Instance.InstalledSurveys.Instruments, true);
            //MessageBox.Show(string.Format("I am in "));
            var c = new NIPSDetails() { DataContext = AppController.Instance.NIPSDetailsDataSet };
            //var c = new SurveyDetails();
            //            var c = new SurveyDetails();
            var d = new Window() { Owner = AppController.Instance.Window, ResizeMode = ResizeMode.NoResize, WindowState = AppController.Instance.Window.WindowState, Icon = AppController.Instance.Window.Icon, WindowStartupLocation = AppController.Instance.Window.WindowStartupLocation, Content = c, Title = "Browse cases" };
            d.ShowDialog();
        }

        private void CountCases_Click(object sender, RoutedEventArgs e)
        {
            String Username = AppController.Instance.GetUsername();
            DateTime dt = DateTime.Today;
            string monthyear = String.Format("{0:yyMM}",dt);
            string strDataModel = " ";
            string strDataInterface = " ";
            //if (AppController.Instance._shiftdetailslocation == "C:\\B5SURVEYS")
            if (AppController.Instance.currentenvironment == "C:\\B5SURVEYS")
                {

                    string strDataModela = Properties.Settings.Default.DeployFolder + "\\" + Username + "\\surveys\\NIPS" + monthyear + "a\\NIPS" + monthyear + "a.bmix";
                string strDataInterfacea = Properties.Settings.Default.DeployFolder + "\\" + Username + "\\surveys\\NIPS" + monthyear + "a\\NIPS" + monthyear + "a.bdix";
                string strDataModelb = Properties.Settings.Default.DeployFolder + "\\" + Username + "\\surveys\\NIPS" + monthyear + "b\\NIPS" + monthyear + "b.bmix";
                string strDataInterfaceb = Properties.Settings.Default.DeployFolder + "\\" + Username + "\\surveys\\NIPS" + monthyear + "b\\NIPS" + monthyear + "b.bdix";
                string strDataModelc = Properties.Settings.Default.DeployFolder + "\\" + Username + "\\surveys\\NIPS" + monthyear + "c\\NIPS" + monthyear + "c.bmix";
                string strDataInterfacec = Properties.Settings.Default.DeployFolder + "\\" + Username + "\\surveys\\NIPS" + monthyear + "c\\NIPS" + monthyear + "c.bdix";


                strDataModel = Properties.Settings.Default.DeployFolder + "\\" + Username + "\\surveys\\NIPS" + monthyear + "a\\NIPS" + monthyear + "a.bmix";
                strDataInterface = Properties.Settings.Default.DeployFolder + "\\" + Username + "\\surveys\\NIPS" + monthyear + "a\\NIPS" + monthyear + "a.bdix";

                if (System.IO.File.Exists(strDataInterfacec))
                {
                    strDataModel = Properties.Settings.Default.DeployFolder + "\\" + Username + "\\surveys\\NIPS" + monthyear + "c\\NIPS" + monthyear + "c.bmix";
                    strDataInterface = Properties.Settings.Default.DeployFolder + "\\" + Username + "\\surveys\\NIPS" + monthyear + "c\\NIPS" + monthyear + "c.bdix";
                }
                else if (System.IO.File.Exists(strDataInterfaceb))
                {
                    strDataModel = Properties.Settings.Default.DeployFolder + "\\" + Username + "\\surveys\\NIPS" + monthyear + "b\\NIPS" + monthyear + "b.bmix";
                    strDataInterface = Properties.Settings.Default.DeployFolder + "\\" + Username + "\\surveys\\NIPS" + monthyear + "b\\NIPS" + monthyear + "b.bdix";

                }
            }
            //else if (AppController.Instance._shiftdetailslocation == "C:\\B5RESPOND")
            else if (AppController.Instance.currentenvironment == "C:\\B5RESPOND")
                {

                    string strDataModela = Properties.Settings.Default.DeployFolderRespond + "\\" + Username + "\\surveys\\NIPS" + monthyear + "a\\NIPS" + monthyear + "a.bmix";
                string strDataInterfacea = Properties.Settings.Default.DeployFolderRespond + "\\" + Username + "\\surveys\\NIPS" + monthyear + "a\\NIPS" + monthyear + "a.bdix";
                string strDataModelb = Properties.Settings.Default.DeployFolderRespond + "\\" + Username + "\\surveys\\NIPS" + monthyear + "b\\NIPS" + monthyear + "b.bmix";
                string strDataInterfaceb = Properties.Settings.Default.DeployFolderRespond + "\\" + Username + "\\surveys\\NIPS" + monthyear + "b\\NIPS" + monthyear + "b.bdix";
                string strDataModelc = Properties.Settings.Default.DeployFolderRespond + "\\" + Username + "\\surveys\\NIPS" + monthyear + "c\\NIPS" + monthyear + "c.bmix";
                string strDataInterfacec = Properties.Settings.Default.DeployFolderRespond + "\\" + Username + "\\surveys\\NIPS" + monthyear + "c\\NIPS" + monthyear + "c.bdix";


                strDataModel = Properties.Settings.Default.DeployFolderRespond + "\\" + Username + "\\surveys\\NIPS" + monthyear + "a\\NIPS" + monthyear + "a.bmix";
                strDataInterface = Properties.Settings.Default.DeployFolderRespond + "\\" + Username + "\\surveys\\NIPS" + monthyear + "a\\NIPS" + monthyear + "a.bdix";

                if (System.IO.File.Exists(strDataInterfacec))
                {
                    strDataModel = Properties.Settings.Default.DeployFolderRespond + "\\" + Username + "\\surveys\\NIPS" + monthyear + "c\\NIPS" + monthyear + "c.bmix";
                    strDataInterface = Properties.Settings.Default.DeployFolderRespond + "\\" + Username + "\\surveys\\NIPS" + monthyear + "c\\NIPS" + monthyear + "c.bdix";
                }
                else if (System.IO.File.Exists(strDataInterfaceb))
                {
                    strDataModel = Properties.Settings.Default.DeployFolderRespond + "\\" + Username + "\\surveys\\NIPS" + monthyear + "b\\NIPS" + monthyear + "b.bmix";
                    strDataInterface = Properties.Settings.Default.DeployFolderRespond + "\\" + Username + "\\surveys\\NIPS" + monthyear + "b\\NIPS" + monthyear + "b.bdix";

                }
            }



            //                string strDataModel = Properties.Settings.Default.DeployFolder + "\\surveys\\" +  SelectedCase.Instrument.Name + "\\" + SelectedCase.Instrument.Name + ".bmix";
            //               string strDataInterface = Properties.Settings.Default.DeployFolder + "\\surveys\\" + SelectedCase.Instrument.Name + "\\" + SelectedCase.Instrument.Name + ".bdix";

            if (System.IO.File.Exists(strDataInterface))
            {
                MetaAPI.IDatamodel dm = MetaAPI.MetaManager.GetDatamodel(strDataModel);
                DataLinkAPI.IDataLink dl = DataLinkAPI.DataLinkManager.GetDataLink(strDataInterface, dm);
                DataRecordAPI.IKey key = GetPrimaryKey(dm);
                DataLinkAPI.IDataSet ds = dl.Read(key, DataLinkAPI.ReadOrder.Ascending, 300, true);
                if (ds != null && ds.RecordCount > 0)
                {
                    Int32 SurveyCount = 0;
                    // Typical way to iterate through an IDataSet:
                    while (!ds.EndOfSet)
                    {
                        DataRecordAPI.IDataRecord dr = ds.ActiveRecord;
                        if ((dr.GetField("admin1.intdate").DataValue.DateValue) == DateTime.Today)
                        {
                            SurveyCount = SurveyCount + 1;
                        }
                        ds.MoveNext();
                    }
                    MessageBox.Show(string.Format("Number of interviews completed today = {0}", SurveyCount));
                }
                else
                {
                    MessageBox.Show(string.Format("Number of interviews completed today erroe = 0"));
                }
            }
            else
            {
                MessageBox.Show(string.Format("Number of interviews completed today error= 0"));
            }

        }


        private void ShiftReport_Click(object sender, RoutedEventArgs e)
        {
            var c = new ShiftDetails() { DataContext = AppController.Instance.ShiftDetailsDataSet };
            //var c = new SurveyDetails();
            //            var c = new SurveyDetails();
            var d = new Window() { Owner = AppController.Instance.Window, ResizeMode = ResizeMode.NoResize, WindowState = AppController.Instance.Window.WindowState, Icon = AppController.Instance.Window.Icon, WindowStartupLocation = AppController.Instance.Window.WindowStartupLocation, Content = c, Title = "Field Trainer Menu" };
            d.ShowDialog();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {

            if (AppController.Instance._connectedtoSurveys == true)
            {
                bool connected = AppController.Instance.Connect("C:\\B5Surveys", "surveys.nisra.gov.uk", false, false);
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
                                Task.Delay(1000).ContinueWith(t => Runsync("SURVEYS"));
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
            if (Environment == "SURVEYS")
            {
                //MessageBox.Show(string.Format("Connected to Surveys= {0} CurrentEnvironment =  {1}", AppController.Instance._connectedtoSurveys, AppController.Instance.currentenvironment));
                AppController.Instance.Looper(AppController.Instance.InstalledonSurveys.Instruments, false, "SURVEYS");
            }
            else if (Environment == "RESPOND")
            {
                //MessageBox.Show(string.Format("Connected to Respond= {0} CurrentEnvironment =  {1}", AppController.Instance._connectedtoRespond, AppController.Instance.currentenvironment));
                AppController.Instance.Looper(AppController.Instance.InstalledonRespond.Instruments, false, "RESPOND");
            }




        }





        private DataRecordAPI.IKey GetPrimaryKey(MetaAPI.IDatamodel dm)
        {
            // Get an IKey interface for the primary key:
            DataRecordAPI.IKey result = DataRecordAPI.DataRecordManager.GetKey(dm, MetaAPI.Constants.KeyNames.Primary);
            return result;
        }

       

        //public void SetDataNIPS(IEnumerable<IInstrumentInfo> data)
        //{
        //    if (data.Count() > 0)
        //    {
        //        // We need to clear the values out of DataSet each time so that the list of cases doesn't get duplicated 

        //        AppController.Instance.NIPSDetailsDataSet.Clear();

        //        IEnumerable<IInstrumentInfo> InstalledInstruments =
        //            InstalledInstruments = data.Where(s => s.InstrumentState == InstrumentState.InstalledOnClient);
        //        foreach (IInstrumentInfo InstalledInstrument in InstalledInstruments)
        //        {
        //            if (InstalledInstrument.Name.ToUpper().Substring(0, 4) == "NIPS")
        //            {
        //                string strDataModel = Properties.Settings.Default.DeployFolder + "\\surveys\\" + InstalledInstrument.Name + "\\" + InstalledInstrument.Name + ".bmix";
        //                string strDataInterface = Properties.Settings.Default.DeployFolder + "\\surveys\\" + InstalledInstrument.Name + "\\" + InstalledInstrument.Name + ".bdix";
        //                //When a survey is removed this section of code is automatically triggered and will try to run for the survey that is being deleted.
        //                if (System.IO.File.Exists(strDataInterface))
        //                {
        //                    MetaAPI.IDatamodel dm = MetaAPI.MetaManager.GetDatamodel(strDataModel);
        //                    DataLinkAPI.IDataLink dl = DataLinkAPI.DataLinkManager.GetDataLink(strDataInterface, dm);
        //                    DataRecordAPI.IKey key = GetPrimaryKey(dm);
        //                    DataLinkAPI.IDataSet ds = dl.Read(key, DataLinkAPI.ReadOrder.Ascending, 300, true);
        //                    //this next line captures the IInstrumentInfo details fo the Progress questionnaire so that it can be used to drive the visibility of the "View progress" button
                           

        //                    if (ds != null && ds.RecordCount > 0)
        //                    {
        //                        // Typical way to iterate through an IDataSet:
        //                        while (!ds.EndOfSet)
        //                        {
        //                            DataRecordAPI.IDataRecord dr = ds.ActiveRecord;
        //                            FillDataRecord(dr, InstalledInstrument);
        //                            ds.MoveNext();
        //                        }
        //                    }
        //                }
 
        //            }
        //        }
        //    }
        //}

        //private void FillDataRecord(DataRecordAPI.IDataRecord dr, IInstrumentInfo e)
        //{
        //    if ((dr.Keys.Contains("PRIMARY")) && (e.Name.ToUpper().Substring(0, 4) == "NIPS"))
        //    {
        //        var survey = e;
        //        DataRecordAPI.IKey tmpkey = dr.Keys.GetItem(MetaAPI.Constants.KeyNames.Primary);
        //        string Primkey = tmpkey.KeyValue;

        //        string Serno = " ";
        //        DataRecordAPI.IDataValue sernofield = null;
        //        try
        //        {
        //            sernofield = dr.GetField("admin1.Serno").DataValue;
        //            Serno = sernofield.ValueAsText;
        //        }
        //        catch { Serno = " "; }

        //        string Authno = " ";
        //        DataRecordAPI.IDataValue authnofield = null;
        //        try
        //        {
        //            authnofield = dr.GetField("admin1.authno").DataValue;
        //            Authno = authnofield.ValueAsText;
        //        }
        //        catch { Authno = " "; }

        //        string Site = " ";
        //        DataRecordAPI.IDataValue sitefield = null;
        //        try
        //        {
        //            sitefield = dr.GetField("admin1.site").DataValue;
        //            if (sitefield.ValueAsText != null)
        //            {
        //                if (sitefield.EnumerationValue == 1)
        //                {
        //                    Site = "BIA";
                            
        //                }
        //            }
        //             //   Site = sitefield.ValueAsText;
        //        }
        //        catch { Site = " "; }


        //        DateTime Intdate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
        //        try
        //        {
        //            DataRecordAPI.IDataValue Intdatefield = dr.GetField("admin1.Intdate").DataValue;
        //            string IntDatestr = Intdatefield.ValueAsText;
        //            Intdate = DateTime.ParseExact(IntDatestr, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None);
        //        }
        //        catch (Exception)
        //        {
        //            Intdate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
        //        }

        //        DateTime Tstart = DateTime.ParseExact("00:00", "HH:mm", null, System.Globalization.DateTimeStyles.None);
        //        try
        //        {
        //            DataRecordAPI.IDataValue Tstartfield = dr.GetField("times.tstart").DataValue;
        //            string Tstartstr = Tstartfield.ValueAsText;
        //            Tstart = DateTime.ParseExact(Tstartstr.Substring(0, 5), "HH:mm", null, System.Globalization.DateTimeStyles.None);
        //        }
        //        catch (Exception)
        //        {
        //            Tstart = DateTime.ParseExact("00:00", "HH:mm", null, System.Globalization.DateTimeStyles.None);
        //        }


        //        string Outcome = " ";
        //        DataRecordAPI.IDataValue outcomefield = null;
        //        try
        //        {
        //            outcomefield = dr.GetField("coding.outcome").DataValue;
        //            if (outcomefield.ValueAsText != null)
        //            {
        //                if (outcomefield.EnumerationValue == 2)
        //                {
        //                    Outcome = "Minimum";
        //                }
        //                else if (outcomefield.EnumerationValue == 3)
        //                {
        //                    Outcome = "Complete";
        //                }
        //                else if (outcomefield.EnumerationValue == 4)
        //                {
        //                    Outcome = "Day Visitor";
        //                }
        //                else if (outcomefield.EnumerationValue == 5)
        //                {
        //                    Outcome = "Refusal";
        //                }
        //                else if (outcomefield.EnumerationValue == 6)
        //                {
        //                    Outcome = "Non-Contact";
        //                }
        //                else if (outcomefield.EnumerationValue == 7)
        //                {
        //                    Outcome = "Non-Eligible";
        //                }
        //                else if (outcomefield.EnumerationValue == 8)
        //                {
        //                    Outcome = "Recross";
        //                }

        //            }
        //        }
        //        catch { Outcome = " "; }

        //        string Nation = " ";
        //        DataRecordAPI.IDataValue nationfield = null;
        //        try
        //        {
        //            nationfield = dr.GetField("screen.Nat").DataValue;
        //            if (nationfield.ValueAsText != null)
        //            {
        //                if (nationfield.EnumerationValue == 1)
        //                {
        //                    Nation = "2+ Countries";
        //                }
        //                else if (nationfield.EnumerationValue == 2)
        //                {
        //                    Nation = "NI";
        //                }
        //                else if (nationfield.EnumerationValue == 3)
        //                {
        //                    Nation = "ROI";
        //                }
        //                else if (nationfield.EnumerationValue == 4)
        //                {
        //                    Nation = "England";
        //                }
        //                else if (nationfield.EnumerationValue == 5)
        //                {
        //                    Nation = "Scotland";
        //                }
        //                else if (nationfield.EnumerationValue == 6)
        //                {
        //                    Nation = "Wales";
        //                }
        //                else if (nationfield.EnumerationValue == 7)
        //                {
        //                    Nation = "IoM / CH";
        //                }
        //                else if (nationfield.EnumerationValue == 8)
        //                {
        //                    Nation = "Other EU";
        //                }
        //                else if (nationfield.EnumerationValue == 9)
        //                {
        //                    Nation = "USA";
        //                }
        //                else if (nationfield.EnumerationValue == 10)
        //                {
        //                    Nation = "Canada";
        //                }
        //                else if (nationfield.EnumerationValue == 11)
        //                {
        //                    Nation = "Rest of World";
        //                }
        //            }
        //        }
        //        catch { Nation = " "; }

        //        Int32 NINights = 0;
        //        DataRecordAPI.IDataValue ninightsfield = null;
        //        try
        //        {
        //            ninightsfield = dr.GetField("screen.NInghts").DataValue;
        //            NINights = Convert.ToInt32(ninightsfield.IntegerValue);

        //        }
        //        catch { NINights = 0; }


        //        Int32 ROINights = 0;
        //        DataRecordAPI.IDataValue roinightsfield = null;
        //        try
        //        {
        //            roinightsfield = dr.GetField("screen.ROInghts").DataValue;
        //            ROINights = Convert.ToInt32(roinightsfield.IntegerValue);

        //        }
        //        catch { ROINights = 0; }

        //        string EstAge = " ";
        //        DataRecordAPI.IDataValue estagefield = null;
        //        try
        //        {
        //            estagefield = dr.GetField("coding.estage").DataValue;
        //            if (estagefield.ValueAsText != null)
                       
        //            {
        //                if (estagefield.EnumerationValue == 1)
        //                {
        //                    EstAge = "0-15";
        //                }
        //                else if (estagefield.EnumerationValue == 2)
        //                {
        //                    EstAge = "16-24";
        //                }
        //                else if (estagefield.EnumerationValue == 3)
        //                {
        //                    EstAge = "25-34";
        //                }
        //                else if (estagefield.EnumerationValue == 4)
        //                {
        //                    EstAge = "35-44";
        //                }
        //                else if (estagefield.EnumerationValue == 5)
        //                {
        //                    EstAge = "45-54";
        //                }
        //                else if (estagefield.EnumerationValue == 6)
        //                {
        //                    EstAge = "55-64";
        //                }
        //                else if (estagefield.EnumerationValue == 7)
        //                {
        //                    EstAge = "65-74";
        //                }
        //                else if (estagefield.EnumerationValue == 8)
        //                {
        //                    EstAge = "75-84";
        //                }
        //                else if (estagefield.EnumerationValue == 9)
        //                {
        //                    EstAge = "85=";
        //                }
        //            }
        //        }
        //        catch { EstAge = " "; }


        //        string Gender = " ";
        //        DataRecordAPI.IDataValue genderfield = null;
        //        try
        //        {
        //            genderfield = dr.GetField("coding.egender").DataValue;
        //            if (genderfield.ValueAsText != null)
        //            {
        //                if (genderfield.EnumerationValue == 1)
        //                {
        //                    Gender = "Male";
        //                }
        //                else
        //                {
        //                    Gender = "Female";
        //                }

        //            }
        //        }
        //        catch { Gender = " "; }

        //        AppController.Instance.NIPSDetailsDataSet.Add(new NIPSDetailsRecord(survey, Serno, Authno, Site, Intdate, Tstart, Outcome, Nation, NINights, ROINights, EstAge, Gender));

        //    }
        //}


















    }
} //ends namespace
