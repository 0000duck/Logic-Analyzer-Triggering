using ABB.Robotics.Paint.RobView.Database.SignalLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace LogialAnalyzerTrigger.ViewModels
{
    class TriggerVm : BindableBase
    {
        //Checks if the user has saved the changes
        public bool IS_SAVED { get; private set; }

        private Trigger _trigger;
        private List<TriggerStateData> _triggerStateDatas;
        private string _statementString;
        private List<StatementData> _statementvalueList;
        private IEnumerable<Signal> _avalibleSignals;
        private bool _canSave = false;

        private readonly DelegateCommand<string> _saveCommand;
        private readonly DelegateCommand<string> _addStatementCommand;
        
        public TriggerVm(Trigger trigger, IEnumerable<Signal> avalibleSignals)
        {
            IS_SAVED = false;
            _trigger = trigger;
            _avalibleSignals = avalibleSignals;

            //Testing
            var triggerstatedata = new TriggerStateData();
            var statement = new Statement.Equals(new Signal() { Name = "A1" }, 0);
            triggerstatedata.StateNumber = 1;

            //The trigger will always start with one state initially
            _triggerStateDatas = new List<TriggerStateData>();
            _triggerStateDatas.Add(triggerstatedata);

            ObservableTriggerStates = new ListCollectionView(_triggerStateDatas);
    

            _addStatementCommand = new DelegateCommand<string>(
                      (s) => { EditStatement(); }, //Execute
                      (s) => { return true; } //CanExecute
                      );

            _saveCommand = new DelegateCommand<string>(
                     (s) => { SaveTrigger(); }, //Execute
                     (s) => { return _canSave; } //CanExecute
                     );
        }

        public ICollectionView ObservableTriggerStates { get; private set; }

        public DelegateCommand<string> SaveCommand
        {
            get { return _saveCommand; }
        }

        public string StatementString
        {
            get { return _statementString; }
            set { SetProperty(ref _statementString, value); }
        }

        private void EditStatement()
        {
            _statementvalueList = new List<StatementData>();

            var _avalibleSignalsByName = new List<string>();
            foreach (Signal signal in _avalibleSignals)
            {
                _avalibleSignalsByName.Add(signal.Name);
            }

            var statementVm2 = new StatementVm3(_statementvalueList, _avalibleSignalsByName);
            var statementView2 = new Views.StatementWindow3();
            statementView2.DataContext = statementVm2;
            statementView2.ShowDialog();
               
            if (statementVm2.VALID_DATA_FLAG)
            {
                if (_statementvalueList.Count == 1)
                {
                    //_trigger.AddStatement(CreateStatement(_statementvalueList.ElementAt(0)));

                    StatementString = _trigger.ToString();
                
                }else
                {
                    foreach (StatementData SV in _statementvalueList)
                    {
                        var compoundstatement = new Statement.StatementCollection();
                        
                    }
                }

                _canSave = true;
                SaveCommand.RaiseCanExecuteChanged();
            }
            else
            {
                StatementString = "";
                _canSave = false;
                SaveCommand.RaiseCanExecuteChanged();

            }
        }

        private Statement.Statement CreateStatement(StatementData statementValues)
        {
            Statement.Statement statement;

            var signal = _avalibleSignals.First(x => x.Name.Equals(statementValues.Signal));
            
            if(_avalibleSignals.Any(x => x.Name.Equals(statementValues.SignalOrValue)))
            {
                var signal2 = _avalibleSignals.First(x => x.Name.Equals(statementValues.SignalOrValue));

                switch (statementValues.ExpressionValue)
                {
                    case "Equals":
                        statement = new Statement.Equals(signal, signal2);
                        break;
                    case "Lagrer Then":
                        statement = new Statement.LargerThen(signal, signal2);
                        break;
                    case "Less Then":
                        statement = new Statement.LessThen(signal, signal2);
                        break;
                    case "Rising Edge":
                        statement = new Statement.RisingEdge(signal, signal2);
                        break;
                    case "Falling Edge":
                        statement = new Statement.FallingEdge(signal, signal2);
                        break;
                    default:
                        throw new Exception();
                }
            
            }
            else
            {
                double value;
                if (double.TryParse(statementValues.SignalOrValue, out value))
                {
                    switch (statementValues.ExpressionValue)
                    {
                        case "Equals":
                            statement = new Statement.Equals(signal, value);
                            break;
                        case "Larger Then":
                            statement = new Statement.LargerThen(signal, value);
                            break;
                        case "Less Then":
                            statement = new Statement.LessThen(signal, value);
                            break;
                        case "Rising Edge":
                            statement = new Statement.RisingEdge(signal, value);
                            break;
                        case "Falling Edge":
                            statement = new Statement.FallingEdge(signal, value);
                            break;
                        default:
                            throw new Exception();
                    }
                }
                else
                {
                    throw new Exception();
                }
            }
            return statement;
        }

        private void SaveTrigger()
        {
            MessageBoxResult msgBox = MessageBox.Show("Changes Saved");
            IS_SAVED = true;
        }

    }
}
