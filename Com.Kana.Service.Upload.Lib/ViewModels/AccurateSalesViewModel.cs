using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.ViewModels
{
	public class AccurateSalesViewModel
	{
		
		public string number { get; set; }
		public string transDate { get; set; }



		public class AccurateSearchSalesViewModel
		{
			public bool s { get; set; }
			public List<AccurateSalesViewModel> d { get; set; }
		}
	}
}
