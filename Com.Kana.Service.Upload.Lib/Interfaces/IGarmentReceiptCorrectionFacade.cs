using Com.Kana.Service.Upload.Lib.Models.GarmentReceiptCorrectionModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.Kana.Service.Upload.Lib.Interfaces
{
    public interface IGarmentReceiptCorrectionFacade
    {
        Tuple<List<GarmentReceiptCorrection>, int, Dictionary<string, string>> Read(int Page = 1, int Size = 25, string Order = "{}", string Keyword = null, string Filter = "{}");
        GarmentReceiptCorrection ReadById(int id);
        Task<int> Create(GarmentReceiptCorrection m, string user, int clientTimeZoneOffset = 7);
    }
}
