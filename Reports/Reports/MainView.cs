using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Core.Domain;
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
        private readonly ProgressPanel _progressPanel = new ProgressPanel();
        private GridDataTable _grid;
        private Header _header;
        private ReportParameters _reportParameters;
        public MainView(/*DataTable dt*/)
        {
            InitializeComponent();
            CoreContext.ViewService.Init(this);
            Init();
        }

        public void ShowMessag(string msg)
        {
            MessageBox.Show(msg);
        }

        void IMainView.Init()
        {
            Init();
        }

        public async void GetTransaction()
        {
            _progressPanel.Visible = true;
            Application.DoEvents();
            Log.Inst.WriteToLogDEBUG("Start thread for get data");
            _dataTable = await NewThreadForTranzaction();
            CoreContext.ViewService.MainView.PaintTable();
            Log.Inst.WriteToLogDEBUG("End thread for get data");
            Log.Inst.WriteToLogDEBUG("Show gridView");
            _progressPanel.Visible = false;
            
        }
        private async Task<DataTable> NewThreadForTranzaction()
        {
            return await Task.Run(() =>
            {
                var transactionReportItemParametrs = new TransactionReportItemParametrs(_reportParameters.OrganizationInfoId, _reportParameters.DateFrom,
                   _reportParameters.DateTo);
                var result =
                    CoreContext.MakerRequest.GetTransactionsReportItems(transactionReportItemParametrs);
                var dt = CoreContext.BuildTableAndSaveExcel.BuiltTable(result.Result);
                return dt;
            });
        }
        private void Init()
        {
            this.Text = string.Format("Отчетность по корпоративному питанию версия {0}", CoreContext.ConfigService.GetConfig().Version);
            Log.Inst.WriteToLogDEBUG(string.Format("Initialize Component {0}", Name));
            _dataTable = new DataTable();
            _secondView = new SecondView();
            _reportParameters = new ReportParameters();

            _grid = new GridDataTable();
            _grid.Size = splitContainerControl1.Panel2.Size;
            splitContainerControl1.Panel2.Controls.Add(_grid);

            _header = new Header();
            _header.Size = this.panelHeader.Size;
            this.panelHeader.Controls.Add(_header);
           
            var today = DateTime.Today.Date;
            var yesterday = DateTime.Today.Date.Subtract(new TimeSpan(1, 0, 0, 0));//Вычитаем день
            dateTimeFrom.MaxDate = yesterday;
            dateTimeTo.MaxDate = today;
            dateTimeFrom.Value = yesterday;
            dateTimeTo.Value = today;
        }

        public void PaintTable()
        {
            _header.Print(_reportParameters.DateFrom.Date.ToString("yyyy-MM-dd"), _reportParameters.DateTo.Date.ToString("yyyy-MM-dd"));
            _grid.PrintDT(_dataTable);
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
            if(_grid!=null)
                _grid.Size = splitContainerControl1.Panel2.Size;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _reportParameters.DateFrom = dateTimeFrom.Value.Date;
            _reportParameters.DateTo = dateTimeTo.Value.Date;
            CoreContext.ViewService.MainView.GetTransaction();
        }

        private void MainView_Shown(object sender, EventArgs e)
        {
            CoreContext.ViewService.MainView.GetTransaction();
        }

        private void createExcel_Click_1(object sender, EventArgs e)
        {
            CoreContext.ViewService.MainView.SaveExcel();
        }
    }
}
