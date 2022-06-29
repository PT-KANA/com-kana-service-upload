using Com.DanLiris.Service.Purchasing.Lib.ViewModels.AccuItemViewModel;
using Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuItemModel;
using Com.Kana.Service.Upload.Lib.ViewModels.ItemViewModel;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.Kana.Service.Upload.Lib.Interfaces.ItemInterface
{
    public interface IItemFacade
    {
        Task<List<AccuItemViewModel>> MapToViewModel(List<ItemCsvViewModel> data);
        Tuple<bool, List<object>> UploadValidate(ref List<ItemCsvViewModel> data, List<KeyValuePair<string, StringValues>> list);
        Task UploadData(List<AccuItem> data, string username);
        List<string> CsvHeader { get; }
        Task<List<AccuItem>> MapToModel(List<AccuItemViewModel> data1);
    }
}
