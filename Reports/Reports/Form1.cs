using System;
using System.Collections.Generic;
using System.Linq;
using Core.Services.Implimintation;
using System.Windows.Forms;
using System.Net.Http;
using Core.Domain;
using IikoBizApi;

namespace Reports
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        private IikoBizToken _apiAccessToken;
        private string idOrg;
        private OrganizationInfo[] ListOrg;
        public Form1()
        {
            var settings = new Settings("vankorAPI", "JUe5J4cuWu", "https://iiko.biz:9900/api/0/", 60000);
            CoreContext.InitService.Init(settings);
            idOrg = CoreContext.BizApiClient.GetOrganizationInfo().Result.First().Id;
            ListOrg = CoreContext.BizApiClient.GetOrganizationInfo().Result;
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
            var list = CoreContext.BizApiClient.GetCorporateNutritionInfo(idOrg).Result;
            //var thisresult = list.Where(t => t.Name == "АвтоСтройСервис (Ахат)").First();
            var thisresult = list.First();
            //var resylt = CoreContext.BizApiClient.GetCorporateNutritionReportItem(idOrg, thisresult.Id);
            //var result = new CorporateNutritionReportItem[2]
            //{
            //    new CorporateNutritionReportItem()
            //    {
            //        BalanceOnPeriodEnd = (decimal)10.1,
            //        BalanceOnPeriodStart = (decimal)60.1,
            //        BalanceRefillSum = (decimal)5.5,
            //        BalanceResetSum = (decimal)5.4,
            //        EmployeeNumber = "10",
            //        GuestCardTrack = "11",
            //        GuestCategoryNames = "GuestCategoryNames",
            //        GuestId = "GuestId",
            //        GuestName = "GuestName",
            //        GuestPhone = "GuestPhone",
            //        PaidOrdersCount = (decimal)4.4,
            //        PayFromWalletSum = (decimal)1.4
            //    },
            //    new CorporateNutritionReportItem()
            //    {
            //        BalanceOnPeriodEnd = (decimal)10.1,
            //        BalanceOnPeriodStart = (decimal)60.1,
            //        BalanceRefillSum = (decimal)5.5,
            //        BalanceResetSum = (decimal)5.4,
            //        EmployeeNumber = "10",
            //        GuestCardTrack = "11",
            //        GuestCategoryNames = "GuestCategoryNames1",
            //        GuestId = "GuestId1",
            //        GuestName = "GuestName1",
            //        GuestPhone = "GuestPhone1",
            //        PaidOrdersCount = (decimal)4.4,
            //        PayFromWalletSum = (decimal)1.4
            //    }
            //};
            //listBox1.Items.Add(result);
        }
        private void button1_Click(object sender, System.EventArgs e)
        {

            //var corp = comboBox2.Text;
            var list = CoreContext.BizApiClient.GetCorporateNutritionInfo(idOrg).Result;
            //var idCor = list.Where(r => r.Name == comboBox2.Text).First().Id;
            var idCor = list.First().Id;
            //textBox2.Text = string.Format("Name: {0} Id {1}", corp, idCor);
            //var t = CoreContext.BizApiClient.GetCorporateNutritionReportItem(idOrg, idCor).Result;
            //listBox1.Items.Clear();
            //listBox1.Items.Add(CoreContext.BizApiClient.GetCorporateNutritionReportItem(idOrg, idCor).Result.Count());
            var result = CoreContext.BizApiClient.GetReportses(idOrg, idCor, dateTimeFrom.Value.Date, dateTimeTo.Value.AddDays(1).Date);
           
            var dt = CoreContext.BuildTable.BuiltTable(result);
            dt = CoreContext.BuildTable.BuiltTable(result);
            Reports re = new Reports(dt);
            re.ShowDialog();
        }

        private void comboBox1_SelectedIndexChanged(object sender, System.EventArgs e)
        {

            idOrg = ListOrg.Where(r => r.Name == comboBox1.Text).First().Id;
            var list = CoreContext.BizApiClient.GetCorporateNutritionInfo(idOrg).Result;
            foreach (var item in list)
            {
                comboBox2.Items.Add(item.Name);
            }
            var org = comboBox1.Text;
            textBox1.Text = string.Format("Name: {0} Id {1}", org, idOrg);
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
            var list = CoreContext.BizApiClient.GetCorporateNutritionInfo(idOrg).Result;
            //foreach (var item in list)
            //{
            //    comboBox2.Items.Add(item.Name);
            //}
            foreach (var item in ListOrg)
            {
                comboBox1.Items.Add(item.Name);
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            idOrg = ListOrg.Where(r => r.Name == comboBox1.Text).First().Id;
            var list = CoreContext.BizApiClient.GetCorporateNutritionInfo(idOrg).Result;
            var cor = list.Where(r => r.Name == comboBox2.Text).First(); ;
            textBox2.Text = string.Format("Name: {0} Id {1}", cor.Name, cor.Id);
        }
    }
}
