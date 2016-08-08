using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace Reports.Controls
{
    public partial class Header : DevExpress.XtraEditors.XtraUserControl
    {
        public Header()
        {
            InitializeComponent();
        }

        public void Print(string dateFrom, string dateTo)
        {
            labelDateTo.Text = dateTo;
            lableDateFrom.Text = dateFrom;
        }
    }
}
