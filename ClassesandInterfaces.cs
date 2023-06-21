using StatNeth.Blaise.API.DataEntry;
using StatNeth.Blaise.API.DataRecord;
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


namespace Blaise_App
{

 


    public class FullRecord : IFullRecordInterface
    {
        private IInstrumentInfo _instrument;
        private string _primkey;
        private string _serno;
        private string _hhno;
        private string _intno;
        private string _surveyshort;
        private string _surveyfull;
        private double _gridx;
        private double _gridy;
        private string _outcome;
        private string _hstatus;
        private string _message;
        private DateTime _surveystart;
        private DateTime _surveyend;
        private string _diarystart;
        private string _surveymonth;
        private Int32 _surveyyear;
        private string _addstrt;
        private string _town;
        private string _townland;
        private string _locality;
        private string _county;
        private string _postcode;
        private Int32 _daysleft;
        private double _distance;
        private string _distancestring;
        private string _practice;
        private string _environment;






        public FullRecord(IInstrumentInfo _instrument, string _primkey, string _serno, string _hhno, string _intno, string _surveyshort, string _surveyfull, double _gridx, double _gridy, 
            string _outcome, string _hstatus, string _message, DateTime _surveystart, DateTime _surveyend, string _diarystart, string _surveymonth, Int32 _surveyyear, string _addstrt, string _town,
            string _townland, string _locality, string _county, string _postcode,Int32 _daysleft, double _distance, string _distancestring, string _practice, string _environment)
        {
            this._instrument = _instrument;
            this._primkey = _primkey;
            this._serno = _serno;
            this._hhno = _hhno;
            this._intno = _intno;
            this._surveyshort = _surveyshort;
            this._surveyfull = _surveyfull;
            this._gridx = _gridx;
            this._gridy = _gridy;
            this._outcome = _outcome;
            this._hstatus = _hstatus;
            this._message = _message;
            this._surveystart = _surveystart;
            this._surveyend = _surveyend;
            this._diarystart = _diarystart;
            this._surveymonth = _surveymonth;
            this._surveyyear = _surveyyear;
            this._addstrt = _addstrt;
            this._town = _town;
            this._townland = _townland;
            this._locality = _locality;
            this._county = _county;
            this._postcode = _postcode;
            this._daysleft = _daysleft;
            this._distance = _distance;
            this._distancestring = _distancestring;
            this._practice = _practice;
            this._environment = _environment;

        }

        public IInstrumentInfo Instrument
        {
            get { return _instrument; }
            set { _instrument = value; }
        }

        public string Primkey
        {
            get { return _primkey; }
            set { _primkey = value; }
        }
        public string Serno
        {
            get { return _serno; }
            set { _serno = value; }
        }
        public string Hhno
        {
            get { return _hhno; }
            set { _hhno = value; }
        }
        public string Intno
        {
            get { return _intno; }
            set { _intno = value; }
        }

        public string SurveyShort
        {
            get { return _surveyshort; }
            set { _surveyshort = value; }
        }

        public string SurveyFull
        {
            get { return _surveyfull; }
            set { _surveyfull = value; }
        }
        public double GridX
        {
            get { return _gridx; }
            set { _gridx = value; }
        }
        public double GridY
        {
            get { return _gridy; }
            set { _gridy = value; }
        }
        public string Outcome
        {
            get { return _outcome; }
            set { _outcome = value; }
        }
        public string HStatus
        {
            get { return _hstatus; }
            set { _hstatus = value; }
        }
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }
        public DateTime SurveyStart
        {
            get { return _surveystart; }
            set { _surveystart = value; }
        }
        public DateTime SurveyEnd
        {
            get { return _surveyend; }
            set { _surveyend = value; }
        }
        public string DiaryStart
        {
            get { return _diarystart; }
            set { _diarystart = value; }
        }

