using Com.Kana.Service.Upload.Lib.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuSalesReturnModel
{
	public class AccuSalesReturnTemp :BaseModel
	{
		public string Number { get; set; }
		public string InvoiceNumber { get; set; }
		public DateTime CreateDate { get; set; }
	}
}
