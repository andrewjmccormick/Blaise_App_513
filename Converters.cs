using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using StatNeth.Blaise.API.DataEntry;
using System.Windows.Media;


namespace Blaise_App {

    /// <summary>
    /// IsEnabledActionConverter tests if an instrument has the specified action available
    /// </summary>
    //[ValueConversion(typeof(object), typeof(bool))]
    //public class IsEnabledActionConverter : IValueConverter {
    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
    //        IInstrumentInfo survey = value as IInstrumentInfo;
    //        if (survey != null && parameter is AvailableSurveyActions) {
    //            AvailableSurveyActions action = (AvailableSurveyActions)parameter;
    //            return (action & AppController.GetAvailableSurveyActions(survey)) > 0;
    //        }
    //        return false;
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
    //        if (value is bool) {
    //            return ((bool)value).ToString();
    //        }
    //        return string.Empty;
    //    }
    //}


    [ValueConversion(typeof(int), typeof(Brush))]
    public class DaysLeftConverter : IValueConverter  
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((int)value) > Cutoff;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public int Cutoff { get; set; }
    }

   

    [ValueConversion(typeof(string), typeof(Brush))]
    public class CaseFinalisedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((string)value) == Cutoff;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public string Cutoff { get; set; }
    }

    [ValueConversion(typeof(string), typeof(Brush))]
    public class CasePracticeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((string)value) != Cutoff;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public string Cutoff { get; set; }
    }

}
