using Com.Kana.Service.Upload.Lib.ViewModels.UnitReceiptNote;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Com.Kana.Service.Upload.Lib.Interfaces
{
    public interface IDetailCreditBalanceReportFacade
    {
        Task<DetailCreditBalanceReportViewModel> GetReport(int categoryId, int accountingUnitId, int divisionId, DateTimeOffset? dateTo, bool isImport, bool isForeignCurrency);
        Task<MemoryStream> GenerateExcel(int categoryId, int accountingUnitId, int divisionId, DateTimeOffset? dateTo, bool isImport, bool isForeignCurrency);
    }
}
