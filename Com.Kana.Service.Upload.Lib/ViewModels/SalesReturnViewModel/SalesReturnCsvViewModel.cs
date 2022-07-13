using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.ViewModels.SalesReturnViewModel
{
    public class SalesReturnCsvViewModel
    {
        public string transDate { get; set; } //tgl transaksi retur
        public string customerNo { get; set; } // No Identitas Customer
        public string salesOrderNo { get; set; } //No Penjualan
        public string itemNo { get; set; } //Kode Barang
        public string unitPrice { get; set; } //Harga Barang
        public string taxNumber { get; set; } //No Faktur Pajak
        public string taxDate { get; set; } //Tgl Faktur Pajak
        public string returnType { get; set; } //Retur Tipe
        public string detailNotes { get; set; } //Keterangan
        public string quantity { get; set; } //Kuantitas
    }
}
