using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.LookAndFeel;

namespace Reports
{
    public class FirstView
    {
        private string _style;
        public string Style
        {
            get
            {
                return _style;
            }
            set
            {
                UserLookAndFeel.Default.SetSkinStyle(value);
                _style = value;
            }
        }

        public FirstView()
        {
            _style = "Dark Side";
            Style = _style;
        }
    }
}
