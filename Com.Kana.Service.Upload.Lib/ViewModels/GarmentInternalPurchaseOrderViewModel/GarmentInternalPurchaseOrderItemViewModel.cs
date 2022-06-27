using Com.Kana.Service.Upload.Lib.Utilities;
using Com.Kana.Service.Upload.Lib.ViewModels.NewIntegrationViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.ViewModels.GarmentInternalPurchaseOrderViewModel
{
    public class GarmentInternalPurchaseOrderItemViewModel : BaseViewModel
    {
        public long GPRItemId { get; set; }
        public string PO_SerialNumber { get; set; }

        public ProductViewModel Product { get; set; }

        public double Quantity { get; set; }
        public double BudgetPrice { get; set; }
        public double RemainingBudget { get; set; }

        public UomViewModel Uom { get; set; }

        public CategoryViewModel Category { get; set; }

        public string ProductRemark { get; set; }
        public string Status { get; set; }
    }
}
