using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.ViewModels.AccuSalesViewModel
{
	public class AccuSalesUploadViewModel
	{
		public string customerNo { get; set; }
		public string orderDownPaymentNumber { get; set; }
		public bool reverseInvoice { get; set; }
		public double cashDiscount { get; set; }
		public string taxDate { get; set; }
		public string taxNumber { get; set; }
		public string transDate { get; set; }
		public string number { get; set; }
		public string branchName { get; set; }
		public string saveAsStatusType { get; set; }
		public List<AccuSalesInvoiceDetailItemUploadViewModel> detailItem { get; set; }

	}
}
