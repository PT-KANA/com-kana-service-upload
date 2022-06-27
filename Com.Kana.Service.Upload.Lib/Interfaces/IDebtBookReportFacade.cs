using Com.Kana.Service.Upload.Lib.ViewModels.DebtBookReportViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.Interfaces
{
    public interface IDebtBookReportFacade
    {
        Tuple<List<DebtBookReportViewModel>, int> GetDebtBookReport(int month, int year, bool? suppliertype, string suppliername);
        MemoryStream GenerateExcelDebtReport(int month, int year, bool? suppliertype, string suppliername);

    }
}
