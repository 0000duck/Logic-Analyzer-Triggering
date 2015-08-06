using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogialAnalyzerTrigger
{

    public enum TriggerActions
    {
        // Trigger Default Action kanskje?
        Trigger,
        Counter
    }

    public class Trigger
    {

        private bool _state;
        private bool _hasTriggered;
        private List<Statement> _triggerList;
        private string _eventName;
        private TriggerActions _triggerAction;
        private int _counter;

		public Trigger()
		{
		}


        public Trigger(List<ITrigger> triggerList)
        {
            this._triggerList = triggerList;
            this._state = false;
            this._hasTriggered = false;
            this._eventName = "No name set";
            this._counter = 0;
        }

        public Trigger(ITrigger trigger)
        {
            this._triggerList = new List<ITrigger>();
            this._triggerList.Add(trigger);
            this._state = false;
            this._hasTriggered = false;
            this._eventName = "No name set";
            this._counter = 0;
        }

		public void AddTrigger(ITrigger trigger)
		{

		}

        public TriggerActions TriggerAction
        {
            get { return this._triggerAction; }
            set { this._triggerAction = value; }
        }

        public List<ITrigger> TriggerList
        {
            get { return this._triggerList; }
        }

        public bool State
        {
            get { return this._state; }
        }

        public string Name
        {
            get { return this._eventName; }
            set { this._eventName = value; }
        }

        public void HandleTriggerExpression()
        {
            bool tmpState = false;

            foreach (ITrigger IT in _triggerList)
            {
                switch (IT.LogicalOperator)
                {
                    case AlgerbraOperator.First:
                        tmpState = IT.State;
                        break;
                    case AlgerbraOperator.And:
                        if (IT.State && tmpState)
                            tmpState = true;
                        else
                            tmpState = false;
                        break;
                    case AlgerbraOperator.Or:
                        if (IT.State || tmpState)
                            tmpState = true;
                        else
                            tmpState = false;
                        break;
                    default:
                        break;
                }
            }
            // Klønete
            switch (_triggerAction)
            {
                case TriggerActions.Counter:
                    if ((tmpState && !_hasTriggered))
                    {
                        if (_counter >= 5)
                            _state = true;
                        else
                            _counter++;

                        _hasTriggered = true;
                    }
                    else if (tmpState && _hasTriggered)
                    {
                        _state = false;
                    }
                    else
                    {
                        _state = false;
                        _hasTriggered = false;
                    }
                    break;
                default:
                    if (tmpState && !_hasTriggered)
                    {
                        _state = true;
                        _hasTriggered = true;
                    }
                    else if (tmpState && _hasTriggered)
                    {
                        _state = false;
                    }
                    else
                    {
                        _state = false;
                        _hasTriggered = false;
                    }
                    break;
            }
        }

        public override string ToString()
        {
            string writeString = "IF:\n";

            foreach (ITrigger trigger in _triggerList)
            {
                writeString += trigger.ToString() + " ";                
            }

            writeString += "\nThen:\nTrigger";
            return writeString;
        }
    }
}
