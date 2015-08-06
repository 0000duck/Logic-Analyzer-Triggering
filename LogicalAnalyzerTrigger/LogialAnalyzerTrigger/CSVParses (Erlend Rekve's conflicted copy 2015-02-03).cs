using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using System.IO;

namespace LogialAnalyzerTrigger
{

    class CSVParser
    {
        public void test()
        {

            var currentDir = Directory.GetCurrentDirectory();
            var dict = new Dictionary<Signal, List<DateTime>>();
            List<DateTime> tmpList = new List<DateTime>();;
            DateTime tmpDateTime;
            Signal sig;

            TextFieldParser parser = new TextFieldParser(currentDir + "\\Pump1Fault1.csv");
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(",");



            while (!parser.EndOfData)
            {

                string[] fields = parser.ReadFields();
                if (fields[0].Contains("DisplayName"))
                {
                    sig = toSignal(fields);
                    tmpList = new List<DateTime>();
                    dict.Add(sig, tmpList);
                }
                tmpList.Add(toDateTime(fields));
            }

            foreach (KeyValuePair<Signal, List<DateTime>> kvp in dict)
            {
                Console.WriteLine(kvp.Key.Name + kvp.Value[0].ToString());
            }
  
            parser.Close();
        }

        private DateTime toDateTime(string[] fields)
        {
          
            int day, month, year, hour, minute, second, millisecond;
            string[] split, split2;

            split = fields[0].Split(':');
            split2 = split[2].Split(' ');

            //day = Convert.ToInt32(split[0].Trim());
            //month = Convert.ToInt32(split[1].Trim());
            //year = Convert.ToInt32(split2[0]);
            //hour = Convert.ToInt32(split2[1]);
            //minute = Convert.ToInt32(split[3]);
            //day = Convert.ToInt32(split[4]);
            //second = Convert.ToInt32(split[5]);
            //millisecond = Convert.ToInt32(split[6]);

            //DateTime dateTime = new DateTime(year, month, day, hour, minute, second, millisecond);
            Console.WriteLine(split[0]);
            return DateTime.Now;
        }

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
    }
}
