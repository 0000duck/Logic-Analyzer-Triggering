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
        private List<ThenOperationModel> _thenOperationsList;

        private readonly DelegateCommand<string> _saveCommand;
        private readonly DelegateCommand<string> _addCommand;
        private readonly DelegateCommand<string> _removeCommand;

        public ThenOperationsVM(List<ThenOperationModel> thenOperations)
        {
            _thenOperationsList = thenOperations;
            ObservableThenOperations = new ListCollectionView(_thenOperationsList);

            _saveCommand = new DelegateCommand<string>(
                    (s) => { SaveThenOperations(); }, //Execute
                    (s) => { return true; } //CanExecute
                    );

            _addCommand = new DelegateCommand<string>(
                    (s) => { AddThenOperation(); }, //Execute
                    (s) => { return true; } //CanExecute
                    );
        }

        public ICollectionView ObservableThenOperations { get; private set; }

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
            get { return _addCommand; }
        }

        private void SaveThenOperations()
        {

        }

        private void AddThenOperation()
        {
            var thenOperationModel = new ThenOperationModel();
            _thenOperationsList.Add(thenOperationModel);
            ObservableThenOperations.Refresh();
        }

    }
}
