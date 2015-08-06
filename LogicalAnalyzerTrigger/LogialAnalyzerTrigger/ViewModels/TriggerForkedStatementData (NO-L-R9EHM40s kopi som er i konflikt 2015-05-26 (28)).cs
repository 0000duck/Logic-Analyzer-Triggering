
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

        private readonly DelegateCommand<string> _addStatementCommand;

        public TriggerForkedStatementData()
        {
            _addStatementCommand = new DelegateCommand<string>(
                  (s) => { AddStatement(); }, //Execute
                  (s) => { return true; } //CanExecute
                  );
        }

        public DelegateCommand<string> AddStatementCommand
        {
            get { return _addStatementCommand; }
        }


        public Statement.Statement Statement
        {
            get { return _statement; }
            set { SetProperty(ref _statement, value); }
        }

        public string StatementString
        {
            get { return _statement.ToString(); }
        }

        public List<TriggerStateThenOperations> ThenOperations
        {
            get { return _thenOperations; }
            set { SetProperty(ref _thenOperations, value); }
        }
    }
}
