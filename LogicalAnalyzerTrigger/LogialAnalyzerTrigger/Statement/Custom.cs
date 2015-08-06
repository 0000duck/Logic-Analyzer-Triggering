using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogialAnalyzerTrigger.Statement
{
    public class Custom : Statement
    {
        private List<Statement> _triggerList;

        private string _name;


        public Custom(List<Statement> TriggerList, AlgerbraOperator nextOperator)
            : base(TriggerList[0].SignalHandler, nextOperator)
        {
            this._name = "Name not set";
            this._triggerList = TriggerList;

            //Virker tungvint, kanskje bedre løsning? Fant bedre løsning bruk UNION, tydeligvis tregt. Dict
            foreach (Statement trigger in _triggerList)
            {
                base.SignalHandlersUsedByTrigger = base.SignalHandlersUsedByTrigger.Union(trigger.SignalHandlersUsedByTrigger).ToList();
            }
        }

        public string Name
        {
            get { return this._name; }
            set { this._name = value; }
        }

        public override void CheckStatement()
        {
            foreach (Statement trigger in _triggerList)
            {
                trigger.CheckStatement();

                switch (trigger.LogicalOperator)
                {
                    case AlgerbraOperator.First:
                        base.State = trigger.State;
                        break;
                    case AlgerbraOperator.And:
                        if (base.State && trigger.State)
                            base.State = true;
                        else
                            base.State = false;
                        break;
                    case AlgerbraOperator.Or:
                        if (base.State || trigger.State)
                            base.State = true;
                        else
                            base.State = false;
                        break;
                    default:
                        break;
                }
            }
        }

        public override string ToString()
        {
            string writeString = "";
            switch (base.LogicalOperator)
            {
                case AlgerbraOperator.And:
                    writeString += "And ";
                    break;
                case AlgerbraOperator.Or:
                    writeString += "Or ";
                    break;
                default:
                    break;
            }
            writeString += "(";
            foreach (Statement trigger in _triggerList)
            {
                writeString += trigger.ToString();
            }
            writeString += ")";
            return writeString;
        }
    }
}
