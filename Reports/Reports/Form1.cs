using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Core.Domain;
using Core.Services.Implimintation;
using DevExpress.LookAndFeel;
using DevExpress.XtraEditors;
using IikoBizApi;
using Newtonsoft.Json;
using Reports.Controls;

namespace Reports
{
    public partial class Form1 : XtraForm
    {
        private readonly ProgressPanel _progressPanel = new ProgressPanel();
        private readonly OrganizationInfo[] ListOrg;
        private IikoBizToken _apiAccessToken;
        private string idOrg;

        public Form1()
        {
            UserLookAndFeel.Default.SetSkinStyle("Dark Side");
            Log.Inst.WriteToLogDEBUG(string.Format("Initialize Component {0}", Name));
            idOrg = CoreContext.MakerRequest.GetOrganizationInfo().Result.First().Id;
            ListOrg = CoreContext.MakerRequest.GetOrganizationInfo().Result;
            InitializeComponent();
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

        private void listBox1_Layout(object sender, LayoutEventArgs e)
        {
            idOrg = ListOrg.First().Id;
            var list = CoreContext.MakerRequest.GetCorporateNutritionInfo(idOrg).Result;
            //var thisresult = list.Where(t => t.Name == "АвтоСтройСервис (Ахат)").First();
            var thisresult = list.First();
            //var resylt = CoreContext.BizApiClient.GetCorporateNutritionReportItem(idOrg, thisresult.Id);

            //listBox1.Items.Add(result);
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            _progressPanel.Visible = true;
            Application.DoEvents();
            //_progressPanel.Show();
            //var corp = comboBox2.Text;
            var p = Path.Combine(Environment.CurrentDirectory, @"logs\\start.log"
               + string.Format("-{0:yyyy}-{0:MM}-{0:dd}", DateTime.Now));
            
            var dt1 = await newThread();
            var re = new MainView(dt1);
            Log.Inst.WriteToLogDEBUG("Show gridView");
            
            _progressPanel.Visible = false;
            re.ShowDialog();
        }

        public async Task<DataTable> newThread()
        {
            return await Task.Run(() =>
            {

                var list = CoreContext.MakerRequest.GetCorporateNutritionInfo(idOrg).Result;
                //var idCor = list.Where(r => r.Name == comboBox2.Text).First().Id;
                var idCor = list.First().Id;
                var reportParametrs = new ReportParameters(idOrg, idCor, dateTimeFrom.Value.Date,
                    dateTimeTo.Value.AddDays(1).Date);
                reportParametrs.WriteToFile();
                //textBox2.Text = string.Format("Name: {0} Id {1}", corp, idCor);
                //var t = CoreContext.BizApiClient.GetCorporateNutritionReportItem(idOrg, idCor).Result;
                //listBox1.Items.Clear();
                //listBox1.Items.Add(CoreContext.BizApiClient.GetCorporateNutritionReportItem(idOrg, idCor).Result.Count());
                var result =
                    CoreContext.MakerRequest.GetReportses(reportParametrs);
                var dt = CoreContext.BuildTable.BuiltTable(result);
                dt = CoreContext.BuildTable.BuiltTable(result);
                return dt;
            });
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            idOrg = ListOrg.Where(r => r.Name == comboBox1.Text).First().Id;
            var list = CoreContext.MakerRequest.GetCorporateNutritionInfo(idOrg).Result;
            foreach (var item in list)
            {
                comboBox2.Items.Add(item.Name);
            }
            var org = comboBox1.Text;
            textBox1.Text = string.Format("Name: {0} Id {1}", org, idOrg);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var list = CoreContext.MakerRequest.GetCorporateNutritionInfo(idOrg).Result;
            foreach (var item in ListOrg)
            {
                comboBox1.Items.Add(item.Name);
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            idOrg = ListOrg.Where(r => r.Name == comboBox1.Text).First().Id;
            var list = CoreContext.MakerRequest.GetCorporateNutritionInfo(idOrg).Result;
            var cor = list.Where(r => r.Name == comboBox2.Text).First();
            textBox2.Text = string.Format("Name: {0} Id {1}", cor.Name, cor.Id);
        }
    }
}