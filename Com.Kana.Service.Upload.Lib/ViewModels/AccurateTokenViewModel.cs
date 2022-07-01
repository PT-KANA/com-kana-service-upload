using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.ViewModels
{
    public class AccurateTokenViewModel
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string refresh_token { get; set; }
        public double expires_in { get; set; }
        public string scope { get; set; }
        public UserViewModel user { get; set; }
    }

    public class UserViewModel
    {
        public string referrer { get; set; }
        public string name { get; set; }
        public string email { get; set; }
    }
}
