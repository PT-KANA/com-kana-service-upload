using System;
using System.IO;

namespace Com.Kana.Service.Upload.Lib.Facades.BudgetCashflowService.ExcelGenerator
{
    public interface IBudgetCashflowUnitExcelGenerator
    {
        MemoryStream Generate(int unitId, DateTimeOffset dueDate);
    }
}
