using Com.Kana.Service.Upload.Lib.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.Kana.Service.Upload.Lib.Interfaces
{
    public interface IIntegrationFacade
    {
        Task<AccurateTokenViewModel> RetrieveToken(string code);
        Task<bool> GetCode();
    }
}
