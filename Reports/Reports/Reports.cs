using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraGrid.Columns;

namespace Reports
{
    public partial class Reports : Form
    {
        private DataTable _dataTable;
        public Reports(DataTable dt)
        {
            _dataTable = dt;
            InitializeComponent();
        }

        private void gridControl1_Load(object sender, EventArgs e)
        {
            gridControl1.DataSource = _dataTable;
        }
    }
}
