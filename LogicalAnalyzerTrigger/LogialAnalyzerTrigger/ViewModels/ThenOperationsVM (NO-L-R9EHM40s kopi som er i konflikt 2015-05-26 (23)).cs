using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LogialAnalyzerTrigger.ViewModels
{
    class ThenOperationsVM : BindableBase
    {
        private List<TriggerStateThenOperations> _thenOperationsList;

        private readonly DelegateCommand<string> _saveCommand;
        private readonly DelegateCommand<string> _addThenOpeationCommand;

        public ThenOperationsVM(List<ThenOperationModel> thenOperations)
        {
            _thenOperationsList = thenOperations;
            _thenOperationsList.Add(TriggerStateThenOperations.Trigger);
            ObservableThenOperations = new ListCollectionView(_thenOperationsList);

            

            _saveCommand = new DelegateCommand<string>(
                    (s) => { SaveThenOperations(); }, //Execute
                    (s) => { return true; } //CanExecute
                    );

            _addThenOpeationCommand = new DelegateCommand<string>(
                    (s) => { AddThenOperation(); }, //Execute
                    (s) => { return true; } //CanExecute
                    );
        }

        public ICollectionView ObservableThenOperations { get; private set; }

        public DelegateCommand<string> SaveCommand
        {
            get { return _saveCommand; }
        }

        public DelegateCommand<string> AddThenOperationCommand
        {
            get { return _addThenOpeationCommand; }
        }

        private void SaveThenOperations()
        {

        }

        private void AddThenOperation()
        {

        }

    }
}
