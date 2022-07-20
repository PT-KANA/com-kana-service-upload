using Com.Kana.Service.Upload.Lib.Helpers;
using Com.Kana.Service.Upload.Lib.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Com.Kana.Service.Upload.Lib.Services
{
    public class SessionClientService : ISessionClientService
    {
        private HttpClient _client = new HttpClient();

        public SessionClientService()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, AuthCredential.AccessToken);
        }

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            return await _client.GetAsync(url);
        }
    }
}
