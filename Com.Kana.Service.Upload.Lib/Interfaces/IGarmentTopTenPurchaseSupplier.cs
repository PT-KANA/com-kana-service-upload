using Com.Kana.Service.Upload.Lib.ViewModels.GarmentExternalPurchaseOrderViewModel.Reports;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.Interfaces
{
    public interface IGarmentTopTenPurchaseSupplier
    {
        List<TopTenGarmenPurchasebySupplierViewModel> GetTopTenGarmentPurchaseSupplierReport(string unit, bool jnsSpl, string payMtd, string category, DateTime? dateFrom, DateTime? dateTo, int offset);
        MemoryStream GenerateExcelTopTenGarmentPurchaseSupplier(string unit, bool jnsSpl, string payMtd, string category, DateTime? dateFrom, DateTime? dateTo, int offset);
    }
}
