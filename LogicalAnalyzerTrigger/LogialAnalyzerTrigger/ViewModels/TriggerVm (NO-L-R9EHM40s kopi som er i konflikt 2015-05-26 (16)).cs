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
        private IEnumerable<Signal> _avalibleSignals;
        private bool _canSave = false;

        private readonly DelegateCommand<string> _saveCommand;
        
        public TriggerVm(Trigger trigger, IEnumerable<Signal> avalibleSignals)
        {
            IS_SAVED = false;
            _trigger = trigger;
            _avalibleSignals = avalibleSignals;

            //Testing
            var triggerstatedata = new TriggerStateData(_avalibleSignals);
            var statement = new Statement.Equals(new Signal() { Name = "A1" }, 0);
            triggerstatedata.StateNumber = 1;

            //The trigger will always start with one state initially
            _triggerStateDatas = new List<TriggerStateData>();
            _triggerStateDatas.Add(triggerstatedata);

            ObservableTriggerStates = new ListCollectionView(_triggerStateDatas);

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


        private void SaveTrigger()
        {
            MessageBoxResult msgBox = MessageBox.Show("Changes Saved");
            IS_SAVED = true;
        }

        private void AddState()
        {
            _triggerStateDatas.Add(new TriggerStateData(_avalibleSignals));
            ObservableTriggerStates.Refresh();
        }

        private void RemoveState()
        {

        }

    }
}
