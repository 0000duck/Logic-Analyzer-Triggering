using ABB.Robotics.Paint.RobView.Database.SignalLog;
using LogialAnalyzerTrigger.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Forms;

namespace LogialAnalyzerTrigger
{
    public class TriggerEngineVm : BindableBase
    {
        private TriggerEngine _triggerEngine;
        private string _triggerSumary;
        private IEnumerable<Signal> _avalibleSignals;

        public ICollectionView ObservableTriggers { get; private set; }

        private readonly DelegateCommand<string> _addNewTriggerCommand;
        private readonly DelegateCommand<string> _removeTriggerCommand;


        public TriggerEngineVm(TriggerEngine triggerEngine, IEnumerable<Signal> avalibleSignals)
        {
            _triggerEngine = triggerEngine;
            _avalibleSignals = avalibleSignals;

            ObservableTriggers = new ListCollectionView(_triggerEngine.TriggerList);
            ObservableTriggers.CurrentChanged += SelectedItemChanged;

            _addNewTriggerCommand = new DelegateCommand<string>(
            (s) => { AddNewTrigger(); }, //Execute
            (s) => { return true; } //CanExecute
            );

            _removeTriggerCommand = new DelegateCommand<string>(
            (s) => { RemoveTrigger(); }, //Execute
            (s) => { return true; } //CanExecute
            );
        }

        public DelegateCommand<string> AddNewTriggerCommand
        {
            get { return _addNewTriggerCommand; }
        }

        public DelegateCommand<string> RemoveTriggerCommand
        {
            get { return _removeTriggerCommand; }
        }


        public string TriggerInfoString
        {
            get { return _triggerSumary; }
            set { SetProperty(ref _triggerSumary, value); }
        }

        private void SelectedItemChanged(object sender, EventArgs e)
        {
            Trigger currentTrigger = ObservableTriggers.CurrentItem as Trigger;

            if (currentTrigger != null)
            {
                TriggerInfoString = currentTrigger.ToString();

            }
                
        }

        private void AddNewTrigger()
        {
            Trigger trigger = new Trigger();
            var triggerVm = new TriggerVm(trigger, _avalibleSignals);
            var triggerWindow = new Views.TriggerWindow();
            triggerWindow.DataContext = triggerVm;

            triggerWindow.ShowDialog();

            if (triggerVm.IS_SAVED)
            {
                _triggerEngine.AddTrigger(trigger);
                ObservableTriggers.Refresh();
            }
        }

        private void RemoveTrigger()
        {
            var trigger = (Trigger)ObservableTriggers.CurrentItem;
            if (trigger == null)
            {
                DialogResult msgBox = MessageBox.Show("No trigger is selected");
                return;
            }

            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete " + trigger.Name + " ?", "Confirm dialog", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                _triggerEngine.RemoveTrigger(trigger);
                ObservableTriggers.Refresh();
            }
            else if (dialogResult == DialogResult.No)
            {

            }
        }
    }
}
