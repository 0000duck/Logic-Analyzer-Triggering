
using ABB.Robotics.Paint.RobView.Database.SignalLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LogialAnalyzerTrigger.ViewModels
{
    public class TriggerStateDataModel : BindableBase
    {

        private int _stateNumber;
        private List<TriggerForkedStateDataModel> _triggerForkedStateDataModels;
        private IEnumerable<Signal> _avalibleSignals;

        private readonly DelegateCommand<string> _addForkedStatementCommand;
        private readonly DelegateCommand<string> _removeForkedStatementCommand;

        public TriggerStateDataModel(IEnumerable<Signal> avalibleSignals, int stateNumber)
        {
            _stateNumber = stateNumber;
            _avalibleSignals = avalibleSignals;
            _triggerForkedStateDataModels = new List<TriggerForkedStateDataModel>();
            
            //Will always start with one initially
            _triggerForkedStateDataModels.Add(new TriggerForkedStateDataModel(_avalibleSignals));
            ObservableForkedTriggerStates = new ListCollectionView(_triggerForkedStateDataModels);

            _addForkedStatementCommand = new DelegateCommand<string>(
               (s) => { AddForkedStatement(); }, //Execute
               (s) => { return true; } //CanExecute
               );

            _removeForkedStatementCommand = new DelegateCommand<string>(
               (s) => { RemoveForkedStatement(); }, //Execute
               (s) => { return _triggerForkedStateDataModels.Count > 1; } //CanExecute
               );
        }


        public ICollectionView ObservableForkedTriggerStates { get; private set; }
       
        public DelegateCommand<string> AddForkedStatementCommand
        {
            get { return _addForkedStatementCommand; }
        }

        public DelegateCommand<string> RemoveForkedStatementCommand
        {
            get { return _removeForkedStatementCommand; }
        }

        public string Name
        {
            get { return "State " + _stateNumber + ". "; }
        }

        public void AddForkedStatement()
        {
            _triggerForkedStateDataModels.Add(new TriggerForkedStateDataModel(_avalibleSignals));
            ObservableForkedTriggerStates.Refresh();
            RemoveForkedStatementCommand.RaiseCanExecuteChanged();
        }

        public void RemoveForkedStatement()
        {
            _triggerForkedStateDataModels.Remove(_triggerForkedStateDataModels.Last());
            ObservableForkedTriggerStates.Refresh();
            RemoveForkedStatementCommand.RaiseCanExecuteChanged();
        }


        public TriggerState GetTriggerState()
        {
            var triggerSate = new TriggerState();
            foreach (TriggerForkedStateDataModel triggerForkedStateDataModel in _triggerForkedStateDataModels)
            {
                triggerSate.AddTriggerStateForkedStatement(triggerForkedStateDataModel.GetTriggerForkedState());
            }
            return triggerSate;
        }
    }
}
