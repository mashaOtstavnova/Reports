using System;
using System.Net.Http;
using System.Text;
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
        //restoCamp
        //9U6cZjDCG3zm9jj

        // iikoDeliveryMan
        //8Yy57894336x8DH

        public void Init( Settings settings, HttpClient client = null
            )
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
            return await GetAsync<CorporateNutritionInfo[]>((string.Format("/organization/{0}/corporate_nutritions", idOrg)), new
            {
            }).ConfigureAwait(false);
        }
        public async Task<CorporateNutritionReportItem[]> GetCorporateNutritionReportItem(string organizationId, string corporateNutritionInfoId)
        {
            /////api/0/organization/{organizationId}/corporate_nutrition_report?corporate_nutrition_id={corporateNutritionProgramId}
            /// &date_from={dateFrom}&date_to={dateTo}&access_token={accessToken}
            var idOrg = CoreContext.BizApiClient.GetOrganizationInfo().Result[0].Id;
            return await GetAsync<CorporateNutritionReportItem[]>((string.Format("/organization/{0}/corporate_nutrition_report", idOrg)), new
            {
                corporate_nutrition_id = corporateNutritionInfoId,
                date_from = "2016-04-01",
                date_to = "2016-07-04"

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