using Com.Kana.Service.Upload.Lib.ViewModels.GarmentReports;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.Interfaces
{
    public interface IDebtCardReportFacade
    {
        Tuple<List<GarmentDebtCardReportViewModel>, int> GetDebtCardReport(int month, int year, string suppliercode, string suppliername, string currencyCode, string paymentMethod, int offset);
        MemoryStream GenerateExcelCardReport(int month, int year, string suppliercode, string suppliername, string currencyCode, string currencyName, string paymentMethod, int offset);
    }
}
