using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.ViewModels.AccuSalesViewModel
{
	public class AccuSalesInvoiceDetailDownPaymentViewModel
	{
		public string status { get; set; }
		public string invoiceNumber { get; set; }
		public double paymentAmount { get; set; }
		public long accuSalesInvoceId { get; set; } 
		 
	}
}
