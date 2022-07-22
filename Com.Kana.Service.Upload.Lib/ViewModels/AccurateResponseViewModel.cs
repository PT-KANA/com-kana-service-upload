using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.ViewModels
{
    public class AccurateResponseViewModel
    {
        public bool s { get; set; }
		public List<string> d { get; set; }
	}

    public class AccurateResponseSpViewModel
    {
        public long page { get; set; }
        public string sort { get; set; }
        public long pageSize { get; set; }
        public long pageCount { get; set; }
        public long rowCount { get; set; }
    }
}
