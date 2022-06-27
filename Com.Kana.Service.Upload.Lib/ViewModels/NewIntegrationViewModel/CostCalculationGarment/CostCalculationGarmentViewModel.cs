using Com.Kana.Service.Upload.Lib.Utilities;
using System.Collections.Generic;

namespace Com.Kana.Service.Upload.Lib.ViewModels.NewIntegrationViewModel.CostCalculationGarment
{
    public class CostCalculationGarmentViewModel : BaseViewModel
    {
        public string RO_Number { get; set; }
        public List<CostCalculationGarment_MaterialViewModel> CostCalculationGarment_Materials { get; set; }
    }
}
