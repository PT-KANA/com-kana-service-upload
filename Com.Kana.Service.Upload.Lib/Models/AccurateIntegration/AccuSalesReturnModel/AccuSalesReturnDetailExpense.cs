using Com.Kana.Service.Upload.Lib.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuSalesReturnModel
{
    public class AccuSalesReturnDetailExpense : BaseModel
    {
        public string Status { get; set; }
        public string AccountNo { get; set; }
        public string DepartmentName { get; set; }
        public double ExpenseAmount { get; set; }
        public string ExpenseName { get; set; }
        public string ExpenseNotes { get; set; }
        public string SalesOrderNumber { get; set; }
        public string SalesQuotationNumber { get; set; }
        public virtual long AccuSalesReturnId { get; set; }
        [ForeignKey("AccuSalesReturnId")]
        public virtual AccuSalesReturn AccuSalesReturn { get; set; }
    }
}
