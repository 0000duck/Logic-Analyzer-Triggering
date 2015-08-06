using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogialAnalyzerTrigger.Statement
{
    public class StatementTimeLargerThen : Statement
    {
        private int _timerThreshold;
        private long _startTime;


        public StatementTimeLargerThen(int timerThreshold)
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
                return false;
            return true;
        }

        public override bool IsUsingSignal(ABB.Robotics.Paint.RobView.Database.SignalLog.Signal signal)
        {
            return true;
        }

        public override string ToString()
        {
            return "Timer LargerThen then " + _timerThreshold; 
        }
    }
}
