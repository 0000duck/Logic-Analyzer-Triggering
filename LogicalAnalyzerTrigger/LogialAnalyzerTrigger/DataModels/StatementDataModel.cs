using ABB.Robotics.Paint.RobView.Database.SignalLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogialAnalyzerTrigger.ViewModels
{
    public enum StatementDataModelType
    {
        RegularStatement,
        TimedStatement
    }

    internal class StatementDataModel : BindableBase
    {
        private string _signal, _expressionsValue, _signalOrValue, _algebraoperator;
        private bool _isAlgebraOperatorVisible;
        private StatementDataModelType _statementDataType;
        private List<string> _avlibleExpressions;
        IEnumerable<Signal> _avalibleSignals;

        public StatementDataModel(IEnumerable<Signal> avalibleSignals, StatementDataModelType statementDataModelType)
        {
            _avalibleSignals = avalibleSignals;
            _statementDataType = statementDataModelType;
            _isAlgebraOperatorVisible = true;

            _avlibleExpressions = new List<string>();
            _avlibleExpressions.Add("Larger Then");
            _avlibleExpressions.Add("Less Then");

            switch (statementDataModelType)
            {
                case StatementDataModelType.RegularStatement:
                    _avlibleExpressions.Add("Rising Edge");
                    _avlibleExpressions.Add("Falling Edge");
                    _avlibleExpressions.Add("Equals");
                    IsTimedStatement = false;
                    IsRegularStatement = true;
                    break;
                case StatementDataModelType.TimedStatement:
                    IsTimedStatement = true;
                    IsRegularStatement = false;
                    break;
                default:
                    break;
            }
        }

        public bool IsTimedStatement { get; private set; }

        public bool IsRegularStatement { get; private set; }

        public StatementDataModelType StatementDataModelType
        {
            get { return _statementDataType; }
        }

        public List<string> AvalibleLogicalOperators
        {
            get
            {
                var list = new List<string>();
                list.Add("And");
                list.Add("Or");
                return list;
            }
        }

        public List<string> AvalibleExpressions
        {
            get
            {
                return _avlibleExpressions;
            }
        }

        public IEnumerable<Signal> AvalibleSignals
        {
            get
            {
                return _avalibleSignals;
            }
        }

        public string Signal 
        {
            get { return _signal; }
            set { SetProperty(ref _signal, value); }
        }
        public string ExpressionValue
        {
            get { return _expressionsValue; }
            set { SetProperty(ref _expressionsValue, value); }
        }
        public string SignalOrValue
        {
            get { return _signalOrValue; }
            set { SetProperty(ref _signalOrValue, value); }
        }
        public string AlgebraOperator
        {
            get { return _algebraoperator; }
            set { SetProperty(ref _algebraoperator, value); }
        }

        public bool IsAlgebraOperatorVisible
        {
            get { return _isAlgebraOperatorVisible; }
            set { SetProperty(ref _isAlgebraOperatorVisible, value); }
        }
    }
}
