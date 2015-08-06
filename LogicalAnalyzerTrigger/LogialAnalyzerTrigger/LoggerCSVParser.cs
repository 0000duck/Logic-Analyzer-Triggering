using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using System.IO;
using ABB.Robotics.Paint.RobView.Database.SignalLog;

namespace LogialAnalyzerTrigger
{

    class CSVParser
    {
        private string URL { get; set; }
        private Dictionary<Signal, List<SignalValues>> dict { get; set; }

        private List<Signal> signalList { get; set; }


        public Dictionary<Signal, List<SignalValues>> Dict
        {
            get { return this.dict; }
        }

        public List<Signal> SignalList
        {
            get { return this.signalList; }
        }

        public CSVParser(string url)
        {
            this.URL = url;
            ParseFile();
        }

        public DateTime getLowestTick()
        {
            DateTime lowest = DateTime.Now;
            foreach (KeyValuePair<Signal, List<SignalValues>> kvp in dict)
            {
                if(kvp.Value.Count > 1)
                {
                    if (kvp.Value[0].Time.Ticks < lowest.Ticks)
                    {
                        lowest = kvp.Value[0].Time;
                    }
                }
                
            }
            return lowest;
        }

        private void ParseFile()
        {

           
            this.dict = new Dictionary<Signal, List<SignalValues>>();
            List<SignalValues> tmpList = new List<SignalValues>();;
            Signal sig;
            signalList = new List<Signal>();

            TextFieldParser parser = new TextFieldParser(this.URL);
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(",");



            while (!parser.EndOfData)
            {

                string[] fields = parser.ReadFields();
                // Removes Empty Fields
                fields = fields.Where(x => !string.IsNullOrEmpty(x)).ToArray();

                if (fields[0].Contains("DisplayName"))
                {
                    sig = toSignal(fields);
                    signalList.Add(sig);
                    tmpList = new List<SignalValues>();
                    dict.Add(sig, tmpList);
                }
                else
                {

                    tmpList.Add(toSignalValues(fields));
                }
                
            }
            parser.Close();

            

            //foreach (KeyValuePair<Signal, List<SignalValues>> kvp in dict)
            //{
            //    Console.WriteLine(kvp.Key.Name + "||" + kvp.Value.Count);
            //}
        }


       /// <summary>
       ///  Converts a appropiate string to a DateTime Object. 
       /// format of string = "DD:MM:YYYY HH:MM:SS:MS"
       /// </summary>
       /// <param name="field"></param>
       /// <returns>DateTime Object if succsessfull, NULL if not</returns>
        private DateTime toDateTime(string field)
        {
            string[] split = field.Split(':');
            string[] split2 = split[2].Split(' ');
            try
            {
                return new DateTime(toInt(split2[0]), toInt(split[1]), toInt(split[0]), toInt(split2[1]), toInt(split[3]), toInt(split[4]), toInt(split[5]));
            }
            catch
            {
                throw new FormatException();
            }    
        }

        private int toInt(string field)
        {
            try
            {
                int value = Convert.ToInt32(field);
                return value;
            }
            catch
            {
                throw new FormatException("Could not convert to int");
            }
        }

        /// <summary>
        /// Converts a appropiate array of strings to a Signal
        /// Format of array:
        /// [0]DisplayName: ObjectModel.can1:mac54/iDrive.Accu@IPS_10.183.155.184
        /// [1]SignalName: ObjectModel.can1:mac54/iDrive.Signal.Accu@IPS_10.183.155.184
        /// [2]Source: RC06
        /// [3]Unit: rev
        /// [4]SignalType: Analog
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        private Signal toSignal(string[] fields)
        {
            string[] split;
            Dictionary<string, string> dict = new Dictionary<string, string>();


            foreach (string field in fields)
            {

                split = field.Split(new String[] { ": " }, 2, StringSplitOptions.RemoveEmptyEntries);

                if (split.Length < 2)
                {
                    dict.Add(split[0].Replace(":", string.Empty), "Not set");
                } else
                    dict.Add(split[0], split[1]);

            }

            SignalType type;
            switch(dict["SignalType"])
            {
                case "Analog":
                    type = SignalType.Analog;
                    break;
                case "Digital":
                    type = SignalType.Digital;
                    break;
                default:
                    type = SignalType.Analog;
                    break;
            }
            return Signal.Create(dict["DisplayName"], dict["SignalName"], null, null, -1, dict["Unit"], type);
        }

        private SignalValues toSignalValues(string[] fields)
        {

            object newValue = fields[1];
            DateTime ticks = toDateTime(fields[0]);
            return new SignalValues(ticks, newValue);
        }
    }

    public class SignalValues
    {
        private DateTime ticks { get; set; }
        private object newValue { get; set; }

        public DateTime Time
        {
            get { return this.ticks; }
        }

        public object NewValue
        {
            get { return this.newValue; }
        }

        public SignalValues(DateTime ticks, object newValue)
        {
            this.ticks = ticks;
            this.newValue = newValue;
        }


        public override string ToString()
        {
            return "Ticks: " + ticks + " NewValue: " + newValue.ToString();
        }
    }
}

