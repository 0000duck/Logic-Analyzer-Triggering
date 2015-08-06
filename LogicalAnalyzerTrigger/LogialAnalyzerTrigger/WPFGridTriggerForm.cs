using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LogialAnalyzerTrigger
{
    class GridTriggerForm : Grid
    {
        private int _rowNumber;
        private Button _addTriggerRowButton;
        private List<SignalHandler> _signalHandlersAvalible;
        private List<SignalHandler> _signalHandlersInUse;
        private List<ITrigger> _triggerList;
        private List<Custom> _customTriggerList;
        private string _rawString;

        public GridTriggerForm(List<SignalHandler> avalibleSignalHandlers)
        {
            _signalHandlersAvalible = avalibleSignalHandlers;
            _customTriggerList = new List<Custom>();
            CreateGrid();
        }

        public GridTriggerForm(List<SignalHandler> avalibleSignalHandlers, List<Custom> customTriggerList)
        {
            _signalHandlersAvalible = avalibleSignalHandlers;
            _customTriggerList = customTriggerList;
            CreateGrid();
        }

        private void CreateGrid()
        {
            _rawString = "";
           
            _triggerList = new List<ITrigger>();
            _signalHandlersInUse = new List<SignalHandler>();
            _rowNumber = 2;

            RowDefinition rowDef;


            this.ShowGridLines = true;
            this.ColumnDefinitions.Add(new ColumnDefinition());

            for (int i = 0; i < 3; i++)
            {
                rowDef = new RowDefinition();
                rowDef.MaxHeight = 30;
                this.RowDefinitions.Add(rowDef);
            }

            Grid.SetRow(this, 1);
            Grid.SetColumn(this, 0);

            var descriptionLabel = new Label();
            descriptionLabel.Content = "Forklaring til bruker";
            Grid.SetRow(descriptionLabel, 0);
            Grid.SetColumn(descriptionLabel, 0);

            this.Children.Add(descriptionLabel);


            this.Children.Add(CreateLogicalSentence(1, 0, true));


            _addTriggerRowButton = CreateButton("+", 2, 0);
            _addTriggerRowButton.Click += new RoutedEventHandler(Add_Expression);
            this.Children.Add(_addTriggerRowButton);

        }

        public List<SignalHandler> SignalHandlersUsed
        {
            get { return this._signalHandlersInUse; }
        }

        public List<ITrigger> TriggerList
        {
            get { return this._triggerList; }
        }

        public List<SignalHandler> SignalHandles
        {
            get { return this._signalHandlersAvalible; }
        }

        private void Add_Expression(object sender, RoutedEventArgs e)
        {

            // Adds Another Expression Line
            RowDefinition gridRow1 = new RowDefinition();
            gridRow1.MaxHeight = 30;
            this.RowDefinitions.Add(gridRow1);
            this.Children.Add(CreateLogicalSentence(_rowNumber, 0, false));
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

            foreach (Grid grid in this.Children.OfType<Grid>())
            {
                triggerValues = new List<string>();
                foreach (var ctrl in grid.Children)
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
            AlgerbraOperator selectedOperator;

            switch (triggerValues[0])
            {
                case "And":
                    selectedOperator = AlgerbraOperator.And;
                    break;
                case "Or":
                    selectedOperator = AlgerbraOperator.Or;                    
                    break;
                default:
                    selectedOperator = AlgerbraOperator.First;
                    break;
            }

            var customTrigger = _customTriggerList.Find(TriggerCuston => TriggerCuston.Name == triggerValues[1]);
            if (customTrigger != null)
            {
                customTrigger.LogicalOperator = selectedOperator;
                _triggerList.Add(customTrigger);
                _rawString += customTrigger.ToString() + " ";

                foreach(SignalHandler sh in customTrigger.SignalHandlersUsedByTrigger)
                {
                    if (!_signalHandlersInUse.Contains(sh))
                    {
                        _signalHandlersInUse.Add(sh);
                    }
                }
                return true;
            }


            var signalHandler = _signalHandlersAvalible.Find(SignalHandler => SignalHandler.Signal.Name == triggerValues[1]);
            if (signalHandler == null)
            {
                MessageBox.Show("Invalid input, " + triggerValues[1] + " is not a valid signal or a custom trigger");
                return false;
            }

            // Adds The SignalHandler to the Singalhandlerlis if its not already added.
            if (!_signalHandlersInUse.Contains(signalHandler))
            {
                _signalHandlersInUse.Add(signalHandler);
            }

            // Checks if the user has entered a Valid signal or left it empty 
            if (_signalHandlersAvalible.Exists(SignalHandler => signalHandler.Signal.Name == triggerValues[3]))
            {
                // Looks up the entered signal, And makes a SignalHandler
                var signalHandler2 = _signalHandlersAvalible.Find(SignalHandler => SignalHandler.Signal.Name == triggerValues[3]);

                if (!_signalHandlersInUse.Contains(signalHandler2))
                {
                    _signalHandlersInUse.Add(signalHandler2);
                }

                // Create The Trigger with only a Signal as input
                switch (triggerValues[2])
                {
                    case "=":
                        trigger = new Equals(signalHandler, signalHandler2, selectedOperator);
                        break;
                    case "<":
                        trigger = new LessThen(signalHandler, signalHandler2, selectedOperator);
                        break;
                    case ">":
                        trigger = new GreaterThen(signalHandler, signalHandler2, selectedOperator);
                        break;
                    case "Rising Edge On":
                        trigger = new RisingEdge(signalHandler, signalHandler2, selectedOperator);
                        break;
                    case "Falling Edge On":
                        trigger = new FallingEdge(signalHandler, signalHandler2, selectedOperator);
                        break;
                    default:
                        MessageBox.Show(("Invalid input, " + triggerValues[2] + " is not a valid input"));
                        return false;
                }        
            }
            else
            {
                // Converts The value to double
                double value = 0;
                try
                {
                    value = double.Parse(triggerValues[3]);
                }
                catch
                {
                    MessageBox.Show(("Invalid input, " + triggerValues[3] + " could not find signal or convert to decimal"));
                    return false;
                }

                // Creates the trigger with only a value as input
                switch (triggerValues[2])
                {
                    case "=":
                        trigger = new Equals(signalHandler, value, selectedOperator);
                        break;
                    case "<":
                        trigger = new LessThen(signalHandler, value, selectedOperator);
                        break;
                    case ">":
                        trigger = new GreaterThen(signalHandler, value, selectedOperator);
                        break;
                    case "Rising Edge On":
                        trigger = new RisingEdge(signalHandler, value, selectedOperator);
                        break;
                    case "Falling Edge On":
                        trigger = new FallingEdge(signalHandler, value, selectedOperator);
                        break;
                    default:
                        MessageBox.Show(("Invalid input, " + triggerValues[2] + " is not a valid input"));
                        return false;
                }         
            }
            _triggerList.Add(trigger);
            _rawString += trigger.ToString() + " "; 
            return true;
        }

        private Grid CreateLogicalSentence(int row, int column, bool isFirstRow)
        {
            ComboBoxItem cbi;
            Grid DynamicGrid = new Grid();
            DynamicGrid.MaxHeight = 30;

            var col = new ColumnDefinition();
            col.MaxWidth = 50;
            DynamicGrid.ColumnDefinitions.Add(col);
            for (int i = 1; i < 4; i++)
            {
                DynamicGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            // Create Row
            RowDefinition gridRow1 = new RowDefinition();
            gridRow1.Height = new GridLength(30);
            DynamicGrid.RowDefinitions.Add(gridRow1);

            // Create Logical Operator cell
            ComboBox logicalOperator = new ComboBox();
            logicalOperator.FontSize = 12;
            logicalOperator.SelectedIndex = 0;
            logicalOperator.FontWeight = FontWeights.Bold;
            Grid.SetRow(logicalOperator, 1);
            Grid.SetColumn(logicalOperator, 0);
            foreach (string s in logicalOperations())
            {
                cbi = new ComboBoxItem();
                cbi.Content = s;
                logicalOperator.Items.Add(cbi);
            }
            cbi = new ComboBoxItem() { Content = "first", IsEnabled = false, Visibility = Visibility.Collapsed };
            logicalOperator.Items.Add(cbi);

            // Create Signal cell
            ComboBox signal = new ComboBox();
            signal.IsEditable = true;
            signal.Text = "Signal/Predefined Trigger";

            signal.FontSize = 12;
            signal.FontWeight = FontWeights.Bold;
            Grid.SetRow(signal, 1);
            Grid.SetColumn(signal, 1);

            foreach (SignalHandler signalHandler in _signalHandlersAvalible)
            {
                cbi = new ComboBoxItem();
                cbi.Tag = DynamicGrid;
                cbi.Content = signalHandler.Signal.Name;
                cbi.Selected += Selected_SignalComboxItem;
                signal.Items.Add(cbi);
            }

            foreach (Custom triggerCustom in _customTriggerList)
            {
                cbi = new ComboBoxItem();
                cbi.Tag = DynamicGrid;
                cbi.Content = triggerCustom.Name;
                cbi.Selected += Selected_CustomTrigger;
                signal.Items.Add(cbi);
            }


            // Create Expression cell
            ComboBox expression = new ComboBox();
            expression.IsEditable = true;
            expression.Text = "=";
            expression.FontSize = 12;
            expression.FontWeight = FontWeights.Bold;
            Grid.SetRow(expression, 1);
            Grid.SetColumn(expression, 2);
            foreach (string s in getOperations())
            {
                cbi = new ComboBoxItem();
                cbi.Content = s;
                expression.Items.Add(cbi);
            }

            // Create Signal Value cell
            ComboBox signalValue = new ComboBox();
            signalValue.IsEditable = true;
            signalValue.Text = "Value";
            signalValue.FontSize = 12;
            signalValue.FontWeight = FontWeights.Bold;
            Grid.SetRow(signalValue, 1);
            Grid.SetColumn(signalValue, 3);
            foreach (SignalHandler signalHandler in _signalHandlersAvalible)
            {
                cbi = new ComboBoxItem();
                cbi.Content = signalHandler.Signal.Name;
                signalValue.Items.Add(cbi);
            }

            foreach (Custom triggerCustom in _customTriggerList)
            {
                cbi = new ComboBoxItem();
                cbi.Content = triggerCustom.Name;
                signalValue.Items.Add(cbi);
            }

          // Add first row to Grid
            DynamicGrid.Children.Add(logicalOperator);
            DynamicGrid.Children.Add(signal);
            DynamicGrid.Children.Add(expression);
            DynamicGrid.Children.Add(signalValue);


            Grid.SetRow(DynamicGrid, row);
            Grid.SetColumn(DynamicGrid, column);

            if (isFirstRow)
            {
                logicalOperator.Visibility = Visibility.Hidden;
                logicalOperator.SelectedIndex = 2;
            }

            // Display grid into a Window
            return DynamicGrid;
        }

        private void Selected_CustomTrigger(object sender, RoutedEventArgs e)
        {
            try
            {
                var comboxitem = (ComboBoxItem)sender;
                if (comboxitem.Tag == null)
                    return;
                var grid = (Grid)comboxitem.Tag;
                int i = 0;
                foreach (object o in grid.Children)
                {
                    if (i < 2)
                    {
                        i++;
                        continue;
                    }
                       
                    if (o is ComboBox)
                    {
                        var cb = (ComboBox)o;                   
                        cb.Visibility = System.Windows.Visibility.Collapsed;
                    }
                    
                }
            }
            catch
            {

            }
        
        }

        private void Selected_SignalComboxItem(object sender, RoutedEventArgs e)
        {
            try
            {
                var comboxitem = (ComboBoxItem)sender;
                if (comboxitem.Tag == null)
                    return;
                var grid = (Grid)comboxitem.Tag;

                int i = 0;
                foreach (object o in grid.Children)
                {
                    if (i < 2)
                    {
                        i++;
                        continue;
                    }
                        
                    if (o is ComboBox)
                    {
                        var cb = (ComboBox)o;
                        cb.Visibility = System.Windows.Visibility.Visible;
                    }
                }
            }
            catch
            {

            }
        }

        private static Button CreateButton(string text, int row, int column)
        {
            Button tb = new Button()
            {
                Content = text,
                VerticalAlignment =
                    VerticalAlignment.Top,
                HorizontalAlignment =
                    HorizontalAlignment.Left,
                Margin = new Thickness(5, 8, 0, 5)
            };
            tb.Width = 90;
            tb.MaxHeight = 30;
            tb.Margin = new Thickness(5);
            Grid.SetColumn(tb, column);
            Grid.SetRow(tb, row);

            return tb;
        }

        private static List<string> getOperations()
        {
            List<string> list = new List<string>();
            list.Add("=");
            list.Add("<");
            list.Add(">");
            list.Add("Rising Edge On");
            list.Add("Falling Edge On");
            return list;
        }

        private static List<string> logicalOperations()
        {
            List<string> list = new List<string>();
            list.Add("And");
            list.Add("Or");

            return list;
        }

        public override string ToString()
        {
            return _rawString;
        }    
    }
}
