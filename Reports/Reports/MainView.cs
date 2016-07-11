using System;
using System.Data;
using System.Windows.Forms;
using Core.Services.Implimintation;
using Core.Views.MainView;
using DevExpress.XtraEditors;
using Reports.Controls;

namespace Reports
{
    public partial class MainView : XtraForm, IMainView
    {
        private DataTable _dataTable;
        private SecondView _secondView;
        private GridDataTable _grid;
        public MainView(DataTable dt)
        {
            _dataTable = dt;
            CoreContext.ViewService.Init(this);
            InitializeComponent();
            Init();
        }
      
        private void Init()
        {
            _secondView = new SecondView();

            CoreContext.ViewService.MainView.PaintTable(_dataTable);
            //инит NavBar
            foreach (var item in _secondView.ListReports)
            {
                var ni = new DevExpress.XtraNavBar.NavBarItem(item) { Tag = item };
                //ni.LinkClicked += new DevExpress.XtraNavBar.NavBarLinkEventHandler(MainForm_LinkClicked);

                navBarGroup1.ItemLinks.Add(ni);
            }
        }

        private void createExcel_Click(object sender, EventArgs e)
        {
            CoreContext.ViewService.MainView.SaveExcel();
        }

        public void PaintTable(DataTable dt)
        {
            _grid = new GridDataTable(dt);
            _grid.Size = splitContainerControl1.Panel2.Size;
            splitContainerControl1.Panel2.Controls.Add(_grid);
        }

        public void SaveExcel()
        {
            var save = new SaveFileDialog();
            save.Filter = "xls files (*.xls)|*.xls|All files|*.*";
            if (save.ShowDialog() == DialogResult.OK)
            {
                //Собсвенно вот тут и передаем DataSet в наш метод который формирует Excel-документ
                CoreContext.BuildTableAndSaveExcel.SaveExel(_dataTable, save.FileName);
            }
        }

        private void MainView_SizeChanged_1(object sender, EventArgs e)
        {
            _grid.Size = splitContainerControl1.Panel2.Size;
        }
    }
}
