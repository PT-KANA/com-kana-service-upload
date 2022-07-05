using Com.Kana.Service.Upload.Lib.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuSalesReturnModel
{
    public class AccuSalesReturnDetailItem : BaseModel
    {
        public string ItemNo { get; set; }
        public double UnitPrice { get; set; }
        public string Status { get; set; }
        public string DepartmentName { get; set; }
        public string DetailName { get; set; }
        public string DetailNotes { get; set; }
        public double ItemCashDiscount { get; set; }
        public string ItemDiscPercent { get; set; }
        public string ItemUnitName { get; set; }
        public string ProjectNo { get; set; }
        public double Quantity { get; set; }
        public bool UseTax1 { get; set; }
        public bool UseTax2 { get; set; }
        public bool UseTax3 { get; set; }
        public string WarehouseName { get; set; }

        public virtual IEnumerable<AccuSalesReturnDetailSerialNumber> DetailSerialNumber { get; set; }
        public virtual long AccuSalesReturnId { get; set; }
        [ForeignKey("AccuSalesReturnId")]
        public virtual AccuSalesReturn AccuSalesReturn { get; set; }
    }
}
