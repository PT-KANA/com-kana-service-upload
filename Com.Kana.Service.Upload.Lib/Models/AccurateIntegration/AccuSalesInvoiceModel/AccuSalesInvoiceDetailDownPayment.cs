using Com.Kana.Service.Upload.Lib.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuSalesInvoiceModel
{
    public class AccuSalesInvoiceDetailDownPayment : BaseModel
    {
        public string Status { get; set; }
        public string InvoiceNumber { get; set; }
        public double PaymentAmount { get; set; }
        public virtual long AccuSalesInvoceId { get; set; }
        [ForeignKey("AccuSalesInvoceId")]
        public virtual AccuSalesInvoice AccuSalesInvoice { get; set; }
    }
}
