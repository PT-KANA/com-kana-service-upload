using Com.Kana.Service.Upload.Lib.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using Com.Kana.Service.Upload.Lib.ViewModels.GarmentDeliveryOrderViewModel;

namespace Com.Kana.Service.Upload.Lib.ViewModels.GarmentInvoiceViewModels
{
    public class GarmentInvoiceItemViewModel : BaseViewModel
    {
        public Com.Kana.Service.Upload.Lib.ViewModels.GarmentDeliveryOrderViewModel.GarmentDeliveryOrderViewModel deliveryOrder { get; set; }
        public List<GarmentInvoiceDetailViewModel> details { get; set; }
	}
}
