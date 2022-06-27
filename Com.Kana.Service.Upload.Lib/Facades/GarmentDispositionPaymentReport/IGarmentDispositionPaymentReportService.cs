using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.Facades.GarmentDispositionPaymentReport
{
    public interface IGarmentDispositionPaymentReportService
    {
        List<GarmentDispositionPaymentReportDto> GetReportByDate(DateTimeOffset startDate, DateTimeOffset endDate);
        List<GarmentDispositionPaymentReportDto> GetReportByDispositionIds(List<int> dispositionIds);
    }
}
