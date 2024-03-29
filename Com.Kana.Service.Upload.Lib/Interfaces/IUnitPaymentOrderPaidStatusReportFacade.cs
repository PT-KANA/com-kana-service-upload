﻿using Com.Kana.Service.Upload.Lib.Helpers.ReadResponse;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.Interfaces
{
    public interface IUnitPaymentOrderPaidStatusReportFacade
    {
        ReadResponse<object> GetReport(int Size, int Page, string Order, string UnitPaymentOrderNo, string SupplierCode, string DivisionCode, string SupplierType, string PaymentMethod, string Status, DateTimeOffset? DateFromDue, DateTimeOffset? DateToDue, DateTimeOffset? DateFrom, DateTimeOffset? DateTo, int Offset);
    }
}
