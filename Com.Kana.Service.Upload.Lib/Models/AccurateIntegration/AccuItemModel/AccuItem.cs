using Com.Kana.Service.Upload.Lib.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuItemModel
{
    public class AccuItem : BaseModel
    {
        public string ItemType { get; set; }
        public string Name { get; set; }
        public bool CalculateGroupPrice { get; set; }
        public string CogsGIAccountNo { get; set; }
        public bool ControlQuality { get; set; }
        public string DefaultDiscount { get; set; }
        public string GoodTransitGIAccountNo { get; set; }
        public string InventoryGIAccountNo { get; set; }
        public string ItemCategoryName { get; set; }
        public bool ManageExpired { get; set; }
        public bool ManageSN { get; set; }
        public double MinimumQuantity { get; set; }
        public double MinimumQuantityReorder { get; set; }
        public string No { get; set; }
        public string Notes { get; set; }
        public double PercentTaxable { get; set; }
        public string PreferedVendorName { get; set; }
        public bool PrintDetailGroup { get; set; }
        public string PurchaseRetGIAccountNo { get; set; }
        public double Ratio2 { get; set; }
        public double Ratio3 { get; set; }
        public double Ratio4 { get; set; }
        public double Ratio5 { get; set; }
        public string SalesDiscountGIAccountNo { get; set; }
        public string SalesGIAccountNo { get; set; }
        public string SalesRetGIAccountNo { get; set; }
        public string SerialNumberType { get; set; }
        public bool Subtituted { get; set; }
        public string SubtitutedItemNo { get; set; }
        public string Tax1Name { get; set; }
        public string Tax2Name { get; set; }
        public string Tax3Name { get; set; }
        public string Tax4Name { get; set; }
        public double TypeAutoNumber { get; set; }
        public string UnBilledGIAccountNo { get; set; }
        public string Unit1Name { get; set; }
        public double UnitPrice { get; set; }
        public string Unit2Name { get; set; }
        public double Unit2Price { get; set; }
        public string Unit3Name { get; set; }
        public double Unit3Price { get; set; }
        public string Unit4Name { get; set; }
        public double Unit4Price { get; set; }
        public string Unit5Name { get; set; }
        public double Unit5Price { get; set; }
        public string UpcNo { get; set; }
        public bool UsePPn { get; set; }
        public bool UseWholesalePrice { get; set; }
        public double VendorPrice { get; set; }
        public string VendorUnitName { get; set; }
        public bool IsAccurate { get; set; }
        public virtual IEnumerable<AccuItemDetailGroup> DetailGroup { get; set; }
        public virtual IEnumerable<AccuItemDetailOpenBalance> DetailOpenBalance { get; set; }
    }
}
