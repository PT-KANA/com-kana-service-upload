using Com.Kana.Service.Upload.Lib.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuSalesReturnModel
{
    public class AccuSalesReturnDetailSerialNumber : BaseModel
    {
        public string Status { get; set; }
        public DateTimeOffset ExpiredDate { get; set; }
        public double Quantity { get; set; }
        public string SerialNumberNo { get; set; }
        public virtual long AccuSalesReturnDetailItemId { get; set; }
        [ForeignKey("AccuSalesReturnDetailItemId")]
        public virtual AccuSalesReturnDetailItem AccuSalesReturnDetailItem { get; set; }
    }
}
