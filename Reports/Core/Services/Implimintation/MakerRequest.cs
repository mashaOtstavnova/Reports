using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core.Domain;
using Microsoft.SqlServer.Server;
using Reports;

namespace Core.Services.Implimintation
{
    public class MakerRequest: IMakerRequest
    {

        private const string Format = "yyyy-MM-dd";
        public async Task<OrganizationInfo[]> GetOrganizationInfo()
        {
            Log.Inst.WriteToLogDEBUG(string.Format("Get OrganizationInfo"));
            /// api / 0 / organization / list ? access_token ={ accessToken}&request_timeout ={ requestTimeout}
            return await CoreContext.BizApiClient.GetAsync<OrganizationInfo[]>("/organization/list", new
            {
                request_timeout = 60000
            }).ConfigureAwait(false);
        }

        public async Task<CorporateNutritionInfo[]> GetCorporateNutritionInfo(string organizationId)
        {
            Log.Inst.WriteToLogDEBUG(string.Format("Get CorporateNutritionInfo"));
            ////api/0/organization/{organizationId}/corporate_nutritions?access_token={accessToken}
            var idOrg = CoreContext.MakerRequest.GetOrganizationInfo().Result[0].Id;
            
            return
                await
                    CoreContext.BizApiClient.GetAsync<CorporateNutritionInfo[]>(
                        (string.Format("/organization/{0}/corporate_nutritions", idOrg)), new
                        {
                        }).ConfigureAwait(false);
        }

        /// <summary>
        /// Получаем данные с запроса на отчет и переводим эти данные в удобный нам формат Reports
        /// </summary>
        public Core.Domain.Reports[] GetReportses(ReportParameters param)
        {
            Log.Inst.WriteToLogDEBUG(string.Format("Get Reportses"));
            //var fromDate = from.Date.ToString();//число с которого нужно считать
            //var toDate = to.Date.ToString();// по какое
            var corporateNutritionReportItemsList = new List<CorporateNutritionReportItem>();//список отчетов
            var reportsList = new List<Core.Domain.Reports>();
            var fromThis = param.DateFrom;
            
            //var arrayRespons = new CorporateNutritionReportItem[2]
            //    {
            //        new CorporateNutritionReportItem
            //        {
            //            BalanceOnPeriodEnd = (decimal) 10.1,
            //            BalanceOnPeriodStart = (decimal) 60.1,
            //            BalanceRefillSum = (decimal) 5.5,
            //            BalanceResetSum = (decimal) 5.4,
            //            EmployeeNumber = "10",
            //            GuestCardTrack = "11",
            //            GuestCategoryNames = "GuestCategoryNames",
            //            GuestId = "GuestId",
            //            GuestName = "GuestName",
            //            GuestPhone = "GuestPhone",
            //            PaidOrdersCount = (decimal) 4.4,
            //            PayFromWalletSum = (decimal) 1.4
            //        },
            //        new CorporateNutritionReportItem
            //        {
            //            BalanceOnPeriodEnd = (decimal) 10.1,
            //            BalanceOnPeriodStart = (decimal) 60.1,
            //            BalanceRefillSum = (decimal) 5.5,
            //            BalanceResetSum = (decimal) 5.4,
            //            EmployeeNumber = "10",
            //            GuestCardTrack = "11",
            //            GuestCategoryNames = "GuestCategoryNames1",
            //            GuestId = "GuestId1",
            //            GuestName = "GuestName1",
            //            GuestPhone = "GuestPhone1",
            //            PaidOrdersCount = (decimal) 4.4,
            //            PayFromWalletSum = (decimal) 1.4
            //        }
            //    };
            var toThis = fromThis.AddDays(1);

            while (fromThis != param.DateTo)
            {

                //Log.Inst.WriteToLogDEBUG(string.Format("Start Get CorporateNutritionReportItem for from {0} to {1}", fromThis.ToString(Format),
                //            fromThis.AddDays(1).ToString(Format)));
                //new Thread(() =>
                //{
                    
                    corporateNutritionReportItemsList.Clear();
                    //var formatFrom = fromThis.ToString(Format);
                    //var formatTo = toThis.ToString(Format);
                    var arrayRespons =
                        this.GetCorporateNutritionReportItem(param.OrganizationInfoId, param.CorporateNutritionProgramId,
                            fromThis.ToString(Format),
                            fromThis.AddDays(1).ToString(Format))
                            .Result;
                    corporateNutritionReportItemsList.AddRange(arrayRespons);

                    var resultArray1 = corporateNutritionReportItemsList.ToArray();
                    var newReports1 = CoreContext.ParseCorparationToReports.Parse(resultArray1, toThis, fromThis);
                    reportsList.AddRange(newReports1);
                //}).Start();
                //Log.Inst.WriteToLogDEBUG(string.Format("End Get CorporateNutritionReportItem for from {0} to {1}", fromThis.ToString(Format),
                //          fromThis.AddDays(1).ToString(Format)));
                fromThis = fromThis.AddDays(1);
                toThis = fromThis.AddDays(1);
            }
            var arrayRespons1 =
                GetCorporateNutritionReportItem(param.OrganizationInfoId, param.CorporateNutritionProgramId, fromThis.ToString(Format),
                    fromThis.AddDays(1).ToString(Format))
                    .Result;
            corporateNutritionReportItemsList.AddRange(arrayRespons1);
            //var resultArray = corporateNutritionReportItemsList.ToArray();
            //var result = GetCorporateNutritionReportItem(organizationId, corporateNutritionInfoId, fromDate, toDate);
            var newReports = reportsList.ToArray();
            return newReports;
        }

        public async Task<Core.Domain.TransactionsReportItem[]> GetTransactionsReportItems(TransactionReportItemParametrs param)
        {
            Log.Inst.WriteToLogDEBUG(string.Format("Get TransactionsReportItems"));
          
            return
                await
                    CoreContext.BizApiClient.GetAsync<TransactionsReportItem[]>(
                        (string.Format("/organization/{0}/transactions_report", param.OrganizationInfoId)), new
                        {
                            date_from = param.DateFrom.ToString(Format),
                            date_to = param.DateTo.ToString(Format)
                        }).ConfigureAwait(false);
        }

        private async Task<CorporateNutritionReportItem[]> GetCorporateNutritionReportItem(string organizationId,
            string corporateNutritionInfoId, string fromDate, string toDate)
        {

            /////api/0/organization/{organizationId}/corporate_nutrition_report?corporate_nutrition_id={corporateNutritionProgramId}
            /// &date_from={dateFrom}&date_to={dateTo}&access_token={accessToken}
            var idOrg = CoreContext.MakerRequest.GetOrganizationInfo().Result[0].Id;
            return
                await
                    CoreContext.BizApiClient.GetAsync<CorporateNutritionReportItem[]>(
                        (string.Format("/organization/{0}/corporate_nutrition_report", idOrg)), new
                        {
                            corporate_nutrition_id = corporateNutritionInfoId,
                            date_from = fromDate,
                            date_to = toDate
                        }).ConfigureAwait(false);
        }

    }
}
