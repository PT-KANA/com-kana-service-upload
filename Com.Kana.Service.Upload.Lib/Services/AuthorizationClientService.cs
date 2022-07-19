using Com.Kana.Service.Upload.Lib.Helpers;
using Com.Kana.Service.Upload.Lib.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Com.Kana.Service.Upload.Lib.Services
{
    public class AuthorizationClientService : IAuthorizationClientService
    {
        private HttpClient _client = new HttpClient();
        public AuthorizationClientService()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(AuthCredential.ClientId + ":" + AuthCredential.ClientSecret)));
        }

        public async Task<HttpResponseMessage> SendAsync(HttpMethod method, string url, HttpContent content)
        {
            var request = new HttpRequestMessage(method, url)
            {
                Content = content
            };

            return await _client.SendAsync(request);
        }
    }
}
