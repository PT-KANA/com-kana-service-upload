using Com.DanLiris.Service.Purchasing.Lib.ViewModels.AccuItemViewModel;
using Com.Kana.Service.Upload.Lib.Facades;
using Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuItemModel;
using Com.Kana.Service.Upload.Lib.ViewModels.AccuItemViewModel.AccuItemUploadViewModel;
using Com.Kana.Service.Upload.Lib.ViewModels.ItemViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.Kana.Service.Upload.Test.DataUtils.ItemDataUtils
{
	public class ItemDataUtil
	{
		private readonly ItemFacade itemFacade;

		public ItemDataUtil(ItemFacade facade/*, GarmentInternalPurchaseOrderDataUtil garmentPurchaseOrderDataUtil*/)
		{
			this.itemFacade = facade;
			//this.garmentPurchaseOrderDataUtil = garmentPurchaseOrderDataUtil;
		}
		public AccuItem GetNewData()
		{
			//var datas = await Task.Run(() => garmentPurchaseOrderDataUtil.GetTestDataByTags());
			return new AccuItem
			{
				ItemType = "type",
				Name = "title",
				No = "no",
				UpcNo = "upcNo",
				Unit1Name = "unit1Name",
				UnitPrice = 500,
				UsePPn = false,
				VendorPrice = 500,
				VendorUnitName = "vendor",
				PreferedVendorName = "preferedVendorName",
				SerialNumberType = "serialNumberType",
				DetailOpenBalance = new List<AccuItemDetailOpenBalance>
				{
					new AccuItemDetailOpenBalance
					{
						AsOf = DateTimeOffset.Now,
						Quantity = 5,
						ItemUnitName = "itemUnitName",
						WarehouseName = "warehouseName",
						UnitCost = 5000
					}
				}
			};
		 
		}

		
		public class ItemDataUtilViewModel
		{
			private readonly ItemFacade facade;

			public ItemDataUtilViewModel(ItemFacade facade)
			{
				this.facade = facade;

			}
			public AccuItemViewModel GetNewDataValid()
			{
				//var datas = await Task.Run(() => garmentPurchaseOrderDataUtil.GetTestDataByTags());
				return new AccuItemViewModel
				{
					itemType = "INVENTORY",
					name = "title",
					unit1Name = "PCS",
					unitPrice = 500,
					upcNo = "barcode",
					no = "245783",
					serialNumberType = "UNIQUE",
					usePPn = false,
					preferedVendorName = "vendor",
					vendorPrice = 100,
					vendorUnitName = "PCS"
				};
			}
		}
		public class ItemDataUtilCSV
		{
			private readonly ItemFacade facade;

			public ItemDataUtilCSV(ItemFacade facade)
			{
				this.facade = facade;

			}
			public ItemCsvViewModel GetNewDataValid()
			{
				//var datas = await Task.Run(() => garmentPurchaseOrderDataUtil.GetTestDataByTags());
				return new ItemCsvViewModel
				{
					variantBarcode = "245783",
					costPeritem="5000",
					vendor="vendor",
					variantInventoryQty="5",
					variantSKU="sku",
					title="title",
					handle="handle"


				};
			}
			public ItemCsvViewModel GetNewData1()
			{
				//var datas = await Task.Run(() => garmentPurchaseOrderDataUtil.GetTestDataByTags());
				return new ItemCsvViewModel
				{
					variantBarcode = "",
					costPeritem = "5000",
					vendor = "vendor",
					variantInventoryQty = "a",
					variantSKU = "sku",
					title = "",
					handle = "handle",
					variantPrice="aasag"


				};
			}
			public ItemCsvViewModel GetNewData2()
			{
				//var datas = await Task.Run(() => garmentPurchaseOrderDataUtil.GetTestDataByTags());
				return new ItemCsvViewModel
				{
					variantBarcode = "",
					costPeritem = "5000",
					vendor = "vendor",
					variantInventoryQty = "",
					variantSKU = "sku",
					title = "",
					handle = "handle",
					variantPrice = "-1"


				};
			}
		}
			public async Task<AccuItem> GetTestData()
			{
				var data = GetNewData();
				List<AccuItem> accuItems = new List<AccuItem>();
				accuItems.Add(data);
				await itemFacade.UploadData(accuItems, "Unit Test");
				return data;
			}
		 
	}
}
