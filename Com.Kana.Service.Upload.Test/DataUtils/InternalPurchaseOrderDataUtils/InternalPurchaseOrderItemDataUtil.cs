﻿using System;
using System.Collections.Generic;
using Com.Kana.Service.Upload.Lib.Models.InternalPurchaseOrderModel;
using Com.Kana.Service.Upload.Lib.Models.PurchaseRequestModel;
using Com.Kana.Service.Upload.Lib.ViewModels.IntegrationViewModel;
using Com.Kana.Service.Upload.Lib.ViewModels.InternalPurchaseOrderViewModel;
using Com.Kana.Service.Upload.Test.DataUtils.PurchaseRequestDataUtils;

namespace Com.Kana.Service.Upload.Test.DataUtils.InternalPurchaseOrderDataUtils
{
    public class InternalPurchaseOrderItemDataUtil
    {
        //private InternalPurchaseOrderDataUtil internalPurchaseOrderDataUtil;
        //private PurchaseRequestDataUtil purchaserequestDataUtil;

        public InternalPurchaseOrderItem GetNewData(List<PurchaseRequestItem> purchaseRequestItem) => new InternalPurchaseOrderItem
        {
            ProductId = purchaseRequestItem[0].ProductId,
            ProductCode = purchaseRequestItem[0].ProductCode,
            ProductName = purchaseRequestItem[0].ProductName,
            Quantity = purchaseRequestItem[0].Quantity,
            UomId = purchaseRequestItem[0].UomId,
            UomUnit = purchaseRequestItem[0].Uom,
            ProductRemark = purchaseRequestItem[0].Remark,
            PRItemId = purchaseRequestItem[0].Id
        };
        public InternalPurchaseOrderItemViewModel GetNewDataViewModel(List<PurchaseRequestItem> purchaseRequestItem) => new InternalPurchaseOrderItemViewModel
        {
            product = new ProductViewModel
            {
                _id = purchaseRequestItem[0].ProductId,
                code = purchaseRequestItem[0].ProductCode,
                name = purchaseRequestItem[0].ProductName,
                uom = new UomViewModel
                {
                    _id = purchaseRequestItem[0].UomId,
                    unit = purchaseRequestItem[0].Uom,
                }
            },
            quantity = purchaseRequestItem[0].Quantity,
            productRemark = purchaseRequestItem[0].Remark
        };
    }
}
