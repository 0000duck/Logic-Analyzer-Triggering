 using ABB.Robotics.Paint.RobView.Database.SignalLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogialAnalyzerTrigger.Statement
{
    public enum LogicalOperators
    {
        And,
        Or,
        First
    }

    public class StatementCollection : Statement
    {
        private Dictionary<Statement, LogicalOperators> _statementAndLogicalOperators;

        public StatementCollection()
        {
            _statementAndLogicalOperators = new Dictionary<Statement, LogicalOperators>();
        }

        public void AddStatement(Statement statement, LogicalOperators logicalOperator)
        {
            _statementAndLogicalOperators.Add(statement, logicalOperator);
        }

        public override bool CheckStatement(SignalValueTimestamp newValue)
        {
            var isTrue = false;

            foreach (KeyValuePair<Statement, LogicalOperators> keyValuePair in _statementAndLogicalOperators)
            {
                switch (keyValuePair.Value)
                {
                    case LogicalOperators.And:
                        isTrue = isTrue && keyValuePair.Key.CheckStatement(newValue);
                        break;
                    case LogicalOperators.Or:
                        isTrue = isTrue || keyValuePair.Key.CheckStatement(newValue);
                        break;
                    case LogicalOperators.First:
                        isTrue = keyValuePair.Key.CheckStatement(newValue);
                        break;
                    default:
                        break;
                }
            }

            return isTrue;
        }

        public override bool IsUsingSignal(Signal signal)
        {
            foreach (KeyValuePair<Statement, LogicalOperators> keyValuePair in _statementAndLogicalOperators)
            {
                if (keyValuePair.Key.IsUsingSignal(signal))
                    return true;
            }
            return false;
        }

        public override void SetStartTimeToTimedStatements(long timestamp)
        {
            foreach (KeyValuePair<Statement, LogicalOperators> keyValuePair in _statementAndLogicalOperators)
            {
                keyValuePair.Key.SetStartTimeToTimedStatements(timestamp);
            }
        }

        public override string ToString()
        {
            var torsting = "";
            foreach (KeyValuePair<Statement, LogicalOperators> keyValuePair in _statementAndLogicalOperators)
            {
                switch (keyValuePair.Value)
                {
                    case LogicalOperators.And:
                        torsting += " And " + keyValuePair.Key.ToString();
                        break;
                    case LogicalOperators.Or:
                        torsting += " Or " + keyValuePair.Key.ToString();
                        break;
                    default:
                        torsting += keyValuePair.Key.ToString();
                        break;
                }
            }
            return torsting;
        }
    }
}
