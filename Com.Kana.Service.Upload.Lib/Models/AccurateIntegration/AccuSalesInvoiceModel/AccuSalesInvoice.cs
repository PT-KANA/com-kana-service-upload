using Com.Kana.Service.Upload.Lib.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuSalesInvoiceModel
{
    public class AccuSalesInvoice : BaseModel
    {
        public string CustomerNo { get; set; }
        public string OrderDownPaymentNumber { get; set; }
        public bool ReverseInvoice { get; set; }
        public DateTimeOffset TaxDate { get; set; }
        public string TaxNumber { get; set; }
        public DateTimeOffset TransDate { get; set; }
        public long BranchId { get; set; }
        public string BranchName { get; set; }
        public string CashDiscPercent { get; set; }
        public double CashDiscount { get; set; }
        public string CurrencyCode { get; set; }
        public string Description { get; set; }
        public string DocumentCode { get; set; }
        public double FiscalRate { get; set; }
        public string FobName { get; set; }
        public bool InclusiveTax { get; set; }
        public double InputDownPayment { get; set; }
        public bool InvoiceDp { get; set; }
        public string Number { get; set; }
        public string PaymentTermName { get; set; }
        public string PoNumber { get; set; }
        public double Rate { get; set; }
        public string RetailIdCard { get; set; }
        public string RetailWpName { get; set; }
        public DateTimeOffset ShipDate { get; set; }
        public string ShipmentName { get; set; }
        public string Tax1Name { get; set; }
        public string TaxType { get; set; }
        public bool Taxable { get; set; }
        public string ToAddress { get; set; }
        public long TypeAutoNumber { get; set; }
        public virtual IEnumerable<AccuSalesInvoiceDetailDownPayment> DetailDownPayment { get; set; }
        public virtual IEnumerable<AccuSalesInvoiceDetailExpense> DetailExpense { get; set; }
        public virtual IEnumerable<AccuSalesInvoiceDetailItem> DetailItem { get; set; }


    }
}
