using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogialAnalyzerTrigger
{
	public class TriggerStateMachineVm : BindableBase
	{
		private Trigger _triggerStateMachine;
		public TriggerStateMachineVm(Trigger triggerStateMachine)
		{
			_triggerStateMachine = triggerStateMachine;
		}

		public string TriggerName
		{
			get { return _triggerStateMachine.Name; }
			set
			{
				_triggerStateMachine.Name = value;
				OnPropertyChanged("TriggerName");
			}
		}

		public List<SignalHandler> SignalHandlers
		{

		}

	}
}
