using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.ViewModels
{
    public class AccurateCustomerViewModel
    {
        public string name { get; set; }
        public Dictionary<string, string> branch { get; set; }
        public string customerNo { get; set; }
    }

    public class AccurateSearchCustomerViewModel
    {
        public bool s { get; set; }
        public List<AccurateCustomerViewModel> d { get; set; }
    }
}
