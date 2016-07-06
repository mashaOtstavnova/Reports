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
    public partial class GridDataTable :UserControl
    {
        public GridDataTable(DataTable dt)
        {
            InitializeComponent();
            this.gridControl1.DataSource = dt;
        }
    }
}
