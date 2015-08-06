
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogialAnalyzerTrigger.ViewModels
{
    class TriggerForkedStatementData : BindableBase
    {
        private Statement.Statement _statement;
        private List<TriggerStateThenOperations> _thenOperations;

        public TriggerForkedStatementData()
        {
                
        }
    }
}
