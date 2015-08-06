using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogialAnalyzerTrigger.Statement
{
    public class StatementTimeLessThen : Statement
    {


        private int _timerThreshold;
        private long _startTime;


        public StatementTimeLessThen(int timerThreshold)
        {
            _timerThreshold = timerThreshold;
        }

        public void SetStartTime(long timestamp)
        {
            _startTime = timestamp;
        }

        public override bool CheckStatement(SignalValueTimestamp newValue)
        {
            
            if (base.StartTimestamp + _timerThreshold > newValue.TimeStamp)
                return true;
            return false;
        }

        public override bool IsUsingSignal(ABB.Robotics.Paint.RobView.Database.SignalLog.Signal signal)
        {
            return true;
        }

        public override string ToString()
        {
            return "Timer Less then " + _timerThreshold; 
        }
    }
}
