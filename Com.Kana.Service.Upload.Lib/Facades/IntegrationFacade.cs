using Com.Kana.Service.Upload.Lib.Helpers;
using Com.Kana.Service.Upload.Lib.Interfaces;
using Com.Kana.Service.Upload.Lib.ViewModels;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Com.Kana.Service.Upload.Lib.Facades
{
    public class IntegrationFacade : IIntegrationFacade
    {
        private readonly IServiceProvider serviceProvider;

        public IntegrationFacade(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public Task<AccurateTokenViewModel> RetrieveToken(string code)
        {
            var basic_token = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(AuthCredential.ClientId+":"+AuthCredential.ClientSecret));
            //var code = AuthCredential.ClientCode;
            var AccurateToken = RequestTokenAsync(code, basic_token);

            if(AccurateToken != null)
            {
                AuthCredential.AccessToken = AccurateToken.Result.access_token;
                AuthCredential.RefreshToken = AccurateToken.Result.refresh_token;
            }

            return AccurateToken;
        }

        private async Task<AccurateTokenViewModel> RequestTokenAsync(string code, string token)
        {
            var httpClient = new HttpClient();
            var data = new[]
            {
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("redirect_uri", APIEndpoint.Upload+"integration/authcallback"),
            };

            var content = new FormUrlEncodedContent(data);

            using (var request = new HttpRequestMessage(HttpMethod.Post, APIEndpoint.Accurate+"oauth/token"))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", token);
                request.Content = content;

                var response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    var message = response.Content.ReadAsStringAsync().Result;
                    AccurateTokenViewModel AccuToken = JsonConvert.DeserializeObject<AccurateTokenViewModel>(message);

                    return AccuToken;
                }
                else
                {
                    return null;
                }
            }
        }

        public Task<AccurateTokenViewModel> RefreshToken()
        {
            var basic_token = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(AuthCredential.ClientId + ":" + AuthCredential.ClientSecret));
            var refresh_token = AuthCredential.RefreshToken;

            var AccurateToken = RenewTokenAsync(basic_token, refresh_token);

            if (AccurateToken != null)
            {
                AuthCredential.AccessToken = AccurateToken.Result.access_token;
                AuthCredential.RefreshToken = AccurateToken.Result.refresh_token;
            }

            return AccurateToken;
        }

        private async Task<AccurateTokenViewModel> RenewTokenAsync(string token, string refToken)
        {
            var httpClient = new HttpClient();
            var data = new[]
            {
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("redirect_uri", APIEndpoint.Upload+"integration/authcallback"),
                new KeyValuePair<string, string>("refresh_token", refToken),
            };

            var content = new FormUrlEncodedContent(data);

            using (var request = new HttpRequestMessage(HttpMethod.Post, APIEndpoint.Accurate + "oauth/token"))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", token);
                request.Content = content;

                var response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    var message = response.Content.ReadAsStringAsync().Result;
                    AccurateTokenViewModel AccuToken = JsonConvert.DeserializeObject<AccurateTokenViewModel>(message);

                    return AccuToken;
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<object> GetDbList()
        {
            var httpClient = new HttpClient();
            using (var request = new HttpRequestMessage(HttpMethod.Get, APIEndpoint.Accurate + "api/db-list.do"))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AuthCredential.AccessToken);

                var response = await httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var message = response.Content.ReadAsStringAsync().Result;

                    return message;
                }
                else
                {
                    return null;
                }
            }

        }
    }
}
