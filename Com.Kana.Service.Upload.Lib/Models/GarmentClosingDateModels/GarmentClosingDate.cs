using Com.Kana.Service.Upload.Lib.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.Models.GarmentClosingDateModels
{
    public class GarmentClosingDate : BaseModel
    {
        public DateTimeOffset CloseDate { get; set; }
    }
}
