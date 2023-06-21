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

namespace StarterKit.Windows {
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
            
                AppController.Instance.SetData(AppController.Instance.InstalledSurveys.Instruments);
            

            if (_controller != null)
                _controller.ExecuteAction(ActionManager.CreateAction(StatNeth.Blaise.API.DataEntry.Actions.ActionKind.Quit));
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
