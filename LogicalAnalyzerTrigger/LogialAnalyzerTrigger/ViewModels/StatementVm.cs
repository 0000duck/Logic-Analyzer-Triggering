using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LogialAnalyzerTrigger.ViewModels
{
    public enum Algebraoperator
    {
        And,
        Or
    }

    public enum ExpressionOperator
    {
        Equals,
        LessThen,
        LargerThen,
        RisingEdge,
        FallingEdge
    }

    class StatementVm : BindableBase
    {
        public ICollectionView Statements { get; private set; }

        public ObservableCollection<string> ExpressionData { get; private set; }

        public string Expression { get; private set; }

        private readonly DelegateCommand<string> _saveStatements;

        public StatementVm(Statement.Statement statement)
        {
       
        }

        public StatementVm()
        {
            var sv1 = new StatementValues() { Signal = "A1", ExpressionValue = "=",
                SignalOrValue = "2" };

            var list = new List<StatementValues>();
            list.Add(sv1);
            Statements = new ListCollectionView(list);

            _saveStatements = new DelegateCommand<string>(
                    (s) => { ConvertToStatemetns(); }, //Execute
                    (s) => { return true; } //CanExecute
                    );

            ExpressionData = new ObservableCollection<string>();
            ExpressionData.Add("=");
            ExpressionData.Add(">");
            
        }

        public DelegateCommand<string> SaveStatements
        {
            get { return _saveStatements; }
        }

        private void ConvertToStatemetns()
        {
            Console.WriteLine("Hei");
            foreach (StatementValues statementValue in Statements)
            {
                Console.WriteLine(statementValue.Signal + statementValue.ExpressionValue);
                
            }
        }

        private Statement.Statement CreateStatement(StatementValues statementValues)
        {
            return null;
        }
    }
}
