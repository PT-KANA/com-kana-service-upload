using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.ViewModels.AccuSalesViewModel
{
	public class AccuSalesInvoiceDetailExpenseViewModel
	{
		public string status { get; set; }
		public string accountNo { get; set; }
		public string departementName { get; set; }
		public double expenseAmount { get; set; }
		public string expenseName { get; set; }
		public string expenseNotes { get; set; }
		public string salesOrderNumber { get; set; }
		public string salesQuotationNumber { get; set; }
		public   long accuSalesInvoceId { get; set; }
		 
	}
}
