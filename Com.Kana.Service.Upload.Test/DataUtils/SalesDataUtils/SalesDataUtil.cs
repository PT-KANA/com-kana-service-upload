using Com.Kana.Service.Upload.Lib.Facades;
using Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuSalesInvoiceModel;
using Com.Kana.Service.Upload.Lib.ViewModels;
using Com.Kana.Service.Upload.Lib.ViewModels.AccuSalesViewModel;
using Com.Kana.Service.Upload.Lib.ViewModels.SalesViewModel;
using Com.Kana.Service.Upload.WebApi.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.Kana.Service.Upload.Test.DataUtils.SalesDataUtils
{
	public class SalesDataUtil
	{
		private readonly SalesUploadFacade SalesUploadFacade;

		public SalesDataUtil(SalesUploadFacade facade/*, GarmentInternalPurchaseOrderDataUtil garmentPurchaseOrderDataUtil*/)
		{
			this.SalesUploadFacade = facade;
			//this.garmentPurchaseOrderDataUtil = garmentPurchaseOrderDataUtil;
		}
		public AccuSalesInvoice GetNewData()
		{
			List<AccuSalesInvoiceDetailItem> invoiceDetailItems = new List<AccuSalesInvoiceDetailItem>();
			var dd = new AccuSalesInvoiceDetailItem()
			{
				UnitPrice = 5000,
				Quantity = 2,
				ItemNo = "itemno",
			};
			invoiceDetailItems.Add(dd);
			//var datas = await Task.Run(() => garmentPurchaseOrderDataUtil.GetTestDataByTags());
			return new AccuSalesInvoice
			{
				OrderDownPaymentNumber = "orderDownPaymentNumber",
				Number = "number",
				ReverseInvoice = false,
				TaxDate = DateTime.Now,
				TaxNumber = "taxNumber",
				TransDate = DateTime.Now,
				CustomerNo = "customerNo",
				BranchName = "branchName",
				CurrencyCode = "currencyCode",
				IsAccurate = false , 
				DetailItem = invoiceDetailItems,
				CashDiscount = 0
			};

		}
		public AccurateSessionViewModel GetDataSession()
		{
			 
			var accurateSessions = new AccurateSessionViewModel()
			{
				host="host",
				session="Result"
			};
			return accurateSessions;
		}
		public AccurateResponseViewModel GetDataResponse()
		{

			var accurateSessions = new AccurateResponseViewModel()
			{
				s = true,
				d = new List<string>()
				{

				}
			};
			return accurateSessions;
		}
		public class SalesDataUtilViewModel
		{
			private readonly SalesUploadFacade facade;

			public SalesDataUtilViewModel(SalesUploadFacade facade)
			{
				this.facade = facade;

			}
			public AccuSalesViewModel GetNewDataValid()
			{
				//var datas = await Task.Run(() => garmentPurchaseOrderDataUtil.GetTestDataByTags());
				return new AccuSalesViewModel
				{
					 
					branchName = "JAKARTA",
					customerNo = "C.00004",
					number = "number",
					orderDownPaymentNumber = "number",
					reverseInvoice = false,
					taxDate = DateTime.Now.Date.ToShortDateString(),
					transDate = DateTime.Now.Date.ToShortDateString(),
					taxNumber = "taxNumber",
					financialStatus="paid",
					detailItem = new List<AccuSalesInvoiceDetailItemViewModel>()
						{
						 new AccuSalesInvoiceDetailItemViewModel()
						{
							unitPrice= 5000,
							quantity= 5,
							itemNo="barcode",
							itemUnitName="PCS"

						 }
						},
					cashDiscount = 0
				};
			}
		}
		public class SalesDataUtilCSV
		{
			private readonly SalesUploadFacade facade;

			public SalesDataUtilCSV(SalesUploadFacade facade)
			{
				this.facade = facade;

			}
			public SalesCsvViewModel GetNewDataValid()
			{
				//var datas = await Task.Run(() => garmentPurchaseOrderDataUtil.GetTestDataByTags());
				return new SalesCsvViewModel
				{
					createdAt=DateTimeOffset.Now.ToString(),
					barcode="barode",
					billingName="name",
					discountAmount="100"


				};
			}
			public SalesCsvViewModel GetNewData1()
			{
				//var datas = await Task.Run(() => garmentPurchaseOrderDataUtil.GetTestDataByTags());
				return new SalesCsvViewModel
				{
					barcode = "barode",
					billingName = "name",
					discountAmount = "100"

				};
			}
			public SalesCsvViewModel GetNewData2()
			{
				//var datas = await Task.Run(() => garmentPurchaseOrderDataUtil.GetTestDataByTags());
				return new SalesCsvViewModel
				{
					barcode = "",
					billingName = "name",
					discountAmount = "100"

				};
			}
		}
		public async Task<AccuSalesInvoice> GetTestData()
		{
			var data = GetNewData();
			List<AccuSalesInvoice> AccuSalesInvoices = new List<AccuSalesInvoice>();
			AccuSalesInvoices.Add(data);
			await SalesUploadFacade.UploadData(AccuSalesInvoices, "Unit Test");
			return data;
		}

		
		public string GetResultFormatterOkString()
		{
			var result = GetResultFormatterOk();

			return JsonConvert.SerializeObject(result);
		}

		public Dictionary<string, object> GetResultFormatterOk()
		{
			var data = GetDataSession();

			Dictionary<string, object> result =
				new ResultFormatter("1.0", General.OK_STATUS_CODE, General.OK_MESSAGE)
				.Ok(data);

			return result;
		}
		public string GetResponseOkString()
		{
			var result = GetResponseOk();

			return JsonConvert.SerializeObject(result);
		}

		public AccurateResponseViewModel GetResponseOk()
		{
			var data = GetDataResponse();

		 
			return data;
		}

	}
}