using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogialAnalyzerTrigger.Statement
{
    public class GreaterThen : Statement
    {
        private double _compareValue;
        private SignalHandler _compareSignal;
        private TriggerInput _triggerInputs;

        public GreaterThen(SignalHandler signal, double compareValue, AlgerbraOperator nextOperator)
            : base(signal, nextOperator)
        {
            this._compareValue = compareValue;
            this._triggerInputs = TriggerInput.OnlyValue;
            base.SignalHandlersUsedByTrigger.Add(signal);
        }

        public GreaterThen(SignalHandler signal, SignalHandler compareSignal, AlgerbraOperator nextOperator)
            : base(signal, nextOperator)
        {
            this._compareSignal = compareSignal;
            this._triggerInputs = TriggerInput.OnlySignal;
            base.SignalHandlersUsedByTrigger.Add(signal);
            base.SignalHandlersUsedByTrigger.Add(compareSignal);
        }

        public GreaterThen(SignalHandler signal, SignalHandler compareSignal, double compareValue, AlgerbraOperator nextOperator)
            : base(signal, nextOperator)
        {
            this._compareSignal = compareSignal;
            this._compareValue = compareValue;
            this._triggerInputs = TriggerInput.SignalAndValue;
            base.SignalHandlersUsedByTrigger.Add(signal);
            base.SignalHandlersUsedByTrigger.Add(compareSignal);

        }

        public override void CheckStatement()
        {
            if (_triggerInputs == TriggerInput.OnlySignal)
            {
                if (base.SignalHandler.Value > _compareSignal.Value)
                    base.State = true;
                else
                    base.State = false;
            }
            else if (_triggerInputs == TriggerInput.OnlyValue)
            {
                if (base.SignalHandler.Value > _compareValue)
                    base.State = true;
                else
                    base.State = false;
            }
            else if (_triggerInputs == TriggerInput.SignalAndValue)
            {
                if (base.SignalHandler.Value > (_compareSignal.Value + _compareValue))
                    base.State = true;
                else
                    base.State = false;

            }
        }

        public override string ToString()
        {
            string writeString = base.ToString() + "> ";

            switch (_triggerInputs)
            {
                case TriggerInput.OnlySignal:
                    writeString += _compareSignal.Signal.Name + " ";
                    break;
                case TriggerInput.OnlyValue:
                    writeString += _compareValue + " ";
                    break;
                default:
                    break;
            }
            return writeString;
        }
    }
}
