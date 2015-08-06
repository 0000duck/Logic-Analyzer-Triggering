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
        private readonly DelegateCommand<string> _saveCommand;

        public ThenOperationsVM(List<TriggerStateThenOperations> thenOperations)
        {
            _thenOperationsList = thenOperations;
            ObservableThenOperations = new ListCollectionView(_thenOperationsList);
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
