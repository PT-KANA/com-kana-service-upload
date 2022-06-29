using Com.Kana.Service.Upload.Lib.Utilities;
using System;

namespace Com.Kana.Service.Upload.Lib.ViewModels.AccuItemViewModel
{
    public class AccuItemDetailSerialNumberViewModel : BaseViewModel
    {
        public string status { get; set; }
        public DateTimeOffset expiredDate { get; set; }
        public double quantity { get; set; }
        public string serialNumberNo { get; set; }

        public long accuItemDetailOpenBalanceId { get; set; }
    }
}