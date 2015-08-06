using ABB.Robotics.Paint.RobView.Database.SignalLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LogialAnalyzerTrigger
{

    class LoggerData
    {
        public Signal sig { get; set;  }
        public SignalValues sigVal { get; set; }

        public LoggerData(Signal sig, SignalValues sigVal)
        {
            this.sig = sig;
            this.sigVal = sigVal;
        }
    }

    public class Logger
    {
        private System.TimeSpan timespan;
        private DateTime now;
        private CSVParser parser;
        private TriggerEngine engine;

        public Logger(string URL, TriggerEngine engine)
        {
            this.parser = new CSVParser(URL);
            this.engine = engine;
        }

        public List<Signal> getSignals()
        {
            return parser.SignalList;
        }

        public void generateSignalsFromCSV()
        {
            List<LoggerData> list = new List<LoggerData>();

            foreach (KeyValuePair<Signal, List<SignalValues>> kvp in parser.Dict)
            {
                foreach (SignalValues sv in kvp.Value)
                {
                    list.Add(new LoggerData(kvp.Key, sv));
                }
            }

            list = list.OrderBy(o => o.sigVal.Time.Ticks).ToList();

            foreach (LoggerData ld in list)
            {
                //engine.logValueChange(ld.sig, ld.sigVal.NewValue, ld.sigVal.Time.Ticks);
            }
        }
    }
}
