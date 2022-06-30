using Com.Kana.Service.Upload.Lib.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuSalesInvoiceModel
{
    public class AccuSalesInvoiceDetailSerialNumber : BaseModel
    {
        public string Status { get; set; }
        public DateTimeOffset ExpiredDate { get; set; }
        public double Quantity { get; set; }
        public string SerialNumberNo { get; set; }
        public virtual long AccuSalesInvoceDetailItemId { get; set; }
        [ForeignKey("AccuSalesInvoceDetailItemId")]
        public virtual AccuSalesInvoiceDetailItem AccuSalesInvoiceDetailItem { get; set; }
    }
}
