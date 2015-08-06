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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LogialAnalyzerTrigger
{
    /// <summary>
    /// Interaction logic for TriggerOverview.xaml
    /// </summary>
    public partial class WPFTriggerOverview : Window
    {
        public event EventHandler<EventArgsUpdateEngine> UpdateEngine;

        private List<TriggerStateMachine> _triggerEvents;
        private List<Custom> _customTriggers;
        private List<SignalHandler> _avalibleSignals;
        private List<SignalHandler> _signalHandlersInUse;
        private Dictionary<TriggerStateMachine, Grid> _triggerGridPosition;
        private Dictionary<TriggerStateMachine, List<string>> _triggerLog;


        public WPFTriggerOverview()
        {
            this._triggerEvents = new List<TriggerStateMachine>();
            this._avalibleSignals = new List<SignalHandler>();
            this._avalibleSignals.Add(new SignalHandler(new Signal() { Name = "Test" }));
            this._signalHandlersInUse = new List<SignalHandler>();
            this._triggerGridPosition = new Dictionary<TriggerStateMachine, Grid>();
            this._triggerLog = new Dictionary<TriggerStateMachine, List<string>>();
            this._customTriggers = new List<Custom>();

            InitializeComponent();
        }

        public void ShowWindow(List<SignalHandler> signalList)
        {
            // The method that gets called from TriggerEngine

            _avalibleSignals = signalList;
            this.Show();
        }

        private void AddCustomTrigger(Object sender, EventArgsCustomTriggerAdded e)
        {
            // Delegate?? 

            var customTrigger = e.Trigger;
            _customTriggers.Add(customTrigger);

            var treeviewitem = new TreeViewItem() { Header = customTrigger.Name, Tag = customTrigger };
            treeviewitem.MouseDoubleClick += DoubleClick_CustomTriggerTreeviewItem;
            _treeViewCustomTriggers.Items.Add(treeviewitem);
            _treeViewCustomTriggers.IsExpanded = true;
        }

        public void UpdateTriggerLog(TriggerStateMachine trigger, DateTime time)
        {
            try
            {
                var triggerLog = _triggerLog[trigger];
                string writeString = "\nNr. " + (triggerLog.Count + 1) + " at " + time.ToString();
                triggerLog.Add(writeString);
            }
            catch (Exception ex)
            {
                ABB.Robotics.Paint.RobView.PluginAPI.Logger.Internal.LogException(ex);
            }

            // Threading stuff
            var grid = _triggerGridPosition[trigger];
            grid.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(
                delegate()
                {
                    grid.Background = Brushes.Green;
                }));
            //grid.Background = null;
            //grid.Background = Brushes.Green;

        }

        private void AddTrigger(Object sender, EventArgsTriggerAdded e)
        {
            var grid = new Grid();

            grid.MouseEnter += MouseEnter_TriggerGrid;
            grid.MouseLeave += MouseLeave_TriggerGrid;
            grid.MouseLeftButtonDown += Click_ViewEntryTriggerGrid;
            grid.Tag = e.Trigger;

            grid.ColumnDefinitions.Add(new ColumnDefinition());
            for (int i = 0; i < 1; i++)
                grid.ColumnDefinitions.Add(new ColumnDefinition() { MaxWidth = 70 });
            grid.RowDefinitions.Add(new RowDefinition() { MaxHeight = 30 });

            var triggerNameLabel = new Label();
            triggerNameLabel.Content = e.Trigger.Name;
            Grid.SetColumn(triggerNameLabel, 0);
            Grid.SetRow(triggerNameLabel, 0);

            var removeButton = new Button();
            removeButton.Content = "Remove";
            removeButton.Tag = e.Trigger;
            removeButton.Click += Click_RemoveTriggerButton;
            Grid.SetColumn(removeButton, 1);
            Grid.SetRow(removeButton, 0);



            grid.Children.Add(triggerNameLabel);
            grid.Children.Add(removeButton);


            _stackPanel.Children.Add(grid);

            _triggerEvents.Add(e.Trigger);


            foreach (SignalHandler signalHandler in e.SignalHandlersInUse)
            {
                if (!_signalHandlersInUse.Contains(signalHandler))
                {
                    _signalHandlersInUse.Add(signalHandler);
                }
            }

            _triggerGridPosition.Add(e.Trigger, grid);
            _triggerLog.Add(e.Trigger, new List<string>());

        }

        private void DoubleClick_CustomTriggerTreeviewItem(Object sender, MouseEventArgs e)
        {
            try
            {
                var treeviewitem = (TreeViewItem)sender;
                if (treeviewitem.Tag == null)
                {
                    return;
                }
                var customTrigger = (Custom)treeviewitem.Tag;
                MessageBox.Show(customTrigger.ToString());
            }
            catch
            {

            }

        }

        private void MouseLeave_TriggerGrid(object sender, MouseEventArgs e)
        {
            var grid = (Grid)sender;
            grid.Background = null;
        }

        private void MouseEnter_TriggerGrid(object sender, MouseEventArgs e)
        {
            var grid = (Grid)sender;
            grid.Background = Brushes.Orange;
        }

        private void Click_SetupNewTriggerButton(object sender, RoutedEventArgs e)
        {
            var configTrigger = new WPFAddTrigger(_avalibleSignals, _customTriggers);
            configTrigger.NotifyGUI += AddTrigger;

            //Textboxene virker ikke
            System.Windows.Forms.Integration.ElementHost.EnableModelessKeyboardInterop(configTrigger);

            configTrigger.Show();
        }

        private void Click_CancelButton(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void Click_SaveButton(object sender, RoutedEventArgs e)
        {
            if (UpdateEngine != null)
            {
                UpdateEngine(this, new EventArgsUpdateEngine() { TriggerEventList = _triggerEvents, SignalHandlersInUse = _signalHandlersInUse });
            }
            MessageBox.Show("Changes has been saved");
        }

        private void Click_RemoveTriggerButton(object sender, RoutedEventArgs e)
        {
            var messageboxResult = MessageBox.Show("Are you sure?", "Delete Configmation", MessageBoxButton.YesNo);

            if (messageboxResult == MessageBoxResult.Yes)
            {
                var button = (Button)sender;

                if (button.Tag == null)
                {
                    return;
                }

                if (button.Tag.GetType() == typeof(TriggerStateMachine))
                {
                    var trigger = (TriggerStateMachine)button.Tag;

                    var gridToBeRemoved = _triggerGridPosition[trigger];

                    //Remove from the trigger event list
                    _stackPanel.Children.Remove(gridToBeRemoved);

                    _triggerEvents.Remove(trigger);
                    _triggerGridPosition.Remove(trigger);
                    _triggerLog.Remove(trigger);
                }
            }
        }

        private void Click_ViewEntryTriggerGrid(object sender, RoutedEventArgs e)
        {

            var grid = (Grid)sender;

            if (grid.Tag == null)
            {
                return;
            }

            if (grid.Tag.GetType() == typeof(TriggerStateMachine))
            {
                var trigger = (TriggerStateMachine)grid.Tag;

                _textBlock.Text = trigger.ToString();

                for (int i = _triggerLog[trigger].Count - 1; i >= 0; i--)
                {
                    _textBlock.Text += _triggerLog[trigger][i] + "\n";
                }
            }
        }

        private void Click_SetupCustomTrigger(object sender, RoutedEventArgs e)
        {
            var addCustomTriggerGUI = new WPFCustomTrigger(_avalibleSignals, _customTriggers);
            addCustomTriggerGUI.NotifyGUI += AddCustomTrigger;


            System.Windows.Forms.Integration.ElementHost.EnableModelessKeyboardInterop(addCustomTriggerGUI);

            addCustomTriggerGUI.Show();
        }

        // Er dette lov? spør Andreas
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }
    }

    public class EventArgsTriggerAdded : EventArgs
    {
        //Used to send new triggers from "WPFAddNewTrigger" to "WPFTriggerOverview"

        public TriggerStateMachine Trigger { get; set; }

        public List<SignalHandler> SignalHandlersInUse { get; set; }
    }

    public class EventArgsCustomTriggerAdded : EventArgs
    {
        //Used to send new custom Triggers/Statements from "WPFAddNEwCustomTrigger" to "WPFTriggerOverview"

        public Custom Trigger { get; set; }
    }

    public class EventArgsUpdateEngine : EventArgs
    {
        //Used to update the triggerEngine class with new Triggers and signalhandlers

        public List<TriggerStateMachine> TriggerEventList { get; set; }
        public List<SignalHandler> SignalHandlersInUse { get; set; }
    }

}
