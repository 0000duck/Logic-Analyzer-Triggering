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
    /// Interaction logic for AddTrigger.xaml
    /// </summary>
    public partial class WPFAddTrigger : Window
    {
        public event EventHandler<EventArgsTriggerAdded> NotifyGUI;

        private GridTriggerForm _triggerForm;
        private List<Label> _ifLabelList;
        private List<Custom> _customTriggerList;
        private TriggerStateMachine _trigger;
        
        private TextBox _triggerNameEntry;
        private bool _isReadyToBeSent = false;


        public WPFAddTrigger()
        {
            var signal = new Signal();
            signal.Name = "Sig1";
            var signalHandler = new SignalHandler(signal);
            var test = new Equals(signalHandler, 0, AlgerbraOperator.First);
            _trigger = new TriggerStateMachine(test);
            _trigger.Name = "Navn";
            InitializeComponent();
        }

        public WPFAddTrigger()
        {
            this._customTriggerList = new List<Custom>();
            InitializeComponent();
        }

        public WPFAddTrigger(List<SignalHandler> signalHandlers, List<Custom> customTriggerList)
        {
            this._customTriggerList = customTriggerList;
            InitializeComponent();
        }

        private Grid CreateStateGrid()
        {
            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            for (int i = 0; i < 3; i++)
                grid.RowDefinitions.Add(new RowDefinition());

            // State Number Label
            var stateNumberLabel = new Label();
            stateNumberLabel.Content = "State: X";
            Grid.SetColumn(stateNumberLabel, 0);
            Grid.SetRow(stateNumberLabel, 0);

            // Grid With the buttons
            var ifThenGrid = new Grid();

            var columnDef = new ColumnDefinition();
            columnDef.MaxWidth = 30;
            ifThenGrid.ColumnDefinitions.Add(columnDef);
            ifThenGrid.ColumnDefinitions.Add(new ColumnDefinition());

            for (int i = 0; i < 2; i++)
            {
                var rowDef = new RowDefinition();
                rowDef.MaxHeight = 30;
                ifThenGrid.RowDefinitions.Add(rowDef);
            }


            var ifButton = new Button();
            ifButton.Content = "If";
            ifButton.Click += Click_IFButton;
            Grid.SetColumn(ifButton, 0);
            Grid.SetRow(ifButton, 0);

            var ifLabel = new Label();
            ifLabel.Content = "---";
            _ifLabelList.Add(ifLabel);
            Grid.SetColumn(ifLabel, 1);
            Grid.SetRow(ifLabel, 0);

            var thenButton = new Button();
            thenButton.Content = "Then";
            Grid.SetColumn(thenButton, 0);
            Grid.SetRow(thenButton, 1);

            var thenLabel = new Label();
            thenLabel.Content = "Trigger";
            Grid.SetColumn(thenLabel, 1);
            Grid.SetRow(thenLabel, 1);

            ifThenGrid.Children.Add(ifButton);
            ifThenGrid.Children.Add(ifLabel);
            ifThenGrid.Children.Add(thenButton);
            ifThenGrid.Children.Add(thenLabel);

            ifThenGrid.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            ifThenGrid.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            ifThenGrid.Margin = new Thickness(50, 0, 0, 0);

            Grid.SetColumn(ifThenGrid, 0);
            Grid.SetRow(ifThenGrid, 1);

            //---------------------------
            // Border
            var seperator = new Separator();
            seperator.BorderBrush = Brushes.Black;
            seperator.Height = 2;
            seperator.Margin = new Thickness(30, 10, 30, 0);
            Grid.SetColumn(seperator, 0);
            Grid.SetRow(seperator, 2);

            grid.Children.Add(stateNumberLabel);
            grid.Children.Add(ifThenGrid);
            grid.Children.Add(seperator);
            return grid;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _ifLabelList = new List<Label>();
            _triggerNameEntry = new TextBox();
            _triggerNameEntry.Text = "Enter Name of Trigger";
            _triggerNameEntry.MaxWidth = 200;
            _triggerNameEntry.MinWidth = 200;
            _triggerNameEntry.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            _triggerNameEntry.IsReadOnly = false;

            _stackPanel.Children.Add(_triggerNameEntry);
            _stackPanel.Children.Add(CreateStateGrid());
        }

        private void Click_IFButton(object sender, RoutedEventArgs e)
        {
            _triggerForm = new GridTriggerForm(_signalHandlerList, _customTriggerList);
            _fillINTriggerFormGrid.Children.Add(_triggerForm);
            _tabControl.SelectedIndex = 1;

        }

        private void Click_CancelTriggerForm(object sender, RoutedEventArgs e)
        {
            _tabControl.SelectedIndex = 0;
        }

        private void Click_SaveTriggerForm(object sender, RoutedEventArgs e)
        {
            var succses = _triggerForm.Assemble_Triggers();
            if (succses)
            {
                _tabControl.SelectedIndex = 0;
                _ifLabelList[0].Content = _triggerForm.ToString();
                _isReadyToBeSent = true;
            }   
        }

        private void Click_SaveAddTrigger(object sender, RoutedEventArgs e)
        {
            if (_triggerNameEntry.Text.Equals("Enter Name of Trigger") || string.IsNullOrWhiteSpace(_triggerNameEntry.Text))
            {
                MessageBox.Show("Please enter name of the Trigger");
                return;
            }

            if (!_isReadyToBeSent)
            {
                MessageBox.Show("Please fill out the requierd fields");
                return;
            }

            var triggerEvent = new TriggerStateMachine(_triggerForm.TriggerList);
            triggerEvent.Name = _triggerNameEntry.Text;

            if (NotifyGUI != null)
            {
                NotifyGUI(this, new EventArgsTriggerAdded() { Trigger = triggerEvent, SignalHandlersInUse = _triggerForm.SignalHandlersUsed });
            }
            this.Close();
        }

        private void Click_CancelAddTrigger(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }


}
