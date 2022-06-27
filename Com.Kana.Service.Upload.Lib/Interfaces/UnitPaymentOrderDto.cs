using System;

namespace Com.Kana.Service.Upload.Lib.Interfaces
{
    public class UnitPaymentOrderDto
    {
        public UnitPaymentOrderDto(int id, string unitPaymentOrderNo, DateTimeOffset unitPaymentOrderDate)
        {
            Id = id;
            UnitPaymentOrderNo = unitPaymentOrderNo;
            UnitPaymentOrderDate = unitPaymentOrderDate;
        }

        public int Id { get; set; }
        public string UnitPaymentOrderNo { get; set; }
        public DateTimeOffset UnitPaymentOrderDate { get; set; }
    }
}