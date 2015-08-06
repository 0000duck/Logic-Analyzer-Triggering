using ABB.Robotics.Paint.RobView.Database.SignalLog;
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

namespace LogialAnalyzerTrigger
{
    /// <summary>
    /// Interaction logic for CustomTriggerWindow.xaml
    /// </summary>
    public partial class WPFCustomTrigger : Window
    {
        public event EventHandler<EventArgsCustomTriggerAdded> NotifyGUI;

        private GridTriggerForm _triggerForm;
        private List<SignalHandler> _avalibleSignalHandlers;
        private List<Custom> _customTriggerList;

        public WPFCustomTrigger()
        {
			DataContext = new TriggerStateMachineVm(new TriggerStateMachine());
            _avalibleSignalHandlers = new List<SignalHandler>();
            _customTriggerList = new List<Custom>();
            // Testing purposes
            var signal = new Signal() { Name = "A1" };
            var signalHandler = new SignalHandler(signal);
            _avalibleSignalHandlers.Add(signalHandler);

            InitializeComponent();
        }

        public WPFCustomTrigger(List<SignalHandler> avalibleSignalHandlers)
        {
            this._avalibleSignalHandlers = avalibleSignalHandlers;
            this._customTriggerList = new List<Custom>();
            InitializeComponent();
        }

        public WPFCustomTrigger(List<SignalHandler> avalibleSignalHandlers, List<Custom> customTriggers)
        {
            this._avalibleSignalHandlers = avalibleSignalHandlers;
            this._customTriggerList = customTriggers;

            InitializeComponent();
        }

        private void Loaded_MainWindow(object sender, RoutedEventArgs e)
        {
            _triggerForm = new GridTriggerForm(_avalibleSignalHandlers, _customTriggerList);
            _formScrollViewer.Content = _triggerForm;
        }

        private void Click_CancelButton(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Click_SaveButton(object sender, RoutedEventArgs e)
        {
            if (_nameInputTextBox.Text.Equals("Enter Name") || string.IsNullOrWhiteSpace(_nameInputTextBox.Text))
            {
                MessageBox.Show("Please enter name of the custom Trigger");
                return;
            }

            var succses = _triggerForm.Assemble_Triggers();
            if (succses)
            {
                var customTrigger = new Custom(_triggerForm.TriggerList, AlgerbraOperator.First);
                customTrigger.Name = _nameInputTextBox.Text;

                if (NotifyGUI != null)
                {
                    NotifyGUI(this, new EventArgsCustomTriggerAdded() { Trigger = customTrigger });
                }

                this.Close();
            }
        }
    }
}
