using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogialAnalyzerTrigger.ViewModels
{
    class ThenOperationsVM : BindableBase
    {
        private List<TriggerStateThenOperations> _thenOperationsList;

        public ThenOperationsVM(List<TriggerStateThenOperations> thenOperations)
        {
            _thenOperationsList = thenOperations;
            _thenOperationsList.Add()
            ObservableThenOperations
        }

        public ICollectionView ObservableThenOperations { get; private set; }
    }
}
