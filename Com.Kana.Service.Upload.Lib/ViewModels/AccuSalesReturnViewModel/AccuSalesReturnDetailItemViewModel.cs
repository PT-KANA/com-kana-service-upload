using Com.Kana.Service.Upload.Lib.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.ViewModels.AccuSalesReturnViewModel
{
    public class AccuSalesReturnDetailItemViewModel : BaseViewModel
    {
        public string itemNo { get; set; }
        public double unitPrice { get; set; }
        public string status { get; set; }
        public double controlQuantity { get; set; }
        public string deliveryOrderNumber { get; set; }
        public string departmentName { get; set; }
        public string detailName { get; set; }
        public string detailNotes { get; set; }
        public double itemCashDiscount { get; set; }
        public string itemDiscPercent { get; set; }
        public string itemUnitName { get; set; }
        public string projectNo { get; set; }
        public double quantity { get; set; }
        public string salesOrderNumber { get; set; }
        public string salesQuotationNumber { get; set; }
        public string salesmanListNumber { get; set; }
        public bool useTax1 { get; set; }
        public bool useTax2 { get; set; }
        public bool useTax3 { get; set; }
        public string warehouseName { get; set; }
        public List<AccuSalesReturnDetailSerialNumberViewModel> detailSerialNumber { get; set; }
        public virtual long accuSalesReturnId { get; set; }

        //public virtual AccuSalesReturnViewModel accuSalesReturn { get; set; }
    }
}
