using Com.Kana.Service.Upload.Lib.Helpers.ReadResponse;
using System;
using System.Threading.Tasks;

namespace Com.Kana.Service.Upload.Lib.Interfaces
{
    public interface IUnitPaymentOrderUnpaidReportFacade
    {
        Task<ReadResponse<object>> GetReport(int Size, int Page, string Order, string UnitPaymentOrderNo, string SupplierCode, DateTimeOffset? DateFrom, DateTimeOffset? DateTo, int Offset);
    }
}
