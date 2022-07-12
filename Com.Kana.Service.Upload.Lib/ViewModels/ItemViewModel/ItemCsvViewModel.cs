using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.ViewModels.ItemViewModel
{
    public class ItemCsvViewModel
    {
        public string handle { get; set; } //route
        public string title { get; set; } //name
        //public string body { get; set; }
        public string vendor { get; set; } //creator

        //public string standardizedProductType { get; set; }
        //public string customProductType { get; set; }
        //public string tags { get; set; }
        //public string published { get; set; }
        //public string option1Name { get; set; }
        public string option1Value { get; set; } //size

        //public string option2Name { get; set; }
        //public string option2Value { get; set; }
        //public string option3Name { get; set; }
        //public string option3Value { get; set; }

        public string variantSKU { get; set; }
        public string variantGrams { get; set; } //berat
        public string variantInventoryTracker { get; set; } //inventory
        public string variantInventoryQty { get; set; }

        //public string variantInventoryPolicy { get; set; }
        //public string variantFulfillmentService { get; set; }

        public string variantPrice { get; set; } //price

        //public string VariantCompareAtPrice { get; set; }
        //public string VariantRequiresShipping { get; set; }
        public string variantTaxable { get; set; } //tax
        public string variantBarcode { get; set; } //barcode

        //public string imageSrc { get; set; }
        //public string imagePosition { get; set; }
        //public string imageAltText { get; set; }
        //public string giftCard { get; set; }
        //public string seoTitle { get; set; }
        //public string seoDescription { get; set; }
        //public string googleShoppingProductCategory { get; set; }
        //public string googleShoppingGender { get; set; }
        //public string googleShoppingAgeGroup { get; set; }
        //public string googleShoppingMPN { get; set; }
        //public string googleShoppingAdWordsGrouping { get; set; }
        //public string googleShoppingAdWordsLabels { get; set; }
        //public string googleShoppingCondition { get; set; }
        //public string googleShoppingCustomProduct { get; set; }
        //public string googleShoppingCustomLabel0 { get; set; }
        //public string googleShoppingCustomLabel1 { get; set; }
        //public string googleShoppingCustomLabel2 { get; set; }
        //public string googleShoppingCustomLabel3 { get; set; }
        //public string googleShoppingCustomLabel4 { get; set; }
        //public string variantImage { get; set; }

        public string variantWeightUnit { get; set; } //uom
        public string variantTaxCode { get; set; } //kode pajak
        public string costPeritem { get; set; } //COGS

        //public string status { get; set; }
    }
}
