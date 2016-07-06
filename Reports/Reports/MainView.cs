using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Core.Services.Implimintation;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using Reports.Controls;

namespace Reports
{
    public partial class MainView : Form
    {
        private DataTable _dataTable;
        public MainView(DataTable dt)
        {
            _dataTable = dt;
            InitializeComponent();
        }

        private void MainView_Load(object sender, EventArgs e)
        {
            var grid = new GridDataTable(_dataTable);
            splitContainerControl1.Panel2.Controls.Add(grid);
        }

        private void createExel_Click(object sender, EventArgs e)
        {
            var save = new SaveFileDialog();
            save.Filter = "xls files (*.xls)|*.xls|All files|*.*";
            if (save.ShowDialog() == DialogResult.OK)
            {
                //Собсвенно вот тут и передаем DataSet в наш метод который формирует Excel-документ
                CoreContext.BuildTable.SaveExel(_dataTable, save.FileName);
            }
        }
    }
}
