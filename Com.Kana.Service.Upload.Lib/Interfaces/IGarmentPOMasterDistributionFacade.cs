using Com.Kana.Service.Upload.Lib.Helpers.ReadResponse;
using Com.Kana.Service.Upload.Lib.ViewModels.GarmentPOMasterDistributionModels;
using Com.Kana.Service.Upload.Lib.ViewModels.GarmentPOMasterDistributionViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.Kana.Service.Upload.Lib.Interfaces
{
    public interface IGarmentPOMasterDistributionFacade
    {
        ReadResponse<dynamic> Read(int Page = 1, int Size = 25, string Order = "{}", string Keyword = null, string Filter = "{}", string Select = "{}", string Search = "[]");
        GarmentPOMasterDistribution ReadById(long id);
        Task<int> Create(GarmentPOMasterDistribution model);
        Task<int> Update(long id , GarmentPOMasterDistribution model);
        Task<int> Delete(long id);
        Dictionary<string, decimal> GetOthersQuantity(GarmentPOMasterDistributionViewModel garmentPOMasterDistributionViewModel);
    }
}
