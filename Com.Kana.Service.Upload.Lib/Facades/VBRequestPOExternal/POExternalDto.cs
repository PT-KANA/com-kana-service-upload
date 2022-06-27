using AutoMapper.QueryableExtensions;
using Com.Kana.Service.Upload.Lib.Models.ExternalPurchaseOrderModel;
using Com.Kana.Service.Upload.Lib.Models.GarmentExternalPurchaseOrderModel;
using Com.Kana.Service.Upload.Lib.Models.GarmentInternalPurchaseOrderModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Kana.Service.Upload.Lib.Facades.VBRequestPOExternal
{
    public class POExternalDto
    {

        public POExternalDto(ExternalPurchaseOrder entity)
        {
            Id = (int)entity.Id;
            No = entity.EPONo;
            Items = entity.Items.SelectMany(element => element.Details).Select(element => new POExternalItemDto(element, entity)).ToList();
        }

        //public POExternalDto(GarmentExternalPurchaseOrder entity, List<long> poIds, List<GarmentInternalPurchaseOrder> purchaseOrders)
        //{
        //    Id = (int)entity.Id;
        //    No = entity.EPONo;
        //    Items = entity.Items.Select(element => new POExternalItemDto(element, entity, purchaseOrders)).ToList();
        //}

        public POExternalDto(GarmentExternalPurchaseOrder entity, List<GarmentInternalPurchaseOrder> purchaseOrders)
        {
            Id = (int)entity.Id;
            No = entity.EPONo;
            Items = entity.Items.Select(element => 
            {
                var purchaseOrder = purchaseOrders.FirstOrDefault(po => po.Id == element.POId);
                return new POExternalItemDto(element, entity, purchaseOrder);
            }).ToList();
        }

        public int Id { get; private set; }
        public string No { get; private set; }
        public List<POExternalItemDto> Items { get; private set; }
    }
}
