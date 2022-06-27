using Com.Kana.Service.Upload.Lib.ViewModels.GarmentPurchasingBookReportViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.Interfaces
{
    public interface IGarmentPurchasingBookReportFacade
    {
        Tuple<List<GarmentPurchasingBookReportViewModel>, int> GetBookReport(int offset, bool? suppliertype, string suppliercode, string tipebarang, int page, int size, string Order, DateTime? dateFrom, DateTime? dateTo);
        MemoryStream GenerateExcelBookReport(string ctg, bool? suppliertype, string suppliercode, DateTime? datefrom, DateTime? dateto, int offset);
    }
}
