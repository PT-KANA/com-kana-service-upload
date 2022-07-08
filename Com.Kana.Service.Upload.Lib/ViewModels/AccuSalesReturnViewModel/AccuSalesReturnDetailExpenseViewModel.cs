using Com.Kana.Service.Upload.Lib.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.ViewModels.AccuSalesReturnViewModel
{
    public class AccuSalesReturnDetailExpenseViewModel : BaseViewModel
    {
        public string status { get; set; }
        public string accountNo { get; set; }
        public string departmentName { get; set; }
        public double expenseAmount { get; set; }
        public string expenseName { get; set; }
        public string expenseNotes { get; set; }
        public string salesOrderNumber { get; set; }
        public string salesQuotationNumber { get; set; }
        public virtual long accuSalesReturnId { get; set; }
        public virtual AccuSalesReturnViewModel accuSalesReturn { get; set; }
    }
}