        public string SurveyMonth
        {
            get { return _surveymonth; }
            set { _surveymonth = value; }
        }
        public Int32 SurveyYear
        {
            get { return _surveyyear; }
            set { _surveyyear = value; }
        }
        public string AddStrt
        {
            get { return _addstrt; }
            set { _addstrt = value; }
        }
        public string Town
        {
            get { return _town; }
            set { _town = value; }
        }
        public string Townland
        {
            get { return _townland; }
            set { _townland = value; }
        }
        public string Locality
        {
            get { return _locality; }
            set { _locality = value; }
        }
        public string County
        {
            get { return _county; }
            set { _county = value; }
        }
        public string PostCode
        {
            get { return _postcode; }
            set { _postcode = value; }
        }
        public Int32 DaysLeft
        {
            get { return _daysleft; }
            set { _daysleft = value; }
        }
        public double Distance
        {
            get { return _distance; }
            set { _distance = value; }
        }
        public string DistanceString
        {
            get { return _distancestring; }
            set { _distancestring = value; }
        }
        public string Practice
        {
            get { return _practice; }
            set { _practice = value; }
        }
        public string Environment
        {
            get { return _environment; }
            set { _environment = value; }
        }

    }

    public interface IFullRecordInterface
    {
        IInstrumentInfo Instrument { get; set; }
        string Primkey { get; set; }
        string Serno { get; set; }
        string Hhno { get; set; }
        string Intno { get; set; }
        string SurveyShort { get; set; }
        string SurveyFull { get; set; }
        double GridX { get; set; }
        double GridY { get; set; }
        string Outcome { get; set; }
        string HStatus { get; set; }
        string Message { get; set; }
        DateTime SurveyStart { get; set; }
        DateTime SurveyEnd { get; set; }
        string DiaryStart { get; set; }
        string SurveyMonth { get; set; }
        Int32 SurveyYear { get; set; }
        string AddStrt { get; set; }
        string Town { get; set; }
        string Townland { get; set; }
        string Locality { get; set; }
        string County { get; set; }
        string PostCode { get; set; }
        Int32 DaysLeft { get; set; }
        double Distance { get; set; }
        string DistanceString { get; set; }
        string Practice { get; set; }
        string Environment { get; set; }

    }

    //==================================================================================================================================================

    public class InstalledSurvey : IInstalledSurvey
    {
        private IInstrumentInfo _instrument;

        public InstalledSurvey(IInstrumentInfo _instrument)
        {
            this._instrument = _instrument;
        }

        public IInstrumentInfo Instrument
        {
            get { return _instrument; }
            set { _instrument = value; }
        }
    }

        public interface IInstalledSurvey
        {
            IInstrumentInfo Instrument { get; set; }
        }

    //=====================================================================================================================================================

    public class ProgressRecord : IProgressRecordInterface
    {
        private IInstrumentInfo _instrument;
        private string _primkey;
        private string _intno;
        private DateTime _intdate;
        private string _intname;
        private string _ftnum;
        private string _environment;

        public ProgressRecord(IInstrumentInfo _instrument, string _primkey, string _intno, DateTime _intdate, string _intname, string _ftnum, string _environment)
        {
            this._instrument = _instrument;
            this._primkey = _primkey;
            this._intno = _intno;
            this._intdate = _intdate;
            this._intname = _intname;
            this._ftnum = _ftnum;
            this._environment = _environment;


        }

        public IInstrumentInfo Instrument
        {
            get { return _instrument; }
            set { _instrument = value; }
        }

        public string Primkey
        {
            get { return _primkey; }
            set { _primkey = value; }
        }
        public string Intno
        {
            get { return _intno; }
            set { _intno = value; }
        }
        public DateTime IntDate
        {
            get { return _intdate; }
            set { _intdate = value; }
        }
        public string IntName
        {
            get { return _intname; }
            set { _intname = value; }
        }
        public string FTNum
        {
            get { return _ftnum; }
            set { _ftnum = value; }
        }
        public string Environment
        {
            get { return _environment; }
            set { _environment = value; }
        }



    }

    public interface IProgressRecordInterface
    {
        IInstrumentInfo Instrument { get; set; }
        string Primkey { get; set; }
        string Intno { get; set; }
        DateTime IntDate { get; set; }
        string IntName { get; set; }
        string FTNum { get; set; }
        string Environment { get; set; }
    }

    //=====================================================================================================================================================

    public class PerformanceRecord : IPerformanceRecordInterface
    {
        private IInstrumentInfo _instrument;
        private string _primkey;
        private string _intno;
        private DateTime _intdate;
        private string _intname;
        private string _ftnum;
        private string _environment;


        public PerformanceRecord(IInstrumentInfo _instrument, string _primkey, string _intno, DateTime _intdate, string _intname, string _ftnum, string _environment)
        {
            this._instrument = _instrument;
            this._primkey = _primkey;
            this._intno = _intno;
            this._intdate = _intdate;
            this._intname = _intname;
            this._ftnum = _ftnum;
            this._environment = _environment;
       

        }

        public IInstrumentInfo Instrument
        {
            get { return _instrument; }
            set { _instrument = value; }
        }

        public string Primkey
        {
            get { return _primkey; }
            set { _primkey = value; }
        }
        public string Intno
        {
            get { return _intno; }
            set { _intno = value; }
        }
        public DateTime IntDate
        {
            get { return _intdate; }
            set { _intdate = value; }
        }
        public string IntName
        {
            get { return _intname; }
            set { _intname = value; }
        }
        public string FTNum
        {
            get { return _ftnum; }
            set { _ftnum = value; }
        }
        public string Environment
        {
            get { return _environment; }
            set { _environment = value; }
        }


    }

    public interface IPerformanceRecordInterface
    {
        IInstrumentInfo Instrument { get; set; }
        string Primkey { get; set; }
        string Intno { get; set; }
        DateTime IntDate { get; set; }
        string IntName { get; set; }
        string FTNum { get; set; }
        string Environment { get; set; }

    }

    //=====================================================================================================================================================

    public class TraineeAppRecord : ITraineeAppRecordInterface
    {
        private IInstrumentInfo _instrument;
        private string _primkey;
        private string _intno;
        private DateTime _intdate;
        private string _intname;
        private string _ftnum;
        private string _environment;


        public TraineeAppRecord(IInstrumentInfo _instrument, string _primkey, string _intno, DateTime _intdate, string _intname, string _ftnum, string _environment)
        {
            this._instrument = _instrument;
            this._primkey = _primkey;
            this._intno = _intno;
            this._intdate = _intdate;
            this._intname = _intname;
            this._ftnum = _ftnum;
            this._environment = _environment;


        }

        public IInstrumentInfo Instrument
        {
            get { return _instrument; }
            set { _instrument = value; }
        }

        public string Primkey
        {
            get { return _primkey; }
            set { _primkey = value; }
        }
        public string Intno
        {
            get { return _intno; }
            set { _intno = value; }
        }
        public DateTime IntDate
        {
            get { return _intdate; }
            set { _intdate = value; }
        }
        public string IntName
        {
            get { return _intname; }
            set { _intname = value; }
        }
        public string FTNum
        {
            get { return _ftnum; }
            set { _ftnum = value; }
        }
        public string Environment
        {
            get { return _environment; }
            set { _environment = value; }
        }

    }

    public interface ITraineeAppRecordInterface
    {
        IInstrumentInfo Instrument { get; set; }
        string Primkey { get; set; }
        string Intno { get; set; }
        DateTime IntDate { get; set; }
        string IntName { get; set; }
        string FTNum { get; set; }
        string Environment { get; set; }

    }
    //====================================================================================================================================================
    public class NIPSMenuRecord : INIPSMenuInterface
    {
        private IInstrumentInfo _instrument;
        private string _nipsmonth;
        private Int32 _nipsmonthno;
        private Int32 _nipsyear;
        private string _environment;


        public NIPSMenuRecord(IInstrumentInfo _instrument, string _nipsmonth, Int32 _nipsmonthno, Int32 _nipsyear, string _environment)
        {
            this._instrument = _instrument;
            this._nipsmonth = _nipsmonth;
            this._nipsmonthno = _nipsmonthno;
            this._nipsyear = _nipsyear;
            this._environment = _environment;


        }

        public IInstrumentInfo Instrument
        {
            get { return _instrument; }
            set { _instrument = value; }
        }

        public string NIPSMonth
        {
            get { return _nipsmonth; }
            set { _nipsmonth = value; }
        }
        public Int32 NIPSMonthNo
        {
            get { return _nipsmonthno; }
            set { _nipsmonthno = value; }
        }
        public Int32 NIPSYear
        {
            get { return _nipsyear; }
            set { _nipsyear = value; }
        }
        public string Environment
        {
            get { return _environment; }
            set { _environment = value; }
        }

    }



    public interface INIPSMenuInterface
    {
    IInstrumentInfo Instrument { get; set; }
    string NIPSMonth { get; set; }
    Int32 NIPSMonthNo { get; set; }
    Int32 NIPSYear { get; set; }
    string Environment { get; set; }

    }

    //========================================================================================================================================================

    public class NIPSDetailsRecord : INIPSDetailsInterface
    {
        private IInstrumentInfo _instrument;
        private string _primkey;
        private string _serno;
        private string _intno;
        private string _site;
        private DateTime _intdate;
        private DateTime _tstart;
        private string _outcome;
        private string _nation;
        private Int32 _ninights;
        private Int32 _roinights;
        private string _age;
        private string _gender;
        private string _environment;


        public NIPSDetailsRecord(IInstrumentInfo _instrument, string _primkey, string _serno, string _intno, string _site, DateTime _intdate, DateTime _tstart, string _outcome, string _nation, Int32 _ninights, Int32 _roinights, string _age, string _gender, string _environment)
        {
            this._instrument = _instrument;
            this._primkey = _primkey;
            this._serno = _serno;
            this._intno = _intno;
            this._site = _site;
            this._intdate = _intdate;
            this._tstart = _tstart;
            this._outcome = _outcome;
            this._nation = _nation;
            this._ninights = _ninights;
            this._roinights = _roinights;
            this._age = _age;
            this._gender = _gender;
            this._environment = _environment;

        }

        public IInstrumentInfo Instrument
        {
            get { return _instrument; }
            set { _instrument = value; }
        }
        public string Primkey
        {
            get { return _primkey; }
            set { _primkey = value; }
        }
        public string Serno
        {
            get { return _serno; }
            set { _serno = value; }
        }
        public string Intno
        {
            get { return _intno; }
            set { _intno = value; }
        }
        public string Site
        {
            get { return _site; }
            set { _site = value; }
        }
        public DateTime Intdate
        {
            get { return _intdate; }
            set { _intdate = value; }
        }
        public DateTime TStart
        {
            get { return _tstart; }
            set { _tstart = value; }
        }
        public string Outcome
        {
            get { return _outcome; }
            set { _outcome = value; }
        }
        public string Nation
        {
            get { return _nation; }
            set { _nation = value; }
        }

        public Int32 NINights
        {
            get { return _ninights; }
            set { _ninights = value; }
        }
        public Int32 ROINights
        {
            get { return _roinights; }
            set { _roinights = value; }
        }
        public string Age
        {
            get { return _age; }
            set { _age = value; }
        }
        public string Gender
        {
            get { return _gender; }
            set { _gender = value; }
        }
        public string Environment
        {
            get { return _environment; }
            set { _environment = value; }
        }

    }



    public interface INIPSDetailsInterface
    {
        IInstrumentInfo Instrument { get; set; }
        string Primkey { get; set; }
        string Serno { get; set; }
        string Intno { get; set; }
        string Site { get; set; }
        DateTime Intdate { get; set; }
        DateTime TStart { get; set; }
        string Outcome { get; set; }
        string Nation { get; set; }
        Int32 NINights { get; set; }
        Int32 ROINights { get; set; }
        string Age { get; set; }
        string Gender { get; set; }
        string Environment { get; set; }
    }


    //========================================================================================================================================================

    public class ShiftDetailsRecord : IShiftDetailsInterface
    {
        private IInstrumentInfo _instrument;
        private string _primkey;
        private string _site;
        private DateTime _shftdate;
        private string _AMPM;
        private string _teaml;
        private string _environment;


        public ShiftDetailsRecord(IInstrumentInfo _instrument, string _primkey, string _site, DateTime _shftdate, string _AMPM, string _teaml,string _environment)
        {
            this._instrument = _instrument;
            this._primkey = _primkey;
            this._site = _site;
            this._shftdate = _shftdate;
            this._AMPM = _AMPM;
            this._teaml = _teaml;
            this._environment = _environment;

        }

        public IInstrumentInfo Instrument
        {
            get { return _instrument; }
            set { _instrument = value; }
        }
        public string Primkey
        {
            get { return _primkey; }
            set { _primkey = value; }
        }
        public string Site
        {
            get { return _site; }
            set { _site = value; }
        }
        public DateTime Shftdate
        {
            get { return _shftdate; }
            set { _shftdate = value; }
        }
        public string AMPM
        {
            get { return _AMPM; }
            set { _AMPM = value; }
        }
        public string TeamL
        {
            get { return _teaml; }
            set { _teaml = value; }
        }
        public string Environment
        {
            get { return _environment; }
            set { _environment = value; }
        }

    }



    public interface IShiftDetailsInterface
    {
        IInstrumentInfo Instrument { get; set; }
        string Primkey { get; set; }
        string Site { get; set; }
        DateTime Shftdate { get; set; }
        string AMPM { get; set; }
        string TeamL { get; set; }
        string Environment { get; set; }

    }

    //=====================================================================================================================================================

    public class CurrentWagesRecord : ICurrentWagesInterface
    {
        private IInstrumentInfo _instrument;
        private string _primkey;
        private string _intno;
        private DateTime _wagedate;
        private Int32 _refno;
        private Int32 _nolines;
        private string _environment;


        public CurrentWagesRecord(IInstrumentInfo _instrument, string _primkey, string _intno, DateTime _wagedate, Int32 _refno, Int32 _nolines, string _environment)
        {
            this._instrument = _instrument;
            this._primkey = _primkey;
            this._intno = _intno;
            this._wagedate = _wagedate;
            this._refno = _refno;
            this._nolines = _nolines;
            this._environment = _environment;


        }

        public IInstrumentInfo Instrument
        {
            get { return _instrument; }
            set { _instrument = value; }
        }

        public string Primkey
        {
            get { return _primkey; }
            set { _primkey = value; }
        }
        public string Intno
        {
            get { return _intno; }
            set { _intno = value; }
        }
        public DateTime WageDate
        {
            get { return _wagedate; }
            set { _wagedate = value; }
        }
        public Int32 RefNo
        {
            get { return _refno; }
            set { _refno = value; }
        }
        public Int32 NoLines
        {
            get { return _nolines; }
            set { _nolines = value; }
        }
        public string Environment
        {
            get { return _environment; }
            set { _environment = value; }
        }


    }

    public interface ICurrentWagesInterface
    {
        IInstrumentInfo Instrument { get; set; }
        string Primkey { get; set; }
        string Intno { get; set; }
        DateTime WageDate { get; set; }
        Int32 RefNo { get; set; }
        Int32 NoLines { get; set; }
        string Environment { get; set; }

    }

    //=====================================================================================================================================================

    public class ArchivedWagesRecord : IArchivedWagesInterface
    {
        private IInstrumentInfo _instrument;
        private string _primkey;
        private string _intno;
        private DateTime _wagedate;
        private Int32 _refno;
        private Int32 _totmiles;
        private Int32 _tothours;
        private Int32 _totmins;
        private string _wageyear;
        private string _environment;


        public ArchivedWagesRecord(IInstrumentInfo _instrument, string _primkey, string _intno, DateTime _wagedate, Int32 _refno, Int32 _totmiles, Int32 _tothours, Int32 _totmins, string _wageyear, string _environment)
        {
            this._instrument = _instrument;
            this._primkey = _primkey;
            this._intno = _intno;
            this._wagedate = _wagedate;
            this._refno = _refno;
            this._totmiles = _totmiles;
            this._tothours = _tothours;
            this._totmins = _totmins;
            this._wageyear = _wageyear;
            this._environment = _environment;


        }

        public IInstrumentInfo Instrument
        {
            get { return _instrument; }
            set { _instrument = value; }
        }

        public string Primkey
        {
            get { return _primkey; }
            set { _primkey = value; }
        }
        public string Intno
        {
            get { return _intno; }
            set { _intno = value; }
        }
        public DateTime WageDate
        {
            get { return _wagedate; }
            set { _wagedate = value; }
        }
        public Int32 RefNo
        {
            get { return _refno; }
            set { _refno = value; }
        }
        public Int32 TotMiles
        {
            get { return _totmiles; }
            set { _totmiles = value; }
        }
        public Int32 TotHours
        {
            get { return _tothours; }
            set { _tothours = value; }
        }
        public Int32 TotMins
        {
            get { return _totmins; }
            set { _totmins = value; }
        }
        public string WageYear
        {
            get { return _wageyear; }
            set { _wageyear = value; }
        }

        public string Environment
        {
            get { return _environment; }
            set { _environment = value; }
        }


    }

    public interface IArchivedWagesInterface
    {
        IInstrumentInfo Instrument { get; set; }
        string Primkey { get; set; }
        string Intno { get; set; }
        DateTime WageDate { get; set; }
        Int32 RefNo { get; set; }
        Int32 TotMiles { get; set; }
        Int32 TotHours { get; set; }
        Int32 TotMins { get; set; }
        string WageYear { get; set; }
        string Environment { get; set; }

    }

    //=====================================================================================================================================================

    public class ExpensesRecord : IExpensesInterface
    {
        private IInstrumentInfo _instrument;
        private string _primkey;
        private string _intno;
        private DateTime _claimdate;
        private Int32 _refno;
        private Decimal _totclaim;
        private string _wageyear;
        private string _environment;


        public ExpensesRecord(IInstrumentInfo _instrument, string _primkey, string _intno, DateTime _claimdate, Int32 _refno, Decimal _totclaim, string _wageyear, string _environment)
        {
            this._instrument = _instrument;
            this._primkey = _primkey;
            this._intno = _intno;
            this._claimdate = _claimdate;
            this._refno = _refno;
            this._totclaim = _totclaim;
            this._wageyear = _wageyear;
            this._environment = _environment;


        }

        public IInstrumentInfo Instrument
        {
            get { return _instrument; }
            set { _instrument = value; }
        }

        public string Primkey
        {
            get { return _primkey; }
            set { _primkey = value; }
        }
        public string Intno
        {
            get { return _intno; }
            set { _intno = value; }
        }
        public DateTime ClaimDate
        {
            get { return _claimdate; }
            set { _claimdate = value; }
        }
        public Int32 RefNo
        {
            get { return _refno; }
            set { _refno = value; }
        }
        public Decimal TotClaim
        {
            get { return _totclaim; }
            set { _totclaim = value; }
        }
        public string WageYear
        {
            get { return _wageyear; }
            set { _wageyear = value; }
        }

        public string Environment
        {
            get { return _environment; }
            set { _environment = value; }
        }


    }

    public interface IExpensesInterface
    {
        IInstrumentInfo Instrument { get; set; }
        string Primkey { get; set; }
        string Intno { get; set; }
        DateTime ClaimDate { get; set; }
        Int32 RefNo { get; set; }
        Decimal TotClaim { get; set; }
        string WageYear { get; set; }
        string Environment { get; set; }

    }


    //=====================================================================================================================================================

    public class AnnualLeaveRecord : IAnnualLeaveInterface
    {
        private IInstrumentInfo _instrument;
        private string _primkey;
        private string _intno;
        private Int32 _refno;
        private string _holtype;
        private DateTime _strtdate;
        private DateTime _enddate;
        private Int32 _holdays;
        private string _wageyear;
        private string _environment;


        public AnnualLeaveRecord(IInstrumentInfo _instrument, string _primkey, string _intno, Int32 _refno, string _holtype, DateTime _strtdate, DateTime _enddate, Int32 _holdays, string _wageyear, string _environment)
        {
            this._instrument = _instrument;
            this._primkey = _primkey;
            this._intno = _intno;
            this._refno = _refno;
            this._holtype = _holtype;
            this._strtdate = _strtdate;
            this._enddate = _enddate;
            this._holdays = _holdays;
            this._wageyear = _wageyear;
            this._environment = _environment;


        }

        public IInstrumentInfo Instrument
        {
            get { return _instrument; }
            set { _instrument = value; }
        }

        public string Primkey
        {
            get { return _primkey; }
            set { _primkey = value; }
        }
        public string Intno
        {
            get { return _intno; }
            set { _intno = value; }
        }
        public Int32 RefNo
        {
            get { return _refno; }
            set { _refno = value; }
        }
        public string Holtype
        {
            get { return _holtype; }
            set { _holtype = value; }
        }

        public DateTime StrtDate
        {
            get { return _strtdate; }
            set { _strtdate = value; }
        }
        public DateTime EndDate
        {
            get { return _enddate; }
            set { _enddate = value; }
        }
        public Int32 Holdays
        {
            get { return _holdays; }
            set { _holdays = value; }
        }
        public string WageYear
        {
            get { return _wageyear; }
            set { _wageyear = value; }
        }

        public string Environment
        {
            get { return _environment; }
            set { _environment = value; }
        }


    }

    public interface IAnnualLeaveInterface
    {
        IInstrumentInfo Instrument { get; set; }
        string Primkey { get; set; }
        string Intno { get; set; }
        Int32 RefNo { get; set; }
        string Holtype { get; set; }
        DateTime StrtDate { get; set; }
        DateTime EndDate { get; set; }
        Int32 Holdays { get; set; }
        string WageYear { get; set; }
        string Environment { get; set; }

    }


}

