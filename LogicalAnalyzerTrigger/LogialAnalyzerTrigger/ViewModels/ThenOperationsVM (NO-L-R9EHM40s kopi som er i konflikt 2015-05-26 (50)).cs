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
            ObservableThenOperations = new Listcollec
        }

        public ICollectionView ObservableThenOperations { get; private set; }
    }
}
