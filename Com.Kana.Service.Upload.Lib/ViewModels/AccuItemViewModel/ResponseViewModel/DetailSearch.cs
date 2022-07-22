using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.ViewModels.AccuItemViewModel.ResponseViewModel
{
    public class DetailSearch
    {
        public string fields { get; set; }
        public Filter filter { get; set; }
        public Sp sp { get; set; }
    }
    public class Filter
    {
        public Val lastUpdate { get; set; }
    }
    public class Val
    {
        public string op { get; set; }
        public List<string> val { get; set; }
    }

    public class Sp
    {
        public long page { get; set; }
        public long pageSize { get; set; }
        public string sort { get; set; }
    }
}
