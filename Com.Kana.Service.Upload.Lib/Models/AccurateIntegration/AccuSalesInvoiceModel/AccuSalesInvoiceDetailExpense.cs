using Com.Kana.Service.Upload.Lib.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuSalesInvoiceModel
{
    public class AccuSalesInvoiceDetailExpense : BaseModel
    {
        public string Status { get; set; }
        public string AccountNo { get; set; }
        public string DepartementName { get; set; }
        public double ExpenseAmount { get; set; }
        public string ExpenseName { get; set; }
        public string ExpenseNotes { get; set; }
        public string SalesOrderNumber { get; set; }
        public string SalesQuotationNumber { get; set; }
        public virtual long AccuSalesInvoceId { get; set; }
        [ForeignKey("AccuSalesInvoceId")]
        public virtual AccuSalesInvoice AccuSalesInvoice { get; set; }

    }
}
