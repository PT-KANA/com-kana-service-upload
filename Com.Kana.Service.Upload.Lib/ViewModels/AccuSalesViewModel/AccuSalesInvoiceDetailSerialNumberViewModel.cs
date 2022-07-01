using Com.Kana.Service.Upload.Lib.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.ViewModels.AccuSalesViewModel
{
	public class AccuSalesInvoiceDetailSerialNumberViewModel : BaseViewModel
	{
		public string status { get; set; }
		public DateTimeOffset expiredDate { get; set; }
		public double quantity { get; set; }
		public string serialNumberNo { get; set; }
		public virtual long accuSalesInvoceDetailItemId { get; set; }
		public virtual AccuSalesInvoiceDetailItemViewModel accuSalesInvoiceDetailItem { get; set; }
	}
}
