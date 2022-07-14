using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.ViewModels.AccuSalesViewModel
{
	public class AccuSalesReceiptDetailInvoiceViewModel
	{
		public string invoiceNo { get; set; }
		public double paymentAmount { get; set; }
		public List<AccuSalesReceiptDetailDiscountViewModel> detailDiscount { get; set; }
	}
}
