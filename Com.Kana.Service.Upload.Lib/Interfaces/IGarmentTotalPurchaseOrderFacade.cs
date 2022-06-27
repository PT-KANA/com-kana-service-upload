using Com.Kana.Service.Upload.Lib.Models.GarmentExternalPurchaseOrderModel;
using Com.Kana.Service.Upload.Lib.ViewModels.GarmentExternalPurchaseOrderViewModel;
using Com.Kana.Service.Upload.Lib.ViewModels.GarmentExternalPurchaseOrderViewModel.Reports;
using Com.Kana.Service.Upload.Lib.ViewModels.NewIntegrationViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Kana.Service.Upload.Lib.Interfaces
{
    public interface IGarmentTotalPurchaseOrderFacade
    {
        List<TotalGarmentPurchaseBySupplierViewModel> GetTotalGarmentPurchaseBySupplierReport(string unit, bool jnsSpl, string payMtd, string category, DateTime? dateFrom, DateTime? dateTo, int offset);
        MemoryStream GenerateExcelTotalGarmentPurchaseBySupplier(string unit, bool jnsSpl, string payMtd, string category, DateTime? dateFrom, DateTime? dateTo, int offset);

    }
}
