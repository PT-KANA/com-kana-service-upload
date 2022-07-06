using Com.DanLiris.Service.Purchasing.Lib.ViewModels.AccuItemViewModel;
using Com.Kana.Service.Upload.Lib.Interfaces.ItemInterface;
using Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuItemModel;
using Com.Kana.Service.Upload.Lib.ViewModels.AccuItemViewModel;
using Com.Kana.Service.Upload.Lib.ViewModels.ItemViewModel;
using Com.Moonlay.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using CsvHelper.TypeConversion;
using System.Dynamic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Com.Kana.Service.Upload.Lib.Helpers;
using Newtonsoft.Json;
using Com.Moonlay.NetCore.Lib;
using Com.Kana.Service.Upload.Lib.Interfaces;
using System.Net.Http;
using System.Net.Http.Headers;
using Com.Kana.Service.Upload.Lib.ViewModels;

namespace Com.Kana.Service.Upload.Lib.Facades
{
    public class ItemFacade : IItemFacade
    {
        private string USER_AGENT = "Facade";

        private readonly UploadDbContext dbContext;
        private readonly DbSet<AccuItem> dbSet;
        public readonly IServiceProvider serviceProvider;
        public readonly IIntegrationFacade facade;

        public object Request { get; private set; }
        public object ApiVersion { get; private set; }

        public ItemFacade(IServiceProvider serviceProvider, IIntegrationFacade integration, UploadDbContext dbContext)
        {
            this.serviceProvider = serviceProvider;
            this.facade = integration;
            this.dbContext = dbContext;
            this.dbSet = dbContext.Set<AccuItem>();
        }

        public List<string> CsvHeader { get; } = new List<string>()
        {
            "Handle", "Title", "Body (HTML)", "Vendor", "Standardized Product Type", "Custom Product Type", "Tags", "Published", "Option1 Name", "Option1 Value", "Option2 Name", "Option2 Value", "Option3 Name", "Option3 Value", "Variant SKU", "Variant Grams", "Variant Inventory Tracker", "Variant Inventory Qty", "Variant Inventory Policy", "Variant Fulfillment Service", "Variant Price", "Variant Compare At Price", "Variant Requires Shipping", "Variant Taxable", "Variant Barcode", "Image Src", "Image Position", "Image Alt Text", "Gift Card", "SEO Title", "SEO Description", "Google Shopping / Google Product Category", "Google Shopping / Gender", "Google Shopping / Age Group",  "Google Shopping / MPN",    "Google Shopping / AdWords Grouping",   "Google Shopping / AdWords Labels", "Google Shopping / Condition",  "Google Shopping / Custom Product", "Google Shopping / Custom Label 0", "Google Shopping / Custom Label 1", "Google Shopping / Custom Label 2", "Google Shopping / Custom Label 3", "Google Shopping / Custom Label 4", "Variant Image", "Variant Weight Unit", "Variant Tax Code", "Cost per item", "Status",
        };

        public sealed class ItemMap : CsvHelper.Configuration.ClassMap<ItemCsvViewModel>
        {
            public ItemMap()
            {
                //Map(p => p.domesticSale).Index(5).TypeConverter<StringConverter>();
                //Map(p => p.quantity).Index(7).TypeConverter<StringConverter>();
                //Map(p => p.domesticCOGS).Index(9).TypeConverter<StringConverter>();

                Map(p => p.handle).Index(0);
                Map(p => p.title).Index(1);
                Map(p => p.vendor).Index(3);
                //Map(p => p.option1Value).Index(9);
                Map(p => p.variantSKU).Index(14);
                Map(p => p.variantGrams).Index(15);
                Map(p => p.variantInventoryTracker).Index(16);
                Map(p => p.variantInventoryQty).Index(17);
                Map(p => p.variantPrice).Index(20);
                //Map(p => p.variantTaxable).Index(23);
                Map(p => p.variantBarcode).Index(24);
                Map(p => p.variantWeightUnit).Index(45);
                Map(p => p.variantTaxCode).Index(46);
                Map(p => p.costPeritem).Index(47);
            }
        }

        public async Task<List<AccuItemViewModel>> MapToViewModel(List<ItemCsvViewModel> csv)
        {
            List<AccuItemViewModel> item = new List<AccuItemViewModel>();

            foreach(var i in csv)
            {
                AccuItemViewModel ii = new AccuItemViewModel
                {
                    itemType = "INVENTORY",
                    name = string.IsNullOrWhiteSpace(i.title) ? csv.Find(x => x.handle == i.handle).title : i.title,
                    no = i.variantBarcode,
                    unit1Name = "PCS",
                    unitPrice = string.IsNullOrWhiteSpace(i.variantPrice) ? 0 : Convert.ToDouble(i.variantPrice),
                    detailGroup = new List<AccuItemDetailGroupViewModel>()
                    {
                        new AccuItemDetailGroupViewModel()
                        {
                            quantity = string.IsNullOrWhiteSpace(i.variantInventoryQty) ? 0 : Convert.ToDouble(i.variantInventoryQty),
                        }
                    },
                    detailOpenBalance = new List<AccuItemDetailOpenBalanceViewModel>()
                    {
                        new AccuItemDetailOpenBalanceViewModel()
                        {
                            quantity = string.IsNullOrWhiteSpace(i.variantInventoryQty) ? 0 : Convert.ToDouble(i.variantInventoryQty),
                            warehouseName = i.variantInventoryTracker,
                            detailSerialNumber = new List<AccuItemDetailSerialNumberViewModel>()
                            {
                                new AccuItemDetailSerialNumberViewModel
                                {
                                    quantity = string.IsNullOrWhiteSpace(i.variantInventoryQty) ? 0 : Convert.ToDouble(i.variantInventoryQty),
                                }
                            }
                        }
                    }
                };

                item.Add(ii);
            }

            return item;
        }

