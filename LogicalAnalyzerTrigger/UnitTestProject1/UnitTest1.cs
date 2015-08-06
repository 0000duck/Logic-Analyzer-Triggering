using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using ABB.Robotics.Paint.RobView.Database.SignalLog;
using LogialAnalyzerTrigger;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {

        private AutoResetEvent _test = new AutoResetEvent(false);
      
     
        [TestMethod]
        public void TestEqualsToVariable()
        {
            //Test for A1 == 0
            _test.Reset();

            var signal = new Signal() { Name = "A1" };
            var statement = new LogialAnalyzerTrigger.Statement.Equals(signal, 0);
            var triggerStateForkedStatement = new TriggerForkedState(statement);
            triggerStateForkedStatement.AddTriggerStateThenOperations(TriggerStateThenOperations.Trigger);

            var triggerstate = new TriggerState();
            triggerstate.AddTriggerStateForkedStatement(triggerStateForkedStatement);
            
            var trigger = new Trigger();
            trigger.AddTriggerState(triggerstate);
            
            var engine = new TriggerEngine();
            engine.AddTrigger(trigger);
            engine.TriggeredUnitTest += engine_Triggered;

            engine.LogValueChange(signal, 0, DateTime.Now.Ticks);

            Assert.IsTrue(_test.WaitOne(10));

            engine.LogValueChange(signal, 0, DateTime.Now.Ticks);

            Assert.IsFalse(_test.WaitOne(10));

            engine.LogValueChange(signal, 2, DateTime.Now.Ticks);

            Assert.IsFalse(_test.WaitOne(10));

            engine.LogValueChange(signal, 0, DateTime.Now.Ticks);

            Assert.IsTrue(_test.WaitOne(10));


        }

        [TestMethod]
        public void TestEqualsToSignal()
        {
            //Test for A1 == 0
            _test.Reset();

            var signal = new Signal() { Name = "A1" };
            var signal2 = new Signal() { Name = "A2" };
            var statement = new LogialAnalyzerTrigger.Statement.Equals(signal, signal2);
            var triggerStateForkedStatement = new TriggerForkedState(statement);
            triggerStateForkedStatement.AddTriggerStateThenOperations(TriggerStateThenOperations.Trigger);

            var triggerstate = new TriggerState();
            triggerstate.AddTriggerStateForkedStatement(triggerStateForkedStatement);

            var trigger = new Trigger();
            trigger.AddTriggerState(triggerstate);

            var engine = new TriggerEngine();
            engine.AddTrigger(trigger);
            engine.TriggeredUnitTest += engine_Triggered;

            engine.LogValueChange(signal, 0, DateTime.Now.Ticks);

            Assert.IsFalse(_test.WaitOne(10));

            engine.LogValueChange(signal2, 2, DateTime.Now.Ticks);

            Assert.IsFalse(_test.WaitOne(10));

            engine.LogValueChange(signal2, 0, DateTime.Now.Ticks);

            Assert.IsTrue(_test.WaitOne(10));
        }


        [TestMethod]
        public void TestLargerToVariable()
        {
            //Test for A1 > 0
            _test.Reset();

            var signal = new Signal() { Name = "A1" };
            var statement = new LogialAnalyzerTrigger.Statement.LargerThen(signal, 0);
            var triggerStateForkedStatement = new TriggerForkedState(statement);
            triggerStateForkedStatement.AddTriggerStateThenOperations(TriggerStateThenOperations.Trigger);

            var triggerstate = new TriggerState();
            triggerstate.AddTriggerStateForkedStatement(triggerStateForkedStatement);

            var trigger = new Trigger();
            trigger.AddTriggerState(triggerstate);

            var engine = new TriggerEngine();
            engine.AddTrigger(trigger);
            engine.TriggeredUnitTest += engine_Triggered;

            engine.LogValueChange(signal, 1, DateTime.Now.Ticks);

            Assert.IsTrue(_test.WaitOne(10));

            engine.LogValueChange(signal, 0, DateTime.Now.Ticks);

            Assert.IsFalse(_test.WaitOne(10));
        }

        [TestMethod]
        public void TestLargerThenToSignal()
        {
            //Test for A1 > 0
            _test.Reset();

            var signal = new Signal() { Name = "A1" };
            var signal2 = new Signal() { Name = "A2" };
            var statement = new LogialAnalyzerTrigger.Statement.LargerThen(signal, signal2);
            var triggerStateForkedStatement = new TriggerForkedState(statement);
            triggerStateForkedStatement.AddTriggerStateThenOperations(TriggerStateThenOperations.Trigger);

            var triggerstate = new TriggerState();
            triggerstate.AddTriggerStateForkedStatement(triggerStateForkedStatement);

            var trigger = new Trigger();
            trigger.AddTriggerState(triggerstate);

            var engine = new TriggerEngine();
            engine.AddTrigger(trigger);
            engine.TriggeredUnitTest += engine_Triggered;

            engine.LogValueChange(signal, 1, DateTime.Now.Ticks);

            Assert.IsFalse(_test.WaitOne(10));

            engine.LogValueChange(signal2, 3, DateTime.Now.Ticks);

            Assert.IsFalse(_test.WaitOne(10));

            engine.LogValueChange(signal2, 0, DateTime.Now.Ticks);

            Assert.IsTrue(_test.WaitOne(10));

        }


        [TestMethod]
        public void TestLessThenTpValue()
        {
            //Test for A1 < 0
            _test.Reset();

            var signal = new Signal() { Name = "A1" };
            var statement = new LogialAnalyzerTrigger.Statement.LessThen(signal, 0);
            var triggerStateForkedStatement = new TriggerForkedState(statement);
            triggerStateForkedStatement.AddTriggerStateThenOperations(TriggerStateThenOperations.Trigger);

            var triggerstate = new TriggerState();
            triggerstate.AddTriggerStateForkedStatement(triggerStateForkedStatement);

            var trigger = new Trigger();
            trigger.AddTriggerState(triggerstate);

            var engine = new TriggerEngine();
            engine.AddTrigger(trigger);
            engine.TriggeredUnitTest += engine_Triggered;

            engine.LogValueChange(signal, -1, DateTime.Now.Ticks);

            Assert.IsTrue(_test.WaitOne(10));

            engine.LogValueChange(signal, 2, DateTime.Now.Ticks);

            Assert.IsFalse(_test.WaitOne(10));
        }

        [TestMethod]
        public void TestLessThenToSignal()
        {
            //Test for A1 > 0
            _test.Reset();

            var signal = new Signal() { Name = "A1" };
            var signal2 = new Signal() { Name = "A2" };
            var statement = new LogialAnalyzerTrigger.Statement.LessThen(signal, signal2);
            var triggerStateForkedStatement = new TriggerForkedState(statement);
            triggerStateForkedStatement.AddTriggerStateThenOperations(TriggerStateThenOperations.Trigger);

            var triggerstate = new TriggerState();
            triggerstate.AddTriggerStateForkedStatement(triggerStateForkedStatement);

            var trigger = new Trigger();
            trigger.AddTriggerState(triggerstate);

            var engine = new TriggerEngine();
            engine.AddTrigger(trigger);
            engine.TriggeredUnitTest += engine_Triggered;

            engine.LogValueChange(signal, 1, DateTime.Now.Ticks);

            Assert.IsFalse(_test.WaitOne(10));

            engine.LogValueChange(signal2, 0, DateTime.Now.Ticks);

            Assert.IsFalse(_test.WaitOne(10));

            engine.LogValueChange(signal2, 3, DateTime.Now.Ticks);

            Assert.IsTrue(_test.WaitOne(10));

        }

        [TestMethod]
        public void TestFallingEdge()
        {
            //Test for A1 on falling edge of 0.5 
            _test.Reset();

            var signal = new Signal() { Name = "A1" };
            var statement = new LogialAnalyzerTrigger.Statement.FallingEdge(signal, 0.5);
            var triggerStateForkedStatement = new TriggerForkedState(statement);
            triggerStateForkedStatement.AddTriggerStateThenOperations(TriggerStateThenOperations.Trigger);

            var triggerstate = new TriggerState();
            triggerstate.AddTriggerStateForkedStatement(triggerStateForkedStatement);

            var trigger = new Trigger();
            trigger.AddTriggerState(triggerstate);

            var engine = new TriggerEngine();
            engine.AddTrigger(trigger);
            engine.TriggeredUnitTest += engine_Triggered;

            engine.LogValueChange(signal, 1, DateTime.Now.Ticks);

            Assert.IsFalse(_test.WaitOne(10));

            engine.LogValueChange(signal, 0, DateTime.Now.Ticks);

            Assert.IsTrue(_test.WaitOne(10));
        }



        [TestMethod]
        public void TestRisingEdge()
        {
            //Test for A1 on rising edge of 0.5
            _test.Reset();

            var signal = new Signal() { Name = "A1" };
            var statement = new LogialAnalyzerTrigger.Statement.RisingEdge(signal, 0.5);
            var triggerStateForkedStatement = new TriggerForkedState(statement);
            triggerStateForkedStatement.AddTriggerStateThenOperations(TriggerStateThenOperations.Trigger);

            var triggerstate = new TriggerState();
            triggerstate.AddTriggerStateForkedStatement(triggerStateForkedStatement);

            var trigger = new Trigger();
            trigger.AddTriggerState(triggerstate);

            var engine = new TriggerEngine();
            engine.AddTrigger(trigger);
            engine.TriggeredUnitTest += engine_Triggered;

            engine.LogValueChange(signal, 0, DateTime.Now.Ticks);

            Assert.IsFalse(_test.WaitOne(10));

            engine.LogValueChange(signal, 1, DateTime.Now.Ticks);

            Assert.IsTrue(_test.WaitOne(10));
        }


        [TestMethod]
        public void TestStatemetnCollection()
        {
            //Test for A1 == 0
            _test.Reset();

            var signal = new Signal() { Name = "A1" };
            var signal2 = new Signal() { Name = "A2" };
          
            var statement = new LogialAnalyzerTrigger.Statement.Equals(signal, 0);
            var statemetn2 = new LogialAnalyzerTrigger.Statement.LessThen(signal2, 5);
            var statemetnCollection = new LogialAnalyzerTrigger.Statement.StatementCollection();
            statemetnCollection.AddStatement(statement, LogialAnalyzerTrigger.Statement.LogicalOperators.First);
            statemetnCollection.AddStatement(statemetn2, LogialAnalyzerTrigger.Statement.LogicalOperators.And);

            var triggerStateForkedStatement = new TriggerForkedState(statemetnCollection);
            triggerStateForkedStatement.AddTriggerStateThenOperations(TriggerStateThenOperations.Trigger);

            var triggerstate = new TriggerState();
            triggerstate.AddTriggerStateForkedStatement(triggerStateForkedStatement);
            
            var trigger = new Trigger();
            trigger.AddTriggerState(triggerstate);



            var engine = new TriggerEngine();
            engine.AddTrigger(trigger);
            engine.TriggeredUnitTest += engine_Triggered;

            engine.LogValueChange(signal, 0, DateTime.Now.Ticks);
            engine.LogValueChange(signal2, 3, DateTime.Now.Ticks);

            Assert.IsTrue(_test.WaitOne(10));

        }

        [TestMethod]
        public void TestEqualsToAnotherSignalInOrder()
        {
            //Test for A1 == 0
            _test.Reset();

            var signal = new Signal() { Name = "A1" };
            var signal2 = new Signal() { Name = "A2" };
            var statement = new LogialAnalyzerTrigger.Statement.Equals(signal, signal2);
            var triggerStateForkedStatement = new TriggerForkedState(statement);
            triggerStateForkedStatement.AddTriggerStateThenOperations(TriggerStateThenOperations.Trigger);

            var triggerstate = new TriggerState();
            triggerstate.AddTriggerStateForkedStatement(triggerStateForkedStatement);
            
            var trigger = new Trigger();
            trigger.AddTriggerState(triggerstate);



            var engine = new TriggerEngine();
            engine.AddTrigger(trigger);
            engine.TriggeredUnitTest += engine_Triggered;

            engine.LogValueChange(signal, 0, DateTime.Now.Ticks);
            engine.LogValueChange(signal2, 0, DateTime.Now.Ticks);

            Assert.IsTrue(_test.WaitOne(10));
        }

        [TestMethod]
        public void TestEqualsToAnotherSignalNotInOrder()
        {
            //Test for A1 == 0
            _test.Reset();

            var signal = new Signal() { Name = "A1" };
            var signal2 = new Signal() { Name = "A2" };
            var statement = new LogialAnalyzerTrigger.Statement.Equals(signal, signal2);
            var triggerStateForkedStatement = new TriggerForkedState(statement);
            triggerStateForkedStatement.AddTriggerStateThenOperations(TriggerStateThenOperations.Trigger);

            var triggerstate = new TriggerState();
            triggerstate.AddTriggerStateForkedStatement(triggerStateForkedStatement);
          
            var trigger = new Trigger();
            trigger.AddTriggerState(triggerstate);



            var engine = new TriggerEngine();
            engine.AddTrigger(trigger);
            engine.TriggeredUnitTest += engine_Triggered;

            engine.LogValueChange(signal2, 0, DateTime.Now.Ticks);
            engine.LogValueChange(signal, 0, DateTime.Now.Ticks);

            Assert.IsTrue(_test.WaitOne(10));
        }

        [TestMethod]
        public void TestStatemetnCollectionWithOneStatementComparingSignals()
        {
            //If: A1 == A2 && A2 < 5 Then: Trigger
            _test.Reset();

            var signal = new Signal() { Name = "A1" };
            var signal2 = new Signal() { Name = "A2" };

            var statement = new LogialAnalyzerTrigger.Statement.Equals(signal, signal2);
            var statemetn2 = new LogialAnalyzerTrigger.Statement.LessThen(signal2, 5);
            var statemetnCollection = new LogialAnalyzerTrigger.Statement.StatementCollection();
            statemetnCollection.AddStatement(statement, LogialAnalyzerTrigger.Statement.LogicalOperators.First);
            statemetnCollection.AddStatement(statemetn2, LogialAnalyzerTrigger.Statement.LogicalOperators.And);

            var triggerStateForkedStatement = new TriggerForkedState(statemetnCollection);
            triggerStateForkedStatement.AddTriggerStateThenOperations(TriggerStateThenOperations.Trigger);

            var triggerstate = new TriggerState();
            triggerstate.AddTriggerStateForkedStatement(triggerStateForkedStatement);
          

            var trigger = new Trigger();
            trigger.AddTriggerState(triggerstate);



            var engine = new TriggerEngine();
            engine.AddTrigger(trigger);
            engine.TriggeredUnitTest += engine_Triggered;

            engine.LogValueChange(signal, 2, DateTime.Now.Ticks);
            engine.LogValueChange(signal2, 2, DateTime.Now.Ticks);

            Assert.IsTrue(_test.WaitOne(10));

        }

        [TestMethod]
        public void TestSimpleTwoState()
        {
            //State 1: IF: A1 == A2 && A2 < 5 Then Goto Next
            //State 2: IF: A1 == 0 Then Trigger
            _test.Reset();

            var signal = new Signal() { Name = "A1" };
            var signal2 = new Signal() { Name = "A2" };

            //TriggerState 1
            var statement = new LogialAnalyzerTrigger.Statement.Equals(signal, signal2);
            var statemetn2 = new LogialAnalyzerTrigger.Statement.LessThen(signal2, 5);
            var statemetnCollection = new LogialAnalyzerTrigger.Statement.StatementCollection();
            statemetnCollection.AddStatement(statement, LogialAnalyzerTrigger.Statement.LogicalOperators.First);
            statemetnCollection.AddStatement(statemetn2, LogialAnalyzerTrigger.Statement.LogicalOperators.And);

            var triggerStateForkedStatement = new TriggerForkedState(statemetnCollection);
            triggerStateForkedStatement.AddTriggerStateThenOperations(TriggerStateThenOperations.GotoNextState);

            var triggerstate = new TriggerState();
            triggerstate.AddTriggerStateForkedStatement(triggerStateForkedStatement);
          
            //TriggerState 2
            var statement3 = new LogialAnalyzerTrigger.Statement.Equals(signal, 0);

            var triggerStateForkedStatement2 = new TriggerForkedState(statement3);
            triggerStateForkedStatement2.AddTriggerStateThenOperations(TriggerStateThenOperations.Trigger);

            var triggerstate2 = new TriggerState();
            triggerstate2.AddTriggerStateForkedStatement(triggerStateForkedStatement2);
          


            var trigger = new Trigger();
            trigger.AddTriggerState(triggerstate);
            trigger.AddTriggerState(triggerstate2);


            var engine = new TriggerEngine();
            engine.AddTrigger(trigger);
            engine.TriggeredUnitTest += engine_Triggered;

            engine.LogValueChange(signal, 2, DateTime.Now.Ticks);
            engine.LogValueChange(signal2, 2, DateTime.Now.Ticks);
            // Will now jump to state 2
            Assert.IsFalse(_test.WaitOne(10));

            engine.LogValueChange(signal, 0, DateTime.Now.Ticks);

            Assert.IsTrue(_test.WaitOne(10));

        }

        [TestMethod]
        public void TestForkedState()
        {
            //IF: A1 == 1 Then Trigger
            //ElseIF: A1 == 0 Then Trigger
              
            _test.Reset();

            var signal = new Signal() { Name = "A1" };

            var statement = new LogialAnalyzerTrigger.Statement.Equals(signal, 1);
            var statemetn2 = new LogialAnalyzerTrigger.Statement.Equals(signal, 0);
         
            var triggerStateForkedStatement = new TriggerForkedState(statement);
            var triggerStateForkedStatement2 = new TriggerForkedState(statemetn2);
            triggerStateForkedStatement.AddTriggerStateThenOperations(TriggerStateThenOperations.Trigger);
            triggerStateForkedStatement2.AddTriggerStateThenOperations(TriggerStateThenOperations.Trigger);

            var triggerstate = new TriggerState();
            triggerstate.AddTriggerStateForkedStatement(triggerStateForkedStatement);
            triggerstate.AddTriggerStateForkedStatement(triggerStateForkedStatement2);

            var trigger = new Trigger();
            trigger.AddTriggerState(triggerstate);



            var engine = new TriggerEngine();
            engine.AddTrigger(trigger);
            engine.TriggeredUnitTest += engine_Triggered;

            engine.LogValueChange(signal, 1, DateTime.Now.Ticks);

            Assert.IsTrue(_test.WaitOne(10));

        }

        [TestMethod]
        public void TestTimeStatement()
        {
            /*State1:
             * IF: Signal1 == 0
             * Then: 1. StartTimer 2.GotoNext
             * 
             *State2: 
             * If: Signal2 == 2 && Timer < 500ms
             * Then: Trigger
             * ElseIf: Timer > 500ms
             * Then: GotoFirst
             */
            _test.Reset();

            var signal = new Signal() { Name = "A1" };
            var signal2 = new Signal() { Name = "A2" };

            var statement = new LogialAnalyzerTrigger.Statement.Equals(signal, 0);
            var triggerStateForkedStatement = new TriggerForkedState(statement);
            triggerStateForkedStatement.AddTriggerStateThenOperations(TriggerStateThenOperations.StartTimer);
            triggerStateForkedStatement.AddTriggerStateThenOperations(TriggerStateThenOperations.GotoNextState);
            var triggerstate = new TriggerState();
            triggerstate.AddTriggerStateForkedStatement(triggerStateForkedStatement);

            var statement2 = new LogialAnalyzerTrigger.Statement.Equals(signal2, 2);
            var statement3 = new LogialAnalyzerTrigger.Statement.StatementTimeLessThen(500);
            var statement4 = new LogialAnalyzerTrigger.Statement.StatementCollection();
            statement4.AddStatement(statement2, LogialAnalyzerTrigger.Statement.LogicalOperators.First);
            statement4.AddStatement(statement3, LogialAnalyzerTrigger.Statement.LogicalOperators.And);
            var statement5 = new LogialAnalyzerTrigger.Statement.StatementTimeLargerThen(500);
            var foredstatement2 = new TriggerForkedState(statement4);
            foredstatement2.AddTriggerStateThenOperations(TriggerStateThenOperations.Trigger);
            var forkedStatemetn3 = new TriggerForkedState(statement5);
            forkedStatemetn3.AddTriggerStateThenOperations(TriggerStateThenOperations.GotoFirsState);
            var triggerstate2 = new TriggerState();
            triggerstate2.AddTriggerStateForkedStatement(foredstatement2);
            triggerstate2.AddTriggerStateForkedStatement(forkedStatemetn3);

            var trigger = new Trigger();
            trigger.AddTriggerState(triggerstate);
            trigger.AddTriggerState(triggerstate2);


            var engine = new TriggerEngine();
            engine.AddTrigger(trigger);
            engine.TriggeredUnitTest += engine_Triggered;

            //The time the first signal changes
            var firstSignalTimestamp = DateTime.Now.Ticks;
            
            // The trigger will jump to state two!
            engine.LogValueChange(signal, 0, firstSignalTimestamp);

            // Signal 2 will change 200 ms after signal1
            engine.LogValueChange(signal2, 2, firstSignalTimestamp + 400);

            Assert.IsTrue(_test.WaitOne(10));
        
        }


        [TestMethod]
        public void TestTimeStatement2()
        {
            /*State1:
             * IF: Signal1 == 0
             * Then: 1. StartTimer 2.GotoNext
             * 
             *State2: 
             * If: Signal2 == 2 && Timer < 500ms
             * Then: Trigger
             * ElseIf: Timer > 500ms
             * Then: GotoFirst
             */
            _test.Reset();

            var signal = new Signal() { Name = "A1" };
            var signal2 = new Signal() { Name = "A2" };

            var statement = new LogialAnalyzerTrigger.Statement.Equals(signal, 0);
            var triggerStateForkedStatement = new TriggerForkedState(statement);
            triggerStateForkedStatement.AddTriggerStateThenOperations(TriggerStateThenOperations.StartTimer);
            triggerStateForkedStatement.AddTriggerStateThenOperations(TriggerStateThenOperations.GotoNextState);
            var triggerstate = new TriggerState();
            triggerstate.AddTriggerStateForkedStatement(triggerStateForkedStatement);

            var statement2 = new LogialAnalyzerTrigger.Statement.Equals(signal2, 2);
            var statement3 = new LogialAnalyzerTrigger.Statement.StatementTimeLessThen(500);
            var statement4 = new LogialAnalyzerTrigger.Statement.StatementCollection();
            statement4.AddStatement(statement2, LogialAnalyzerTrigger.Statement.LogicalOperators.First);
            statement4.AddStatement(statement3, LogialAnalyzerTrigger.Statement.LogicalOperators.And);
            var statement5 = new LogialAnalyzerTrigger.Statement.StatementTimeLargerThen(500);
            var foredstatement2 = new TriggerForkedState(statement4);
            foredstatement2.AddTriggerStateThenOperations(TriggerStateThenOperations.Trigger);
            var forkedStatemetn3 = new TriggerForkedState(statement5);
            forkedStatemetn3.AddTriggerStateThenOperations(TriggerStateThenOperations.GotoFirsState);
            var triggerstate2 = new TriggerState();
            triggerstate2.AddTriggerStateForkedStatement(foredstatement2);
            triggerstate2.AddTriggerStateForkedStatement(forkedStatemetn3);

            var trigger = new Trigger();
            trigger.AddTriggerState(triggerstate);
            trigger.AddTriggerState(triggerstate2);


            var engine = new TriggerEngine();
            engine.AddTrigger(trigger);
            engine.TriggeredUnitTest += engine_Triggered;

            //The time the first signal changes
            var firstSignalTimestamp = DateTime.Now.Ticks;

            // The trigger will jump to state two!
            engine.LogValueChange(signal, 0, firstSignalTimestamp);

            // Signal 2 will change 200 ms after signal1
            engine.LogValueChange(signal2, 2, firstSignalTimestamp + 600);

            Assert.IsFalse(_test.WaitOne(10));

        }

        [TestMethod]
        public void TestTimeStatement3()
        {
            /*State1:
             * IF: Signal1 == 0
             * Then: 1. StartTimer 2.GotoNext
             * 
             *State2: 
             * If: Signal2 == 2 && Timer < 500ms
             * Then: Trigger
             * ElseIf: Timer > 500ms
             * Then: GotoFirst
             */
            _test.Reset();

            var signal = new Signal() { Name = "A1" };
            var signal2 = new Signal() { Name = "A2" };

            var statement = new LogialAnalyzerTrigger.Statement.Equals(signal, 0);
            var triggerStateForkedStatement = new TriggerForkedState(statement);
            triggerStateForkedStatement.AddTriggerStateThenOperations(TriggerStateThenOperations.StartTimer);
            triggerStateForkedStatement.AddTriggerStateThenOperations(TriggerStateThenOperations.GotoNextState);
            var triggerstate = new TriggerState();
            triggerstate.AddTriggerStateForkedStatement(triggerStateForkedStatement);

            var statement2 = new LogialAnalyzerTrigger.Statement.Equals(signal2, 2);
            var statement3 = new LogialAnalyzerTrigger.Statement.StatementTimeLessThen(500);
            var statement4 = new LogialAnalyzerTrigger.Statement.StatementCollection();
            statement4.AddStatement(statement2, LogialAnalyzerTrigger.Statement.LogicalOperators.First);
            statement4.AddStatement(statement3, LogialAnalyzerTrigger.Statement.LogicalOperators.And);
            var statement5 = new LogialAnalyzerTrigger.Statement.StatementTimeLargerThen(500);
            var foredstatement2 = new TriggerForkedState(statement4);
            foredstatement2.AddTriggerStateThenOperations(TriggerStateThenOperations.Trigger);
            var forkedStatemetn3 = new TriggerForkedState(statement5);
            forkedStatemetn3.AddTriggerStateThenOperations(TriggerStateThenOperations.GotoFirsState);
            var triggerstate2 = new TriggerState();
            triggerstate2.AddTriggerStateForkedStatement(foredstatement2);
            triggerstate2.AddTriggerStateForkedStatement(forkedStatemetn3);

            var trigger = new Trigger();
            trigger.AddTriggerState(triggerstate);
            trigger.AddTriggerState(triggerstate2);


            var engine = new TriggerEngine();
            engine.AddTrigger(trigger);
            engine.TriggeredUnitTest += engine_Triggered;

            //The time the first signal changes
            var firstSignalTimestamp = DateTime.Now.Ticks;

            // The trigger will jump to state two!
            engine.LogValueChange(signal, 0, firstSignalTimestamp);
            Assert.IsFalse(_test.WaitOne(10));
          
            // Signal 2 will change 600 ms after signal1
            engine.LogValueChange(signal2, 1, firstSignalTimestamp + 200);
            Assert.IsFalse(_test.WaitOne(10));
            engine.LogValueChange(signal2, 2, firstSignalTimestamp + 600);
            Assert.IsFalse(_test.WaitOne(10));

            // The trigger will jump back to state1 since the time excseeded 500ms
            engine.LogValueChange(signal, 2, firstSignalTimestamp);
            Assert.IsFalse(_test.WaitOne(10));

            //New time for the signal1 signalchange
            firstSignalTimestamp = DateTime.Now.Ticks;
            engine.LogValueChange(signal, 1, firstSignalTimestamp);
            Assert.IsFalse(_test.WaitOne(10));

            //New time for the signal1 signalchange //Will Jump to state2
            firstSignalTimestamp = DateTime.Now.Ticks;
            engine.LogValueChange(signal, 0, firstSignalTimestamp);
            Assert.IsFalse(_test.WaitOne(10));

            // Signal 2 will change 200 ms after signal1
            engine.LogValueChange(signal2, 2, firstSignalTimestamp + 400);

            Assert.IsTrue(_test.WaitOne(10));
        }

        void engine_Triggered(object sender, EventArgs e)
        {
            _test.Set();

        }
    }
}
