using AutoMapper;
using Com.Kana.Service.Upload.Lib.Helpers;
using Com.Kana.Service.Upload.Lib.Interfaces;
using Com.Kana.Service.Upload.Lib.Interfaces.SalesUploadInterface;
using Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuSalesInvoiceModel;
using Com.Kana.Service.Upload.Lib.ViewModels;
using Com.Kana.Service.Upload.Lib.ViewModels.AccuSalesViewModel;
using Com.Kana.Service.Upload.Lib.ViewModels.SalesViewModel;
using Com.Moonlay.Models;
using Com.Moonlay.NetCore.Lib;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Com.Kana.Service.Upload.Lib.Facades
{
	public class SalesUploadFacade : ISalesUpload
	{
		private string USER_AGENT = "Facade";
		protected readonly IHttpClientService _http;
		private readonly UploadDbContext dbContext;
		private readonly DbSet<AccuSalesInvoice> dbSet;
		public readonly IServiceProvider serviceProvider;
		public object Request { get; private set; }
		public object ApiVersion { get; private set; }
		public readonly IIntegrationFacade facade;
		private readonly IMapper mapper;
		public SalesUploadFacade(IServiceProvider serviceProvider, UploadDbContext dbContext, IIntegrationFacade facade, IMapper mapper)
		{
			this.serviceProvider = serviceProvider;
			this.dbContext = dbContext;
			this.dbSet = dbContext.Set<AccuSalesInvoice>();
			this.facade = facade;
			this.mapper = mapper;

		}

		public List<string> CsvHeader { get; } = new List<string>()
		{
			"Barcode","Name", "Email",    "Financial Status", "Paid at",  "Fulfillment Status",   "Fulfilled at", "Accepts Marketing",    "Currency", "Subtotal", "Shipping", "Taxes",    "Total",    "Discount Code",    "Discount Amount",  "Shipping Method",  "Created at",   "Lineitem quantity",    "Lineitem name",    "Lineitem price",   "Lineitem compare at price",    "Lineitem sku", "Lineitem requires shipping",   "Lineitem taxable", "Lineitem fulfillment status",  "Billing Name", "Billing Street",   "Billing Address1", "Billing Address2", "Billing Company",  "Billing City", "Billing Zip",  "Billing Province", "Billing Country",  "Billing Phone",    "Shipping Name",    "Shipping Street",  "Shipping Address1",    "Shipping Address2",    "Shipping Company", "Shipping City",    "Shipping Zip", "Shipping Province",    "Shipping Country", "Shipping Phone",   "Notes",    "Note Attributes",  "Cancelled at", "Payment Method",   "Payment Reference",    "Refunded Amount",  "Vendor",   "Outstanding Balance",  "Employee", "Location", "Device ID",    "Id",   "Tags", "Risk Level",   "Source",   "Lineitem discount",    "Tax 1 Name",   "Tax 1 Value",  "Tax 2 Name",   "Tax 2 Value",  "Tax 3 Name",   "Tax 3 Value",  "Tax 4 Name",   "Tax 4 Value",  "Tax 5 Name",   "Tax 5 Value",  "Phone",    "Receipt Number",   "Duties",   "Billing Province Name",    "Shipping Province Name",   "Payment ID",   "Payment Terms Name",   "Next Payment Due At"
		};



		public async Task<List<AccuSalesViewModel>> MapToViewModel(List<SalesCsvViewModel> csv)
		{
			List<AccuSalesViewModel> item = new List<AccuSalesViewModel>();
			List<AccuSalesInvoiceDetailItemViewModel> detailItemViewModels = new List<AccuSalesInvoiceDetailItemViewModel>();
			List<string> tempNo = new List<string>();
			foreach (var i in csv)
			{

				var isSameSales = tempNo.FirstOrDefault(s => s == i.name);
				if (isSameSales == null)
				{
					tempNo.Add(i.name);
					AccuSalesViewModel ii = new AccuSalesViewModel
					{
						customerNo = string.IsNullOrWhiteSpace(i.billingName) ? "CUST" : i.billingName,
						orderDownPaymentNumber = i.name,
						documentCode="INVOICE",
						taxType= "PPN_TDK_DIPUNGUT",
						number = i.name,
						branchName = i.location,
						CreatedUtc = Convert.ToDateTime(i.createdAt),
						cashDiscount = Convert.ToDouble(i.discountAmount),
						transDate1 = Convert.ToDateTime(i.createdAt),
						transDate = Convert.ToDateTime(i.createdAt).ToShortDateString(),
						reverseInvoice = false,
						shipDate1= Convert.ToDateTime(i.createdAt),
						taxDate1 = Convert.ToDateTime(i.createdAt),
						taxable = Convert.ToBoolean(i.lineitemtaxable),
						currencyCode = i.currency,
						isAccurate = false,
						financialStatus = i.financialStatus,
						detailItem = new List<AccuSalesInvoiceDetailItemViewModel>()
						{
						 new AccuSalesInvoiceDetailItemViewModel()
						{
							unitPrice= Convert.ToDouble(i.lineitemPrice),
							quantity= Convert.ToDouble(i.lineItemQuantity),
							itemNo=i.barcode,
							itemUnitName="PCS"

						 }
						}
					};

					item.Add(ii);
				}
				else
				{
					var b = new AccuSalesInvoiceDetailItemViewModel()
					{
						unitPrice = Convert.ToDouble(i.lineitemPrice),
						quantity = Convert.ToDouble(i.lineItemQuantity),
						itemNo = i.barcode

					};

					AccuSalesViewModel header = item.Where(a => a.orderDownPaymentNumber == i.name).FirstOrDefault();

					header.detailItem.Add(b);
				}
			}
			return item;
		}

		public async Task<List<AccuSalesInvoice>> MapToModel(List<AccuSalesViewModel> data1)
		{
			List<AccuSalesInvoice> salesInvoices = new List<AccuSalesInvoice>();


			foreach (var i in data1.Where(s=>s.financialStatus =="paid" || s.financialStatus == "Paid"))
			{
				List<AccuSalesInvoiceDetailItem> invoiceDetailItems = new List<AccuSalesInvoiceDetailItem>();


				foreach (var ii in i.detailItem)
				{
					var dd = new AccuSalesInvoiceDetailItem()
					{
						UnitPrice = ii.unitPrice,
						Quantity = ii.quantity,
						ItemNo = ii.itemNo,
					};
					invoiceDetailItems.Add(dd);

				};
				AccuSalesInvoice accuSales = new AccuSalesInvoice
				{
					OrderDownPaymentNumber = i.orderDownPaymentNumber,
					Number  = i.number ,
					ReverseInvoice = i.reverseInvoice,
					TaxDate = i.CreatedUtc,
					TaxNumber = i.taxNumber,
					TransDate = i.transDate1,
					CustomerNo = i.customerNo,
					BranchName = i.branchName,
					CurrencyCode = i.currencyCode,
					IsAccurate = i.isAccurate,
					DetailItem = invoiceDetailItems,
					CashDiscount= i.cashDiscount
				};

				salesInvoices.Add(accuSales);
			}

			return salesInvoices;
		}
		public Tuple<bool, List<object>> UploadValidate(ref List<SalesCsvViewModel> data, List<KeyValuePair<string, StringValues>> list)
		{
			List<object> ErrorList = new List<object>();
			string ErrorMessage;
			bool Valid = true;
			IQueryable<AccuSalesInvoice> Query = this.dbSet.Include(x => x.DetailItem);

			foreach (SalesCsvViewModel item in data)
			{
				ErrorMessage = "";

				if (string.IsNullOrWhiteSpace(item.name))
				{
					ErrorMessage = string.Concat(ErrorMessage, "No Penjualan Tidak Boleh Kosong, ");
				}
				if (string.IsNullOrWhiteSpace(item.barcode))
				{
					ErrorMessage = string.Concat(ErrorMessage, "Barcode Tidak Boleh Kosong, ");
				}
				var isExist = Query.Where(s => s.OrderDownPaymentNumber == item.name);
				if (isExist.Count() > 0)
				{
					ErrorMessage = string.Concat(ErrorMessage, "No Penjualan sudah ada, ");
				}

				if (!string.IsNullOrEmpty(ErrorMessage))
				{
					ErrorMessage = ErrorMessage.Remove(ErrorMessage.Length - 2);
					var Error = new ExpandoObject() as IDictionary<string, object>;
					Error.Add("No Penjualan", item.name);
					Error.Add("Error", ErrorMessage);
					ErrorList.Add(Error);
				}
			}

			if (ErrorList.Count > 0)
			{
				Valid = false;
			}

			return Tuple.Create(Valid, ErrorList);
		}


		public async Task UploadData(List<AccuSalesInvoice> data, string username)
		{
			foreach (var i in data)
			{
				EntityExtension.FlagForCreate(i, username, USER_AGENT);
				foreach (var iii in i.DetailItem)
				{
					EntityExtension.FlagForCreate(iii, username, USER_AGENT);

				}
				
				dbSet.Add(i);
			}
			var result = await dbContext.SaveChangesAsync();
		}
		public sealed class SalesInvoiceMap : CsvHelper.Configuration.ClassMap<SalesCsvViewModel>
		{
			public SalesInvoiceMap()
			{
				Map(p => p.barcode ).Index(0);
				Map(p => p.name).Index(1);
				Map(p => p.financialStatus).Index(3);
				Map(p => p.paidAt).Index(4);
				Map(p => p.currency).Index(8);
				Map(p => p.taxes).Index(11);
				Map(p => p.total).Index(12);
				Map(p => p.discountAmount).Index(14);
				Map(p => p.createdAt).Index(16);
				Map(p => p.lineItemQuantity).Index(17);
				Map(p => p.lineItemName).Index(18);
				Map(p => p.lineitemPrice).Index(19);
				Map(p => p.lineitemsku).Index(21);
				Map(p => p.lineitemtaxable).Index(23);
				Map(p => p.billingName).Index(25);
				Map(p => p.isRefund).Index(50);
				Map(p => p.location).Index(54);
			}
		}
		public Tuple<List<AccuSalesInvoice>, int, Dictionary<string, string>> ReadForUpload(int Page = 1, int Size = 25, string Order = "{}", string Keyword = null, string Filter = "{}")
		{
			IQueryable<AccuSalesInvoice> Query = this.dbSet.Include(x => x.DetailExpense).Include(x => x.DetailItem);

			List<string> searchAttributes = new List<string>()
			{
				"CustomerNo", "Number","BranchName"
			};

			Query = QueryHelper<AccuSalesInvoice>.ConfigureSearch(Query, searchAttributes, Keyword);

			Dictionary<string, string> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Filter);
			Query = QueryHelper<AccuSalesInvoice>.ConfigureFilter(Query, FilterDictionary);

			Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Order);
			Query = QueryHelper<AccuSalesInvoice>.ConfigureOrder(Query, OrderDictionary);

			Pageable<AccuSalesInvoice> pageable = new Pageable<AccuSalesInvoice>(Query, Page - 1, Size);
			List<AccuSalesInvoice> Data = pageable.Data.ToList<AccuSalesInvoice>();
			int TotalData = pageable.TotalCount;

			return Tuple.Create(Data, TotalData, OrderDictionary);
		}
		public Tuple<List<AccuSalesInvoice>, int, Dictionary<string, string>> ReadForApproved(int Page = 1, int Size = 25, string Order = "{}", string Keyword = null, string Filter = "{}")
		{
			IQueryable<AccuSalesInvoice> Query = this.dbSet.Where(s=>s.IsAccurate== true && s.IsAccurateReceipt == false).Include(x => x.DetailExpense).Include(x => x.DetailItem);

			List<string> searchAttributes = new List<string>()
			{
				"CustomerNo", "Number","BranchName"
			};

			Query = QueryHelper<AccuSalesInvoice>.ConfigureSearch(Query, searchAttributes, Keyword);

			Dictionary<string, string> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Filter);
			Query = QueryHelper<AccuSalesInvoice>.ConfigureFilter(Query, FilterDictionary);

			Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Order);
			Query = QueryHelper<AccuSalesInvoice>.ConfigureOrder(Query, OrderDictionary);

			Pageable<AccuSalesInvoice> pageable = new Pageable<AccuSalesInvoice>(Query, Page - 1, Size);
			List<AccuSalesInvoice> Data = pageable.Data.ToList<AccuSalesInvoice>();
			int TotalData = pageable.TotalCount;

			return Tuple.Create(Data, TotalData, OrderDictionary);
		}
		private async Task<AccurateSessionViewModel> OpenDb()
		{
			var httpClient = new HttpClient();

			var url = "https://account.accurate.id/api/open-db.do?id=578154";

			using (var request = new HttpRequestMessage(HttpMethod.Get, url))
			{
				request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AuthCredential.AccessToken);

				var response = await httpClient.SendAsync(request);

				response.EnsureSuccessStatusCode();

				if (response.IsSuccessStatusCode)
				{
					var message = response.Content.ReadAsStringAsync().Result;
					AccurateSessionViewModel AccuSession = JsonConvert.DeserializeObject<AccurateSessionViewModel>(message);
					return AccuSession;

				}
				else
				{
					var message = response.Content.ReadAsStringAsync().Result;
					return null;
				}
			}
		}
		public async Task Create(List<AccuSalesViewModel> dataviewModel,string username)
		{
			var session = facade.OpenDb();
			List<AccuSalesUploadViewModel> data = new List<AccuSalesUploadViewModel>();
			List<AccuSalesInvoiceDetailItemUploadViewModel> dataDetail = new List<AccuSalesInvoiceDetailItemUploadViewModel>();
			var httpClient = new HttpClient();
			var url = session.Result.host + "/accurate/api/sales-invoice/save.do";
			 
			foreach (var i in dataviewModel)
			{
			 
				
				var detail = from a in i.detailItem select a;
				foreach (var d in detail)
				{
					var detailItem=  new AccuSalesInvoiceDetailItemUploadViewModel
					{
							itemNo= d.itemNo,
							unitPrice = d.unitPrice,
							quantity= d.quantity
					};
					dataDetail.Add(detailItem);
				}
				AccuSalesUploadViewModel accuSalesUploadView = new AccuSalesUploadViewModel
				{
					saveAsStatusType= "UNAPPROVED",
					branchName = "JAKARTA",
					customerNo = "C.00004",
					number = i.number,
					orderDownPaymentNumber = i.number,
					reverseInvoice = i.reverseInvoice,
					taxDate = Convert.ToDateTime( i.transDate).Date.ToShortDateString(),
					transDate = Convert.ToDateTime(i.transDate).Date.ToShortDateString(),
					taxNumber = i.taxNumber,
					detailItem = dataDetail,
					cashDiscount = i.cashDiscount
				};

				var dataToBeSend = JsonConvert.SerializeObject(accuSalesUploadView);

				var content = new StringContent(dataToBeSend, Encoding.UTF8, General.JsonMediaType);
			 
				using (var request = new HttpRequestMessage(HttpMethod.Post, url))
				{
					request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AuthCredential.AccessToken);
					request.Headers.Add("X-Session-ID", session.Result.session);
					request.Content = content;

					var response = await httpClient.SendAsync(request);
					var res = response.Content.ReadAsStringAsync().Result;
					var message = JsonConvert.DeserializeObject<AccurateResponseViewModel>(res);

					if (response.IsSuccessStatusCode && message.s)
					{
						AccuSalesInvoice invoice = (from a in dbContext.AccuSalesInvoices
													where a.Number == i.number
													select a).FirstOrDefault();
						invoice.IsAccurate = true;

						EntityExtension.FlagForUpdate(invoice, username, USER_AGENT);
					}
				}
				
			}
			await dbContext.SaveChangesAsync();
		}
		public async Task CreateSalesReceipt(List<AccuSalesViewModel> dataviewModel, string username)
		{
			var session = facade.OpenDb();
			List<AccuSalesReceiptViewModel> data = new List<AccuSalesReceiptViewModel>();
			List<AccuSalesReceiptDetailInvoiceViewModel> detailInvoice = new List<AccuSalesReceiptDetailInvoiceViewModel>();
			List<AccuSalesReceiptDetailDiscountViewModel> detailDiscount = new List<AccuSalesReceiptDetailDiscountViewModel>();
			var httpClient = new HttpClient();
			double totalPayment = 0;
			var url = session.Result.host + "/accurate/api/sales-receipt/save.do";

			foreach (var i in dataviewModel)
			{

				var detail = from a in i.detailItem select a;
				foreach (var d in detail)
				{
					var detailDiscounts = new AccuSalesReceiptDetailDiscountViewModel
					{
						accountNo = "422.000-01",
						amount = d.itemCashDiscount

					};
					detailDiscount.Add(detailDiscounts);

					var detailItem = new AccuSalesReceiptDetailInvoiceViewModel
					{
						invoiceNo= i.number,
						detailDiscount= detailDiscount,
						paymentAmount = d.unitPrice * d.quantity

					};
					totalPayment += d.unitPrice * d.quantity;
					detailInvoice.Add(detailItem);
				}
				AccuSalesReceiptViewModel accuSalesUploadView = new AccuSalesReceiptViewModel
				{
					bankNo ="12356489",
					branchName = "JAKARTA",
					customerNo = "C.00004",
					number = i.number,
					chequeAmount= totalPayment,
					transDate = Convert.ToDateTime(i.transDate).Date.ToShortDateString(),
					detailInvoice= detailInvoice
				};

				var dataToBeSend = JsonConvert.SerializeObject(accuSalesUploadView);

				var content = new StringContent(dataToBeSend, Encoding.UTF8, General.JsonMediaType);

				using (var request = new HttpRequestMessage(HttpMethod.Post, url))
				{
					request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AuthCredential.AccessToken);
					request.Headers.Add("X-Session-ID", session.Result.session);
					request.Content = content;

					var response = await httpClient.SendAsync(request);
					var res = response.Content.ReadAsStringAsync().Result;
					var message = JsonConvert.DeserializeObject<AccurateResponseViewModel>(res);

					if (response.IsSuccessStatusCode && message.s)
					{

						AccuSalesInvoice invoice = (from a in dbContext.AccuSalesInvoices
													where a.Number == i.number
													select a).FirstOrDefault();
						invoice.IsAccurateReceipt = true;

						EntityExtension.FlagForUpdate(invoice, username, USER_AGENT);
					}
					//else
					//{
					//	throw new Exception("data " + i.number + " gagal diupload");
					//}
				}
			}
			await dbContext.SaveChangesAsync();
		}
		
	}
}