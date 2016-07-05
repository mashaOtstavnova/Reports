using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Core.Domain;
using Core.Services;
using Core.Services.Implimintation;
using Flurl;
using Framework.MyExtention;
using Newtonsoft.Json;

namespace IikoBizApi
{
    public class IikoBizApiClient : IBizApiClient
    {
        private IikoBizToken _apiAccessToken;
        private HttpClient _client;
        private Settings _settings;
        private const string Format = "yyyy-MM-dd";
        //restoCamp
        //9U6cZjDCG3zm9jj

        // iikoDeliveryMan
        //8Yy57894336x8DH

        public void Init(Settings settings, HttpClient client = null)
        {
            _client = client ?? new HttpClient();
            _settings = settings ?? new Settings("restoCamp", "9U6cZjDCG3zm9jj", "https://iiko.biz:9900/api/0/", 60000);
            _client.Timeout = TimeSpan.FromMilliseconds(settings.TIMEOUT);

            _apiAccessToken = new IikoBizToken(
                $"{_settings.BaseAddress}auth/access_token?user_id={settings.Login}&user_secret={settings.Password}",
                _client);
        }

        public async Task<OrganizationInfo[]> GetOrganizationInfo()
        {
            /// api / 0 / organization / list ? access_token ={ accessToken}&request_timeout ={ requestTimeout}
            return await GetAsync<OrganizationInfo[]>("/organization/list", new
            {
                request_timeout = 60000
            }).ConfigureAwait(false);
        }

        public async Task<CorporateNutritionInfo[]> GetCorporateNutritionInfo(string organizationId)
        {
            ////api/0/organization/{organizationId}/corporate_nutritions?access_token={accessToken}
            var idOrg = CoreContext.BizApiClient.GetOrganizationInfo().Result[0].Id;
            return
                await
                    GetAsync<CorporateNutritionInfo[]>(
                        (string.Format("/organization/{0}/corporate_nutritions", idOrg)), new
                        {
                        }).ConfigureAwait(false);
        }

        /// <summary>
        /// Получаем данные с запроса на отчет и переводим эти данные в удобный нам формат Reports
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="corporateNutritionInfoId"> </param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public Reports[] GetReportses(string organizationId, string corporateNutritionInfoId, DateTime from, DateTime to)
        {
            //var fromDate = from.Date.ToString();//число с которого нужно считать
            //var toDate = to.Date.ToString();// по какое
            var corporateNutritionReportItemsList = new List<CorporateNutritionReportItem>();//список отчетов
            var reportsList = new List<Reports>();
            var fromThis = from;
            var arrayRespons = new CorporateNutritionReportItem[2]
                {
                    new CorporateNutritionReportItem
                    {
                        BalanceOnPeriodEnd = (decimal) 10.1,
                        BalanceOnPeriodStart = (decimal) 60.1,
                        BalanceRefillSum = (decimal) 5.5,
                        BalanceResetSum = (decimal) 5.4,
                        EmployeeNumber = "10",
                        GuestCardTrack = "11",
                        GuestCategoryNames = "GuestCategoryNames",
                        GuestId = "GuestId",
                        GuestName = "GuestName",
                        GuestPhone = "GuestPhone",
                        PaidOrdersCount = (decimal) 4.4,
                        PayFromWalletSum = (decimal) 1.4
                    },
                    new CorporateNutritionReportItem
                    {
                        BalanceOnPeriodEnd = (decimal) 10.1,
                        BalanceOnPeriodStart = (decimal) 60.1,
                        BalanceRefillSum = (decimal) 5.5,
                        BalanceResetSum = (decimal) 5.4,
                        EmployeeNumber = "10",
                        GuestCardTrack = "11",
                        GuestCategoryNames = "GuestCategoryNames1",
                        GuestId = "GuestId1",
                        GuestName = "GuestName1",
                        GuestPhone = "GuestPhone1",
                        PaidOrdersCount = (decimal) 4.4,
                        PayFromWalletSum = (decimal) 1.4
                    }
                };
            var toThis = fromThis.AddDays(1);
            
            while (fromThis != to)
            {
                corporateNutritionReportItemsList.Clear();
                //var formatFrom = fromThis.ToString(Format);
                //var formatTo = toThis.ToString(Format);
                //var arrayRespons =
                //    this.GetCorporateNutritionReportItem(organizationId, corporateNutritionInfoId, formatFrom, formatTo)
                //        .Result;
                
                corporateNutritionReportItemsList.AddRange(arrayRespons);

                var resultArray1 = corporateNutritionReportItemsList.ToArray();
                var newReports1 = CoreContext.ParseCorparationToReports.Parse(resultArray1, toThis, fromThis);
                reportsList.AddRange(newReports1);
                fromThis = fromThis.AddDays(1);
                toThis = fromThis.AddDays(1);
            }
            var arrayRespons1 =
                GetCorporateNutritionReportItem(organizationId, corporateNutritionInfoId, fromThis.ToString(Format),
                    fromThis.AddDays(1).ToString(Format))
                    .Result;
            corporateNutritionReportItemsList.AddRange(arrayRespons1);
            //var resultArray = corporateNutritionReportItemsList.ToArray();
            //var result = GetCorporateNutritionReportItem(organizationId, corporateNutritionInfoId, fromDate, toDate);
            var newReports = reportsList.ToArray();
            return newReports;
        }

