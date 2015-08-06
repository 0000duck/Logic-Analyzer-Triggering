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
        private List<Th>

        public ThenOperationsVM(List<ThenOperationsVM> thenOperations)
        {

        }

        public ICollectionView ObservableThenOperations { get; private set; }
    }
}
