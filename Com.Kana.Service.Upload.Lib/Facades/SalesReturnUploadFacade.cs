using Com.Kana.Service.Upload.Lib.Helpers;
using Com.Kana.Service.Upload.Lib.Interfaces.SalesReturnInterface;
using Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuSalesReturnModel;
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
        public object Request { get; private set; }
        public object ApiVersion { get; private set; }

        public SalesReturnUploadFacade(IServiceProvider serviceProvider, UploadDbContext dbContext)
        {
            this.serviceProvider = serviceProvider;
            this.dbContext = dbContext;
            this.dbSet = dbContext.Set<AccuSalesReturn>();
        }

        public List<string> CsvHeader { get; } = new List<string>()
        {
            "Tanggal Retur",   "No Identitas Customer",   "No Penjualan",    "Kode Barang", "Harga Barang",    "No Faktur Pajak", "Tanggal Faktur Pajak",    "Keterangan"
            
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
                        customerNo = string.IsNullOrWhiteSpace(i.customerNo) ? "CUST" : i.customerNo,
                        salesOrderNo = i.salesOrderNo,
                        transDate = Convert.ToDateTime(i.transDate),
                        taxDate = Convert.ToDateTime(i.taxDate),
                        taxNumber = i.taxNumber,
                        returnType="Invoice",
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

                    AccuSalesReturnViewModel header = item.Where(a => a.salesOrderNo == i.salesOrderNo).FirstOrDefault();

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

        public Task Create(List<AccuSalesReturnViewModel> data, string username, string token)
        {
            throw new NotImplementedException();
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
