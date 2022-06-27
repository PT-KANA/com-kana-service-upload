using Com.Kana.Service.Upload.Lib.Helpers.ReadResponse;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.Interfaces
{
    public interface IPPHBankExpenditureNoteReportFacade
    {
        ReadResponse<object> GetReport(int Size, int Page, string No, string UnitPaymentOrderNo, string InvoiceNo, string SupplierCode, DateTimeOffset? DateFrom, DateTimeOffset? DateTo, int Offset);
    }
}
