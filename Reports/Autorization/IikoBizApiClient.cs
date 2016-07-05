using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Flurl;
using Newtonsoft.Json;

namespace IikoBizApi
{
    public class IikoBizApiClient : IBizTokenRequest
    {
        private readonly IikoBizToken _apiAccessToken;
        private readonly IikoBizToken _bizAccessToken;
        private readonly HttpClient _client;
        private readonly string _externalUserId;
        private readonly string _login;
        private readonly string _password;
        private readonly string TIMEOUT = "60000";
        public string IikoBizBaseAddress = "https://iiko.biz:9900/api/0/";
        //restoCamp
        //9U6cZjDCG3zm9jj

        // iikoDeliveryMan
        //8Yy57894336x8DH

        public IikoBizApiClient(string login = "restoCamp", string password = "9U6cZjDCG3zm9jj",
            string externalUserId = "RRR9P0", HttpClient client = null,
            string baseAddress = "https://iiko.biz:9900/api/0/", int timeout = 60000)
        {
            _client = client ?? new HttpClient();

            _client.Timeout = TimeSpan.FromMilliseconds(timeout);

            IikoBizBaseAddress = baseAddress;

            _login = login;
            _password = password;

            _externalUserId = externalUserId;

            _apiAccessToken = new IikoBizToken(
                $"{IikoBizBaseAddress}auth/access_token?user_id={_login}&user_secret={_password}",
                _client);
            _bizAccessToken = new IikoBizToken(
                $"{IikoBizBaseAddress}auth/biz_access_token?user_ext_id={_externalUserId}",
                _client);
            
        }


        ///// <summary>
        /////     Добавить проблему к заказу
        ///// </summary>
        //public async Task<string> AddOrderProblem(string organizationId, AddOrderProblemRequest request)
        //{
        //    // /api/0/orders/add_order_problem?access_token={accessToken}&organization={organizationId}&request_timeout={requestTimeout}

        //    return
        //        await
        //            PostAsync("orders/add_order_problem", request, new {organization = organizationId})
        //                .ConfigureAwait(false);
        //}

        ///// <summary>
        /////     Запрос для изменения статуса заказа курьером, отмечающий его как доставленный или “не доставленный”.
        ///// </summary>
        //public async Task<string> SetOrderDelivered(string organizationId, SetOrderDeliveredRequest request)
        //{
        //    // /api/0/orders/set_order_delivered?access_token={accessToken}&organization={organizationId}&request_timeout={requestTimeout}

        //    return
        //        await
        //            PostAsync("orders/set_order_delivered", request, new {organization = organizationId})
        //                .ConfigureAwait(false);
        //}

        //public async Task<DeliveryOrderInfo> GetOrderInfo(string organizationId, string orderId)
        //{
        //    // /api/0/orders/info?access_token={accessToken}&organization={organizationId}&order={orderId}&request_timeout={requestTimeout}

        //    return
        //        await
        //            GetAsync<DeliveryOrderInfo>("orders/info", new {organization = organizationId, order = orderId})
        //                .ConfigureAwait(false);
        //}

        //public async Task<DeliveryTerminalsDto> GetDeliveryTerminals(string organizationId)
        //{
        //    // /api/0/deliverySettings/getDeliveryTerminals?access_token={accessToken}&organization={organizationId}

        //    return
        //        await
        //            GetAsync<DeliveryTerminalsDto>("deliverySettings/getDeliveryTerminals",
        //                new {organization = organizationId}).ConfigureAwait(false);
        //}

        //public async Task<DeliveryOrderInfo[]> GetCourierOrders(string organizationId, string courierId)
        //{
        //    // /api/0/orders/get_courier_orders?access_token={accessToken}&organization={organizationId}&courier={courierId}&request_timeout={requestTimeout}

        //    return
        //        await
        //            GetAsync<DeliveryOrderInfo[]>("orders/get_courier_orders",
        //                new {request_timeout = TIMEOUT, organization = organizationId, courier = courierId})
        //                .ConfigureAwait(false);
        //}

        ///// <summary>
        /////     Запрос логина курьера доставки на удаленный РМС сервер
        ///// </summary>
        ///// <param name="loginRequestDto"></param>
        ///// <param name="organizationId"></param>
        ///// <returns></returns>
        //public async Task<MobileLoginResultDto> MobileSignIn(MobileLoginRequestDto loginRequestDto,
        //    string organizationId)
        //{
        //    // /api/0/mobile/signin?access_token={accessToken}&request_timeout={requestTimeout}&organization={organizationId}

        //    return
        //        await
        //            PostAsync<MobileLoginResultDto>("mobile/signin", loginRequestDto,
        //                new {request_timeout = TIMEOUT, organization = organizationId}).ConfigureAwait(false);
        //}

        ///// <summary>
        /////     Запрос полной синхронизации мобильного приложения и сервера доставок
        ///// </summary>
        ///// <param name="updateDto"></param>
        ///// <param name="organizationId"></param>
        ///// <returns></returns>
        //public async Task<SyncResultDto> MobileSync(SendUpdateDto updateDto, string organizationId)
        //{
        //    //"/api/0/mobile/sync?access_token={accessToken}&request_timeout={requestTimeout}&organization={organizationId}";

        //    return
        //        await
        //            PostAsync<SyncResultDto>("mobile/sync", updateDto,
        //                new {request_timeout = TIMEOUT, organization = organizationId}).ConfigureAwait(false);
        //}

        //public async Task<UserInfo> ApplicationMarketUserInfo()
        //{
        //    return await GetAsync<UserInfo>("applicationMarket/userInfo", new
        //    {
        //        biz_access_token = _bizAccessToken.Value,
        //        api_access_token = _apiAccessToken.Value
        //    }, false).ConfigureAwait(false);
        //}

        public async Task<string> OlapsOlap(string organizationId, string olapRequest)
        {
            return await PostAsync("olaps/olap", new
            {
                organizationId,
                olapSettings = olapRequest
            }).ConfigureAwait(false);
        }

        public async Task<GeocodingResponse> GetCoordinates(string address)
        {
            return await _geocoder.Geocode(address, 1, LangType.ru_RU).ConfigureAwait(false);
        }

        private async Task<T> GetAsync<T>(string path, object additionalQueryValues = null, bool needsAccessToken = true)
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

        private async Task<string> GetAsync(string path, object additionalQueryValues = null,
            bool needsAccessToken = true)
        {
            var url = IikoBizBaseAddress.AppendPathSegment(path);
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
            var url = IikoBizBaseAddress.AppendPathSegment(path);

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

    public class ApiException : Exception
    {
        public Exception Exception;
        public string Response;

        public ApiException(string response = null, string message = "", Exception ex = null) : base(message)
        {
            Exception = ex;
            Response = response;
        }
    }
}