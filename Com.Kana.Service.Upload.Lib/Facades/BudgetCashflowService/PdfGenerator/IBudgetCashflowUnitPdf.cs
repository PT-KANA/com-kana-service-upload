using System;
using System.IO;

namespace Com.Kana.Service.Upload.Lib.Facades.BudgetCashflowService.PdfGenerator
{
    public interface IBudgetCashflowUnitPdf
    {
        MemoryStream Generate(int unitId, DateTimeOffset dueDate);
    }
}
