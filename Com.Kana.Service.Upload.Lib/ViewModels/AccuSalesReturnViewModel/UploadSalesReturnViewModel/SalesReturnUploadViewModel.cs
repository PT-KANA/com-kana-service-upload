using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.ViewModels.AccuSalesReturnViewModel.UploadSalesReturnViewModel
{
    public class SalesReturnUploadViewModel
    {
		public string customerNo { get; set; }
		public string invoiceNumber { get; set; }
		public string returnType { get; set; }
		public string taxDate { get; set; }
		public string transDate { get; set; }
		public string branchName { get; set; }
		public List<SalesReturnDetailItemViewModel> detailItem { get; set; }
	}

	public class SalesReturnDetailItemViewModel
    {
		public string itemNo { get; set; }
		public double unitPrice { get; set; }
		public double quantity { get; set; }
		public string detailNotes { get; set; }
		public string itemUnitName { get; set; }
		public string warehouseName { get; set; }
    }

}
