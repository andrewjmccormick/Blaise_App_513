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

namespace Blaise_App
{
    /// <summary>
    /// Interaction logic for WAGESMenu.xaml
    /// </summary>
    public partial class WAGESMenu : UserControl
    {
        public WAGESMenu()
        {
            InitializeComponent();
            if (AppController.Instance._currentwagesinstrument != null)
            {
                Button btn2 = Show_Current_Wages;
                btn2.IsEnabled = true;
                btn2.Content = "Current Wages";
                btn2.Background = Brushes.CornflowerBlue;
            }
            else
            {
                Button btn2 = Show_Current_Wages;
                btn2.IsEnabled = false;
                btn2.Content = "Not available";
            }
            if (AppController.Instance._currentwagesinstrument != null)
            {
                Button btn2 = Show_Archived_Wages;
                btn2.IsEnabled = true;
                btn2.Content = "Archived Wages";
                btn2.Background = Brushes.CornflowerBlue;
            }
            else
            {
                Button btn2 = Show_Archived_Wages;
                btn2.IsEnabled = false;
                btn2.Content = "Not available";
            }


            if (AppController.Instance._expensesinstrument != null)
            {
                Button btn2 = Show_Expenses_Claim;
                btn2.IsEnabled = true;
                btn2.Content = "Expenses Claim";
                btn2.Background = Brushes.CornflowerBlue;
            }
            else
            {
                Button btn2 = Show_Expenses_Claim;
                btn2.IsEnabled = false;
                btn2.Content = "Not available";
            }

            if (AppController.Instance._annualleaveinstrument != null)
            {
                Button btn2 = Show_Leave_Request;
                btn2.IsEnabled = true;
                btn2.Content = "Leave Request";
                btn2.Background = Brushes.CornflowerBlue;
            }
            else
            {
                Button btn2 = Show_Leave_Request;
                btn2.IsEnabled = false;
                btn2.Content = "Not available";
            }

        }

        private void Show_Current_Wages_Click(object sender, RoutedEventArgs e)
        {
            var c = new Current_Wages_Menu() { DataContext = AppController.Instance.CurrentWagesDataSet };
            var d = new Window() { Owner = AppController.Instance.Window, ResizeMode = ResizeMode.NoResize, WindowState = AppController.Instance.Window.WindowState, Icon = AppController.Instance.Window.Icon, WindowStartupLocation = AppController.Instance.Window.WindowStartupLocation, Content = c, Title = "Current Wages" };
            d.ShowDialog();
        }
        private void Show_Archived_Wages_Click(object sender, RoutedEventArgs e)
        {
            var c = new Archived_Wages_Menu() { DataContext = AppController.Instance.ArchivedWagesDataSet };
            var d = new Window() { Owner = AppController.Instance.Window, ResizeMode = ResizeMode.NoResize, WindowState = AppController.Instance.Window.WindowState, Icon = AppController.Instance.Window.Icon, WindowStartupLocation = AppController.Instance.Window.WindowStartupLocation, Content = c, Title = "Archived Wages" };
            d.ShowDialog();
        }

        private void Show_Expenses_Claim_Click(object sender, RoutedEventArgs e)
        {
            var c = new ExpensesMenu() { DataContext = AppController.Instance.ExpensesDataSet };
            var d = new Window() { Owner = AppController.Instance.Window, ResizeMode = ResizeMode.NoResize, WindowState = AppController.Instance.Window.WindowState, Icon = AppController.Instance.Window.Icon, WindowStartupLocation = AppController.Instance.Window.WindowStartupLocation, Content = c, Title = "Expenses" };
            d.ShowDialog();
        }

        private void Show_Leave_Request_Click(object sender, RoutedEventArgs e)
        {
            var c = new AnnualLeave() { DataContext = AppController.Instance.AnnualLeaveDataSet };
            var d = new Window() { Owner = AppController.Instance.Window, ResizeMode = ResizeMode.NoResize, WindowState = AppController.Instance.Window.WindowState, Icon = AppController.Instance.Window.Icon, WindowStartupLocation = AppController.Instance.Window.WindowStartupLocation, Content = c, Title = "Annual Leave" };
            d.ShowDialog();
        }


    }
}
