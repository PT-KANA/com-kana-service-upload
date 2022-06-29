using Com.Kana.Service.Upload.Lib.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.ViewModels.AccuItemViewModel
{
    public class AccuItemDetailOpenBalanceViewModel : BaseViewModel
    {
        public string status { get; set; }
        public DateTimeOffset asOf { get; set; }
        public string itemUnitName { get; set; }
        public double quantity { get; set; }
        public double unitCost { get; set; }
        public string warehouseName { get; set; }
        public List<AccuItemDetailSerialNumberViewModel> detailSerialNumber { get; set; }
        public long accuItemId { get; set; }
    }
}
