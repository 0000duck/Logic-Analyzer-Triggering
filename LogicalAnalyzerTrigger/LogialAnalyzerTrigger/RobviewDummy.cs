using ABB.Robotics.Paint.RobView.Database.SignalLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LogialAnalyzerTrigger
{
    class RobviewDummy
    {
        public static void Main()
        {
            var signal = new Signal() { Name = "A1" };
            var signal2 = new Signal() { Name = "A2" };
            var signalist = new List<Signal>();
            signalist.Add(signal);
            signalist.Add(signal2);


            var statement = new LogialAnalyzerTrigger.Statement.RisingEdge(signal, 0.5);
            var triggerStateForkedStatement = new TriggerForkedState(statement);
            triggerStateForkedStatement.AddTriggerStateThenOperations(TriggerStateThenOperations.StartTimer);
            triggerStateForkedStatement.AddTriggerStateThenOperations(TriggerStateThenOperations.GotoNextState);
            var triggerstate = new TriggerState();
            triggerstate.AddTriggerStateForkedStatement(triggerStateForkedStatement);

            var statement2 = new LogialAnalyzerTrigger.Statement.RisingEdge(signal2, 0.5);
            var statement3 = new LogialAnalyzerTrigger.Statement.StatementTimeLessThen(600);
            var statement4 = new LogialAnalyzerTrigger.Statement.StatementCollection();
            statement4.AddStatement(statement2, LogialAnalyzerTrigger.Statement.LogicalOperators.First);
            statement4.AddStatement(statement3, LogialAnalyzerTrigger.Statement.LogicalOperators.And);
            var statement5 = new LogialAnalyzerTrigger.Statement.StatementTimeLargerThen(600);
            var foredstatement2 = new TriggerForkedState(statement4);
            foredstatement2.AddTriggerStateThenOperations(TriggerStateThenOperations.Trigger);
            var forkedStatemetn3 = new TriggerForkedState(statement5);
            forkedStatemetn3.AddTriggerStateThenOperations(TriggerStateThenOperations.GotoFirsState);
            var triggerstate2 = new TriggerState();
            triggerstate2.AddTriggerStateForkedStatement(foredstatement2);
            triggerstate2.AddTriggerStateForkedStatement(forkedStatemetn3);


            var trigger = new Trigger() {Name = "Test Trigger" };
            trigger.AddTriggerState(triggerstate);
            trigger.AddTriggerState(triggerstate2);

            var engine = new TriggerEngine();
            engine.AddTrigger(trigger);

            Thread thread = new Thread(() => engine.ShowConfigDialog(signalist));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }

    }
}
