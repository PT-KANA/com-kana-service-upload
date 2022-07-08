using Com.Kana.Service.Upload.Lib.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.ViewModels.AccuSalesReturnViewModel
{
    public class AccuSalesReturnViewModel : BaseViewModel
    {
        public string salesOrderNo { get; set; }
        public string customerNo { get; set; }
        public string returnType { get; set; }
        public DateTimeOffset taxDate { get; set; }
        public string taxNumber { get; set; }
        public DateTimeOffset transDate { get; set; }
        public long branchId { get; set; }
        public string branchName { get; set; }
        public string cashDiscPercent { get; set; }
        public double cashDiscount { get; set; }
        public string currencyCode { get; set; }
        public string deliveryOrderNumber { get; set; }
        public string description { get; set; }
        public double fiscalRate { get; set; }
        public string fobName { get; set; }
        public bool inclusiveTax { get; set; }
        public string invoiceNumber { get; set; }
        public string number { get; set; }
        public string paymentTermName { get; set; }
        public double rate { get; set; }
        public string shipmentName { get; set; }
        public bool taxable { get; set; }
        public string toAddress { get; set; }
        public long typeAutoNumber { get; set; }


        public List<AccuSalesReturnDetailExpenseViewModel> detailExpense { get; set; }
        public List<AccuSalesReturnDetailItemViewModel> detailItem { get; set; }
    }
}
