using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;


namespace LogialAnalyzerTrigger.ViewModels
{
    class StatementVm2 : BindableBase
    {

        public CollectionView Queries { get; private set; }

        void queries_CurrentChanged(object sender, EventArgs e)
        {
            string currentQuery = (string)Queries.CurrentItem;
        }

        public StatementVm2()
        {
            IList<string> availableQueries = new List<string>();
            // fill the list...
            availableQueries.Add("test;");

            Queries = new CollectionView(availableQueries);
            Queries.MoveCurrentTo(availableQueries[0]);
            Queries.CurrentChanged += new EventHandler(queries_CurrentChanged);
        }
    }
}