        private async Task<CorporateNutritionReportItem[]> GetCorporateNutritionReportItem(string organizationId,
            string corporateNutritionInfoId, string fromDate, string toDate)
        {
            /////api/0/organization/{organizationId}/corporate_nutrition_report?corporate_nutrition_id={corporateNutritionProgramId}
            /// &date_from={dateFrom}&date_to={dateTo}&access_token={accessToken}
            var idOrg = CoreContext.BizApiClient.GetOrganizationInfo().Result[0].Id;
            return
                await
                    GetAsync<CorporateNutritionReportItem[]>(
                        (string.Format("/organization/{0}/corporate_nutrition_report", idOrg)), new
                        {
                            corporate_nutrition_id = corporateNutritionInfoId,
                            date_from = fromDate,
                            date_to = toDate
                        }).ConfigureAwait(false);
        }

        public async Task<T> GetAsync<T>(string path, object additionalQueryValues = null, bool needsAccessToken = true)
        {
            var response = await GetAsync(path, additionalQueryValues, needsAccessToken).ConfigureAwait(false);

            try
            {
                var result = JsonConvert.DeserializeObject<T>(response);
                return result;
            }
            catch (Exception ex)
            {
                // here can be response with an error
                throw new ApiException(response, ex: ex);
            }
        }

        public async Task<string> OlapsOlap(string organizationId, string olapRequest)
        {
            return await PostAsync("olaps/olap", new
            {
                organizationId,
                olapSettings = olapRequest
            }).ConfigureAwait(false);
        }

        private async Task<T> PostAsync<T>(string path, object requestBody, object additionalQueryValues = null,
            bool needsAccessToken = true)
        {
            var response =
                await PostAsync(path, requestBody, additionalQueryValues, needsAccessToken).ConfigureAwait(false);

            try
            {
                var result = JsonConvert.DeserializeObject<T>(response);

                return result;
            }
            catch (Exception ex)
            {
                // here can be response with an error
                throw new ApiException(response, ex: ex);
            }
        }

        public async Task<string> GetAsync(string path, object additionalQueryValues = null,
            bool needsAccessToken = true)
        {
            var url = _settings.BaseAddress.AppendPathSegment(path);
            if (needsAccessToken)
            {
                url = url.SetQueryParam("access_token", _apiAccessToken.Value);
            }

            if (additionalQueryValues != null)
            {
                url = url.SetQueryParams(additionalQueryValues);
            }

            return await _client.GetStringAsync(url).ConfigureAwait(false);
        }

        private async Task<string> PostAsync(string path, object requestBody, object additionalQueryValues = null,
            bool needsAccessToken = true)
        {
            var url = _settings.BaseAddress.AppendPathSegment(path);

            if (needsAccessToken)
            {
                url = url.SetQueryParam("access_token", _apiAccessToken.Value);
            }

            if (additionalQueryValues != null)
            {
                url = url.SetQueryParams(additionalQueryValues);
            }

            var json = JsonConvert.SerializeObject(requestBody, Formatting.None, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            }
                );

            var responseMessage = await _client.PostAsync(
                url,
                new StringContent(json,
                    Encoding.Unicode,
                    "application/json")
                )
                .ConfigureAwait(false);

            return await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
        }
    }
}