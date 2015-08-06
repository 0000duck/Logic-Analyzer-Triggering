using ABB.Robotics.Paint.RobView.Database.SignalLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LogialAnalyzerTrigger
{
    class Mainwindw
    {
        public static void Main()
        {
            var engine = new TriggerEngine();
            var signal = new Signal() { Name = "A1" };
            var signalList = new List<Signal>();
            signalList.Add(signal);
            signalList.Add(new Signal(){Name = "A2"});

            var trigger = new Trigger() { Name = "Trigger1" };
            var trigger2 = new Trigger() { Name = "Trigger2" };
            var statement = new Statement.Equals(signal, 2);
            trigger.AddStatement(statement);
            trigger2.AddStatement(statement);
            //engine.AddTrigger(trigger);
            //engine.AddTrigger(trigger2);


            Thread thread = new Thread(() => engine.ShowConfigDialog(signalList));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }
    }
}
