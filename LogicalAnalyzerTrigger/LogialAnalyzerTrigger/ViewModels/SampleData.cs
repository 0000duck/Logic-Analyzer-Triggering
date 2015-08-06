using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogialAnalyzerTrigger.ViewModels
{
	internal class SampleData
	{
		public static StatementVm DemoData
		{
			get { return new StatementVm(); }
		}

		public static TriggerVm TriggerVmDemoData { get { return new TriggerVm(); } }
	}
}
