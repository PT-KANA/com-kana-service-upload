using Com.Kana.Service.Upload.Lib.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuItemModel
{
    public class AccuItemDetailGroup : BaseModel
    {
        public string Status { get; set; }
        public string DetailName { get; set; }
        public string ItemNo { get; set; }
        public string ItemUnitName { get; set; }
        public double Quantity { get; set; }

        public virtual long AccuItemId { get; set; }
        [ForeignKey("AccuItemId")]
        public virtual AccuItem AccuItem { get; set; }
    }
}
