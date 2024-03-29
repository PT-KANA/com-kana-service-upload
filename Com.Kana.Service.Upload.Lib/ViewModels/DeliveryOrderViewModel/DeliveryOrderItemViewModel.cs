﻿using Com.Kana.Service.Upload.Lib.Utilities;
using System.Collections.Generic;

namespace Com.Kana.Service.Upload.Lib.ViewModels.DeliveryOrderViewModel
{
    public class DeliveryOrderItemViewModel : BaseViewModel
    {
        public PurchaseOrderExternal purchaseOrderExternal { get; set; }
        public List<DeliveryOrderFulFillMentViewModel> fulfillments { get; set; }
    }

    public class PurchaseOrderExternal
    {
        public long _id { get; set; }
        public string no { get; set; }
    }
}
