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
using System.Windows.Shapes;
using StatNeth.Blaise.API.DataEntry;
using System.Threading;

namespace Blaise_App {
    /// <summary>
    /// Interaction logic for SurveyWindow.xaml
    /// </summary>
    public partial class SurveyWindow : Window {
        public SurveyWindow() {
            InitializeComponent();
            Closing += SurveyWindow_Closing;
           
        }

        private void SurveyWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //This line rebuilds the Observable Collection FullDataSet which shows the list of cases for interviewers 
            if (AppController.Instance._connectedtoSurveys == true)
            {
                //MessageBox.Show(string.Format("Connected to Surveys= {0} CurrentEnvironment =  {1}", AppController.Instance._connectedtoSurveys, AppController.Instance.currentenvironment));
                
                //AppController.Instance.SetData(AppController.Instance.InstalledonSurveys.Instruments, true, "C:\\B5SURVEYS");
            }
            else if (AppController.Instance._connectedtoRespond == true)
            {
                //MessageBox.Show(string.Format("Connected to Respond= {0} CurrentEnvironment =  {1}", AppController.Instance._connectedtoRespond, AppController.Instance.currentenvironment));
                
                AppController.Instance.SetData(AppController.Instance.InstalledonRespond.Instruments, true, "C:\\B5RESPOND");
            }



            //MessageBox.Show(string.Format("Connected to Surveys= {0} Connected to Respond =  {1}", AppController.Instance._connectedtoSurveys, AppController.Instance._connectedtoRespond));


            if (_controller != null)
                try
                {
                    _controller.ExecuteAction(ActionManager.CreateAction(StatNeth.Blaise.API.DataEntry.Actions.ActionKind.Quit));
                    AppController.Instance.SetData(AppController.Instance.InstalledonSurveys.Instruments, true, "C:\\B5RESEARCH");
                }
                catch (Exception)
                { }
           // ApplicationCommands.ShowSurveyDetails;
        }

        private void DataEntryController_OnQuestionnaireCompleted(object sender, EventArgs e) {
            // dont close yet;)
        }

        private IDataEntryController _controller;
        public IDataEntryController DataEntryController {
            get {
                return _controller;
            }
            set {
                if (_controller != null) _controller.OnQuestionnaireCompleted -= DataEntryController_OnQuestionnaireCompleted;
                
                _controller = value;
                _controller.OnQuestionnaireCompleted += DataEntryController_OnQuestionnaireCompleted;
            }
        }

       
    }
}
