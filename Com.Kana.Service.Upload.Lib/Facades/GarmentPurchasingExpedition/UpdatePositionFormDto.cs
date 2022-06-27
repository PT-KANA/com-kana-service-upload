using Com.Kana.Service.Upload.Lib.Enums;
using System.Collections.Generic;

namespace Com.Kana.Service.Upload.Lib.Facades.GarmentPurchasingExpedition
{
    public class UpdatePositionFormDto
    {
        public List<int> Ids { get; set; }
        public PurchasingGarmentExpeditionPosition Position { get; set; }
    }
}