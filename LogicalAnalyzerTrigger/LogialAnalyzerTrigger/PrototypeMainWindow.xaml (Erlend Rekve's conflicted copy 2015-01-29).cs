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

namespace LogialAnalyzerTrigger
{
    /// <summary>
    /// Interaction logic for PrototypeMainWindow.xaml
    /// </summary>
    public partial class PrototypeMainWindow : Window
    {
        private List<ITrigger> triggerList { get; set; }

        
        public PrototypeMainWindow()
        {
                    
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            triggerList = new List<ITrigger>();
        }  

        private void AddStatement(object sender, RoutedEventArgs e)
        {
            PrototypeAddStatement window = new PrototypeAddStatement(this);
            window.Show();
        }

        private void Run(object sender, RoutedEventArgs e)
        {
            TriggerEngine engine = new TriggerEngine(triggerList);
            engine.TestData();
            this.Close();
        }

        public void createTrigger(string signal, string op, int value, AlgerbraOperator nextOperator)
        {
            ITrigger trigger;

            switch (op)
            {
                case "=":
                    trigger = new TriggerEquals(signal, value, nextOperator);
                    break;
                case ">":
                    trigger = new TriggerGreaterThen(signal, value, nextOperator);
                    break;
                case "<":
                    trigger = new TriggerLessThen(signal, value, nextOperator);
                    break;
                default:
                    throw new Exception();
            }
            
            triggerList.Add(trigger);
        }
    }
}
