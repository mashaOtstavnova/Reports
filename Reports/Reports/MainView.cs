using System;
using System.Data;
using System.Windows.Forms;
using Core.Services.Implimintation;
using DevExpress.XtraEditors;
using Reports.Controls;

namespace Reports
{
    public partial class MainView : XtraForm
    {
        private DataTable _dataTable;
        private GridDataTable _grid;
        private SecondView _secondView;
        public MainView(DataTable dt)
        {
            _dataTable = dt;
            InitializeComponent();
            Init();
           
        }

        private void Init()
        {
            _secondView = new SecondView();
           
            _grid = new GridDataTable(_dataTable);
            //инит NavBar
            foreach (var item in _secondView.ListReports)
            {
                var ni = new DevExpress.XtraNavBar.NavBarItem(item) { Tag = item };
                //ni.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(MainForm_LinkClicked);

                navBarGroup1.ItemLinks.Add(ni);
            }
        }
        private void MainView_Load(object sender, EventArgs e)
        {
            _grid.Size = splitContainerControl1.Panel2.Size;
            splitContainerControl1.Panel2.Controls.Add(_grid);
        }

        private void MainView_SizeChanged(object sender, EventArgs e)
        {
            _grid.Size = splitContainerControl1.Panel2.Size;
        }

        private void createExcel_Click(object sender, EventArgs e)
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
