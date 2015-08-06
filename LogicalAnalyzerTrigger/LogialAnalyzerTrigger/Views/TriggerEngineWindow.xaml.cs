using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LogialAnalyzerTrigger.Views
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class TriggerEngineWindow : Window
	{
		private TriggerEngineVm _triggerEngineVm;

		public TriggerEngineWindow()
		{
			InitializeComponent();
		}

		internal void Databind(TriggerEngineVm triggerEngineVm)
		{
			_triggerEngineVm = triggerEngineVm;
			DataContext = _triggerEngineVm;
		}

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
	}
}
