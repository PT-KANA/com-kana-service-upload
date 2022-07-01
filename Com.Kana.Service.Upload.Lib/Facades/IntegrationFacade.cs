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
        //private readonly string uricallback = "http://localhost:5000/v1/integration/authcallback";
        //private readonly string uriauth = "https://account.accurate.id/oauth/token";

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
            }

            return AccurateToken;
        }

        public async Task<bool> GetCode()
        {
            var httpClient = new HttpClient();
            var url = APIEndpoint.Accurate + "/authorize?client_id=" + AuthCredential.ClientId + "&response_type=code&redirect_uri=" + APIEndpoint.Upload + "integration/authcallback&scope=" + AuthCredential.Scope;

            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private async Task<AccurateTokenViewModel> RequestTokenAsync(string code, string token)
        {
            //JsonConvert.SerializeObject("{\"code\":\"" + code + "\",\"grant_type\":\"authorization_code\",\"redirect_uri\":\"" + uricallback + "\"}");

            var httpClient = new HttpClient();
            var data = new[]
            {
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("redirect_uri", APIEndpoint.Upload+"integration/authcallback"),
            };

            //Dictionary<string, string> dict = new Dictionary<string, string>();
            //dict["code"] = code;
            //dict["grant_type"] = "authorization_code";
            //dict["redirect_uri"] = uricallback;

            //var data = JsonConvert.SerializeObject(dict);
            //var content = new StringContent(data, Encoding.UTF8, "application/x-www-form-urlencoded");

            var content = new FormUrlEncodedContent(data);

            using (var request = new HttpRequestMessage(HttpMethod.Post, APIEndpoint.Accurate+"token"))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", token);
                request.Content = content;

                var response = await httpClient.SendAsync(request);
                //response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    var message = response.Content.ReadAsStringAsync().Result;
                    //var decode = JsonConvert.DeserializeObject<Dictionary<string, object>>(message);
                    AccurateTokenViewModel AccuToken = JsonConvert.DeserializeObject<AccurateTokenViewModel>(message);

                    return AccuToken;
                }
                else
                {
                    var message = response.Content.ReadAsStringAsync().Result;
                    return null;
                }
            }
        }
    }
}
