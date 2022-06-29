using Com.Kana.Service.Upload.Lib.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.ViewModels.AccuItemViewModel
{
    public class AccuItemDetailGroupViewModel : BaseViewModel
    {
        public string status { get; set; }
        public string detailName { get; set; }
        public string itemNo { get; set; }
        public string itemUnitName { get; set; }
        public double quantity { get; set; }
        public long accuItemId { get; set; }
    }
}
