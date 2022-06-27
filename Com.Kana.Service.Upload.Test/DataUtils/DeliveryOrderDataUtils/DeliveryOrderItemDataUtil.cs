using Com.Kana.Service.Upload.Lib.Models.DeliveryOrderModel;
using Com.Kana.Service.Upload.Lib.Models.ExternalPurchaseOrderModel;
using Com.Kana.Service.Upload.Lib.ViewModels.DeliveryOrderViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Kana.Service.Upload.Test.DataUtils.DeliveryOrderDataUtils
{
    public class DeliveryOrderItemDataUtil
    {
        private DeliveryOrderDetailDataUtil deliveryOrderDetailDataUtil;

        public DeliveryOrderItemDataUtil(DeliveryOrderDetailDataUtil deliveryOrderDetailDataUtil)
        {
            this.deliveryOrderDetailDataUtil = deliveryOrderDetailDataUtil;
        }

        public DeliveryOrderItem GetNewData(ExternalPurchaseOrder externalPurchaseOrder)
        {
            return new DeliveryOrderItem
            {
                EPOId = externalPurchaseOrder.Id,
                EPONo = externalPurchaseOrder.EPONo,
                Details = deliveryOrderDetailDataUtil.GetNewData(externalPurchaseOrder.Items.ToList())
            };
        }

        public DeliveryOrderItemViewModel GetNewDataViewModel(ExternalPurchaseOrder externalPurchaseOrder)
        {
            return new DeliveryOrderItemViewModel
            {
                purchaseOrderExternal = new PurchaseOrderExternal
                {
                    _id = externalPurchaseOrder.Id,
                    no = externalPurchaseOrder.EPONo,
                },
                fulfillments = deliveryOrderDetailDataUtil.GetNewDataViewModel(externalPurchaseOrder.Items.ToList())
            };
        }
    }
}
