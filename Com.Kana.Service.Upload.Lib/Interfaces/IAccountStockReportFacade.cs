﻿using Com.Kana.Service.Upload.Lib.ViewModels.GarmentReports;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Com.Kana.Service.Upload.Lib.Interfaces
{
    public interface IAccountingStockReportFacade
    {
        Tuple<List<AccountingStockReportViewModel>, int> GetStockReport(int offset, string unitcode, string tipebarang, int page, int size, string Order, DateTime? dateFrom, DateTime? dateTo);
        MemoryStream GenerateExcelAStockReport(string ctg, string categoryname, string unitcode, string unitname, DateTime? datefrom, DateTime? dateto, int offset);
    }
}
