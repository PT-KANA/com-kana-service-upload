using Com.Kana.Service.Upload.Lib.Utilities;
using System.Collections.Generic;

namespace Com.Kana.Service.Upload.Lib.ViewModels.GarmentPOMasterDistributionViewModels
{
    public class GarmentPOMasterDistributionItemViewModel : BaseViewModel
    {
        public long DOItemId { get; set; }
        public long DODetailId { get; set; }

        public long SCId { get; set; }

        public double DOQuantity { get; set; }

        public List<GarmentPOMasterDistributionDetailViewModel> Details { get; set; }
    }
}
