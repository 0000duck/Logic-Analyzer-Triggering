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
    /// Trigger contains a list of triggerstates, use AddTriggerState to add states.
    /// Call CheckTrigger to see if it triggers on a signalchange
    /// </summary>
    public class Trigger
    {
        private List<TriggerState> _triggerStates;
        private int _triggerState;
        private bool _isAlreadyTriggerd;
        private string _name;

        public Trigger()
        {
            _triggerStates = new List<TriggerState>();
            _isAlreadyTriggerd = false;
            _triggerState = 0;
        }

        public int TriggerState
        {
            get { return _triggerState; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public void AddTriggerState(TriggerState triggerState)
        {
            _triggerStates.Add(triggerState);
        }

        public bool CheckTrigger(SignalValueTimestamp signalValueTimestamp)
        {
            if (_triggerState > _triggerStates.Count)
                throw new Exception("Cannont jump to that state, it does not exsist");
            
            //Selects the curret state of the triger
            var currentTriggerState = _triggerStates.ElementAt(_triggerState);

            //Cheks if the state is true for the signal change
            var isStateTrue = currentTriggerState.CheckTriggerState(signalValueTimestamp);

            if (isStateTrue)
            {
                //Loops through the then operations of the state e.g. IF: A1 == 0 Then: StatTrimer, GoToNextState
                foreach (TriggerStateThenOperations thenOperation in currentTriggerState.TriggerStateOptions)
                {
                    switch (thenOperation)
                    {
                        case TriggerStateThenOperations.GotoFirsState:
                            _triggerState = 0;
                            break;
                        case TriggerStateThenOperations.GotoNextState:
                            _triggerState++;
                            break;
                        // The trigger sets a start time to the statements if a timer is used for the trigger
                        case TriggerStateThenOperations.StartTimer:
                            SetStartTimeStampToTimeStatements(signalValueTimestamp.TimeStamp);
                            break;
                        case TriggerStateThenOperations.Trigger:
                            _triggerState = 0;
                            /* Logic to avoid the trigger to trigger multiple times during a conditon
                             * e.g If a the trigger is IF: A1 < 0 Then: Trigger
                             * The trigger will only trigger again after A1 has gone above 0 */
                            if (_isAlreadyTriggerd && isStateTrue)
                            {
                                isStateTrue = false;
                            }
                            else if (isStateTrue)
                            {
                                _isAlreadyTriggerd = true;
                            }

                            return isStateTrue;
                        default:
                            throw new Exception("No Then operation has been set");
                    }
                }   
            }
            _isAlreadyTriggerd = false;
            return false;
        }
        

        private void SetStartTimeStampToTimeStatements(long timestamp)
        {
            foreach (TriggerState triggerstate in _triggerStates)
            {
                triggerstate.SetStartTimeToTimeStatements(timestamp);
            }
        }

        public bool IsUsingSignal(Signal signal)
        {
            foreach (TriggerState triggerstate in _triggerStates)
            {
                if (triggerstate.IsUsingSignal(signal))
                    return true;
            }
            return false;
        }

        public override string ToString()
        {
            var tostring = Name + ":\n\n";
            int i = 1;
            foreach (TriggerState triggerstate in _triggerStates)
            {
                tostring += "State" + i +"\n";
                tostring += triggerstate.ToString();
                i++;
            }
            return tostring;
        }
    }
}
