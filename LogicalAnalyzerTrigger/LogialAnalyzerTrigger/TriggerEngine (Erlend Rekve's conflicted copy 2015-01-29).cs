using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogialAnalyzerTrigger
{
   
   public class TriggerEngine
    {
       public event EventHandler Triggered;
       
       ITrigger trigger;
       List<ITrigger> triggerList;

       public TriggerEngine(ITrigger trigger)
       {
           this.trigger = trigger;
       }

       public TriggerEngine(List<ITrigger> triggerList)
       {
           this.triggerList = triggerList;
       }


        public void GiveData(int p)
        {

            trigger.HandleTrigger(p);

            if(trigger.State)
            {
                if (Triggered != null)
                    Triggered(this, new EventArgs());
            }
        }

        public void GiveData2(int p)
        {
            if (checkList(p))
            {
                if (Triggered != null)
                    Triggered(this, new EventArgs());
            }

            foreach (ITrigger IT in triggerList)
            {
                IT.State = false;
            }
        }

        public bool checkList(int p)
        {
            bool state = false;

            foreach (ITrigger IT in triggerList)
            {
                IT.HandleTrigger(p);
                switch (IT.NextOperator)
                {
                    case AlgerbraOperator.First:
                        state = IT.State;
                        break;
                    case AlgerbraOperator.And:
                        if (IT.State && state)
                            state = true;
                        else
                            state = false;
                        break;
                    case AlgerbraOperator.Or:
                        if (IT.State || state)
                            state = true;
                        else
                            state = false;
                        break;
                    default:
                        break;
                }
            }
            return state;
        }
        public void TestData()
        {
            //Reads from SignalData.txt in /bin/debug
            string line;
            int number;
            var currentDir = Directory.GetCurrentDirectory();
            System.IO.StreamReader file = new System.IO.StreamReader(currentDir + "\\SignalData.txt");
            while ((line = file.ReadLine()) != null)
            {
                number = Convert.ToInt32(line);
                if (checkList(number))
                {
                    Console.WriteLine("Triggered on:");
                    break;
                }
                
            }
            file.Close();
        }
   }
}
