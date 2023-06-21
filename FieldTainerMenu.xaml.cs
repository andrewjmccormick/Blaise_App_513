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

namespace Blaise_App
{

    /// <summary>
    /// Interaction logic for Surveys.xaml
    /// </summary>
    public partial class FieldTrainerMenu : UserControl
    {
        public delegate void dg_synchronised();
      

        public FieldTrainerMenu()
        {

            InitializeComponent();
            //AppController.Instance.SetData(AppController.Instance.InstalledSurveys.Instruments);
            //Show_cases.IsEnabled = true;
            //this.ev_repaintsynchronising_buttons += new dg_synchronised(EventSynchronisingMessage);
            if (AppController.Instance._progressinstrument != null)
            {
                Button btn2 = Show_Progress;
                btn2.IsEnabled = true;
                btn2.Content = "Team Progress";
                btn2.Background = Brushes.CornflowerBlue;
            }
            else
            {
                Button btn2 = Show_Progress;
                btn2.IsEnabled = false;
                btn2.Content = "Not available";
            }
            if (AppController.Instance._performanceinstrument != null)
            {
                Button btn2 = Show_performance;
                btn2.IsEnabled = true;
                btn2.Content = "Team Performance";
                btn2.Background = Brushes.CornflowerBlue;
            }
            else
            {
                Button btn2 = Show_performance;
                btn2.IsEnabled = false;
                btn2.Content = "Not available";
            }
            if (AppController.Instance._traineeappinstrument != null)
            {
                Button btn2 = Show_TraineeApp;
                btn2.IsEnabled = true;
                btn2.Content = "Trainee Appraisal";
                btn2.Background = Brushes.CornflowerBlue;
            }
            else
            {
                Button btn2 = Show_TraineeApp;
                btn2.IsEnabled = false;
                btn2.Content = "Not available";
            }

            //surveysMain.ItemsSource = FullDataSet;
        }


        private void Show_Progress_Click(object sender, RoutedEventArgs e)
        {
            var c = new ProgressDetails() { DataContext = AppController.Instance.ProgressDataSet };
            var d = new Window() { Owner = AppController.Instance.Window, ResizeMode = ResizeMode.NoResize, WindowState = AppController.Instance.Window.WindowState, Icon = AppController.Instance.Window.Icon, WindowStartupLocation = AppController.Instance.Window.WindowStartupLocation, Content = c, Title = "Browse Communications with your team" };
            d.ShowDialog();
        }

        private void Show_Performance_Click(object sender, RoutedEventArgs e)
        {
            var c = new PerformanceDetails() { DataContext = AppController.Instance.PerformanceDataSet };
            var d = new Window() { Owner = AppController.Instance.Window, ResizeMode = ResizeMode.NoResize, WindowState = AppController.Instance.Window.WindowState, Icon = AppController.Instance.Window.Icon, WindowStartupLocation = AppController.Instance.Window.WindowStartupLocation, Content = c, Title = "Browse Communications with your team" };
            d.ShowDialog();
        }

        private void Show_TraineeApp_Click(object sender, RoutedEventArgs e)
        {
            var c = new TraineeAppDetails() { DataContext = AppController.Instance.TraineeAppDataSet };
            var d = new Window() { Owner = AppController.Instance.Window, ResizeMode = ResizeMode.NoResize, WindowState = AppController.Instance.Window.WindowState, Icon = AppController.Instance.Window.Icon, WindowStartupLocation = AppController.Instance.Window.WindowStartupLocation, Content = c, Title = "Browse Communications with your team" };
            d.ShowDialog();
        }

    }
}

















