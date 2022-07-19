using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.ViewModels
{
	public class AccurateGeneralAccountViewModel
	{
		public string no { get; set; }
		 
	}

	public class AccurateSearchGAViewModel
	{
		public bool s { get; set; }
		public List<AccurateGeneralAccountViewModel> d { get; set; }
	}
}
