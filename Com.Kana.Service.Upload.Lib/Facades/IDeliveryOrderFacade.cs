using Com.Kana.Service.Upload.Lib.Models.DeliveryOrderModel;
using Com.Kana.Service.Upload.Lib.ViewModels.DeliveryOrderViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Com.Kana.Service.Upload.Lib.Facades
{
    public interface IDeliveryOrderFacade
    {
        Tuple<List<DeliveryOrder>, int, Dictionary<string, string>> Read(int Page = 1, int Size = 25, string Order = "{}", string Keyword = null, string Filter = "{}");
        Tuple<DeliveryOrder, List<long>> ReadById(int id);
        Task<int> Create(DeliveryOrder model, string username);
        Task<int> Update(int id, DeliveryOrder model, string user);
        int Delete(int id, string username);
        List<DeliveryOrder> ReadBySupplier(string Keyword = null, string unitId = "", string supplierId = "");
        Tuple<List<DeliveryOrderReportViewModel>, int> GetReport(string no, string supplierId, DateTime? dateFrom, DateTime? dateTo, int page, int size, string Order, int offset);
        MemoryStream GenerateExcel(string no, string supplierId, DateTime? dateFrom, DateTime? dateTo, int offset);


    }
}
