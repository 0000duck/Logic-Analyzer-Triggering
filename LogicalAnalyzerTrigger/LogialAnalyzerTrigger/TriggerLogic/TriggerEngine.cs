using ABB.Robotics.Paint.RobView.Database.SignalLog;
using ABB.Robotics.Paint.RobView.Plugin.SignalAnalyzer.Logger.Trigger;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace LogialAnalyzerTrigger
{
    /// <summary>
    /// TriggerEngine
    /// </summary>
    public class TriggerEngine : ITriggerEngine
    {
    
        
        //Event For UnitTests
        public event EventHandler TriggeredUnitTest;

        /// <summary>
        /// Occurs when Trigger triggers on a signal change
        /// Sends an TriggerEventArgs which contains a timestamp and name of trigger
        /// </summary>
        public event EventHandler<TriggerEventArgs> Triggered;


        private IEnumerable<Signal> _signalsList;
        private List<Trigger> _triggerList;

        public TriggerEngine()
        {
            _triggerList = new List<Trigger>();
            
        }

        internal List<Trigger> TriggerList
        {
            get { return _triggerList; }
        }

        public void AddTrigger(Trigger trigger)
        {
            _triggerList.Add(trigger);
        }

        public void RemoveTrigger(Trigger trigger)
        {
            _triggerList.Remove(trigger);
        }


        /// <summary>
        /// Opens the GUI.
        /// </summary>
        /// <param name="signals"></param>
        public void ShowConfigDialog(IEnumerable<Signal> signals)
        {
            _signalsList = signals;
            var triggerEngineVm = new TriggerEngineVm(this, signals);
            var mainWindow = new Views.TriggerEngineWindow();
			mainWindow.Databind(triggerEngineVm);
            mainWindow.ShowDialog();
        }

        private void FireTriggered(string name)
        {
            if (TriggeredUnitTest != null)
            {
                TriggeredUnitTest(this, new EventArgs());
            }
            if (Triggered != null)
            {
                Triggered(this, new TriggerEventArgs(name, DateTime.Now));
            }
        }

        /// <summary>
        /// Call this method when a signal change occurs
        /// </summary>
        /// <param name="signal">Signal Object</param>
        /// <param name="newValue">Value of the signal change, Can be a bool or double</param>
        /// <param name="timeStamp">Timestamp of the signal change of type Ticks</param>
        public void LogValueChange(ABB.Robotics.Paint.RobView.Database.SignalLog.Signal signal, object newValue, long timeStamp)
        {
            //Ser ut som Signalene bytter metadata underveis, workaround
          // var findsignal = _signalsList.First(x => x.Name == signal.Name);
            Signal findsignal;

           if (TriggeredUnitTest != null)
           {
               findsignal = signal;
           }
           else
           {
               findsignal = _signalsList.First(x => x.Name == signal.Name);
           }

          //  var findsignal = signal;
            if (newValue == null || !IsUsingSignal(findsignal))
            {
                return;
            }

            SignalValueTimestamp signalHandler;
          
            try
            {
                var value = Convert.ToDouble(newValue);


                signalHandler = new SignalValueTimestamp(findsignal, value, timeStamp);

                foreach (Trigger trigger in _triggerList)
                {
                    //var triggerd = trigger.CheckTrigger(signal, newValue);

                    if (trigger.IsUsingSignal(findsignal))
                    {
                        if (trigger.CheckTrigger(signalHandler))
                        {
                            FireTriggered(trigger.Name);
                        }
                    }
                }
            }
            catch (FormatException ex)
            {
                Console.WriteLine(ex);
                //ABB.Robotics.Paint.RobView.PluginAPI.Logger.Internal.LogException(ex);
            }
            catch (InvalidCastException ex)
            {
                Console.WriteLine(ex);
                //ABB.Robotics.Paint.RobView.PluginAPI.Logger.Internal.LogException(ex);
            }
            
        }

        private bool IsUsingSignal(Signal signal)
        {
            foreach (Trigger trigger in _triggerList)
            {
                if (trigger.IsUsingSignal(signal))
                    return true;
            }
            return false;
        }
    }
}


