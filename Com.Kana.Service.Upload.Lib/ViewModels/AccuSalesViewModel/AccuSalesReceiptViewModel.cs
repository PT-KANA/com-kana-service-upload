using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.ViewModels.AccuSalesViewModel
{
	public class AccuSalesReceiptViewModel
	{
		public string customerNo { get; set; }
		public string bankNo { get; set; }
		public double chequeAmount { get; set; }
		public string transDate { get; set; }
		public string number { get; set; }
		public string branchName { get; set; }
		public List<AccuSalesReceiptDetailInvoiceViewModel> detailInvoice { get; set; }

	}
}
