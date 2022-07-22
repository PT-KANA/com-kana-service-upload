using AutoMapper;
using Com.Kana.Service.Upload.Lib.Helpers;
using Com.Kana.Service.Upload.Lib.Interfaces;
using Com.Kana.Service.Upload.Lib.Interfaces.SalesReturnInterface;
using Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuSalesReturnModel;
using Com.Kana.Service.Upload.Lib.ViewModels;
using Com.Kana.Service.Upload.Lib.ViewModels.AccuSalesReturnViewModel;
using Com.Kana.Service.Upload.Lib.ViewModels.AccuSalesReturnViewModel.UploadSalesReturnViewModel;
using Com.Kana.Service.Upload.Lib.ViewModels.SalesReturnViewModel;
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
    public class SalesReturnUploadFacade : ISalesReturnUpload
    {
        private string USER_AGENT = "Facade";

        private readonly UploadDbContext dbContext;
        private readonly DbSet<AccuSalesReturn> dbSet;
        public readonly IServiceProvider serviceProvider;
        public readonly IIntegrationFacade facade;
        private readonly IMapper mapper;

        public object Request { get; private set; }
        public object ApiVersion { get; private set; }

        public SalesReturnUploadFacade(IServiceProvider serviceProvider, IIntegrationFacade integration, UploadDbContext dbContext, IMapper mapper)
        {
            this.serviceProvider = serviceProvider;
            this.facade = integration;
            this.dbContext = dbContext;
            this.dbSet = dbContext.Set<AccuSalesReturn>();
            this.mapper = mapper;
        }

        public List<string> CsvHeader { get; } = new List<string>()
        {
            "Tanggal Retur", "No Identitas Customer", "No Penjualan", "Kode Barang", "Harga Barang", "No Faktur Pajak", "Tanggal Faktur Pajak", "Kuantitas", "Keterangan"
        };

        public async Task<List<AccuSalesReturnViewModel>> MapToViewModel(List<SalesReturnCsvViewModel> csv)
        {
            List<AccuSalesReturnViewModel> item = new List<AccuSalesReturnViewModel>();
            List<AccuSalesReturnDetailItemViewModel> detailItemViewModels = new List<AccuSalesReturnDetailItemViewModel>();
            List<string> tempNo = new List<string>();
            foreach (var i in csv)
            {
                var isSameSales = tempNo.FirstOrDefault(s => s == i.salesOrderNo);
                if (isSameSales == null)
                {
                    tempNo.Add(i.salesOrderNo);
                    AccuSalesReturnViewModel ii = new AccuSalesReturnViewModel
                    {
                        customerNo = string.IsNullOrWhiteSpace(i.customerNo) ? "PELANGGAN SHOPIFY" : i.customerNo,
                        invoiceNumber = i.salesOrderNo,
                        transDate = Convert.ToDateTime(i.transDate),
                        taxDate = Convert.ToDateTime(i.taxDate),
                        taxNumber = i.taxNumber,
                        returnType= "INVOICE",
                        branchName= "JAKARTA",
                        
                        detailItem = new List<AccuSalesReturnDetailItemViewModel>()
                        {
                            new AccuSalesReturnDetailItemViewModel()
                            {
                                unitPrice= Convert.ToDouble(i.unitPrice),
                                itemNo=i.itemNo,
                                detailNotes=i.detailNotes,
                                quantity= Convert.ToDouble(i.quantity),
                                itemUnitName = "PCS",
                                warehouseName = "shopify"
                            }
                        }

                    };

                    item.Add(ii);
                }
                else
                {
                    var b = new AccuSalesReturnDetailItemViewModel()
                    {
                        unitPrice = Convert.ToDouble(i.unitPrice),
                        itemNo = i.itemNo,
                        detailNotes = i.detailNotes,
                        quantity = Convert.ToDouble(i.quantity),
                        itemUnitName = "PCS",
                        warehouseName = "shopify"
                    };

                    AccuSalesReturnViewModel header = item.Where(a => a.invoiceNumber == i.salesOrderNo).FirstOrDefault();
                    header.detailItem.Add(b);
                }
            }
            return item;
        }

        public async Task<List<AccuSalesReturn>> MapToModel(List<AccuSalesReturnViewModel> data1)
        {
            List<AccuSalesReturn> salesReturns = new List<AccuSalesReturn>();
            foreach (var i in data1)
            {
                List<AccuSalesReturnDetailItem> ReturnDetailItems = new List<AccuSalesReturnDetailItem>();
                foreach (var ii in i.detailItem)
                {
                    var dd = new AccuSalesReturnDetailItem()
                    {
                        UnitPrice = ii.unitPrice,
                        Quantity = ii.quantity,
                        ItemNo = ii.itemNo,
                        DetailNotes=ii.detailNotes,
                        ItemUnitName=ii.itemUnitName,
                        WarehouseName = ii.warehouseName
                    };

                    ReturnDetailItems.Add(dd);

                };

                AccuSalesReturn accuSales = new AccuSalesReturn
                {
                    InvoiceNumber = i.invoiceNumber,
                    CustomerNo = i.customerNo,
                    TaxDate = i.taxDate,
                    TaxNumber = i.taxNumber,
                    TransDate = i.transDate,
                    BranchName = i.branchName,
                    CurrencyCode = i.currencyCode,
                    ReturnType = i.returnType,
                    DetailItem = ReturnDetailItems
                };

                salesReturns.Add(accuSales);
            }

            return salesReturns;
        }

        public async Task<int> Create(List<AccuSalesReturnViewModel> viewModel, string username)
        {
            await facade.RefreshToken();
            await facade.OpenDb();
            var created = 0;

            IAccurateClientService httpClient = (IAccurateClientService)serviceProvider.GetService(typeof(IAccurateClientService));
            var url = $"{AuthCredential.Host}/accurate/api/sales-return/save.do";

            foreach (var i in viewModel)
            {
                var dataToBeMapped = dbSet.Where(x => x.Id == i.Id).Include(m => m.DetailItem).FirstOrDefault();
                var Customer = await SearchCustomerNo(dataToBeMapped.CustomerNo);

                List<SalesReturnDetailItemViewModel> detailItem = new List<SalesReturnDetailItemViewModel>();

                foreach(var x in dataToBeMapped.DetailItem)
                {
                    detailItem.Add( new SalesReturnDetailItemViewModel { 

                        itemNo = x.ItemNo,
                        unitPrice = x.UnitPrice,
                        quantity = x.Quantity,
                        detailNotes = x.DetailNotes,
                        itemUnitName = x.ItemUnitName,
                        warehouseName = x.WarehouseName
                    });
                }

                var dataToBeSerialize = new SalesReturnUploadViewModel
                {
                    customerNo = Customer.customerNo,
                    branchName = Customer.branch["name"],
                    invoiceNumber = dataToBeMapped.InvoiceNumber,
                    returnType = dataToBeMapped.ReturnType,
                    taxDate = dataToBeMapped.TaxDate.Date.ToShortDateString(),
                    transDate = dataToBeMapped.TransDate.Date.ToShortDateString(),
                    detailItem = detailItem
                };

                var dataToBeSend = JsonConvert.SerializeObject(dataToBeSerialize);

                var content = new StringContent(dataToBeSend, Encoding.UTF8, General.JsonMediaType);
                var response = await httpClient.PostAsync(url, content);
                var msg = await response.Content.ReadAsStringAsync();
                var message = JsonConvert.DeserializeObject<AccurateResponseViewModel>(msg);

                if (response.IsSuccessStatusCode && message.s)
                {
                    dataToBeMapped.IsAccurate = true;
                    EntityExtension.FlagForUpdate(dataToBeMapped, username, USER_AGENT);
                }
            }

            created += await dbContext.SaveChangesAsync();
            return created;
        }

        private async Task<AccurateCustomerViewModel> SearchCustomerNo(string name)
        {
            IAccurateClientService httpClient = (IAccurateClientService)serviceProvider.GetService(typeof(IAccurateClientService));
            var url = $"{AuthCredential.Host}/accurate/api/customer/list.do";

            var dataToBeSerialize = new DetailSearch
            {
                fields = "name,customerNo,branch",
                filter = new Dictionary<string, string>
                {
                    { "keywords", name }
                }
            };

            var dataToBeSend = JsonConvert.SerializeObject(dataToBeSerialize);

            var content = new StringContent(dataToBeSend, Encoding.UTF8, General.JsonMediaType);
            var response = await httpClient.SendAsync(HttpMethod.Get, url, content);
            var message = JsonConvert.DeserializeObject<AccurateSearchCustomerViewModel>(await response.Content.ReadAsStringAsync());
            //result.GetValueOrDefault("data").ToString()

            if (response.IsSuccessStatusCode && message.s)
            {
                var customer = message.d;
                return customer.First();
            }
            else
            {
                return null;
            }

        }

        public Tuple<List<AccuSalesReturn>, int, Dictionary<string, string>> ReadForUpload(int Page = 1, int Size = 25, string Order = "{}", string Keyword = null, string Filter = "{}")
        {
            IQueryable<AccuSalesReturn> Query = this.dbSet.Include(x => x.DetailItem);

            List<string> searchAttributes = new List<string>()
            {
                "CustomerNo", "SalesReturnReturnType"
            };

            Query = QueryHelper<AccuSalesReturn>.ConfigureSearch(Query, searchAttributes, Keyword);

            Dictionary<string, string> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Filter);
            Query = QueryHelper<AccuSalesReturn>.ConfigureFilter(Query, FilterDictionary);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Order);
            Query = QueryHelper<AccuSalesReturn>.ConfigureOrder(Query, OrderDictionary);

            Pageable<AccuSalesReturn> pageable = new Pageable<AccuSalesReturn>(Query, Page - 1, Size);
            List<AccuSalesReturn> Data = pageable.Data.ToList<AccuSalesReturn>();
            int TotalData = pageable.TotalCount;

            return Tuple.Create(Data, TotalData, OrderDictionary);
        }

        public async Task UploadData(List<AccuSalesReturn> data, string username)
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

        public Tuple<bool, List<object>> UploadValidate(ref List<SalesReturnCsvViewModel> data, List<KeyValuePair<string, StringValues>> list)
        {
            List<object> ErrorList = new List<object>();
            string ErrorMessage;
            bool Valid = true;
            IQueryable<AccuSalesReturn> Query = this.dbSet;

            foreach (SalesReturnCsvViewModel item in data)
            {
                ErrorMessage = "";
                var isExist = Query.Where(s => s.InvoiceNumber == item.salesOrderNo).ToList();

                if (string.IsNullOrWhiteSpace(item.customerNo))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "No Identitas Customer Tidak Boleh Kosong, ");
                }

                if (isExist.Count > 0)
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Nomor Penjualan Sudah Ada, ");
                }

                if (string.IsNullOrWhiteSpace(item.salesOrderNo))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Nomor Penjualan Tidak Boleh Kosong, ");
                }

                if (string.IsNullOrWhiteSpace(item.itemNo))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Barcode Tidak Boleh Kosong, ");
                }

                decimal domesticSale = 0;
                if (string.IsNullOrWhiteSpace(item.unitPrice))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Harga tidak boleh kosong, ");
                }
                else if (!decimal.TryParse(item.unitPrice, out domesticSale))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Harga harus numerik, ");
                }
                else if (domesticSale < 0)
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Harga harus lebih besar dari 0, ");
                }

                decimal qty = 0;
                if (string.IsNullOrWhiteSpace(item.quantity))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Qty tidak boleh kosong, ");
                }
                else if (!decimal.TryParse(item.quantity, out qty))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Harga harus numerik, ");
                }

                if (!string.IsNullOrEmpty(ErrorMessage))
                {
                    ErrorMessage = ErrorMessage.Remove(ErrorMessage.Length - 2);
                    var Error = new ExpandoObject() as IDictionary<string, object>;
                    Error.Add("No Identitas Customer", item.customerNo);
                    Error.Add("No Penjualan", item.salesOrderNo);
                    Error.Add("Barcode", item.itemNo);
                    Error.Add("Kuantitas", item.quantity);
                    Error.Add("Harga", item.unitPrice);
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

        public sealed class SalesReturnMap : CsvHelper.Configuration.ClassMap<SalesReturnCsvViewModel>
        {
            public SalesReturnMap()
            {

                Map(p => p.transDate).Index(0);
                Map(p => p.customerNo).Index(1);
                Map(p => p.salesOrderNo).Index(2);
                Map(p => p.itemNo).Index(3);
                Map(p => p.unitPrice).Index(4);
                Map(p => p.taxNumber).Index(5);
                Map(p => p.taxDate).Index(6);
                Map(p => p.quantity).Index(7);
                Map(p => p.detailNotes).Index(8);
                
            }
        }

        private class DetailSearch
        {
            public string fields { get; set; }
            public Dictionary<string, string> filter { get; set; }
        }
    }
}
