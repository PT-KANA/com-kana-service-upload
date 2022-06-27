using Com.Kana.Service.Upload.Lib.ViewModels.GarmentReports;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.Interfaces
{
    public interface IBeacukaiNoFeature
    {
        List<BeacukaiNoFeatureViewModel> GetBeacukaiNo(string filter, string keyword);
    }
}
