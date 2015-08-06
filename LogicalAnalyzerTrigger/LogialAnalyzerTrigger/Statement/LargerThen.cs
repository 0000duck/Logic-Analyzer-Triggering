using ABB.Robotics.Paint.RobView.Database.SignalLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogialAnalyzerTrigger.Statement
{
    public class LargerThen : Statement
    {
        private SignalValueTimestamp _oldSignalValueTimestamp, _oldCompareSignalValueTimestamp;
        private bool _isStatementTrue; 

        public LargerThen(Signal signal, double value)
            : base(signal, value)
        {
            _isStatementTrue = false;
        }

        public LargerThen(Signal signal, Signal compareSignal)
            : base(signal, compareSignal)
        {
            _isStatementTrue = false;
        }

        public override bool CheckStatement(SignalValueTimestamp newSignalValueTimestamp)
        {
            switch (base.CompareTo)
            {
                case LogialAnalyzerTrigger.Statement.CompareTo.SignalValue:
                    CheckStatementWithTwoSignals(newSignalValueTimestamp);
                    break;
                case LogialAnalyzerTrigger.Statement.CompareTo.StaticVariable:
                    if (base.Signal == newSignalValueTimestamp.Signal)
                    {
                        if (newSignalValueTimestamp.Value > base.CompareVariable)
                            _isStatementTrue = true;
                        else
                            _isStatementTrue = false;
                    }
                    break;
                default:
                    throw new Exception("Compare option not set");
            }
            return _isStatementTrue;
        }

        private void CheckStatementWithTwoSignals(SignalValueTimestamp newSignalValueTimestamp)
        {
            //Checks if it is the first or second signal that has been changed.
            if (newSignalValueTimestamp.Signal == base.Signal)
            {
                if (_oldCompareSignalValueTimestamp != null)
                {
                    if (newSignalValueTimestamp.Value < _oldCompareSignalValueTimestamp.Value)
                        _isStatementTrue = true;
                    else
                        _isStatementTrue = false;
                }
                _oldSignalValueTimestamp = newSignalValueTimestamp;
            }
            else if (newSignalValueTimestamp.Signal == base.CompareSignal)
            {
                if (_oldSignalValueTimestamp != null)
                {
                    if (newSignalValueTimestamp.Value < _oldSignalValueTimestamp.Value)
                        _isStatementTrue = true;
                    else
                        _isStatementTrue = false;
                }
                _oldCompareSignalValueTimestamp = newSignalValueTimestamp;
            }
        }

        public override string ToString()
        {
            switch (base.CompareTo)
            {
                case LogialAnalyzerTrigger.Statement.CompareTo.SignalValue:
                    return base.Signal.Name + " Larger Then " + base.CompareSignal.Name;
                case LogialAnalyzerTrigger.Statement.CompareTo.StaticVariable:
                    return base.Signal.Name + " Larger Then " + base.CompareVariable;
                default:
                    return "";
            }
        }
    }
}
