using ABB.Robotics.Paint.RobView.Database.SignalLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogialAnalyzerTrigger
{
    /// <summary>
    /// Contains the statement of the forked state and a list of then operations to be used if the 
    /// state is true. Use AddTriggerStateThenOperations to add then operations
    /// </summary>
    public class TriggerForkedState
    {
        private Statement.Statement _statement;
        private List<TriggerStateThenOperations> _triggerStateThenOperations;

        public TriggerForkedState(Statement.Statement statement)
        {
            _statement = statement;
            _triggerStateThenOperations = new List<TriggerStateThenOperations>();
        }

        public List<TriggerStateThenOperations> TriggerStateThenOperations
        {
            get { return _triggerStateThenOperations; }
        }

        public void AddTriggerStateThenOperations(TriggerStateThenOperations triggerStateThenOperation)
        {
            _triggerStateThenOperations.Add(triggerStateThenOperation);
        }

        public bool CheckTriggerStateForkedStatement(SignalValueTimestamp signalValueTimestamp)
        {
            return _statement.CheckStatement(signalValueTimestamp);
        }

        public void SetStartTimeToTimedStatemetns(long timestamp)
        {
            _statement.SetStartTimeToTimedStatements(timestamp);
        }

        public bool IsUsingSignal(Signal signal)
        {
            return _statement.IsUsingSignal(signal);
        }

        public override string ToString()
        {
            var tostring =  _statement.ToString() + "\n Then: ";
            foreach (TriggerStateThenOperations thenoperation in _triggerStateThenOperations)
            {
                switch (thenoperation)
                {
                    case LogialAnalyzerTrigger.TriggerStateThenOperations.GotoFirsState:
                        tostring += "Goto First State, ";
                        break;
                    case LogialAnalyzerTrigger.TriggerStateThenOperations.GotoNextState:
                        tostring += "Goto Next State, ";
                        break;
                    case LogialAnalyzerTrigger.TriggerStateThenOperations.StartTimer:
                        tostring += "Start Timer, "; 
                        break;
                    case LogialAnalyzerTrigger.TriggerStateThenOperations.Trigger:
                        tostring += "Trigger, "; 
                        break;
                    default:
                        break;
                }
            }
            return tostring;
        }
    }
}
