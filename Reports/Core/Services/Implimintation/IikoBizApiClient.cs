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
using Core.Domain;
using Reports;

namespace IikoBizApi
{
    /// <summary>
    /// функции на запросы
    /// </summary>
    public class IikoBizApiClient : IBizApiClient
    {
        private IikoBizToken _apiAccessToken;
        private HttpClient _client;
        private Settings _settings;
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

        
        public async Task<T> GetAsync<T>(string path, object additionalQueryValues = null, bool needsAccessToken = true)
        {
            Log.Inst.WriteToLogDEBUG(string.Format(" --->Start get async answer"));
            var response = await GetAsync(path, additionalQueryValues, needsAccessToken).ConfigureAwait(false);

            try
            {
                var result = JsonConvert.DeserializeObject<T>(response);
                Log.Inst.WriteToLogDEBUG(string.Format("<----End get async answer"));
                return result;
            }
            catch (Exception ex)
            {
                // here can be response with an error
                Log.Inst.WriteToLogDEBUG(string.Format("ERROR JsonConvert for get: {0}", ex.Message));
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
            Log.Inst.WriteToLogDEBUG(string.Format("Start post async answer"));
            var response =
                await PostAsync(path, requestBody, additionalQueryValues, needsAccessToken).ConfigureAwait(false);

            try
            {
                var result = JsonConvert.DeserializeObject<T>(response);
                Log.Inst.WriteToLogDEBUG(string.Format("End post async answer"));
                return result;
            }
            catch (Exception ex)
            {
                // here can be response with an error
                Log.Inst.WriteToLogDEBUG(string.Format("ERROR JsonConvert for post: {0}", ex.Message));
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
                Log.Inst.WriteToLogDEBUG(string.Format("This access token for get request: {0}", _apiAccessToken.Value));
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
                Log.Inst.WriteToLogDEBUG(string.Format("This access token for post request: {0}", _apiAccessToken.Value));
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