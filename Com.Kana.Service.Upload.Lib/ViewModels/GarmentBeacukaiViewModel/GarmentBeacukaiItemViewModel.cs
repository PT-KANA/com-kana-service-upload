using Com.Kana.Service.Upload.Lib.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.ViewModels.GarmentBeacukaiViewModel
{
	public class GarmentBeacukaiItemViewModel : BaseViewModel
	{
		public Com.Kana.Service.Upload.Lib.ViewModels.GarmentDeliveryOrderViewModel.GarmentDeliveryOrderViewModel deliveryOrder { get; set; }
		public string billNo { get; set; }
		public double quantity { get; set; }
		public bool selected { get; set; }



	}
}
