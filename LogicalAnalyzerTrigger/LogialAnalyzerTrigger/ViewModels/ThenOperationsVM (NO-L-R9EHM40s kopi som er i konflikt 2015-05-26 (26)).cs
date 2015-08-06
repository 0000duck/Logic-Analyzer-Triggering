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

        public ThenOperationsVM(List<TriggerStateThenOperations> thenOperations)
        {
            _thenOperationsList = thenOperations;
            ObservableThenOperations = new ListCollectionView(_thenOperationsList);

            _saveCommand = new DelegateCommand<string>(
                    (s) => { SaveTrigger(); }, //Execute
                    (s) => { return _canSave; } //CanExecute
                    );

            _addThenOpeationCommand = new DelegateCommand<string>(
                    (s) => { SaveTrigger(); }, //Execute
                    (s) => { return _canSave; } //CanExecute
                    );
        }

        public ICollectionView ObservableThenOperations { get; private set; }

        public DelegateCommand<string> SaveCommand
        {
            get { return _saveCommand; }
        }

        public DelegateCommand<string> SaveCommand
        {
            get { return _saveCommand; }
        }


    }
}
