using Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuSalesReturnModel;
using Com.Kana.Service.Upload.Lib.ViewModels.AccuSalesReturnViewModel;
using Com.Kana.Service.Upload.Lib.ViewModels.SalesReturnViewModel;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.Kana.Service.Upload.Lib.Interfaces.SalesReturnInterface
{
    public interface ISalesReturnUpload
    {
        Tuple<bool, List<object>> UploadValidate(ref List<SalesReturnCsvViewModel> data, List<KeyValuePair<string, StringValues>> list);
        Task UploadData(List<AccuSalesReturn> data, string username);
        Task Create(List<AccuSalesReturnViewModel> data, string username, string token);
        List<string> CsvHeader { get; }
        Tuple<List<AccuSalesReturn>, int, Dictionary<string, string>> ReadForUpload(int Page = 1, int Size = 25, string Order = "{}", string Keyword = null, string Filter = "{}");
        Task<List<AccuSalesReturnViewModel>> MapToViewModel(List<SalesReturnCsvViewModel> csv);
        Task<List<AccuSalesReturn>> MapToModel(List<AccuSalesReturnViewModel> data1);
    }
}
