using Com.Kana.Service.Upload.Lib.ViewModels.IntegrationViewModel;

namespace Com.Kana.Service.Upload.Lib.ViewModels.PurchaseOrder
{
    public class PurchaseOrderViewModel
    {
        public CategoryViewModel category { get; set; }
        public bool useIncomeTax { get; set; }
    }
}