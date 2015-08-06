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
    class ThenOperationsVM : BindableBase
    {
        public bool IS_SAVED { get; private set; }
        
        private List<ThenOperationDataModel> _thenOperationsList;

        private readonly DelegateCommand<string> _saveCommand;
        private readonly DelegateCommand<string> _addCommand;
        private readonly DelegateCommand<string> _removeCommand;

        public ThenOperationsVM(List<ThenOperationDataModel> thenOperations)
        {
            IS_SAVED = false;
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

            _removeCommand = new DelegateCommand<string>(
              (s) => { RemoveThenOperation(); }, //Execute
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
            get { return _removeCommand; }
        }

        private void SaveThenOperations()
        {
            IS_SAVED = true;
        }

        private void AddThenOperation()
        {
            var thenOperationModel = new ThenOperationDataModel();
            _thenOperationsList.Add(thenOperationModel);
            ObservableThenOperations.Refresh();
        }

        private void RemoveThenOperation()
        {
            var operationToBeRemoved = (ThenOperationDataModel)ObservableThenOperations.CurrentItem;
            if (operationToBeRemoved == null)
            {
                MessageBoxResult msgBox = MessageBox.Show("No operation is selected");
            }

            _thenOperationsList.Remove(operationToBeRemoved);
            ObservableThenOperations.Refresh();
        }
    }
}
