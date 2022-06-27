using Com.Kana.Service.Upload.Lib.Utilities;
using Com.Kana.Service.Upload.Lib.ViewModels.GarmentInvoiceViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.ViewModels.GarmentInternNoteViewModel
{
    public class GarmentInternNoteItemViewModel : BaseViewModel
    {
        public GarmentInvoiceViewModel garmentInvoice { get; set; }
        public List<GarmentInternNoteDetailViewModel> details { get; set; }
    }
}
