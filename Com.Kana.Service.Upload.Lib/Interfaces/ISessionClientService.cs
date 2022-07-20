using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Com.Kana.Service.Upload.Lib.Interfaces
{
    public interface ISessionClientService
    {
        Task<HttpResponseMessage> GetAsync(string url);
    }
}
