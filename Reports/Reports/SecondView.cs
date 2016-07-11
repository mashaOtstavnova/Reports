using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reports
{
    public class SecondView
    {
        private List<string> _listReports;

        public List<string> ListReports
        {
            get
            {
                return _listReports;
            }
            set
            {
                _listReports = value;
            }
        }

        public SecondView()
        {
            ListReports = new List<string>() { "Отчет 1", "Отчет 2" };
        }
    }
}
