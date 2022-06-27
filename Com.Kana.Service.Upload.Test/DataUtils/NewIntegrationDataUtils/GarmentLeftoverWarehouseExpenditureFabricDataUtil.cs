using Com.Kana.Service.Upload.Lib.ViewModels.NewIntegrationViewModel;
using Com.Kana.Service.Upload.WebApi.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Kana.Service.Upload.Test.DataUtils.NewIntegrationDataUtils
{
    public class GarmentLeftoverWarehouseExpenditureFabricDataUtil
    {
        public GarmentLeftoverWarehouseExpenditureFabricViewModel GetNewData()
        {
            long nowTicks = DateTimeOffset.Now.Ticks;

            var data = new GarmentLeftoverWarehouseExpenditureFabricViewModel
            {
                Id = 1,
                ExpenditureNo = "exNo",
                IsUsed = false,
                Items = new List<GarmentLeftoverWarehouseExpenditureFabricItemViewModel>
                {
                    new GarmentLeftoverWarehouseExpenditureFabricItemViewModel
                    {
                        Quantity=1,
                    }
                }
            };
            return data;
        }

        public Dictionary<string, object> GetResultFormatterOk()
        {
            var data = GetNewData();

            Dictionary<string, object> result =
                new ResultFormatter("1.0", General.OK_STATUS_CODE, General.OK_MESSAGE)
                .Ok(data);

            return result;
        }

        public string GetResultFormatterOkString()
        {
            var result = GetResultFormatterOk();

            return JsonConvert.SerializeObject(result);
        }
    }
}