        public async Task<List<AccuItem>> MapToModel(List<AccuItemViewModel> data1)
        {
            List<AccuItem> item = new List<AccuItem>();
            List<AccuItemDetailGroup> itemDetailGroup = new List<AccuItemDetailGroup>();
            List<AccuItemDetailOpenBalance> itemDetailOpenBalance = new List<AccuItemDetailOpenBalance>();
            List<AccuItemDetailSerialNumber> itemDetailSerialNumber = new List<AccuItemDetailSerialNumber>();

            foreach (var i in data1)
            {
                foreach(var ii in i.detailGroup)
                {
                    AccuItemDetailGroup temp2 = new AccuItemDetailGroup
                    {
                        Quantity = ii.quantity
                    };

                    itemDetailGroup.Add(temp2);
                }

                foreach(var iii in i.detailOpenBalance)
                {
                    foreach(var iv in iii.detailSerialNumber)
                    {
                        AccuItemDetailSerialNumber temp4 = new AccuItemDetailSerialNumber
                        {
                            Quantity = iv.quantity
                        };

                        itemDetailSerialNumber.Add(temp4);
                    }

                    AccuItemDetailOpenBalance temp3 = new AccuItemDetailOpenBalance
                    {
                        Quantity = iii.quantity,
                        WarehouseName = iii.warehouseName,
                        DetailSerialNumber = itemDetailSerialNumber
                    };

                    itemDetailOpenBalance.Add(temp3);
                }

                AccuItem temp1 = new AccuItem
                {
                    ItemType = i.itemType,
                    Name = i.name,
                    No = i.no,
                    Unit1Name = i.unit1Name,
                    UnitPrice = i.unitPrice,
                    DetailGroup = itemDetailGroup,
                    DetailOpenBalance = itemDetailOpenBalance,
                };

                item.Add(temp1);
            }

            return item;
        }

        public Tuple<bool, List<object>> UploadValidate(ref List<ItemCsvViewModel> data, List<KeyValuePair<string, StringValues>> list)
        {
            List<object> ErrorList = new List<object>();
            string ErrorMessage;
            bool Valid = true;

            IQueryable<AccuItem> Query = this.dbSet;

            foreach (ItemCsvViewModel item in data)
            {
                ErrorMessage = "";

                var x = data.Find(y => y.handle == item.handle && !string.IsNullOrWhiteSpace(y.handle));
                var isExist = Query.Where(s => s.Name == x.title);

                if (x == null || string.IsNullOrWhiteSpace(x.title))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Nama Barang Tidak Boleh Kosong, ");
                }

                if (isExist != null)
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Barang Sudah Ada, ");
                }

                if (!string.IsNullOrEmpty(ErrorMessage))
                {
                    ErrorMessage = ErrorMessage.Remove(ErrorMessage.Length - 2);
                    var Error = new ExpandoObject() as IDictionary<string, object>;
                    Error.Add("title", item.title);
                    Error.Add("handle", item.handle);

                    ErrorList.Add(Error);
                }
            }

            if (ErrorList.Count > 0)
            {
                Valid = false;
            }

            return Tuple.Create(Valid, ErrorList);
        }

        public async Task UploadData(List<AccuItem> data, string username)
        {
            foreach(var i in data)
            {
                EntityExtension.FlagForCreate(i, username, USER_AGENT);

                foreach(var ii in i.DetailGroup)
                {
                    EntityExtension.FlagForCreate(ii, username, USER_AGENT);
                }
                foreach (var iii in i.DetailOpenBalance)
                {
                    EntityExtension.FlagForCreate(iii, username, USER_AGENT);

                    foreach (var iv in iii.DetailSerialNumber)
                    {
                        EntityExtension.FlagForCreate(iv, username, USER_AGENT);
                    }
                }

                dbSet.Add(i);
            }
            var result = await dbContext.SaveChangesAsync();
        }

        public Tuple<List<AccuItem>, int, Dictionary<string, string>> ReadForUpload(int page, int size, string order, string keyword, string filter)
        {
            IQueryable<AccuItem> Query = this.dbSet.Include(x => x.DetailGroup).Include(x => x.DetailOpenBalance);

            List<string> searchAttributes = new List<string>()
            {
                "No", "Name"
            };

            Query = QueryHelper<AccuItem>.ConfigureSearch(Query, searchAttributes, keyword);

            Dictionary<string, string> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(filter);
            Query = QueryHelper<AccuItem>.ConfigureFilter(Query, FilterDictionary);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            Query = QueryHelper<AccuItem>.ConfigureOrder(Query, OrderDictionary);

            Pageable<AccuItem> pageable = new Pageable<AccuItem>(Query, page - 1, size);
            List<AccuItem> Data = pageable.Data.ToList<AccuItem>();
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

        public async Task Create(List<AccuItemViewModel> viewModel)
        {
            var Session = OpenDb();

            var httpClient = new HttpClient();
            var url = Session.Result.host + "/accurate/api/item/list.do";

            var data = new[]
            {
                new KeyValuePair<string, string>("fields", "id,name,no"),
                new KeyValuePair<string, string>("filter.itemType", "INVENTORY"),
            };

            var content = new FormUrlEncodedContent(data);

            using (var request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AuthCredential.AccessToken);
                request.Headers.Add("X-Session-ID", Session.Result.session);
                request.Content = content;

                var response = await httpClient.SendAsync(request);

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
    }
}
