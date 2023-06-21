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
        private DataRecordAPI.IKey GetPrimaryKey(MetaAPI.IDatamodel dm)
        {
            // Get an IKey interface for the primary key:
            DataRecordAPI.IKey result = DataRecordAPI.DataRecordManager.GetKey(dm, MetaAPI.Constants.KeyNames.Primary);
            return result;
        }




        public ObservableCollection<FullRecord> FullDataSet = new ObservableCollection<FullRecord>();
        public ObservableCollection<ProgressRecord> ProgressDataSet = new ObservableCollection<ProgressRecord>();
        public ObservableCollection<PerformanceRecord> PerformanceDataSet = new ObservableCollection<PerformanceRecord>();
        public ObservableCollection<TraineeAppRecord> TraineeAppDataSet = new ObservableCollection<TraineeAppRecord>();
        public ObservableCollection<NIPSMenuRecord> NIPSMenuDataSet = new ObservableCollection<NIPSMenuRecord>();
        public ObservableCollection<NIPSDetailsRecord> NIPSDetailsDataSet = new ObservableCollection<NIPSDetailsRecord>();
        public ObservableCollection<ShiftDetailsRecord> ShiftDetailsDataSet = new ObservableCollection<ShiftDetailsRecord>();
        public ObservableCollection<CurrentWagesRecord> CurrentWagesDataSet = new ObservableCollection<CurrentWagesRecord>();
        public ObservableCollection<ArchivedWagesRecord> ArchivedWagesDataSet = new ObservableCollection<ArchivedWagesRecord>();
        public ObservableCollection<ExpensesRecord> ExpensesDataSet = new ObservableCollection<ExpensesRecord>();
        public ObservableCollection<AnnualLeaveRecord> AnnualLeaveDataSet = new ObservableCollection<AnnualLeaveRecord>();


        public void SetData(IEnumerable<IInstrumentInfo> data, bool IncludeNIPS, string Environment)
        {

            //if (Environment == "C:\\B5RESPOND")
            //{
            //    MessageBox.Show(string.Format("Going into SetData {0} Connected to surveys={1} Connected toRespond = {2} ", Environment, AppController.Instance._connectedtoSurveys, AppController.Instance._connectedtoRespond));
            //}
            //Thread.Sleep(1000);
            String Username = GetUsername();
            DateTime TimeNow = DateTime.Now;
            TimeSpan howlongsincelastrun = TimeNow - TimeNow;
            if (_alreadyconnectedonce == true)
            {
                howlongsincelastrun = (TimeNow - _lastruntime);
            }
            //this section ensures that the list of survey cases isn't built twice on the launch of the programme
            //this caused a selected case to lose focus if the user was very quick at selecting. The 15 seconds limit mght need to be increased to 20
            //if (((_alreadyconnectedonce == true) && (howlongsincelastrun.Seconds > 15)) | (_alreadyconnectedonce == false) | (Environment.ToUpper() == "C:\\B5RESPOND"))
            //{
            //MessageBox.Show(string.Format("Data.Count = {0}: {1}", data.Count(), Environment));
            if (data.Count() > 0)
            {
                //MessageBox.Show(string.Format("Data.Count = {0}: {1}", data.Count(),Environment));
                // We need to clear the values out of DataSet each time so that the list of cases doesn't get duplicated 

                if ((Environment.ToUpper() == "C:\\B5RESEARCH") | ((Environment.ToUpper() == "C:\\B5RESPOND") && (AppController.Instance._connectedtoSurveys == false)))
                {
                    //MessageBox.Show(string.Format("In data clear = {0} :{1}: {2}", data.Count(),Environment, AppController.Instance._connectedtoSurveys));
                    FullDataSet.Clear();
                    ProgressDataSet.Clear();
                    PerformanceDataSet.Clear();
                    NIPSMenuDataSet.Clear();
                    NIPSDetailsDataSet.Clear();
                    TraineeAppDataSet.Clear();
                    ShiftDetailsDataSet.Clear();
                    CurrentWagesDataSet.Clear();
                    ArchivedWagesDataSet.Clear();
                    ExpensesDataSet.Clear();
                    AnnualLeaveDataSet.Clear();

                }
                IEnumerable<IInstrumentInfo> InstalledInstruments =
                    InstalledInstruments = data.Where(s => s.InstrumentState == InstrumentState.InstalledOnClient);
                foreach (IInstrumentInfo InstalledInstrument in InstalledInstruments)
                {
                    //MessageBox.Show(string.Format("Got to here :{0}: {1}", Environment, AppController.Instance._connectedtoSurveys));
                    if ((((InstalledInstrument.Name.Length >= 4) && (InstalledInstrument.Name.ToUpper().Substring(0, 4) == "NIPS")) & (IncludeNIPS = true)) | ((InstalledInstrument.Name.Length >= 4) && (InstalledInstrument.Name.ToUpper().Substring(0, 4) != "NIPS")))
                    {
                        string strDataModel = " ";
                        string strDataInterface = " ";
                        if (Environment.ToUpper() == "C:\\B5RESEARCH")
                        {

                            strDataModel = Properties.Settings.Default.DeployFolder + "\\" + Username +"\\Surveys\\" + InstalledInstrument.Name + "\\" + InstalledInstrument.Name + ".bmix";
                            strDataInterface = Properties.Settings.Default.DeployFolder + "\\" + Username + "\\Surveys\\" + InstalledInstrument.Name + "\\" + InstalledInstrument.Name + ".bdix";
                        }
                        else if (Environment.ToUpper() == "C:\\B5RESPOND")
                        {
                            //MessageBox.Show(string.Format("Got to here :{0}: {1}", Environment, AppController.Instance._connectedtoSurveys));
                            strDataModel = Properties.Settings.Default.DeployFolderRespond + "\\" + Username + "\\Surveys\\" + InstalledInstrument.Name + "\\" + InstalledInstrument.Name + ".bmix";
                            strDataInterface = Properties.Settings.Default.DeployFolderRespond + "\\" + Username + "\\Surveys\\" + InstalledInstrument.Name + "\\" + InstalledInstrument.Name + ".bdix";
                        }
                        //When a survey is removed this section of code is automatically triggered and will try to run for the survey that is being deleted.
                        if (System.IO.File.Exists(strDataInterface))
                        {

                            try
                            {

                                MetaAPI.IDatamodel dm = MetaAPI.MetaManager.GetDatamodel(strDataModel);
                                DataLinkAPI.IDataLink dl = DataLinkAPI.DataLinkManager.GetDataLink(strDataInterface, dm);
                                DataRecordAPI.IKey key = GetPrimaryKey(dm);
                                DataLinkAPI.IDataSet ds = dl.Read(key, DataLinkAPI.ReadOrder.Ascending, 300, true);
                                //MessageBox.Show(string.Format("I am in {0} seconds {1}", _alreadyconnectedonce, howlongsincelastrun.Seconds));
                                //this next line captures the IInstrumentInfo details fo the Progress questionnaire so that it can be used to drive the visibility of the "View progress" button
                                //if ((InstalledInstrument.Name.ToUpper() != "PERFORMANCE") & (InstalledInstrument.Name.ToUpper() != "PROGRESS") & (InstalledInstrument.Name.ToUpper() != "TRAINEEAPP") & (InstalledInstrument.Name.ToUpper().Substring(0, 4) != "NIPS")& (InstalledInstrument.Name.ToUpper().Substring(0, 8) != "SHIFTDET"))
                                //{ _surveyinstrument = InstalledInstrument; }

                                if ((InstalledInstrument.Name.Length >= 8) && (InstalledInstrument.Name.ToUpper().Substring(0, 8) == "PROGRESS"))
                                //MessageBox.Show(string.Format("I am in "));
                                {
                                    _progressinstrument = InstalledInstrument;
                                    _progresslocation = Environment.ToUpper();
                                }
                                else if ((InstalledInstrument.Name.Length >= 11) && (InstalledInstrument.Name.ToUpper().Substring(0, 11) == "PERFORMANCE"))
                                {
                                    _performanceinstrument = InstalledInstrument;
                                    _performancelocation = Environment.ToUpper();
                                }
                                else if ((InstalledInstrument.Name.Length >= 11) && (InstalledInstrument.Name.ToUpper().Substring(0, 11) == "ALLOCATIONS"))
                                {
                                    _allocationsinstrument = InstalledInstrument;
                                    _allocationslocation = Environment.ToUpper();
                                }
                                else if ((InstalledInstrument.Name.Length >= 10) && (InstalledInstrument.Name.ToUpper().Substring(0, 10) == "TRAINEEAPP"))
                                {
                                    _traineeappinstrument = InstalledInstrument;
                                    _traineeapplocation = Environment.ToUpper();

                                }
                                else if ((InstalledInstrument.Name.Length >= 8) && (InstalledInstrument.Name.ToUpper().Substring(0, 8) == "SHIFTDET"))
                                {
                                    _shiftdetailsinstrument = InstalledInstrument;
                                    _shiftdetailslocation = Environment.ToUpper();
                                }
                                else if ((InstalledInstrument.Name.Length >= 7) && (InstalledInstrument.Name.ToUpper().Substring(0, 7) == "B5WAGES"))
                                {
                                    //GetWageYearStartDate();
                                    DateTime Now = DateTime.Now;
                                    DateTime CurrWages = _wageyearstartdate;

                                    //MessageBox.Show(string.Format("{0} {1} ", Now, CurrWages));
                                    //MessageBox.Show(string.Format("B5wages yr: {0}  Current yr less 1: {1}", Int32.Parse(InstalledInstrument.Name.ToUpper().Substring(7, 2)), Int32.Parse(Now.ToString("yy"))-1 ));

                                    //if (((Now >= CurrWages) && (Int32.Parse(InstalledInstrument.Name.ToUpper().Substring(7, 2))) == (Int32.Parse(Now.ToString("yy")))) | ((Now < CurrWages) && (Int32.Parse(InstalledInstrument.Name.ToUpper().Substring(7, 2))) == (Int32.Parse(Now.ToString("yy"))) - 1))
                                    if (((Now >= CurrWages) && (Int32.Parse(InstalledInstrument.Name.ToUpper().Substring(7, 2))) == (Int32.Parse(CurrWages.ToString("yy")))) | ((Now < CurrWages) && (Int32.Parse(InstalledInstrument.Name.ToUpper().Substring(7, 2))) == (Int32.Parse(CurrWages.ToString("yy"))) - 1))



                                    {
                                        _currentwagesinstrument = InstalledInstrument;
                                        _currentwageslocation = Environment.ToUpper();
                                        //MessageBox.Show(string.Format("Installedinstrument: {0} at {1}", CurrWages, Environment));
                                    }
                                }
                                else if ((InstalledInstrument.Name.Length >= 13) && (InstalledInstrument.Name.ToUpper().Substring(0, 13) == "EXPENSESCLAIM"))
                                {
                                    DateTime Now = DateTime.Now;
                                    DateTime CurrWages = _wageyearstartdate;

                                    if (((Now >= CurrWages) && (Int32.Parse(InstalledInstrument.Name.ToUpper().Substring(13, 2))) == (Int32.Parse(CurrWages.ToString("yy")))) | ((Now < CurrWages) && (Int32.Parse(InstalledInstrument.Name.ToUpper().Substring(13, 2))) == (Int32.Parse(CurrWages.ToString("yy"))) - 1))

                                    {
                                        //MessageBox.Show(string.Format("B5wages yr: {0}  Current yr less 1: {1}", Int32.Parse(InstalledInstrument.Name.ToUpper().Substring(13, 2)), Int32.Parse(Now.ToString("yy"))-1 ));

                                        _expensesinstrument = InstalledInstrument;
                                        _expenseslocation = Environment.ToUpper();
                                    }
                                    //MessageBox.Show(string.Format("Expenses found: {0} ", InstalledInstrument.Name));

                                }
                                else if ((InstalledInstrument.Name.Length >= 7) && (InstalledInstrument.Name.ToUpper().Substring(0, 7) == "INTHOLS"))
                                {
                                    DateTime Now = DateTime.Now;
                                    DateTime CurrWages = _wageyearstartdate;

                                    //if (((Now >= CurrWages) && (Int32.Parse(InstalledInstrument.Name.ToUpper().Substring(7, 2))) == (Int32.Parse(Now.ToString("yy")))) | ((Now < CurrWages) && (Int32.Parse(InstalledInstrument.Name.ToUpper().Substring(7, 2))) == (Int32.Parse(Now.ToString("yy"))) - 1))
                                    if (((Now >= CurrWages) && (Int32.Parse(InstalledInstrument.Name.ToUpper().Substring(7, 2))) == (Int32.Parse(CurrWages.ToString("yy")))) | ((Now < CurrWages) && (Int32.Parse(InstalledInstrument.Name.ToUpper().Substring(7, 2))) == (Int32.Parse(CurrWages.ToString("yy"))) - 1))

                                    {
                                        _annualleaveinstrument = InstalledInstrument;
                                        _annualleavelocation = Environment.ToUpper();
                                    }
                                }
                                else
                                {
                                    _surveyinstrument = InstalledInstrument;
                                    //MessageBox.Show(string.Format("{0} being loaded from {1} ", InstalledInstrument.Name, Environment));
                                }

                                if (ds != null && ds.RecordCount > 0)

                                {
                                    // Typical way to iterate through an IDataSet:
                                    while (!ds.EndOfSet)
                                    {
                                        DataRecordAPI.IDataRecord dr = ds.ActiveRecord;
                                        FillDataRecord(dr, InstalledInstrument, Environment);
                                        ds.MoveNext();
                                    }
                                }
                            }
                            catch
                            {

                            }

                        }
                    }
                    if ((InstalledInstrument.Name.Length >= 4) && (InstalledInstrument.Name.ToUpper().Substring(0, 4) == "NIPS") && (IncludeNIPS = true))
                    {
                        //MessageBox.Show(string.Format("I am in "));
                        if (InstalledInstrument.Name.ToUpper().Substring(0, 4) == "NIPS")
                        { _NIPSMenuinstrument = InstalledInstrument; }
                        //This section builds a dataset of NIPS allocated months to populate the NIPS menu page
                        string NIPSMonth = " ";
                        if (InstalledInstrument.Name.Substring(6, 2) == "01")
                        { NIPSMonth = "Jan"; }
                        else if (InstalledInstrument.Name.Substring(6, 2) == "02")
                        { NIPSMonth = "Feb"; }
                        else if (InstalledInstrument.Name.Substring(6, 2) == "03")
                        { NIPSMonth = "Mar"; }
                        else if (InstalledInstrument.Name.Substring(6, 2) == "04")
                        { NIPSMonth = "Apr"; }
                        else if (InstalledInstrument.Name.Substring(6, 2) == "05")
                        { NIPSMonth = "May"; }
                        else if (InstalledInstrument.Name.Substring(6, 2) == "06")
                        { NIPSMonth = "Jun"; }
                        else if (InstalledInstrument.Name.Substring(6, 2) == "07")
                        { NIPSMonth = "Jul"; }
                        else if (InstalledInstrument.Name.Substring(6, 2) == "08")
                        {NIPSMonth = "Aug";}
                        else if (InstalledInstrument.Name.Substring(6, 2) == "09")
                        { NIPSMonth = "Sep"; }
                        else if (InstalledInstrument.Name.Substring(6, 2) == "10")
                        { NIPSMonth = "Oct"; }
                        else if (InstalledInstrument.Name.Substring(6, 2) == "11")
                        { NIPSMonth = "Nov"; }
                        else if (InstalledInstrument.Name.Substring(6, 2) == "12")
                        { NIPSMonth = "Dec"; }
                        else if (InstalledInstrument.Name.Substring(6, 2) == "00")
                        { NIPSMonth = "Prct"; }
                        Int32 NIPSYear = Convert.ToInt32(InstalledInstrument.Name.Substring(4, 2));
                        NIPSMenuDataSet.Add(new NIPSMenuRecord(InstalledInstrument, NIPSMonth, Convert.ToInt32(InstalledInstrument.Name.Substring(6, 2)), NIPSYear, Environment));
                    }

                    //This next bit triggers the event on startup (and only on startup) that turns the menu buttons blue and activates them. 
                }

                //if (_casesloadedSurveys == false)
                //{
                //    ev_repaint_buttons();
                //}
                //_casesloadedSurveys = true;

                if ((_casesloadedSurveys == false) & (Environment == "C:\\B5RESEARCH"))
                {
                    _casesloadedSurveys = true;
                    //MessageBox.Show(string.Format("Casesloadedsurveys = {0}", _casesloadedSurveys));
                }
                if ((_casesloadedRespond == false) & (Environment == "C:\\B5RESPOND"))
                {
                    _casesloadedRespond = true;
                    //MessageBox.Show(string.Format("Casesloadedrespond = {0}", _casesloadedRespond));
                }

                if (((_casesloadedSurveys == true) & (_casesloadedRespond == true)) | ((_casesloadedSurveys == true) & (_connectedtoRespond == false)) | ((_casesloadedRespond == true) & (_connectedtoSurveys == false)))
                {
                    ev_repaint_buttons();
                    //Now reset the counters
                    _casesloadedSurveys = false;
                    _casesloadedRespond = false;

                }



            }
            if ((Environment.ToUpper() == "C:\\B5RESEARCH") && ((data.Count() == 0)))
            {
                _casesloadedSurveys = true;
                FullDataSet.Clear();
                ProgressDataSet.Clear();
                PerformanceDataSet.Clear();
                NIPSMenuDataSet.Clear();
                NIPSDetailsDataSet.Clear();
                TraineeAppDataSet.Clear();
                ShiftDetailsDataSet.Clear();
                CurrentWagesDataSet.Clear();
                ArchivedWagesDataSet.Clear();
                ExpensesDataSet.Clear();
                AnnualLeaveDataSet.Clear();
            }
            else if ((Environment.ToUpper() == "C:\\B5RESPOND") && ((data.Count() == 0)))
            {
                ev_repaint_buttons();
                _casesloadedSurveys = false;
                _casesloadedRespond = false;
            }

            if ((Environment.ToUpper() == "C:\\B5RESEARCH") && (!_connectedtoRespond.HasValue))
            {
                //MessageBox.Show(string.Format("Going into Respond now = ConnectedtoRespond =  {0}", _connectedtoRespond));
                //Blaise_App.AppController.Instance.Window.Dispatcher.Invoke((Action)delegate
                //{
                //currentenvironment = "C:\\B5RESPOND";
                //AppController.Instance.Connect("C:\\B5RESPOND", "RESPONDx.NISRA.GOV.UK", true);
                if (!AppController.Instance.Connect("C:\\B5RESPOND", "RESPONDx.NISRA.GOV.UK", true, false))
                {
                    _connectedtoRespond = false;
                    //MessageBox.Show(string.Format("Respond failed ConnectedtoRespond =  {0}", _connectedtoRespond));
                    ev_repaint_buttons();
                    //Now reset the counters
                    _casesloadedSurveys = false;
                    _casesloadedRespond = false;
                }
                //});
            }
            else if ((Environment.ToUpper() == "C:\\B5RESEARCH") && (AppController.Instance._connectedtoRespond == true))
            {
                //currentenvironment = "C:\\B5RESPOND";

                AppController.Instance.SetData(AppController.Instance.InstalledonRespond.Instruments, true, "C:\\B5RESPOND");
            }


            _lastruntime = DateTime.Now;
            _alreadyconnectedonce = true;
            //}time stamping in statement

        }



        private void FillDataRecord(DataRecordAPI.IDataRecord dr, IInstrumentInfo e, string Environment)
        {
            if (((dr.Keys.Contains("PRIMARY")) && ((e.Name.Length >= 8) && (e.Name.ToUpper().Substring(0, 8) == "PROGRESS")) | ((e.Name.Length >= 11) && (e.Name.ToUpper().Substring(0, 11) == "PERFORMANCE")) | ((e.Name.Length >= 10) && (e.Name.ToUpper().Substring(0, 10) == "TRAINEEAPP"))))
            {
                var survey = e;

                DataRecordAPI.IKey tmpkey = dr.Keys.GetItem(MetaAPI.Constants.KeyNames.Primary);
                string Primkey = tmpkey.KeyValue;

                string Intno = " ";
                DataRecordAPI.IDataValue Intnumfield = null;
                try
                {
                    Intnumfield = dr.GetField("Intnum").DataValue;
                    Intno = Intnumfield.ValueAsText;
                }
                catch { Intno = " "; }

                DateTime IntDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                try
                {
                    DataRecordAPI.IDataValue IntDatefield = dr.GetField("IntDate").DataValue;
                    string IntDatestr = IntDatefield.ValueAsText;
                    IntDate = DateTime.ParseExact(IntDatestr, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None);
                }
                catch (Exception)
                {
                    IntDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                }

                string IntName = " ";
                DataRecordAPI.IDataValue Intnamefield = null;

                try
                {
                    Intnamefield = dr.GetField("Intname").DataValue;
                    IntName = Intnamefield.ValueAsText;
                }
                catch { IntName = " "; }


                string FTNum = " ";
                DataRecordAPI.IDataValue FT_numfield = null;
                try
                {
                    FT_numfield = dr.GetField("FT_Num").DataValue;
                    FTNum = FT_numfield.ValueAsText;
                }
                catch { FTNum = " "; }


                //MessageBox.Show(string.Format("Primary Key= {0}, Intno = {1}, Date = {2}, Ft = {3} ", Primkey, Intno, IntDate, FTNum));
                if ((dr.Keys.Contains("PRIMARY")) && ((e.Name.Length >= 8) && (e.Name.ToUpper().Substring(0, 8) == "PROGRESS")))
                {
                    ProgressDataSet.Add(new ProgressRecord(survey, Primkey, Intno, IntDate, IntName, FTNum, Environment));
                }
                else if ((dr.Keys.Contains("PRIMARY")) && ((e.Name.Length >= 11) && (e.Name.ToUpper().Substring(0, 11) == "PERFORMANCE")))
                {
                    PerformanceDataSet.Add(new PerformanceRecord(survey, Primkey, Intno, IntDate, IntName, FTNum, Environment));
                }
                else if ((dr.Keys.Contains("PRIMARY")) && ((e.Name.Length >= 10) && (e.Name.ToUpper().Substring(0, 10) == "TRAINEEAPP")))
                {
                    TraineeAppDataSet.Add(new TraineeAppRecord(survey, Primkey, Intno, IntDate, IntName, FTNum, Environment));
                }

            }

            //============================================================================================================================================================================================

            else if ((dr.Keys.Contains("PRIMARY")) && ((e.Name.Length >= 7) && (e.Name.ToUpper().Substring(0, 7) == "B5WAGES")))

            {
                var survey = e;
                //MessageBox.Show(string.Format("I am in "));
                //MessageBox.Show(string.Format("survey= {0}", e.Name));
                DataRecordAPI.IKey tmpkey = dr.Keys.GetItem(MetaAPI.Constants.KeyNames.Primary);
                string Primkey = tmpkey.KeyValue;

                string Intno = " ";
                DataRecordAPI.IDataValue Intnumfield = null;
                try
                {
                    Intnumfield = dr.GetField("Intno").DataValue;
                    Intno = Intnumfield.ValueAsText;
                }
                catch { Intno = " "; }

                DateTime WageDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                try
                {
                    DataRecordAPI.IDataValue WageDatefield = dr.GetField("Date").DataValue;
                    string WageDatestr = WageDatefield.ValueAsText;
                    WageDate = DateTime.ParseExact(WageDatestr, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None);
                }
                catch (Exception)
                {
                    WageDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                }

                Int32 RefNo = 0;
                DataRecordAPI.IDataValue refnofield = null;
                try
                {
                    refnofield = dr.GetField("serial").DataValue;
                    RefNo = Convert.ToInt32(refnofield.IntegerValue);

                }
                catch { RefNo = 0; }

                Int32 NoLines = 0;
                DataRecordAPI.IDataValue nolinesfield = null;
                try
                {
                    nolinesfield = dr.GetField("numlines").DataValue;
                    NoLines = Convert.ToInt32(nolinesfield.IntegerValue);

                }
                catch { NoLines = 0; }

                Int32 TotMiles = 0;
                DataRecordAPI.IDataValue totmilesfield = null;
                try
                {
                    totmilesfield = dr.GetField("totmiles").DataValue;
                    TotMiles = Convert.ToInt32(totmilesfield.IntegerValue);

                }
                catch { TotMiles = 0; }


                Int32 TotHours = 0;
                Int32 TotMins = 0;
                DataRecordAPI.IDataValue tothoursfield = null;
                try
                {
                    tothoursfield = dr.GetField("totworkmins").DataValue;
                    //TotHours = Convert.ToInt32(tothoursfield.IntegerValue);
                    TotHours = (Math.DivRem(Convert.ToInt32(tothoursfield.IntegerValue), 60, out TotMins));

                }
                catch { TotHours = 0; TotMins = 0; }

                string WageYear = (e.Name.ToUpper().Substring(7, 2));
                Int32 intwageyear = Convert.ToInt32(WageYear);
                string strwageyear = Convert.ToString(intwageyear + 1);
                WageYear = WageYear + "-" + strwageyear;



                DataRecordAPI.IDataValue submitfield = null;
                submitfield = dr.GetField("submit").DataValue;


                //MessageBox.Show(string.Format("Primary Key= {0}, Intno = {1}, Date = {2}, totmiles = {3}, Tothours = {4}, TotMins = {5}, submit = {6} ", Primkey, Intno, WageDate, TotMiles, TotHours, TotMins, submitfield.EnumerationValue));
                if  (submitfield.EnumerationValue == 1)
                {
                    

                    ArchivedWagesDataSet.Add(new ArchivedWagesRecord(survey, Primkey, Intno, WageDate, RefNo, TotMiles, TotHours, TotMins, WageYear, Environment));

                }
                else
                {
                    DateTime Now = DateTime.Now;
                    DateTime CurrWages = _wageyearstartdate;

                    //if (((Now >= CurrWages) && (Int32.Parse(e.Name.ToUpper().Substring(7, 2))) == (Int32.Parse(Now.ToString("yy")))) | ((Now < CurrWages) && (Int32.Parse(e.Name.ToUpper().Substring(7, 2))) == (Int32.Parse(Now.ToString("yy"))) - 1))
                    if (((Now >= CurrWages) && (Int32.Parse(e.Name.ToUpper().Substring(7, 2))) == (Int32.Parse(CurrWages.ToString("yy")))) | ((Now < CurrWages) && (Int32.Parse(e.Name.ToUpper().Substring(7, 2))) == (Int32.Parse(CurrWages.ToString("yy"))) - 1))

                        //MessageBox.Show(string.Format("Added current cases for: {0}", Int32.Parse(e.Name.ToUpper().Substring(7, 2))));
                        CurrentWagesDataSet.Add(new CurrentWagesRecord(survey, Primkey, Intno, WageDate, RefNo, NoLines, Environment));

                }


            }



            //============================================================================================================================================================================================

            else if ((dr.Keys.Contains("PRIMARY")) && ((e.Name.Length >= 13) && (e.Name.ToUpper().Substring(0, 13) == "EXPENSESCLAIM")))

            {
                var survey = e;
                //MessageBox.Show(string.Format("I am in "));
                //MessageBox.Show(string.Format("survey= {0}", e.Name));
                DataRecordAPI.IKey tmpkey = dr.Keys.GetItem(MetaAPI.Constants.KeyNames.Primary);
                string Primkey = tmpkey.KeyValue;

                string Intno = " ";
                DataRecordAPI.IDataValue Intnumfield = null;
                try
                {
                    Intnumfield = dr.GetField("Intno").DataValue;
                    Intno = Intnumfield.ValueAsText;
                }
                catch { Intno = " "; }

                DateTime ClaimDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                try
                {
                    DataRecordAPI.IDataValue ClaimDatefield = dr.GetField("SignDate").DataValue;
                    string ClaimDatestr = ClaimDatefield.ValueAsText;
                    ClaimDate = DateTime.ParseExact(ClaimDatestr, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None);
                }
                catch (Exception)
                {
                    ClaimDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                }

                Int32 RefNo = 0;
                DataRecordAPI.IDataValue refnofield = null;
                try
                {
                    refnofield = dr.GetField("ClaimNumber").DataValue;
                    RefNo = Convert.ToInt32(refnofield.IntegerValue);

                }
                catch { RefNo = 0; }


                Decimal TotClaim = 0;
                DataRecordAPI.IDataValue totclaimfield = null;
                try
                {
                    totclaimfield = dr.GetField("TotClaim").DataValue;
                    TotClaim = Convert.ToDecimal(totclaimfield.RealValue);
                   

                }
                catch { TotClaim = 0; }



                string WageYear = (e.Name.ToUpper().Substring(7, 2));





                //MessageBox.Show(string.Format("Primary Key= {0}, Intno = {1}, Date = {2}, totclaim = {3} ", Primkey, Intno, ClaimDate, TotClaim));

                    ExpensesDataSet.Add(new ExpensesRecord(survey, Primkey, Intno, ClaimDate, RefNo, TotClaim, WageYear, Environment));
               


            }




            //============================================================================================================================================================================================


            else if ((dr.Keys.Contains("PRIMARY")) && ((e.Name.Length >= 7) && (e.Name.ToUpper().Substring(0, 7) == "INTHOLS")))

            {
                var survey = e;
                //MessageBox.Show(string.Format("I am in "));
                //MessageBox.Show(string.Format("survey= {0}", e.Name));
                DataRecordAPI.IKey tmpkey = dr.Keys.GetItem(MetaAPI.Constants.KeyNames.Primary);
                string Primkey = tmpkey.KeyValue;

                string Intno = " ";
                DataRecordAPI.IDataValue Intnumfield = null;
                try
                {
                    Intnumfield = dr.GetField("Intno").DataValue;
                    Intno = Intnumfield.ValueAsText;
                }
                catch { Intno = " "; }


                Int32 RefNo = 0;
                DataRecordAPI.IDataValue refnofield = null;
                try
                {
                    refnofield = dr.GetField("HolNO").DataValue;
                    RefNo = Convert.ToInt32(refnofield.IntegerValue);

                }
                catch { RefNo = 0; }


                string Holtype = " ";
                DataRecordAPI.IDataValue holtypefield = null;
                try
                {
                    holtypefield = dr.GetField("holtype").DataValue;
                    if (holtypefield.ValueAsText != null)
                    {
                        if (holtypefield.EnumerationValue == 1)
                        {
                            Holtype = "Paid";
                        }
                        else if (holtypefield.EnumerationValue == 2)
                        {
                            Holtype = "Sick";
                        }
                        else if (holtypefield.EnumerationValue == 3)
                        {
                            Holtype = "Unpaid";
                        }
                    }
                    //   Site = sitefield.ValueAsText;
                }
                catch { Holtype = " "; }





                DateTime StrtDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                try
                {
                    DataRecordAPI.IDataValue StrtDatefield = dr.GetField("StrtDate").DataValue;
                    string StrtDatestr = StrtDatefield.ValueAsText;
                    StrtDate = DateTime.ParseExact(StrtDatestr, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None);
                }
                catch (Exception)
                {
                    StrtDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                }

                DateTime EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                try
                {
                    DataRecordAPI.IDataValue EndDatefield = dr.GetField("EndDate").DataValue;
                    string EndDatestr = EndDatefield.ValueAsText;
                    EndDate = DateTime.ParseExact(EndDatestr, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None);
                }
                catch (Exception)
                {
                    EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                }

                Int32 Holdays = 0;
                DataRecordAPI.IDataValue holdaysfield = null;
                try
                {
                    holdaysfield = dr.GetField("Holdays").DataValue;
                    Holdays = Convert.ToInt32(holdaysfield.IntegerValue);

                }
                catch { Holdays = 0; }


                string WageYear = (e.Name.ToUpper().Substring(7, 2));





                //MessageBox.Show(string.Format("Primary Key= {0}, Intno = {1}, StartDate = {2}, EndDate = {3} ", Primkey, Intno, StrtDate, EndDate));

                AnnualLeaveDataSet.Add(new AnnualLeaveRecord(survey, Primkey, Intno,  RefNo, Holtype,StrtDate,EndDate, Holdays, WageYear, Environment));



            }


            //============================================================================================================================================================================================


            else if ((dr.Keys.Contains("PRIMARY")) && ((e.Name.Length >= 4) && (e.Name.ToUpper().Substring(0, 4) == "NIPS")))
            {
                var survey = e;
                DataRecordAPI.IKey tmpkey = dr.Keys.GetItem(MetaAPI.Constants.KeyNames.Primary);
                string Primkey = tmpkey.KeyValue;

                string Serno = " ";
                DataRecordAPI.IDataValue sernofield = null;
                try
                {
                    sernofield = dr.GetField("admin1.Serno").DataValue;
                    Serno = sernofield.ValueAsText;
                }
                catch { Serno = " "; }

                string Authno = " ";
                DataRecordAPI.IDataValue authnofield = null;
                try
                {
                    authnofield = dr.GetField("admin1.authno").DataValue;
                    Authno = authnofield.ValueAsText;
                }
                catch { Authno = " "; }

                string Site = " ";
                DataRecordAPI.IDataValue sitefield = null;
                try
                {
                    sitefield = dr.GetField("admin1.site").DataValue;
                    if (sitefield.ValueAsText != null)
                    {
                        if (sitefield.EnumerationValue == 1)
                        {
                            Site = "BFS";
                        }
                        else if (sitefield.EnumerationValue == 2)
                        {
                            Site = "BHD";
                        }
                        else if (sitefield.EnumerationValue == 3)
                        {
                            Site = "Der";
                        }
                        else if (sitefield.EnumerationValue == 4)
                        {
                            Site = "Stena";
                        }
                        else if (sitefield.EnumerationValue == 5)
                        {
                            Site = "L'Pool";
                        }
                        else if (sitefield.EnumerationValue == 6)
                        {
                            Site = "Larne";
                        }

                    }
                    //   Site = sitefield.ValueAsText;
                }
                catch { Site = " "; }


                DateTime Intdate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
                try
                {
                    DataRecordAPI.IDataValue Intdatefield = dr.GetField("admin1.Intdate").DataValue;
                    string IntDatestr = Intdatefield.ValueAsText;
                    Intdate = DateTime.ParseExact(IntDatestr, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None);
                }
                catch (Exception)
                {
                    Intdate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
                }

                DateTime Tstart = DateTime.ParseExact("00:00", "HH:mm", null, System.Globalization.DateTimeStyles.None);
                try
                {
                    DataRecordAPI.IDataValue Tstartfield = dr.GetField("times.tstart").DataValue;
                    string Tstartstr = Tstartfield.ValueAsText;
                    Tstart = DateTime.ParseExact(Tstartstr.Substring(0, 5), "HH:mm", null, System.Globalization.DateTimeStyles.None);
                }
                catch (Exception)
                {
                    Tstart = DateTime.ParseExact("00:00", "HH:mm", null, System.Globalization.DateTimeStyles.None);
                }


                string Outcome = " ";
                DataRecordAPI.IDataValue outcomefield = null;
                try
                {
                    outcomefield = dr.GetField("coding.outcome").DataValue;
                    if (outcomefield.ValueAsText != null)
                    {
                        if (outcomefield.EnumerationValue == 2)
                        {
                            Outcome = "Partial";
                        }
                        else if (outcomefield.EnumerationValue == 3)
                        {
                            Outcome = "Complete";
                        }
                        else if (outcomefield.EnumerationValue == 4)
                        {
                            Outcome = "Refusal";
                        }
                        else if (outcomefield.EnumerationValue == 5)
                        {
                            Outcome = "Non-Contact";
                        }
                        else if (outcomefield.EnumerationValue == 6)
                        {
                            Outcome = "Non-Eligible";
                        }
                        else if (outcomefield.EnumerationValue == 8)
                        {
                            Outcome = "Re-Cross";
                        }
                        else if (outcomefield.EnumerationValue == 9)
                        {
                            Outcome = "Day Visitor";
                        }

                    }
                }
                catch { Outcome = " "; }

                string Nation = " ";
                DataRecordAPI.IDataValue nationfield = null;
                try
                {
                    nationfield = dr.GetField("screen.Nat").DataValue;
                    if (nationfield.ValueAsText != null)
                    {
                        if (nationfield.EnumerationValue == 1)
                        {
                            Nation = "2+ Countries";
                        }
                        else if (nationfield.EnumerationValue == 2)
                        {
                            Nation = "NI";
                        }
                        else if (nationfield.EnumerationValue == 3)
                        {
                            Nation = "ROI";
                        }
                        else if (nationfield.EnumerationValue == 4)
                        {
                            Nation = "England";
                        }
                        else if (nationfield.EnumerationValue == 5)
                        {
                            Nation = "Scotland";
                        }
                        else if (nationfield.EnumerationValue == 6)
                        {
                            Nation = "Wales";
                        }
                        else if (nationfield.EnumerationValue == 7)
                        {
                            Nation = "IoM / CH";
                        }
                        else if (nationfield.EnumerationValue == 8)
                        {
                            Nation = "Other EU";
                        }
                        else if (nationfield.EnumerationValue == 9)
                        {
                            Nation = "USA";
                        }
                        else if (nationfield.EnumerationValue == 10)
                        {
                            Nation = "Canada";
                        }
                        else if (nationfield.EnumerationValue == 11)
                        {
                            Nation = "Rest of World";
                        }
                    }
                }
                catch { Nation = " "; }

                Int32 NINights = 0;
                DataRecordAPI.IDataValue ninightsfield = null;
                try
                {
                    ninightsfield = dr.GetField("screen.NInghts").DataValue;
                    NINights = Convert.ToInt32(ninightsfield.IntegerValue);

                }
                catch { NINights = 0; }


                Int32 ROINights = 0;
                DataRecordAPI.IDataValue roinightsfield = null;
                try
                {
                    roinightsfield = dr.GetField("screen.ROInghts").DataValue;
                    ROINights = Convert.ToInt32(roinightsfield.IntegerValue);

                }
                catch { ROINights = 0; }

                string EstAge = " ";
                DataRecordAPI.IDataValue estagefield = null;
                try
                {
                    estagefield = dr.GetField("coding.estage").DataValue;
                    if (estagefield.ValueAsText != null)

                    {
                        if (estagefield.EnumerationValue == 1)
                        {
                            EstAge = "0-15";
                        }
                        else if (estagefield.EnumerationValue == 2)
                        {
                            EstAge = "16-24";
                        }
                        else if (estagefield.EnumerationValue == 3)
                        {
                            EstAge = "25-34";
                        }
                        else if (estagefield.EnumerationValue == 4)
                        {
                            EstAge = "35-44";
                        }
                        else if (estagefield.EnumerationValue == 5)
                        {
                            EstAge = "45-54";
                        }
                        else if (estagefield.EnumerationValue == 6)
                        {
                            EstAge = "55-64";
                        }
                        else if (estagefield.EnumerationValue == 7)
                        {
                            EstAge = "65-74";
                        }
                        else if (estagefield.EnumerationValue == 8)
                        {
                            EstAge = "75-84";
                        }
                        else if (estagefield.EnumerationValue == 9)
                        {
                            EstAge = "85=";
                        }
                    }
                }
                catch { EstAge = " "; }


                string Gender = " ";
                DataRecordAPI.IDataValue genderfield = null;
                try
                {
                    genderfield = dr.GetField("coding.egender").DataValue;
                    if (genderfield.ValueAsText != null)
                    {
                        if (genderfield.EnumerationValue == 1)
                        {
                            Gender = "Male";
                        }
                        else if (genderfield.EnumerationValue == 2)
                        {
                            Gender = "Female";
                        }

                    }
                }
                catch { Gender = " "; }
                DateTime tday = DateTime.Now;
                TimeSpan span = tday - Intdate;
                if (span.Days <= 10)
                {
                    AppController.Instance.NIPSDetailsDataSet.Add(new NIPSDetailsRecord(survey, Primkey, Serno, Authno, Site, Intdate, Tstart, Outcome, Nation, NINights, ROINights, EstAge, Gender, Environment));
                }
            }

            else if ((dr.Keys.Contains("PRIMARY")) && ((e.Name.Length >= 8) && (e.Name.ToUpper().Substring(0, 8) == "SHIFTDET")))
            {
                var survey = e;
                DataRecordAPI.IKey tmpkey = dr.Keys.GetItem(MetaAPI.Constants.KeyNames.Primary);
                string Primkey = tmpkey.KeyValue;

                string Site = " ";
                DataRecordAPI.IDataValue sitefield = null;
                try
                {
                    sitefield = dr.GetField("site").DataValue;
                    if (sitefield.ValueAsText != null)
                    {
                        if (sitefield.EnumerationValue == 1)
                        {
                            Site = "BFS";
                        }
                        else if (sitefield.EnumerationValue == 2)
                        {
                            Site = "BHD";
                        }
                        else if (sitefield.EnumerationValue == 3)
                        {
                            Site = "Der";
                        }
                        else if (sitefield.EnumerationValue == 4)
                        {
                            Site = "Stena";
                        }
                        else if (sitefield.EnumerationValue == 5)
                        {
                            Site = "L'Pool";
                        }
                        else if (sitefield.EnumerationValue == 6)
                        {
                            Site = "Larne";
                        }

                    }
                    //   Site = sitefield.ValueAsText;
                }
                catch { Site = " "; }


                DateTime Shftdate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
                try
                {
                    DataRecordAPI.IDataValue Shftdatefield = dr.GetField("ShftDate").DataValue;
                    string Shftdatestr = Shftdatefield.ValueAsText;
                    Shftdate = DateTime.ParseExact(Shftdatestr, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None);
                }
                catch (Exception)
                {
                    Shftdate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
                }

                string AMPM = " ";
                DataRecordAPI.IDataValue AMPMfield = null;
                try
                {
                    AMPMfield = dr.GetField("AMPM").DataValue;
                    if (AMPMfield.ValueAsText != null)
                    {
                        if (AMPMfield.EnumerationValue == 1)
                        {
                            AMPM = "AM";
                        }
                        else if (AMPMfield.EnumerationValue == 2)
                        {
                            AMPM = "PM";
                        }
                    }
                }
                catch { AMPM = " "; }


                string TeamL = " ";
                DataRecordAPI.IDataValue TeamLfield = null;
                try
                {
                    TeamLfield = dr.GetField("authno").DataValue;
                    TeamL = TeamLfield.ValueAsText;
                }
                catch { TeamL = " "; }

                DateTime tday = DateTime.Now;
                TimeSpan span = tday - Shftdate;
                if (span.Days <= 10)
                {
                    AppController.Instance.ShiftDetailsDataSet.Add(new ShiftDetailsRecord(survey, Primkey, Site, Shftdate, AMPM, TeamL, Environment));
                }
            }
            else if ((dr.Keys.Contains("PRIMARY")) && !((e.Name.Length >= 11) && (e.Name.ToUpper().Substring(0, 11) == "ALLOCATIONS")))//((dr.Keys.Contains("PRIMARY")) && (e.Name.ToUpper().Substring(0, 8) != "PROGRESS") && (e.Name.ToUpper().Substring(0, 11) != "PERFORMANCE") && (e.Name.ToUpper().Substring(0, 10) != "TRAINEEAPP") && (e.Name.ToUpper().Substring(0, 4) != "NIPS") && (e.Name.ToUpper().Substring(0, 8) != "SHIFTDET") && (e.Name.ToUpper().Substring(0, 11) != "ALLOCATIONS"))
            {
                // Show the primary key value:
                // Note that the property IKey.KeyValue is read only.
               
                var survey = e;
                //MessageBox.Show(string.Format("survey= {0}", e.Name));
                //Get the promary key field as a string
                DataRecordAPI.IKey tmpkey = dr.Keys.GetItem(MetaAPI.Constants.KeyNames.Primary);
                string primkey = tmpkey.KeyValue;


                DataRecordAPI.IDataValue sernofield = null;
                string serno = " ";
                try
                {
                    sernofield = dr.GetField("Serno").DataValue;
                    serno = sernofield.ValueAsText;
                }
                catch { serno = " "; }


                string hhno = " ";
                DataRecordAPI.IDataValue hhnofield = null;
                try
                {
                    hhnofield = dr.GetField("hhno").DataValue;
                    hhno = hhnofield.ValueAsText;
                }
                catch { hhno = " "; }

                string intno = " ";
                DataRecordAPI.IDataValue intnofield = null;
                try
                {
                    intnofield = dr.GetField("Intno").DataValue;
                    intno = intnofield.ValueAsText;
                }
                catch { intno = " "; }

                string SurveyShort = " ";
                DataRecordAPI.IDataValue surveyshortfield = null;
                try
                {
                    surveyshortfield = dr.GetField("admin1.ToWhom_CSU.surveyshort").DataValue;
                    SurveyShort = surveyshortfield.ValueAsText;
                }
                catch { SurveyShort = " "; }

                string SurveyFull = " ";
                DataRecordAPI.IDataValue surveyfullfield = null;
                try
                {
                    surveyfullfield = dr.GetField("admin1.ToWhom_CSU.surveyfull").DataValue;
                    SurveyFull = surveyfullfield.ValueAsText;
                }
                catch { SurveyFull = " "; }

                double GridX = 0;
                DataRecordAPI.IDataValue gridxfield = null;
                try
                {
                    gridxfield = dr.GetField("admin1.ToWhom_CSU.gridx").DataValue;
                    GridX = Convert.ToDouble(gridxfield.ValueAsText);
                }
                catch { GridX = 0; }

                double GridY = 0;
                DataRecordAPI.IDataValue gridyfield = null;
                try
                {
                    gridyfield = dr.GetField("admin1.ToWhom_CSU.gridy").DataValue;
                    GridY = Convert.ToDouble(gridyfield.ValueAsText);
                }
                catch { GridY = 0; }


                string Message = " ";
                DataRecordAPI.IDataValue messagefield = null;
                try
                {
                    messagefield = dr.GetField("admin1.arf.message").DataValue;
                    Message = messagefield.ValueAsText;
                    //MessageBox.Show(string.Format("Message= {0}", Message));

                }
                catch { Message = " "; }

                string Outcome = " ";
                string HStatus = " ";
                DataRecordAPI.IDataValue resphhfield = null;
                DataRecordAPI.IDataValue harmoutfield = null;
                DataRecordAPI.IDataValue hstatusfield = null;
                try
                {
                    resphhfield = dr.GetField("admin1.arf.resphh").DataValue;
                    harmoutfield = dr.GetField("Outcome.Outcome.HARMOUT").DataValue;
                    hstatusfield = dr.GetField("admin1.arf.hstatus").DataValue;

                    if (resphhfield.EnumerationValue == 3)
                    {
                        Outcome = "Reallocation";
                    }
                    else if (resphhfield.EnumerationValue != 3 & (harmoutfield.IntegerValue > 0 & harmoutfield.IntegerValue < 300))
                    {
                        Outcome = "Complete";
                    }
                    else if (resphhfield.EnumerationValue != 3 & (harmoutfield.IntegerValue > 300 & harmoutfield.IntegerValue < 400))
                    {
                        Outcome = "Non-Contact";
                    }
                    else if (resphhfield.EnumerationValue != 3 & (harmoutfield.IntegerValue > 400 & harmoutfield.IntegerValue < 500))
                    {
                        Outcome = "Refusal";
                    }
                    else if (resphhfield.EnumerationValue != 3 & (harmoutfield.IntegerValue > 500 & harmoutfield.IntegerValue < 600))
                    {
                        Outcome = "Other Non-Response";
                    }
                    else if (resphhfield.EnumerationValue != 3 & (harmoutfield.IntegerValue > 600 & harmoutfield.IntegerValue < 700))
                    {
                        Outcome = "Unknown Eligibility";
                    }
                    else if (resphhfield.EnumerationValue != 3 & harmoutfield.IntegerValue > 700)
                    {
                        Outcome = "Ineligible";
                    }



                    if (hstatusfield.EnumerationValue == 0)
                    {
                        HStatus = "No work done yet";
                    }
                    else if (hstatusfield.EnumerationValue == 1)
                    {
                        HStatus = "Calls made but no contact";
                    }
                    else if (hstatusfield.EnumerationValue == 2)
                    {
                        HStatus = "Contact made";
                    }
                    else if (hstatusfield.EnumerationValue == 3)
                    {
                        HStatus = "Interview started/done";
                    }
                    else if (hstatusfield.EnumerationValue == 4)
                    {
                        HStatus = "No interviewing required";
                    }
                    else { HStatus = "Untouched"; }
                    if (resphhfield.EnumerationValue != 3 & (harmoutfield.IntegerValue > 0))
                    {
                        HStatus = "Finalised";
                    }
                }
                catch
                {
                    Outcome = " ";
                    HStatus = " ";
                }

                DateTime SurveyStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                DataRecordAPI.IDataValue SurveyStartfield = null;
                try
                {
                    SurveyStartfield = dr.GetField("admin1.ToWhom_CSU.SurveyStart").DataValue;
                    string SurveyStartstr = SurveyStartfield.ValueAsText;
                    SurveyStart = DateTime.ParseExact(SurveyStartstr, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None);
                }
                catch (Exception)
                {
                    SurveyStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                }

                DateTime SurveyEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
                Int32 DaysLeft = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                DataRecordAPI.IDataValue SurveyEndfield = null;
                try
                {
                    SurveyEndfield = dr.GetField("admin1.ToWhom_CSU.SurveyEnd").DataValue;
                    string SurveyEndstr = SurveyEndfield.ValueAsText;
                    SurveyEnd = DateTime.ParseExact(SurveyEndstr, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None);
                    TimeSpan DaysLeftfield = SurveyEnd - DateTime.Now;
                    DaysLeft = DaysLeftfield.Days;
                }
                catch (Exception)
                {
                    SurveyEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
                    DaysLeft = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                }

                DateTime DiaryStartdt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                string DiaryStart = " ";
                DataRecordAPI.IDataValue DiaryStartfield = null;
                DataRecordAPI.IDataValue DiaryPlacedfield = null;

                if (SurveyShort.ToUpper() == "TRV")
                {
                    try
                    {
                        //                        DiaryStartfield = dr.GetField("admin1.ToWhom_CSU.DiaryStart").DataValue;
                        DiaryStartfield = dr.GetField("admin1.ToWhom_CSU.DiaryDate").DataValue;
                        string DiaryStartstr = DiaryStartfield.ValueAsText;
                        DiaryPlacedfield = dr.GetField("DiaryPlaced").DataValue;
                        //string DiaryPlacedstr = DiaryStartfield.ValueAsText;

                        DiaryStartdt = DateTime.ParseExact(DiaryStartstr, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None);
                        //    if (DiaryPlacedfield.EnumerationValue == 1)
                        //            {
                        //                DiaryStart = "     Diary Placed on: " + DiaryStartdt.ToString("dd/MM/yyyy");
                        //            }
                        //    else 
                        //            {
                        DiaryStart = "     Diary Start: " + DiaryStartdt.ToString("dd/MM/yyyy");
                        //            }
                        //THis next line removes the Diary text so as not to clutter the screen when there is a final outcome
                        if (resphhfield.EnumerationValue != 3 & harmoutfield.IntegerValue > 0)
                        {
                            DiaryStart = " ";
                        }

                    }
                    catch (Exception)
                    {
                        DiaryStartdt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                        DiaryStart = " ";
                    }
                }

                string SurveyMonth = " ";
                DataRecordAPI.IDataValue surveymonthfield = null;
                try
                {
                    surveymonthfield = dr.GetField("admin1.ToWhom_CSU.surveymonth").DataValue;
                    SurveyMonth = surveymonthfield.ValueAsText;
                    
                }
                catch { SurveyMonth = " "; }

                Int32 SurveyYear = 0;
                DataRecordAPI.IDataValue surveyyearfield = null;
                try
                {
                    surveyyearfield = dr.GetField("admin1.ToWhom_CSU.surveyyear").DataValue;
                    SurveyYear = Convert.ToInt32(surveyyearfield.IntegerValue);
                }
                catch { SurveyYear = 0; }

                string AddStrt = " ";
                string Practice = " ";
                DataRecordAPI.IDataValue addstrtfield = null;
                try
                {
                    addstrtfield = dr.GetField("admin1.ToWhom_CSU.add_strt").DataValue;
                    AddStrt = addstrtfield.ValueAsText;
                    if (intno == "9999")
                    {
                        AddStrt = "Practice";
                        Practice = "Practice";
                    }
                    else { Practice = "Active"; }
                }
                catch
                {
                    AddStrt = " ";
                    Practice = " ";
                }

                string Town = " ";
                DataRecordAPI.IDataValue townfield = null;
                try
                {
                    townfield = dr.GetField("admin1.ToWhom_CSU.town").DataValue;
                    Town = townfield.ValueAsText;
                }
                catch { Town = " "; }

                string Townland = " ";
                DataRecordAPI.IDataValue townlandfield = null;
                try
                {
                    townlandfield = dr.GetField("admin1.ToWhom_CSU.townland").DataValue;
                    Townland = townlandfield.ValueAsText;
                }
                catch { Townland = " "; }

                string Locality = " ";
                DataRecordAPI.IDataValue localityfield = null;
                try
                {
                    localityfield = dr.GetField("admin1.ToWhom_CSU.locality").DataValue;
                    Locality = localityfield.ValueAsText;
                }
                catch { Locality = " "; }

                string County = " ";
                DataRecordAPI.IDataValue countyfield = null;
                try
                {
                    countyfield = dr.GetField("admin1.ToWhom_CSU.county").DataValue;
                    County = countyfield.ValueAsText;
                }
                catch { County = " "; }

                string PostCode = " ";
                DataRecordAPI.IDataValue postcodefield = null;
                try
                {
                    postcodefield = dr.GetField("admin1.ToWhom_CSU.postcode").DataValue;
                    PostCode = postcodefield.ValueAsText;
                }
                catch { PostCode = " "; }
                //DateTime DataToday = DateTime.Now



                var CurrentCoord = new GeoCoordinate(_txtLat, _txtLong);
                var RecordCoord = new GeoCoordinate(GridX, GridY);
                //Convert from metres to miles
                double Distance = Math.Round((CurrentCoord.GetDistanceTo(RecordCoord) / 1609.344), 1);
                string DistanceString = " ";
                if (_txtLat == 0 | GridX == 0)
                { DistanceString = "N/A"; }
                else
                { DistanceString = Convert.ToString(Distance) + " miles"; }



                //MessageBox.Show(string.Format("Primary Key= {0}, Serial Number = {1}, Hhno = {2}, intno = {3} ", primkey, serno, hhno, intno));
                FullDataSet.Add(new FullRecord(survey, primkey, serno, hhno, intno, SurveyShort, SurveyFull, GridX, GridY, Outcome, HStatus, Message, SurveyStart, SurveyEnd, DiaryStart, SurveyMonth, SurveyYear, AddStrt, Town,
                    Townland, Locality, County, PostCode, DaysLeft, Distance, DistanceString, Practice, Environment));

            }


        }


    }
}
