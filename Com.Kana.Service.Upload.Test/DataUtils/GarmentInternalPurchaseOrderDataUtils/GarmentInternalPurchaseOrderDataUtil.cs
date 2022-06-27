using Com.Kana.Service.Upload.Lib.Facades.GarmentInternalPurchaseOrderFacades;
using Com.Kana.Service.Upload.Lib.Models.GarmentInternalPurchaseOrderModel;
using Com.Kana.Service.Upload.Lib.Models.GarmentPurchaseRequestModel;
using Com.Kana.Service.Upload.Test.DataUtils.GarmentPurchaseRequestDataUtils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.Kana.Service.Upload.Test.DataUtils.GarmentInternalPurchaseOrderDataUtils
{
    public class GarmentInternalPurchaseOrderDataUtil
    {
        private readonly GarmentInternalPurchaseOrderFacade facade;
        private readonly GarmentPurchaseRequestDataUtil garmentPurchaseRequestDataUtil;

        public GarmentInternalPurchaseOrderDataUtil(GarmentInternalPurchaseOrderFacade facade, GarmentPurchaseRequestDataUtil garmentPurchaseRequestDataUtil)
        {
            this.facade = facade;
            this.garmentPurchaseRequestDataUtil = garmentPurchaseRequestDataUtil;
        }

        public async Task<List<GarmentInternalPurchaseOrder>> GetNewData(GarmentPurchaseRequest garmentPurchaseRequest = null)
        {
            return await Task.Run(() => garmentPurchaseRequestDataUtil.GetTestDataByTags(garmentPurchaseRequest));
        }

        public async Task<List<GarmentInternalPurchaseOrder>> GetTestData()
        {
            var data = await GetNewData();
            await facade.CreateMultiple(data, "Unit Test");
            return data;
        }

        public async Task<List<GarmentInternalPurchaseOrder>> GetTestDataByTags(List<GarmentInternalPurchaseOrder> data = null)
        {
            //var testData = await GetTestData();
            data = data ?? await GetNewData();
            await facade.CreateMultiple(data, "Unit Test");
            return facade.ReadByTags("accessories", null, DateTimeOffset.MinValue, DateTimeOffset.MinValue, "Unit Test");
        }

    }
}
