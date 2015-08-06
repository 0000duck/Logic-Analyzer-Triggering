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
    /// Interaction logic for SetUpTriggers.xaml
    /// </summary>
    public partial class ConfigTriggers : Window
    {
        private Grid _mainPanel, _setUpTriggerPanel;
        private TextBlock _textField;
        private TriggerEngine _engine;
        private GUISetUpTriggerEvent _setUpTriggerGrid;
        private List<Signal> _signals;
        private List<TriggerEvent> _triggerEvents;

        public ConfigTriggers()
        {
            // For testing without robview;
            this._signals = new List<Signal>();
            var signal1 = new Signal();
            signal1.Name = "A1";
            _signals.Add(signal1);

            this._engine = new TriggerEngine();

            InitializeComponent();
        }

        public ConfigTriggers(List<Signal> signals, TriggerEngine engine)
        {
            this._signals = signals;
            this._engine = engine;
            InitializeComponent();
        }

        private void Main_Window_Loaded(object sender, RoutedEventArgs e)
        {
            _triggerEvents = new List<TriggerEvent>();

            _mainPanel = Create_MainPanel();
            MainPanelTabItem.Children.Add(_mainPanel);
        

        }
       
        private Grid Create_MainPanel()
        {
            var mainPanel = new Grid();
            mainPanel.ColumnDefinitions.Add(new ColumnDefinition());
            mainPanel.RowDefinitions.Add(new RowDefinition());
            RowDefinition rowDef = new RowDefinition();
            rowDef.MaxHeight = 30;
            mainPanel.RowDefinitions.Add(rowDef);

            _textField = new TextBlock();
            Grid.SetRow(_textField, 0);
            Grid.SetColumn(_textField, 0);


       
            // Button Panel
            var buttonPanel = new Grid();

            buttonPanel.RowDefinitions.Add(new RowDefinition());
            for (int i = 0; i < 2;i++)
                buttonPanel.ColumnDefinitions.Add(new ColumnDefinition());
            
            Grid.SetRow(buttonPanel, 1);
            Grid.SetColumn(buttonPanel, 0);

            var addEventButton = new Button();
            addEventButton.Content = "Add State";
            addEventButton.MaxWidth = 100;
            Grid.SetColumn(addEventButton, 0);
            Grid.SetRow(addEventButton, 0);
            addEventButton.Click += ChangeTabTo1;


            var runButton = new Button();
            runButton.Content = "Save";
            runButton.MaxWidth = 100;
            Grid.SetColumn(runButton, 1);
            Grid.SetRow(runButton, 0);
            runButton.Click += runButton_Click;

            buttonPanel.Children.Add(addEventButton);
            buttonPanel.Children.Add(runButton);

            mainPanel.Children.Add(_textField);
            mainPanel.Children.Add(buttonPanel);

            return mainPanel;
        }

        private Grid Create_AddEventPanel()
        {
            var addEventPanel = new Grid();
            addEventPanel.ColumnDefinitions.Add(new ColumnDefinition());
            RowDefinition rowDefTop = new RowDefinition();
            rowDefTop.MaxHeight = 30;
            addEventPanel.RowDefinitions.Add(rowDefTop);
            
            
            addEventPanel.RowDefinitions.Add(new RowDefinition());
          
            RowDefinition rowDefBot = new RowDefinition();
            rowDefBot.MaxHeight = 30;
            addEventPanel.RowDefinitions.Add(rowDefBot);
            // Header Panel
            var headerPanel = GUICreateWPFControls.CreateHeader(0, 0);

            // Button Panel
            var buttonPanel = new Grid();

            buttonPanel.RowDefinitions.Add(new RowDefinition());
            for (int i = 0; i < 2; i++)
                buttonPanel.ColumnDefinitions.Add(new ColumnDefinition());

            Grid.SetRow(buttonPanel, 2);
            Grid.SetColumn(buttonPanel, 0);

            var addEventButton = new Button();
            addEventButton.Content = "Back <-";
            addEventButton.MaxWidth = 100;
            Grid.SetColumn(addEventButton, 0);
            Grid.SetRow(addEventButton, 0);
            addEventButton.Click += ChangeTabTo0;


            var runButton = new Button();
            runButton.Content = "Add Event";
            runButton.MaxWidth = 100;
            Grid.SetColumn(runButton, 1);
            Grid.SetRow(runButton, 0);
            runButton.Click += save_event_Click;

            buttonPanel.Children.Add(addEventButton);
            buttonPanel.Children.Add(runButton);

            addEventPanel.Children.Add(headerPanel);
            addEventPanel.Children.Add(_setUpTriggerGrid.InputGrid);
            addEventPanel.Children.Add(buttonPanel);

            return addEventPanel;
        }

        private void ChangeTabTo1(object sender, RoutedEventArgs e)
        {
            _setUpTriggerGrid = new GUISetUpTriggerEvent(_signals);
            _setUpTriggerPanel = Create_AddEventPanel();
            AddEventPanelTabItem.Children.Add(_setUpTriggerPanel);
            TabController.SelectedIndex = 1;
        }
       
        private void ChangeTabTo0(object sender, RoutedEventArgs e)
        {
            TabController.SelectedIndex = 0;
        }
       
        void runButton_Click(object sender, RoutedEventArgs e)
        {
            //_engine.SignalList = _setUpTriggerGrid.SignalHandles;
            _engine.TriggerEvents = _triggerEvents;
            this.Close();
        }

        void save_event_Click(object sender, RoutedEventArgs e)
        {
            if (_setUpTriggerGrid.Assemble_Triggers())
            {
                TabController.SelectedIndex = 0;
        
                var triggerEvent = new TriggerEvent(_setUpTriggerGrid.TriggerList);
                triggerEvent.Name = _setUpTriggerGrid.EventName;
                _triggerEvents.Add(triggerEvent);
       
                 _textField.Text += triggerEvent.Name + ":\nIF:\n" + _setUpTriggerGrid.ToString() + "\nTHEN\nTrigger\n---\n";

                 foreach (SignalHandler sh in _setUpTriggerGrid.SignalHandles)
                 {
                     _engine.SignalList.Add(sh);
                 }
            }
            else
            {
                MessageBox.Show("Not Valid Input");
            }
          
        }
        
    }
}
