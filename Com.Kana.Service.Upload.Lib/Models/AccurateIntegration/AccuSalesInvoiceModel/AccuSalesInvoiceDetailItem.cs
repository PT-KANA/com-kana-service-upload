using Com.Kana.Service.Upload.Lib.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuSalesInvoiceModel
{
    public class AccuSalesInvoiceDetailItem : BaseModel
    {
        public string ItemNo { get; set; }
        public double UnitPrice { get; set; }
        public string Status { get; set; }
        public double ControlQuantity { get; set; }
        public string DeliveryOrderNumber { get; set; }
        public string DepartmentName { get; set; }
        public string DetailName { get; set; }
        public string DetailNotes { get; set; }
        public double ItemCashDiscount { get; set; }
        public string ItemDiscPercent { get; set; }
        public string ItemUnitName { get; set; }
        public string ProjectNo { get; set; }
        public double Quantity { get; set; }
        public string SalesOrderNumber { get; set; }
        public string SalesQuotationNumber { get; set; }
        public string SalesmanListNumber { get; set; }
        public bool UseTax1 { get; set; }
        public bool UseTax2 { get; set; }
        public bool UseTax3 { get; set; }
        public string WarehouseName { get; set; }
        public virtual IEnumerable<AccuSalesInvoiceDetailSerialNumber> DetailSerialNumber { get; set; }
        public virtual long AccuSalesInvoceId { get; set; }
        [ForeignKey("AccuSalesInvoceId")]
        public virtual AccuSalesInvoice AccuSalesInvoice { get; set; }


    }
}
