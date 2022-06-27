using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.ViewModels.UnitPaymentCorrectionNoteViewModel
{
    public class CorrectionState
    {
        public bool IsHavingQuantityCorrection { get; set; }
        public bool IsHavingPricePerUnitCorrection { get; set; }
        public bool IsHavingPriceTotalCorrection { get; set; }
    }
}
