
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogialAnalyzerTrigger.ViewModels
{
    public class ThenOperationDataModel : BindableBase
    {
        private string _thenOperationString;

        public ThenOperationDataModel()
        {

        }

        public List<string> AvalibleThenOperations
        {
            get { return Enum.GetNames(typeof(TriggerStateThenOperations)).ToList(); }
        }

        public string ThenOperationString
        {
            get { return _thenOperationString; }
            set { SetProperty(ref _thenOperationString, value); }
        }

        public TriggerStateThenOperations ThenOperation
        {
            get
            {
                // The defalut is trigger
                TriggerStateThenOperations thenoperation = TriggerStateThenOperations.Trigger;
                switch (_thenOperationString)
                {
                    case "GotoNextState":
                        thenoperation = TriggerStateThenOperations.GotoNextState;
                        break;
                    case "GotoFirsState":
                        thenoperation = TriggerStateThenOperations.GotoFirsState;
                        break;
                    case "StartTimer":
                        thenoperation = TriggerStateThenOperations.StartTimer;
                        break;
                    default:
                        break;
                }
                return thenoperation;
            }
        }

    }
}
