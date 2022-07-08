using Com.Kana.Service.Upload.Lib.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuSalesReturnModel
{
    public class AccuSalesReturn : BaseModel
    {
        public string CustomerNo { get; set; }
        public string ReturnType { get; set; }
        public DateTimeOffset TaxDate { get; set; }
        public string TaxNumber { get; set; }
        public DateTimeOffset TransDate { get; set; }
        public long BranchId { get; set; }
        public string BranchName { get; set; }
        public string CashDiscPercent { get; set; }
        public double CashDiscount { get; set; }
        public string CurrencyCode { get; set; }
        public string DeliveryOrderNumber { get; set; }
        public string Description { get; set; }
        public double FiscalRate { get; set; }
        public string FobName { get; set; }
        public bool InclusiveTax { get; set; }
        public string InvoiceNumber { get; set; }
        public string Number { get; set; }
        public string PaymentTermName { get; set; }
        public double Rate { get; set; }
        public string ShipmentName { get; set; }
        public bool Taxable { get; set; }
        public string ToAddress { get; set; }
        public long TypeAutoNumber { get; set; }
        public bool IsAccurate { get; set; }
        public virtual IEnumerable<AccuSalesReturnDetailExpense> DetailExpense { get; set; }
        public virtual IEnumerable<AccuSalesReturnDetailItem> DetailItem { get; set; }
    }
}
