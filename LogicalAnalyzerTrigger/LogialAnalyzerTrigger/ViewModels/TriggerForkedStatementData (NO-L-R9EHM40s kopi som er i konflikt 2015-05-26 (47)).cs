
using ABB.Robotics.Paint.RobView.Database.SignalLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogialAnalyzerTrigger.ViewModels
{
    class TriggerForkedStatementData : BindableBase
    {
        private Statement.Statement _statement;
        private List<TriggerStateThenOperations> _thenOperations;

        private readonly DelegateCommand<string> _addStatementCommand;

        public TriggerForkedStatementData()
        {
            _statement = new Statement.Equals(new Signal { Name = "A1" }, 0);

            _addStatementCommand = new DelegateCommand<string>(
                  (s) => { AddStatement(); }, //Execute
                  (s) => { return true; } //CanExecute
                  );
        }

        public DelegateCommand<string> AddStatementCommand
        {
            get { return _addStatementCommand; }
        }


        public Statement.Statement Statement
        {
            get { return _statement; }
            set { SetProperty(ref _statement, value); }
        }

        public string StatementString
        {
            get { return _statement.ToString(); }
        }

        public List<TriggerStateThenOperations> ThenOperations
        {
            get { return _thenOperations; }
            set { SetProperty(ref _thenOperations, value); }
        }

        private void AddStatement()
        {
            Console.WriteLine("Hei");
        }

        //private void EditStatement()
        //{
        //    _statementvalueList = new List<StatementData>();

        //    var _avalibleSignalsByName = new List<string>();
        //    foreach (Signal signal in _avalibleSignals)
        //    {
        //        _avalibleSignalsByName.Add(signal.Name);
        //    }

        //    var statementVm2 = new StatementVm3(_statementvalueList, _avalibleSignalsByName);
        //    var statementView2 = new Views.StatementWindow3();
        //    statementView2.DataContext = statementVm2;
        //    statementView2.ShowDialog();

        //    if (statementVm2.VALID_DATA_FLAG)
        //    {
        //        if (_statementvalueList.Count == 1)
        //        {
        //            //_trigger.AddStatement(CreateStatement(_statementvalueList.ElementAt(0)));

        //            StatementString = _trigger.ToString();

        //        }else
        //        {
        //            foreach (StatementData SV in _statementvalueList)
        //            {
        //                var compoundstatement = new Statement.StatementCollection();

        //            }
        //        }

        //        _canSave = true;
        //        SaveCommand.RaiseCanExecuteChanged();
        //    }
        //    else
        //    {
        //        StatementString = "";
        //        _canSave = false;
        //        SaveCommand.RaiseCanExecuteChanged();

        //    }
        //}

        //private Statement.Statement CreateStatement(StatementData statementValues)
        //{
        //    Statement.Statement statement;

        //    var signal = _avalibleSignals.First(x => x.Name.Equals(statementValues.Signal));

        //    if (_avalibleSignals.Any(x => x.Name.Equals(statementValues.SignalOrValue)))
        //    {
        //        var signal2 = _avalibleSignals.First(x => x.Name.Equals(statementValues.SignalOrValue));

        //        switch (statementValues.ExpressionValue)
        //        {
        //            case "Equals":
        //                statement = new Statement.Equals(signal, signal2);
        //                break;
        //            case "Lagrer Then":
        //                statement = new Statement.LargerThen(signal, signal2);
        //                break;
        //            case "Less Then":
        //                statement = new Statement.LessThen(signal, signal2);
        //                break;
        //            case "Rising Edge":
        //                statement = new Statement.RisingEdge(signal, signal2);
        //                break;
        //            case "Falling Edge":
        //                statement = new Statement.FallingEdge(signal, signal2);
        //                break;
        //            default:
        //                throw new Exception();
        //        }

        //    }
        //    else
        //    {
        //        double value;
        //        if (double.TryParse(statementValues.SignalOrValue, out value))
        //        {
        //            switch (statementValues.ExpressionValue)
        //            {
        //                case "Equals":
        //                    statement = new Statement.Equals(signal, value);
        //                    break;
        //                case "Larger Then":
        //                    statement = new Statement.LargerThen(signal, value);
        //                    break;
        //                case "Less Then":
        //                    statement = new Statement.LessThen(signal, value);
        //                    break;
        //                case "Rising Edge":
        //                    statement = new Statement.RisingEdge(signal, value);
        //                    break;
        //                case "Falling Edge":
        //                    statement = new Statement.FallingEdge(signal, value);
        //                    break;
        //                default:
        //                    throw new Exception();
        //            }
        //        }
        //        else
        //        {
        //            throw new Exception();
        //        }
        //    }
        //    return statement;
        //}
    }
}
