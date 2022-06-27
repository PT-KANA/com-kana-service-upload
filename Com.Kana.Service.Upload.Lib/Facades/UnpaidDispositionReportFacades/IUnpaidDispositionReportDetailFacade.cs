using Com.Kana.Service.Upload.Lib.ViewModels.UnpaidDispositionReport;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Com.Kana.Service.Upload.Lib.Facades.UnpaidDispositionReportFacades
{
    public interface IUnpaidDispositionReportDetailFacade
    {
        Task<UnpaidDispositionReportDetailViewModel> GetReport(int accountingUnitId, int categoryId, int divisionId, DateTimeOffset? dateTo, bool isImport, bool isForeignCurrency);
        Task<MemoryStream> GenerateExcel(int accountingUnitId, int categoryId, int divisionId, DateTimeOffset? dateTo, bool isImport, bool isForeignCurrency);
    }
}
