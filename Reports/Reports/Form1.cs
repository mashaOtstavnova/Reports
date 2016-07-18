using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Core.Domain;
using Core.Services.Implimintation;
using Core.Views.FirstView;
using Core.Views.MainView;
using DevExpress.LookAndFeel;
using DevExpress.XtraEditors;
using IikoBizApi;
using Newtonsoft.Json;
using Reports.Controls;

namespace Reports
{
    public partial class Form1 : XtraForm, IFirstView
    {
        private readonly ProgressPanel _progressPanel = new ProgressPanel();
        private OrganizationInfo[] _listOrg;
        private IikoBizToken _apiAccessToken;
        private string _idOrg;
        private readonly FirstView _firstView;

        public Form1()
        {

            CoreContext.ViewService.Init(this);
            _firstView = new FirstView();
            InitializeComponent();
            CoreContext.ViewService.FirstView.Init();
           
        }

        private void textBox3_Layout(object sender, LayoutEventArgs e)
        {
            //restoCamp
            //9U6cZjDCG3zm9jj

            var settings = new Settings("vankorAPI", "JUe5J4cuWu", "https://iiko.biz:9900/api/0/", 60000);
            _apiAccessToken = new IikoBizToken(
                $"{settings.BaseAddress}auth/access_token?user_id={settings.Login}&user_secret={settings.Password}",
                new HttpClient());
            textBox3.Text = _apiAccessToken.Value;
        }

     
        private async Task<DataTable> newThread()
        {
            return await Task.Run(() =>
            {

                var list = CoreContext.MakerRequest.GetCorporateNutritionInfo(_idOrg).Result;
                //var idCor = list.Where(t => t.Name==comboBox2.Text).FirstOrDefault().Id;
                var reportParametrs = new ReportParameters(_idOrg, "c5cb34d5-eacd-11e5-80d8-d8d38565926f", dateTimeFrom.Value.Date,
                    dateTimeTo.Value.Date);
                //textBox2.Text = string.Format("Name: {0} Id {1}", corp, idCor);
                //var t = CoreContext.BizApiClient.GetCorporateNutritionReportItem(idOrg, idCor).Result;
                //listBox1.Items.Clear();
                //listBox1.Items.Add(CoreContext.BizApiClient.GetCorporateNutritionReportItem(idOrg, idCor).Result.Count());
                var result =
                    CoreContext.MakerRequest.GetReportses(reportParametrs);
                var dt = CoreContext.BuildTableAndSaveExcel.BuiltTable(result);
                dt = CoreContext.BuildTableAndSaveExcel.BuiltTable(result);
                return dt;
            });
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _idOrg = _listOrg.Where(r => r.Name == comboBox1.Text).First().Id;
            var list = CoreContext.MakerRequest.GetCorporateNutritionInfo(_idOrg).Result;
            foreach (var item in list)
            {
                comboBox2.Items.Add(item.Name);
            }
            var org = comboBox1.Text;
            textBox1.Text = string.Format("Name: {0} Id {1}", org, _idOrg);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var list = CoreContext.MakerRequest.GetCorporateNutritionInfo(_idOrg).Result;
            foreach (var item in _listOrg)
            {
                comboBox1.Items.Add(item.Name);
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            _idOrg = _listOrg.Where(r => r.Name == comboBox1.Text).First().Id;
            var list = CoreContext.MakerRequest.GetCorporateNutritionInfo(_idOrg).Result;
            var cor = list.Where(r => r.Name == comboBox2.Text).First();
            textBox2.Text = string.Format("Name: {0} Id {1}", cor.Name, cor.Id);
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            _firstView.Style = comboBox3.Text;
           
        }

        private async void button1_Click_1(object sender, EventArgs e)
        {
            
            CoreContext.ViewService.FirstView.GetReports();
        }

        public async void GetReports()
        {;
            _progressPanel.Visible = true;
            Application.DoEvents();
            
            var dt1 = await newThread();
            var re = new MainView(dt1);
            Log.Inst.WriteToLogDEBUG("Show gridView");

            _progressPanel.Visible = false;
            re.ShowDialog();
        }

        public void ShowMessag(string msg)
        {
            MessageBox.Show(msg);
        }

        public void Init()
        {
            Log.Inst.WriteToLogDEBUG(string.Format("Initialize Component {0}", Name));
            //CoreContext.ViewService.FirstView.ShowMessag("При первом запуске в качестве организации берется первая из списка.");
            _idOrg = CoreContext.MakerRequest.GetOrganizationInfo().Result.First().Id;
            _listOrg = CoreContext.MakerRequest.GetOrganizationInfo().Result;
            var today = DateTime.Today.Date;
            var yesterday = DateTime.Today.Date.Subtract(new TimeSpan(1, 0, 0, 0));//Вычитаем день
            dateTimeFrom.MaxDate = yesterday;
            dateTimeTo.MaxDate = today;
            dateTimeFrom.Value = yesterday;
            dateTimeTo.Value = today;
           
        }

        private void dateTimeTo_ValueChanged(object sender, EventArgs e)
        {
            if (dateTimeTo.Value <= dateTimeFrom.Value)
            {
                CoreContext.ViewService.FirstView.ShowMessag("Ошибка даты!");
                dateTimeTo.Value = dateTimeFrom.Value.AddDays(1);
            }
        }
    }
}