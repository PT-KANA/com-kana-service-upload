using Com.Kana.Service.Upload.Lib.ViewModels.AccuItemViewModel.AccuItemUploadViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.ViewModels
{
    public class ItemSearchResultViewModel
    {
        public bool s { get; set; }
        public List<ItemSearchResponse> d { get; set; }
        public Page sp { get; set; }
    }

    public class Page
    {
        public long page { get; set; }
        public long pageCount { get; set; }
        public long rowCount { get; set; }
    }

    public class ItemSearchResponse
    {
        public string no { get; set; }
        public string createDate { get; set; }
    }
}
