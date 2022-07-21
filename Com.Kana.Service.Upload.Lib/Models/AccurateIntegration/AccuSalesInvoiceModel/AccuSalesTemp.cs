using Com.Kana.Service.Upload.Lib.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuSalesInvoiceModel
{
	public class AccuSalesTemp :BaseModel
	{
		public string Number { get; set; }
		public DateTimeOffset TransDate { get; set; }
	}
}
