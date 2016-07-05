using System;
using System.Net.Http;

namespace IikoBizApi
{
    /// <summary>
    /// один запрос
    /// </summary>
    public class IikoBizToken
    {
        private readonly HttpClient _client;
        private readonly string _refreshUrl;
        private DateTime _expiresAt;
        private string _value;

        public IikoBizToken(string refreshUrl, HttpClient client)
        {
            _refreshUrl = refreshUrl;
            _client = client;
        }

        public string Value
        {
            get
            {
                if ((_value == null) || (DateTime.Now > _expiresAt))
                {
                    _value = Request().Trim('"');

                    _expiresAt = DateTime.Now.AddMinutes(10);
                }

                return _value;
            }
        }

        private string Request()
        {
            try
            {
                var response = _client.GetAsync(_refreshUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;

                    return responseContent.ReadAsStringAsync().Result;
                }
            }
            catch (Exception ex)
            {
                throw;
            }


            return null;
        }
    }
}