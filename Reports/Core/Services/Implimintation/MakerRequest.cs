using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Core.Domain;
using Newtonsoft.Json;
using Reports;

namespace Core.Services.Implimintation
{
    public class MakerRequest : IMakerRequest
    {
        private const string Format = "yyyy-MM-dd";

        public async Task<OrganizationInfo[]> GetOrganizationInfo()
        {
            Log.Inst.WriteToLogDEBUG("Get OrganizationInfo");
            /// api / 0 / organization / list ? access_token ={ accessToken}&request_timeout ={ requestTimeout}
            return await CoreContext.BizApiClient.GetAsync<OrganizationInfo[]>("/organization/list", new
            {
                request_timeout = 60000
            }).ConfigureAwait(false);
        }

        public async Task<CorporateNutritionInfo[]> GetCorporateNutritionInfo(string organizationId)
        {
            Log.Inst.WriteToLogDEBUG("Get CorporateNutritionInfo");
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
        ///     Получаем данные с запроса на отчет и переводим эти данные в удобный нам формат Reports
        /// </summary>
        public Domain.Reports[] GetReportses(ReportParameters param)
        {
            Log.Inst.WriteToLogDEBUG("Get Reportses");
            var corporateNutritionReportItemsList = new List<CorporateNutritionReportItem>(); //список отчетов
            var reportsList = new List<Domain.Reports>();
            var fromThis = param.DateFrom;
            var toThis = fromThis.AddDays(1);

            while (fromThis != param.DateTo)
            {
                corporateNutritionReportItemsList.Clear();
                //ДЛЯ КОРРЕКТНОЙ РАБОТЫ ДОБАВИТЬ АЙДИШНИК CorporateNutrition ВМЕСТО  ""
                var arrayRespons =
                    GetCorporateNutritionReportItem(param.OrganizationInfoId, "",
                        fromThis.ToString(Format),
                        fromThis.AddDays(1).ToString(Format))
                        .Result;
                corporateNutritionReportItemsList.AddRange(arrayRespons);

                var resultArray1 = corporateNutritionReportItemsList.ToArray();
                var newReports1 = CoreContext.ParseCorparationToReports.Parse(resultArray1, toThis, fromThis);
                reportsList.AddRange(newReports1);
            
                fromThis = fromThis.AddDays(1);
                toThis = fromThis.AddDays(1);
            }
            var arrayRespons1 =
                GetCorporateNutritionReportItem(param.OrganizationInfoId, "",
                    fromThis.ToString(Format),
                    fromThis.AddDays(1).ToString(Format))
                    .Result;
            corporateNutritionReportItemsList.AddRange(arrayRespons1);
            var newReports = reportsList.ToArray();
            return newReports;
        }

        public async Task<TransactionsReportItem[]> GetTransactionsReportItems(TransactionReportItemParametrs param)
        {
            Log.Inst.WriteToLogDEBUG("Get TransactionsReportItems");

            return
                await
                    CoreContext.BizApiClient.GetAsync<TransactionsReportItem[]>(
                        (string.Format("/organization/{0}/transactions_report", param.OrganizationInfoId)), new
                        {
                            date_from = param.DateFrom.ToString(Format),
                            date_to = param.DateTo.ToString(Format)
                        }).ConfigureAwait(false);
        }

        public async Task<CardInfo> GetInfoByCard(string cardNumber, string organizationId)
        {
            /////api/0/organization/{organizationId}/corporate_nutrition_report?corporate_nutrition_id={corporateNutritionProgramId}
            /// &date_from={dateFrom}&date_to={dateTo}&access_token={accessToken}
            var idOrg = CoreContext.MakerRequest.GetOrganizationInfo().Result[0].Id;
            return
                await
                    CoreContext.BizApiClient.GetAsync<CardInfo>(
                        (string.Format("customers/get_customer_by_card", idOrg)), new
                        {
                            organization = organizationId,
                            card = cardNumber
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