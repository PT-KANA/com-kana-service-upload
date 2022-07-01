using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.ViewModels.SalesViewModel
{
	public class SalesCsvViewModel
	{
		public string name { get; set; }//no penjualan
		public string paidAt { get; set; } //tgl bayar?
		public string createdAt { get; set; } //tanggal Penjualan
		public string total { get; set; } //tanggal Penjualan
		public string discountAmount { get; set; }
		public string lineItemName { get; set; } //nama  barang + size
		public string billingName { get; set; } //cust
		public string location { get; set; } //branch 
		public string lineItemQuantity { get; set; }
		public string lineitemPrice { get; set; } //harga barang
		public string lineitemsku { get; set; } //nama barang
		public string lineitemtaxable { get; set; } //kena pajak or not
		public string taxes { get; set; }//nilai pajak
		public string currency { get; set; }//nilai pajak
		public string isRefund { get; set; }






	}
}
