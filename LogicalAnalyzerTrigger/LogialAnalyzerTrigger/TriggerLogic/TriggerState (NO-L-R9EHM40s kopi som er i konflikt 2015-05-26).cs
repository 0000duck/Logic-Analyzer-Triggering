using ABB.Robotics.Paint.RobView.Database.SignalLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogialAnalyzerTrigger
{
    /// <summary>
    /// A enum for the different Then operations a state can preform
    /// </summary>
    public enum TriggerStateThenOperations
    {
        Trigger,
        GotoNextState,
        GotoFirsState,
        StartTimer
    }

    /// <summary>
    /// TriggerState handles one state of a trigger, it contaions a list of forked States and a list of TriggerStateThenOperations 
    /// from the forked statement that has been invoked
    /// </summary>
    public class TriggerState
    {
        private List<TriggerStateThenOperations> _triggerStateOptions;
        private List<TriggerStateForkedState> _triggerStateForkedStatements;

        public TriggerState()
        {
            _triggerStateForkedStatements = new List<TriggerStateForkedState>();
        }

        public IEnumerable TriggerStateOptions
        {
            get { return _triggerStateOptions; }
        }

        public void AddTriggerStateForkedStatement(TriggerStateForkedState triggerStateForkedStatement)
        {
            _triggerStateForkedStatements.Add(triggerStateForkedStatement);
        }

        public bool CheckTriggerState(SignalValueTimestamp signalValueTimestamp)
        {
            foreach (TriggerStateForkedState triggerStateForkedStatemetn in _triggerStateForkedStatements)
            {
                if (triggerStateForkedStatemetn.CheckTriggerStateForkedStatement(signalValueTimestamp))
                {
                    // Sets the then operations to the state that has been true.
                    _triggerStateOptions = triggerStateForkedStatemetn.TriggerStateThenOperations;
                    return true;
                }
            }
            return false;
        }

        public void SetStartTimeToTimeStatements(long timestamp)
        {
            foreach (TriggerStateForkedState triggerStateForkedStatemetn in _triggerStateForkedStatements)
            {
                triggerStateForkedStatemetn.SetStartTimeToTimedStatemetns(timestamp);
            }
        }



        public bool IsUsingSignal(Signal signal)
        {
            foreach (TriggerStateForkedState triggerStateForkedStatemetn in _triggerStateForkedStatements)
            {
                if (triggerStateForkedStatemetn.IsUsingSignal(signal))
                    return true;
            }
            return false;
        }

        public override string ToString()
        {
            var tostring = "";
            int i = 0;
            foreach (TriggerStateForkedState triggerStateForkedStatemetn in _triggerStateForkedStatements)
            {
                if (i == 0)
                    tostring += "IF: ";
                else
                    tostring += "Else IF: ";
                tostring += triggerStateForkedStatemetn.ToString() + "\n";
                i++;
            }
            return tostring;
        }

    }
}
