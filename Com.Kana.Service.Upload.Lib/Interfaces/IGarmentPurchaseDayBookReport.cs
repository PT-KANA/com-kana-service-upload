﻿using Com.Kana.Service.Upload.Lib.ViewModels.GarmentReports;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.Interfaces
{
    public interface IGarmentPurchaseDayBookReport
    {
        Tuple<List<GarmentPurchaseDayBookReportViewModel>, int> GetReport(string unit, bool supplier, string jnsbyr, string category, int Year, int offset, string order, int page, int size);
        MemoryStream GenerateExcel(string unit, bool supplier, string jnsbyr, string category, int Year, int offset);
    }
}
