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

		public SalesUploadFacade(IServiceProvider serviceProvider, UploadDbContext dbContext)
		{
			this.serviceProvider = serviceProvider;
			this.dbContext = dbContext;
			this.dbSet = dbContext.Set<AccuSalesInvoice>();
			 
		}

		public List<string> CsvHeader { get; } = new List<string>()
		{
			"Name", "Email",    "Financial Status", "Paid at",  "Fulfillment Status",   "Fulfilled at", "Accepts Marketing",    "Currency", "Subtotal", "Shipping", "Taxes",    "Total",    "Discount Code",    "Discount Amount",  "Shipping Method",  "Created at",   "Lineitem quantity",    "Lineitem name",    "Lineitem price",   "Lineitem compare at price",    "Lineitem sku", "Lineitem requires shipping",   "Lineitem taxable", "Lineitem fulfillment status",  "Billing Name", "Billing Street",   "Billing Address1", "Billing Address2", "Billing Company",  "Billing City", "Billing Zip",  "Billing Province", "Billing Country",  "Billing Phone",    "Shipping Name",    "Shipping Street",  "Shipping Address1",    "Shipping Address2",    "Shipping Company", "Shipping City",    "Shipping Zip", "Shipping Province",    "Shipping Country", "Shipping Phone",   "Notes",    "Note Attributes",  "Cancelled at", "Payment Method",   "Payment Reference",    "Refunded Amount",  "Vendor",   "Outstanding Balance",  "Employee", "Location", "Device ID",    "Id",   "Tags", "Risk Level",   "Source",   "Lineitem discount",    "Tax 1 Name",   "Tax 1 Value",  "Tax 2 Name",   "Tax 2 Value",  "Tax 3 Name",   "Tax 3 Value",  "Tax 4 Name",   "Tax 4 Value",  "Tax 5 Name",   "Tax 5 Value",  "Phone",    "Receipt Number",   "Duties",   "Billing Province Name",    "Shipping Province Name",   "Payment ID",   "Payment Terms Name",   "Next Payment Due At"
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
						branchName = i.location,
						CreatedUtc = Convert.ToDateTime(i.createdAt),
						cashDiscount = Convert.ToDouble(i.discountAmount),
						transDate = Convert.ToDateTime(i.createdAt),
						reverseInvoice = false,
						taxDate = Convert.ToDateTime(i.createdAt),
						taxable = Convert.ToBoolean(i.lineitemtaxable),
						currencyCode = i.currency,
						isAccurate=false,
						detailItem = new List<AccuSalesInvoiceDetailItemViewModel>()
						{
						 new AccuSalesInvoiceDetailItemViewModel()
						{
							unitPrice= Convert.ToDouble(i.lineitemPrice),
							quantity= Convert.ToDouble(i.lineItemQuantity),
							itemNo=i.lineItemName

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
						itemNo = i.lineItemName

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
		

			foreach (var i in data1)
			{
				List<AccuSalesInvoiceDetailItem> invoiceDetailItems = new List<AccuSalesInvoiceDetailItem>();

				
				foreach (var ii in i.detailItem)
				{
					var dd = new AccuSalesInvoiceDetailItem()
					{
						UnitPrice = ii.unitPrice,
						Quantity = ii.quantity,
						ItemNo = ii.itemNo

					};
					invoiceDetailItems.Add(dd);
					 
				};
				AccuSalesInvoice accuSales = new AccuSalesInvoice
				{
					OrderDownPaymentNumber = i.orderDownPaymentNumber,
					ReverseInvoice = i.reverseInvoice,
					TaxDate = i.CreatedUtc,
					TaxNumber = i.taxNumber,
					TransDate = i.transDate,
					CustomerNo = i.customerNo,
					BranchName = i.branchName,
					CurrencyCode = i.currencyCode,
					IsAccurate=i.isAccurate,
					DetailItem=invoiceDetailItems
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
				var isExist = Query.Where(s => s.OrderDownPaymentNumber == item.name);
				if (isExist.Count() >0)
				{
					ErrorMessage = string.Concat(ErrorMessage, "No Penjualan sudah ada, ");
				}

				if (!string.IsNullOrEmpty(ErrorMessage))
				{
					ErrorMessage = ErrorMessage.Remove(ErrorMessage.Length - 2);
					var Error = new ExpandoObject() as IDictionary<string, object>;
					Error.Add("No Penjualan", item.name );
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

					//foreach (var iv in iii.DetailSerialNumber)
					//{
					//	EntityExtension.FlagForCreate(iv, username, USER_AGENT);
					//}
				}
				//foreach (var ii in i.DetailExpense)
				//{
				//	EntityExtension.FlagForCreate(ii, username, USER_AGENT);
				//}
				//foreach (var ii in i.DetailDownPayment)
				//{
				//	EntityExtension.FlagForCreate(ii, username, USER_AGENT);
				//}
				dbSet.Add(i);
			}
			var result = await dbContext.SaveChangesAsync();
		}
		public sealed class SalesInvoiceMap : CsvHelper.Configuration.ClassMap<SalesCsvViewModel>
		{
			public SalesInvoiceMap()
			{

				Map(p => p.name).Index(0);
				Map(p => p.paidAt).Index(3);
				Map(p => p.currency).Index(7);
				Map(p => p.taxes).Index(10);
				Map(p => p.total).Index(11);
				Map(p => p.discountAmount).Index(13);
				Map(p => p.createdAt).Index(15);
				Map(p => p.lineItemQuantity).Index(16);
				Map(p => p.lineItemName).Index(17);
				Map(p => p.lineitemPrice).Index(18);
				Map(p => p.lineitemsku).Index(20);
				Map(p => p.lineitemtaxable).Index(22);
				Map(p => p.billingName).Index(24);
				Map(p => p.isRefund).Index(49);
				Map(p => p.location).Index(53);
			}
		}
		public Tuple<List<AccuSalesInvoice>, int, Dictionary<string, string>> ReadForUpload(int Page = 1, int Size = 25, string Order = "{}", string Keyword = null, string Filter = "{}")
		{
			IQueryable<AccuSalesInvoice> Query = this.dbSet.Include(x => x.DetailExpense).Include(x => x.DetailDownPayment).Include(x => x.DetailItem);
			  
			List<string> searchAttributes = new List<string>()
			{
				"CustomerNo", "OrderDownPaymentNumber","BranchName"
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
		public async Task CreateRead(List<AccuSalesViewModel> data, string username, string token)
		{
			var httpClient = new HttpClient();

			//var url = "https://public.accurate.id/accurate/api/sales-invoice/list.do";
			var url = username+ "/api/sales-invoice/list.do";


			using (var request = new HttpRequestMessage(HttpMethod.Get, url))
			{
				request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AuthCredential.AccessToken);
				request.Headers.Add("X-Session-ID", token);
				 

				var response = await httpClient.SendAsync(request);
				//response.EnsureSuccessStatusCode();

				if (response.IsSuccessStatusCode)
				{
					var message = response.Content.ReadAsStringAsync().Result;
				}
				else
				{
					var message = response.Content.ReadAsStringAsync().Result;
					//	return message;
				}
			}

		}

			public async Task Create(List<AccuSalesViewModel> data, string username, string token)
		{
			var httpClient = new HttpClient();

			var url = "https://account.accurate.id/api/open-db.do?id=578154";
			using (var request = new HttpRequestMessage(HttpMethod.Get, url))
			{
				request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AuthCredential.AccessToken);

				var response = await httpClient.SendAsync(request);
				//response.EnsureSuccessStatusCode();

				if (response.IsSuccessStatusCode)
				{
					var message = response.Content.ReadAsStringAsync().Result;
					AccurateSessionViewModel AccuToken = JsonConvert.DeserializeObject<AccurateSessionViewModel>(message);
					await CreateRead(data, AccuToken.host, AccuToken.session);

				}
				else
				{
					var message = response.Content.ReadAsStringAsync().Result;
					//	return message;
				}
			}
		}
	}
}