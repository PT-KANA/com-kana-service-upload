using AutoMapper;
using Com.Kana.Service.Upload.Lib.Helpers;
using Com.Kana.Service.Upload.Lib.Interfaces;
using Com.Kana.Service.Upload.Lib.Interfaces.SalesReturnInterface;
using Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuSalesReturnModel;
using Com.Kana.Service.Upload.Lib.ViewModels;
using Com.Kana.Service.Upload.Lib.ViewModels.AccuSalesReturnViewModel;
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
            "Tanggal Retur", "No Identitas Customer", "No Penjualan", "Kode Barang", "Harga Barang", "No Faktur Pajak", "Tanggal Faktur Pajak", "Keterangan"
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
                        //customerNo = string.IsNullOrWhiteSpace(i.customerNo) ? "CUST" : i.customerNo,
                        customerNo = string.IsNullOrWhiteSpace(i.customerNo) ? "C.00004" : i.customerNo,
                        //salesOrderNo = i.salesOrderNo,
                        invoiceNumber = i.salesOrderNo,
                        transDate1 = Convert.ToDateTime(i.transDate),
                        taxDate1 = Convert.ToDateTime(i.taxDate),
                        taxNumber = i.taxNumber,
                        returnType= "INVOICE",
                        branchName= "JAKARTA",
                        
                        detailItem = new List<AccuSalesReturnDetailItemViewModel>()
                        {
                            new AccuSalesReturnDetailItemViewModel()
                            {
                                unitPrice= Convert.ToDouble(i.unitPrice),
                                itemNo=i.itemNo,
                                detailNotes=i.detailNotes
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
                        detailNotes = i.detailNotes
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
                        DetailNotes=ii.detailNotes
                    };

                    ReturnDetailItems.Add(dd);

                };

                AccuSalesReturn accuSales = new AccuSalesReturn
                {
                    InvoiceNumber = i.invoiceNumber,
                    CustomerNo = i.customerNo,
                    TaxDate = i.taxDate1,
                    TaxNumber = i.taxNumber,
                    TransDate = i.transDate1,
                    BranchName = i.branchName,
                    CurrencyCode = i.currencyCode,
                    ReturnType = i.returnType,
                    DetailItem = ReturnDetailItems
                };

                salesReturns.Add(accuSales);
            }

            return salesReturns;
        }

        public async Task Create(List<AccuSalesReturnViewModel> viewModel, string username)
        {
            var session = await facade.OpenDb();

            //var httpClient = new HttpClient();
            IAccurateClientService httpClient = (IAccurateClientService)serviceProvider.GetService(typeof(IAccurateClientService));
            IHttpClientService httpClient1 = (IHttpClientService)serviceProvider.GetService(typeof(IHttpClientService));
            var url = $"{AuthCredential.Host}/accurate/api/sales-return/save.do";

            foreach (var i in viewModel)
            {
                var dataToBeMapped = dbSet.Where(x => x.Id == i.Id).Include(m => m.DetailItem).FirstOrDefault();
                var dataToBeConvert = mapper.Map<AccuSalesReturnViewModel>(dataToBeMapped);
                dataToBeConvert.transDate = Convert.ToDateTime(dataToBeConvert.transDate).Date.ToString();
                dataToBeConvert.taxDate = Convert.ToDateTime(dataToBeConvert.taxDate).Date.ToString();
                dataToBeConvert.branchName = "JAKARTA";
                dataToBeConvert.customerNo = "C.00004";
                dataToBeConvert.Id = 0;

                foreach(var x in dataToBeConvert.detailItem)
                {
                    x.Id = 0;
                }

                var dataToBeSend = JsonConvert.SerializeObject(dataToBeConvert);
                var content = new StringContent(dataToBeSend, Encoding.UTF8, General.JsonMediaType);

                var response = httpClient.PostAsync(url, content).Result;
                var message = JsonConvert.DeserializeObject<AccurateResponseViewModel>(response.Content.ReadAsStringAsync().Result);

                if (response.IsSuccessStatusCode && message.s)
                {
                    dataToBeMapped.IsAccurate = true;
                    EntityExtension.FlagForUpdate(dataToBeMapped, username, USER_AGENT);
                }
                else
                {
                    throw new Exception("data " + i.invoiceNumber + " gagal diupload");
                }

                //using(var request = new HttpRequestMessage(HttpMethod.Post, url))
                //{
                //    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AuthCredential.AccessToken);
                //    request.Headers.Add("X-Session-ID", session.Result.session);
                //    request.Content = content;

                //    var response = await httpClient.SendAsync(request);
                //    var res = response.Content.ReadAsStringAsync().Result;
                //    var message = JsonConvert.DeserializeObject<AccurateResponseViewModel>(res);

                //    if (response.IsSuccessStatusCode && message.s)
                //    {
                //        dataToBeMapped.IsAccurate = true;
                //        EntityExtension.FlagForUpdate(dataToBeMapped, username, USER_AGENT);
                //    }
                //    else
                //    {
                //        throw new Exception("data " + i.invoiceNumber + " gagal diupload");
                //    }
                //}
            }

            dbContext.SaveChanges();
        }

        public Tuple<List<AccuSalesReturn>, int, Dictionary<string, string>> ReadForUpload(int Page = 1, int Size = 25, string Order = "{}", string Keyword = null, string Filter = "{}")
        {
            IQueryable<AccuSalesReturn> Query = this.dbSet.Include(x => x.DetailExpense).Include(x => x.DetailItem);

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
            IQueryable<AccuSalesReturn> Query = this.dbSet.Include(x => x.DetailItem);

            foreach (SalesReturnCsvViewModel item in data)
            {
                ErrorMessage = "";

                if (string.IsNullOrWhiteSpace(item.customerNo))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "No Identitas Customer Tidak Boleh Kosong, ");
                }

                if (!string.IsNullOrEmpty(ErrorMessage))
                {
                    ErrorMessage = ErrorMessage.Remove(ErrorMessage.Length - 2);
                    var Error = new ExpandoObject() as IDictionary<string, object>;
                    Error.Add("No Identitas Customer", item.customerNo);
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
                Map(p => p.detailNotes).Index(7);
            }
        }
    }
}
