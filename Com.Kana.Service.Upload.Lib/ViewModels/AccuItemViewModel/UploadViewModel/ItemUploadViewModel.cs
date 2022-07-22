using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.ViewModels.AccuItemViewModel.AccuItemUploadViewModel
{
    public class ItemUploadViewModel
    {
        public string itemType { get; set; }
        public string name { get; set; }
        public string no { get; set; }
        public string preferedVendorName { get; set; }
        public string serialNumberType { get; set; }
        public string unit1Name { get; set; }
        public double unitPrice { get; set; }
        public string upcNo { get; set; }
        public bool usePPn { get; set; }
        public double vendorPrice { get; set; }
        public string vendorUnitName { get; set; }
    }
}
