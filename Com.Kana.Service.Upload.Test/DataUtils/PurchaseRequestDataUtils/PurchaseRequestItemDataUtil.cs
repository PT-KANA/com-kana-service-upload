using Com.Kana.Service.Upload.Lib.Models.PurchaseRequestModel;
using Com.Kana.Service.Upload.Lib.ViewModels.IntegrationViewModel;
using Com.Kana.Service.Upload.Lib.ViewModels.PurchaseRequestViewModel;

namespace Com.Kana.Service.Upload.Test.DataUtils.PurchaseRequestDataUtils
{
    public class PurchaseRequestItemDataUtil
    {
        public PurchaseRequestItem GetNewData() => new PurchaseRequestItem
        {
            ProductId = "ProductId",
            ProductCode = "ProductCode",
            ProductName = "ProductName",
            Quantity = 10,
            UomId = "UomId",
            Uom = "Uom",
            Remark = "Remark"
        };
        public PurchaseRequestItemViewModel GetNewDataViewModel() => new PurchaseRequestItemViewModel
        {
            product = new ProductViewModel
            {
                _id = "ProductId",
                code  = "ProductCode",
                name  = "ProductName",
                uom = new UomViewModel
                {
                    _id = "UomId",
                    unit = "Uom",
                }
            },
            quantity = 10,
            remark = "Remark"
        };
    }
}
