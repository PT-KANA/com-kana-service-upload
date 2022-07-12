using Com.Kana.Service.Upload.Lib.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.ViewModels.AccuSalesViewModel
{
	public class AccuSalesViewModel : BaseViewModel
	{
		public string customerNo { get; set; }
		public string orderDownPaymentNumber { get; set; }
		public bool reverseInvoice { get; set; }
		public DateTimeOffset taxDate1 { get; set; }
		public string taxDate { get; set; }
		public string taxNumber { get; set; }
		public DateTimeOffset transDate1 { get; set; }
		public string transDate { get; set; }
		public long branchId { get; set; }
		public string branchName { get; set; }
		public string cashDiscPercent { get; set; }
		public double cashDiscount { get; set; }
		public string currencyCode { get; set; }
		public string description { get; set; }
		public string documentCode { get; set; }
		public double fiscalRate { get; set; }
		public string fobName { get; set; }
		public bool inclusiveTax { get; set; }
		public double inputDownPayment { get; set; }
		public bool invoiceDp { get; set; }
		public string number { get; set; }
		public string paymentTermName { get; set; }
		public string poNumber { get; set; }
		public double rate { get; set; }
		public string retailIdCard { get; set; }
		public string retailWpName { get; set; }
		public DateTimeOffset shipDate1 { get; set; }
		public string shipDate { get; set; }
		public string shipmentName { get; set; }
		public string tax1Name { get; set; }
		public string taxType { get; set; }
		public bool taxable { get; set; }
		public bool isAccurate { get; set; }
		public string toAddress { get; set; }
		public string financialStatus { get; set; }
		public long typeAutoNumber { get; set; }
		//public List<AccuSalesInvoiceDetailDownPaymentViewModel> detailDownPayment { get; set; }
		//public List<AccuSalesInvoiceDetailExpenseViewModel> detailExpense { get; set; }
		public List<AccuSalesInvoiceDetailItemViewModel> detailItem { get; set; }
	}
}
