using Com.Kana.Service.Upload.Lib.ViewModels.GarmentReports;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.Interfaces
{
    public interface IGarmentDebtBalanceReportFacade
    {
        Tuple<List<GarmentDebtBalanceViewModel>, int> GetDebtBookReport(int month, int year, bool? suppliertype, string category);
        MemoryStream GenerateExcelDebtReport(int month, int year, bool? suppliertype, string category);
    }
}
