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

        public ThenOperationsVM(List<ThenOperationsVM> thenOperation)
        {

        }

        public ICollectionView ObservableThenOperations { get; private set; }
    }
}
