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
    class StatementVm : BindableBase
    {
        // Flag that can be read by parent class, to see if the statement was saved correctly
        public bool IS_SAVED { get; private set; }

        private List<StatementDataModel> _statementValuesList;

        private IEnumerable<Signal> _avalibleSignals;

        private readonly DelegateCommand<string> _addRegularExpressionCommand;

        private readonly DelegateCommand<string> _addTimeExpressionCommand;

        private readonly DelegateCommand<string> _removeExpressionCommand;

        private readonly DelegateCommand<string> _saveStatementCommand;

        public StatementVm(List<StatementDataModel> statementList, IEnumerable<Signal> avalibleSignals)
        {
            IS_SAVED = false;
            _avalibleSignals = avalibleSignals;
            _statementValuesList = statementList;

            //var statementvalue = new StatementDataModel(_avalibleSignals, StatementDataModelType.RegularStatement);
            //statementvalue.IsAlgebraOperatorVisible = false;
            //statementvalue.PropertyChanged += StatementValueField_Changed;
            //_statementValuesList.Add(statementvalue);

            ObservableStatements = new ListCollectionView(_statementValuesList);
            ObservableStatements.CollectionChanged += ObservableStatements_CollectionChanged;

            _addRegularExpressionCommand = new DelegateCommand<string>(
                (s) => { AddRegularExpression(); }, //Execute
                (s) => { return true; } //CanExecute
                );

            _addTimeExpressionCommand = new DelegateCommand<string>(
                (s) => { AddTimeExpression(); }, //Execute
                (s) => { return true; } //CanExecute
                );

            _removeExpressionCommand = new DelegateCommand<string>(
                (s) => { RemoveRow(); }, //Execute
                (s) => { return true; } //CanExecute
                );

            _saveStatementCommand = new DelegateCommand<string>(
                (s) => { Save(); }, //Execute
                (s) => { return IsDataValid(); } //CanExecute
                );

        }

        public DelegateCommand<string> AddRegularExpressionCommand
        {
            get { return _addRegularExpressionCommand; }
        }

        public DelegateCommand<string> AddTimeExpressionCommand
        {
            get { return _addTimeExpressionCommand; }
        }

        public bool IsEnabled
        {
            get { return false; }
        }

        public DelegateCommand<string> RemoveExpressionCommand
        {
            get { return _removeExpressionCommand; }
        }

        public DelegateCommand<string> SaveStatementCommand
        {
            get { return _saveStatementCommand; }
        }

        public ICollectionView ObservableStatements { get; private set; }

        private bool IsDataValid()
        {
            bool isDataValid = true;

            foreach (StatementDataModel statementValues in _statementValuesList)
            {
                //Checks if the Signal or value field contains either a valid double or signal
                double testDouble;
                int testInt;

                switch (statementValues.StatementDataModelType)
                {
                    case StatementDataModelType.RegularStatement:
                        if (!Double.TryParse(statementValues.SignalOrValue, out testDouble) && !statementValues.AvalibleSignals.Any(x => x.Name == statementValues.SignalOrValue))
                            isDataValid = false;

                        //Checks if one of the statementvalues is null or empty
                        if (String.IsNullOrEmpty(statementValues.Signal) ||
                            String.IsNullOrEmpty(statementValues.ExpressionValue) ||
                            String.IsNullOrEmpty(statementValues.SignalOrValue))
                            isDataValid = false;


                        //Checks if the Algebraoperator is null of empty. Does not care about the first one, since it wont be used.
                        //Noen måte å gjemme den på?
                     
                        break;
                    case StatementDataModelType.TimedStatement:
                        if (!Int32.TryParse(statementValues.SignalOrValue, out testInt))
                            isDataValid = false;
                        if (String.IsNullOrEmpty(statementValues.SignalOrValue) || String.IsNullOrEmpty(statementValues.ExpressionValue))
                            isDataValid = false;

                        break;
                    default:
                        break;
                }

                if (String.IsNullOrEmpty(statementValues.AlgebraOperator) && statementValues != _statementValuesList.First())
                    isDataValid = false;

            }
            if (_statementValuesList.Count < 1)
                isDataValid = false;

            return isDataValid;
        }

        private void AddRegularExpression()
        {
            var statementvalue = new StatementDataModel(_avalibleSignals, StatementDataModelType.RegularStatement);
            if (_statementValuesList.Count < 1)
                statementvalue.IsAlgebraOperatorVisible = false;
            statementvalue.PropertyChanged += StatementValueField_Changed;
            _statementValuesList.Add(statementvalue);
            _removeExpressionCommand.RaiseCanExecuteChanged();
            ObservableStatements.Refresh();
        }

        private void AddTimeExpression()
        {
            var statementvalue = new StatementDataModel(_avalibleSignals, StatementDataModelType.TimedStatement);
            if (_statementValuesList.Count < 1)
                statementvalue.IsAlgebraOperatorVisible = false;
            statementvalue.PropertyChanged += StatementValueField_Changed;
            _statementValuesList.Add(statementvalue);
            _removeExpressionCommand.RaiseCanExecuteChanged();
            ObservableStatements.Refresh();
        }

        private void RemoveRow()
        {
            var currentSelectedStatementDataModel = (StatementDataModel)ObservableStatements.CurrentItem;
            if (currentSelectedStatementDataModel == null)
            {
                MessageBoxResult msgBox =  MessageBox.Show("No statement is selected");
                return;
            }
            _statementValuesList.Remove(currentSelectedStatementDataModel);
            ObservableStatements.Refresh();
        }

        private void Save()
        {
            MessageBoxResult msgBox = MessageBox.Show("Statement succsefully created");
            IS_SAVED = true;
        }

        private void ObservableStatements_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SaveStatementCommand.RaiseCanExecuteChanged();
        }

        private void StatementValueField_Changed(object sender, EventArgs e)
        {
            SaveStatementCommand.RaiseCanExecuteChanged();
        }
    }
}
