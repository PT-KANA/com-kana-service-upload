using Com.Kana.Service.Upload.Lib.ViewModels.AccuItemViewModel.AccuItemUploadViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.ViewModels.AccuItemViewModel.ResponseViewModel
{
    public class ItemSearchResultViewModel
    {
        public bool s { get; set; }
        public List<ItemUploadViewModel> d { get; set; }
        public Page sp { get; set; }
    }

    public class Page
    {
        public long page { get; set; }
        public long pageCount { get; set; }
        public long rowCount { get; set; }
    }
}
