
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

        public TriggerStateDataModel(IEnumerable<Signal> avalibleSignals)
        {
            _avalibleSignals = avalibleSignals;
            _triggerForkedStateDataModels = new List<TriggerForkedStateDataModel>();
            _triggerForkedStateDataModels.Add(new TriggerForkedStateDataModel(_avalibleSignals));
            ObservableForkedTriggerStates = new ListCollectionView(_triggerForkedStateDataModels);

            _addForkedStatementCommand = new DelegateCommand<string>(
               (s) => { AddForkedStatement(); }, //Execute
               (s) => { return true; } //CanExecute
               );
        }


        public ICollectionView ObservableForkedTriggerStates { get; private set; }
       
        public DelegateCommand<string> AddForkedStatementCommand
        {
            get { return _addForkedStatementCommand; }
        }

        public int StateNumber
        {
            get { return _stateNumber; }
            set { SetProperty(ref _stateNumber, value); }
        }

        public void AddForkedStatement()
        {
            _triggerForkedStateDataModels.Add(new TriggerForkedStateDataModel(_avalibleSignals));
            ObservableForkedTriggerStates.Refresh();
        }
    }
}
