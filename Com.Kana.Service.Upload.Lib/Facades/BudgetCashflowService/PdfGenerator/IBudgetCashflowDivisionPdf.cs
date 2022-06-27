using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.Facades.BudgetCashflowService.PdfGenerator
{
    public interface IBudgetCashflowDivisionPdf
    {
        MemoryStream Generate(int divisionId, DateTimeOffset dueDate);
    }
}
