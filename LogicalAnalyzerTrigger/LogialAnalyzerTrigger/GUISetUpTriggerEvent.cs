using ABB.Robotics.Paint.RobView.Database.SignalLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LogialAnalyzerTrigger
{
    public class GUISetUpTriggerEvent
    {
        private Grid _inputPanel;
        private int _rowNumber;
        private Button _addTriggerRowButton;
        private List<SignalHandler> _signalHandlerList;
        private List<Signal> _avalibleSignals;
        private List<ITrigger> _triggerList;
        private string _rawString;
        private TextBox _eventNameBox;

        public GUISetUpTriggerEvent(List<Signal> avalibleSignals)
        {
            _rawString = "";
            _signalHandlerList = new List<SignalHandler>();
            _triggerList = new List<ITrigger>();
            _rowNumber = 2;
            _inputPanel = new Grid();
            this._avalibleSignals = avalibleSignals;
            RowDefinition rowDef;


            _inputPanel.ShowGridLines = true;
            _inputPanel.ColumnDefinitions.Add(new ColumnDefinition());

            for (int i = 0; i < 3; i++)
            {
                rowDef = new RowDefinition();
                rowDef.MaxHeight = 30;
                _inputPanel.RowDefinitions.Add(rowDef);
            }

            Grid.SetRow(_inputPanel, 1);
            Grid.SetColumn(_inputPanel, 0);

            _eventNameBox = new TextBox();
            _eventNameBox.Text = "Enter Event Name";
            Grid.SetRow(_eventNameBox, 0);
            Grid.SetColumn(_eventNameBox, 0);

            _inputPanel.Children.Add(_eventNameBox);


            _inputPanel.Children.Add(GUICreateWPFControls.CreateLogicalSentence(1, 0, true, avalibleSignals));


            _addTriggerRowButton = GUICreateWPFControls.CreateButton("+", 2, 0);
            _addTriggerRowButton.Click += new RoutedEventHandler(Add_Expression);
            _inputPanel.Children.Add(_addTriggerRowButton);

        }

        public Grid InputGrid
        {
            get { return this._inputPanel; }
        }

        public List<ITrigger> TriggerList
        {
            get { return this._triggerList; }
        }

        public List<SignalHandler> SignalHandles
        {
            get { return this._signalHandlerList; }
        }

        public string EventName
        {
            get { return this._eventNameBox.Text; }
        }

        private void Add_Expression(object sender, RoutedEventArgs e)
        {

            // Adds Another Expression Line
            RowDefinition gridRow1 = new RowDefinition();
            gridRow1.MaxHeight = 30;
            _inputPanel.RowDefinitions.Add(gridRow1);
            _inputPanel.Children.Add(GUICreateWPFControls.CreateLogicalSentence(_rowNumber, 0, false, _avalibleSignals));
            _rowNumber++;

            // Moves The Button to the last line
            Grid.SetRow(_addTriggerRowButton, _rowNumber);

        }

        public bool Assemble_Triggers()
        {
            // Returns True if succsefull or False if failed;
            ComboBox cb;
            List<string> triggerValues;
            bool validInput = true;

            foreach (Grid grid in _inputPanel.Children.OfType<Grid>())
            {
                triggerValues = new List<string>();
                foreach (Control ctrl in grid.Children)
                {
                    if (ctrl is ComboBox)
                    {
                        cb = ctrl as ComboBox;
                        triggerValues.Add(cb.Text);
                    }
                }
                validInput = Create_Trigger(triggerValues);

            }

            return validInput;
        }

        private bool Create_Trigger(List<string> triggerValues)
        {
            ITrigger trigger;
            SignalHandler signalHandler;
            AlgerbraOperator aO;

            switch (triggerValues[0])
            {
                case "&&":
                    aO = AlgerbraOperator.And;
                    _rawString += "&& ";
                    break;
                case "||":
                    aO = AlgerbraOperator.Or;
                    _rawString += "|| ";
                    break;
                default:
                    aO = AlgerbraOperator.First;
                    break;
            }


            // Looks up the entered signal, And makes a signalHandler
            Signal signal = _avalibleSignals.Find(Signal => Signal.Name == triggerValues[1]);
            signalHandler = new SignalHandler(signal);


            // Adds The SignalHandler to the Singalhandlerlis if its not already added.
            if (!_signalHandlerList.Contains(signalHandler))
            {
                _signalHandlerList.Add(signalHandler);
            }

            // Checks if the user has entered a Valid signal or left it empty 
            if (_avalibleSignals.Exists(Signal => Signal.Name == triggerValues[3]))
            {
                // Looks up the entered signal, And makes a SignalHandler
                var signal2 = _avalibleSignals.Find(Signal => Signal.Name == triggerValues[3]);
                var signalHandler2 = new SignalHandler(signal2);

                // Adds The SignalHandler to the engine if its not already added
                if (!_signalHandlerList.Contains(signalHandler2))
                {
                    _signalHandlerList.Add(signalHandler2);
                }

                // Create The Trigger with only a Signal as input
                switch (triggerValues[2])
                {
                    case "=":
                        trigger = new TriggerEquals(signalHandler, signalHandler2, aO);
                        break;
                    case "<":
                        trigger = new TriggerLessThen(signalHandler, signalHandler2, aO);
                        break;
                    case ">":
                        trigger = new TriggerGreaterThen(signalHandler, signalHandler2, aO);
                        break;
                    case "Rising Edge On":
                        trigger = new TriggerRisingEdge(signalHandler, signalHandler2, aO);
                        break;
                    case "Falling Edge On":
                        trigger = new TriggerFallingEdge(signalHandler, signalHandler2, aO);
                        break;
                    default:
                        throw new Exception();
                }
                _rawString += triggerValues[1] + " is " + triggerValues[2] + " " + triggerValues[3] + "\n";

            }
            else
            {
                // Converts The value to double
                double value = 0;
                try
                {
                    value = double.Parse(triggerValues[3]);
                }
                catch (Exception e)
                {
                    return false;
                }

                // Creates the trigger with only a value as input
                switch (triggerValues[2])
                {
                    case "=":
                        trigger = new TriggerEquals(signalHandler, value, aO);
                        break;
                    case "<":
                        trigger = new TriggerLessThen(signalHandler, value, aO);
                        break;
                    case ">":
                        trigger = new TriggerGreaterThen(signalHandler, value, aO);
                        break;
                    case "Rising Edge On":
                        trigger = new TriggerRisingEdge(signalHandler, value, aO);
                        break;
                    case "Falling Edge On":
                        trigger = new TriggerFallingEdge(signalHandler, value, aO);
                        break;
                    default:
                        throw new Exception();
                }
                _rawString += triggerValues[1] + " is " + triggerValues[2] + " " + triggerValues[3] + "\n";
            }
            _triggerList.Add(trigger);
            return true;
        }

        public override string ToString()
        {
            return _rawString;
        }
    }
}
