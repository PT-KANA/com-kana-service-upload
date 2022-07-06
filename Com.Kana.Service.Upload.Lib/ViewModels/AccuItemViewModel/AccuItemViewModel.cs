using Com.Kana.Service.Upload.Lib.Utilities;
using Com.Kana.Service.Upload.Lib.ViewModels.AccuItemViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.DanLiris.Service.Purchasing.Lib.ViewModels.AccuItemViewModel
{
    public class AccuItemViewModel : BaseViewModel
    {
        public string itemType { get; set; }
        public string name { get; set; }
        public bool calculateGroupPrice { get; set; }
        public string cogsGIAccountNo { get; set; }
        public bool controlQuality { get; set; }
        public string defaultDiscount { get; set; }
        public List<AccuItemDetailGroupViewModel> detailGroup { get; set; }
        public List<AccuItemDetailOpenBalanceViewModel> detailOpenBalance { get; set; }
        public string goodTransitGIAccountNo { get; set; }
        public string inventoryGIAccountNao { get; set; }
        public string itemCategoryName { get; set; }
        public bool manageExpired { get; set; }
        public bool manageSN { get; set; }
        public double minimumQuantity { get; set; }
        public double minimumQuantityReorder { get; set; }
        public string no { get; set; }
        public string notes { get; set; }
        public double percentTaxable { get; set; }
        public string preferedVendorName { get; set; }
        public bool printDetailGroup { get; set; }
        public string purchaseRetGIAccountNo { get; set; }
        public double ratio2 { get; set; }
        public double ratio3 { get; set; }
        public double ratio4 { get; set; }
        public double ratio5 { get; set; }
        public string salesDiscountGIAccountNo { get; set; }
        public string salesGIAccountNo { get; set; }
        public string salesRetGIAccountNo { get; set; }
        public string serialNumberType { get; set; }
        public bool subtituted { get; set; }
        public string subtitutedItemNo { get; set; }
        public string tax1Name { get; set; }
        public string tax2Name { get; set; }
        public string tax3Name { get; set; }
        public string tax4Name { get; set; }
        public double typeAutoNumber { get; set; }
        public string unBilledGIAccountNo { get; set; }
        public string unit1Name { get; set; }
        public double unitPrice { get; set; }
        public string unit2Name { get; set; }
        public double unit2Price { get; set; }
        public string unit3Name { get; set; }
        public double unit3Price { get; set; }
        public string unit4Name { get; set; }
        public double unit4Price { get; set; }
        public string unit5Name { get; set; }
        public double unit5Price { get; set; }
        public string upcNo { get; set; }
        public bool usePPn { get; set; }
        public bool useWholesalePrice { get; set; }
        public double vendorPrice { get; set; }
        public string vendorUnitName { get; set; }

        public bool isAccurate { get; set; }
    }
}
