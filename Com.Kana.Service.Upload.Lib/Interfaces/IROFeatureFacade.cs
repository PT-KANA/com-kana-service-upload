using Com.Kana.Service.Upload.Lib.ViewModels.GarmentReports;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.Interfaces
{
    public interface IROFeatureFacade
    {
        Tuple<List<ROFeatureViewModel>, int> GetROReport(int offset, string RO, int page, int size, string Order);
    }
}
