using ABB.Robotics.Paint.RobView.Database.SignalLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogialAnalyzerTrigger.Statement
{
    /// <summary>
    /// An enum which tells the statement to either compare to another signal or just a staic variable.
    /// </summary>
    public enum CompareTo
    {
        SignalValue,
        StaticVariable
    }

    /// <summary>
    /// Abstract class that conatins the base logic of the statements
    /// </summary>
    public abstract class Statement
    {
        private Signal _signal, _compareSignal;
        private CompareTo _compareTo;
        private double _compareVariable;
        private long _startTimeStamp;

        public Statement(){}

        public Statement(Signal signal, double compareValue)
        {
            _signal = signal;
            _compareVariable = compareValue;
            _compareTo = CompareTo.StaticVariable;
        }

        public Statement(Signal signal, Signal compareSignal)
        {
            _signal = signal;
            _compareSignal = compareSignal;
            _compareTo = CompareTo.SignalValue;
        }

        public Signal Signal
        {
            get { return _signal; }
        }

        public Signal CompareSignal
        {
            get { return _compareSignal; }
        }

        public double CompareVariable
        {
            get { return _compareVariable; }
        }

        public CompareTo CompareTo
        {
            get { return _compareTo; }
        }

        public long StartTimestamp
        {
            get { return _startTimeStamp; }
        }

        public abstract bool CheckStatement(SignalValueTimestamp newSignalValueTimestamp);

        public virtual void SetStartTimeToTimedStatements(long timestamp)
        {
            _startTimeStamp = timestamp;
        }

        public virtual bool IsUsingSignal(Signal signal)
        {
            if (signal == _signal || signal == _compareSignal)
            {
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return base.ToString();
        }
        
    }
}
