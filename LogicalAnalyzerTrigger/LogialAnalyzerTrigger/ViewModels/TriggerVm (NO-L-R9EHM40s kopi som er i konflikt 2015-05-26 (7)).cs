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
        private List<TriggerStateDataModel> _triggerStateDataModels;
        private IEnumerable<Signal> _avalibleSignals;

        private readonly DelegateCommand<string> _saveCommand;
        private readonly DelegateCommand<string> _addCommand;
        private readonly DelegateCommand<string> _removeCommand;

        public TriggerVm(Trigger trigger, IEnumerable<Signal> avalibleSignals)
        {
            IS_SAVED = false;
            _trigger = trigger;
            _avalibleSignals = avalibleSignals;

            //The trigger will always start with one state initially
            _triggerStateDataModels = new List<TriggerStateDataModel>();
            _triggerStateDataModels.Add(new TriggerStateDataModel(_avalibleSignals));

            ObservableTriggerStates = new ListCollectionView(_triggerStateDataModels);

            _saveCommand = new DelegateCommand<string>(
                     (s) => { SaveTrigger(); }, //Execute
                     (s) => { return true; } //CanExecute
                     );

            _addCommand = new DelegateCommand<string>(
                    (s) => { AddState(); }, //Execute
                    (s) => { return true; } //CanExecute
                    );

            _removeCommand = new DelegateCommand<string>(
                    (s) => { RemoveState(); }, //Execute
                    (s) => { return true; } //CanExecute
                    );
        }

        public ICollectionView ObservableTriggerStates { get; private set; }

        public DelegateCommand<string> SaveCommand
        {
            get { return _saveCommand; }
        }
        public DelegateCommand<string> AddCommand
        {
            get { return _addCommand; }
        }
        public DelegateCommand<string> RemoveCommand
        {
            get { return _removeCommand; }
        }

        private void SaveTrigger()
        {
            MessageBoxResult msgBox = MessageBox.Show("Changes Saved");
            CreateTrigger();

            IS_SAVED = true;
        }

        private void AddState()
        {
            _triggerStateDataModels.Add(new TriggerStateDataModel(_avalibleSignals));
            ObservableTriggerStates.Refresh();
        }

        private void RemoveState()
        {
            _triggerStateDataModels.Remove(_triggerStateDataModels.Last());
            ObservableTriggerStates.Refresh();
        }

        private void CreateTrigger()
        {
            foreach (TriggerStateDataModel triggerSateDataModel in _triggerStateDataModels)
            {
                var triggerState = new TriggerState();
             
                _trigger.AddTriggerState(triggerState);
            }
        }

    }
}
