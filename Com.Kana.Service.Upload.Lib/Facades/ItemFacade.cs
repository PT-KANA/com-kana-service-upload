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
using AutoMapper;
using Com.Kana.Service.Upload.Lib.ViewModels.AccuItemViewModel.AccuItemUploadViewModel;
using Newtonsoft.Json.Linq;

namespace Com.Kana.Service.Upload.Lib.Facades
{
    public class ItemFacade : IItemFacade
    {
        private string USER_AGENT = "Facade";

        private readonly UploadDbContext dbContext;
        private readonly DbSet<AccuItem> dbSet;
        private readonly DbSet<AccuItemTemp> dbSetTemp;

        public readonly IServiceProvider serviceProvider;
        public readonly IIntegrationFacade facade;
        private readonly IMapper mapper;

        public object Request { get; private set; }
        public object ApiVersion { get; private set; }

        public ItemFacade(IServiceProvider serviceProvider, IIntegrationFacade integration, UploadDbContext dbContext, IMapper mapper)
        {
            this.serviceProvider = serviceProvider;
            this.facade = integration;
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.dbSet = dbContext.Set<AccuItem>();
            this.dbSetTemp = dbContext.Set<AccuItemTemp>();
        }

        public List<string> CsvHeader { get; } = new List<string>()
        {
          //  "Handle", "Title", "Body (HTML)", "Vendor", "Standardized Product Type", "Custom Product Type", "Tags", "Published", "Option1 Name", "Option1 Value", "Option2 Name", "Option2 Value", "Option3 Name", "Option3 Value", "Variant SKU", "Variant Grams", "Variant Inventory Tracker", "Variant Inventory Qty", "Variant Inventory Policy", "Variant Fulfillment Service", "Variant Price", "Variant Compare At Price", "Variant Requires Shipping", "Variant Taxable", "Variant Barcode", "Image Src", "Image Position", "Image Alt Text", "Gift Card", "SEO Title", "SEO Description", "Google Shopping / Google Product Category", "Google Shopping / Gender", "Google Shopping / Age Group",  "Google Shopping / MPN",    "Google Shopping / AdWords Grouping",   "Google Shopping / AdWords Labels", "Google Shopping / Condition",  "Google Shopping / Custom Product", "Google Shopping / Custom Label 0", "Google Shopping / Custom Label 1", "Google Shopping / Custom Label 2", "Google Shopping / Custom Label 3", "Google Shopping / Custom Label 4", "Variant Image", "Variant Weight Unit", "Variant Tax Code", "Cost per item", "Status",
        "Handle",   "Title",    "Body (HTML)",  "Vendor",   "Product Category", "Type", "Tags", "Published",    "Option1 Name", "Option1 Value",    "Option2 Name", "Option2 Value",    "Option3 Name", "Option3 Value",    "Variant SKU",  "Variant Grams",    "Variant Inventory Tracker",    "Variant Inventory Qty",    "Variant Inventory Policy", "Variant Fulfillment Service",  "Variant Price",    "Variant Compare At Price", "Variant Requires Shipping",    "Variant Taxable",  "Variant Barcode",  "Image Src",    "Image Position",   "Image Alt Text",   "Gift Card",    "SEO Title",    "SEO Description",  "Google Shopping / Google Product Category",    "Google Shopping / Gender", "Google Shopping / Age Group",  "Google Shopping / MPN",    "Google Shopping / AdWords Grouping",   "Google Shopping / AdWords Labels", "Google Shopping / Condition",  "Google Shopping / Custom Product", "Google Shopping / Custom Label 0", "Google Shopping / Custom Label 1", "Google Shopping / Custom Label 2", "Google Shopping / Custom Label 3", "Google Shopping / Custom Label 4", "Variant Image",    "Variant Weight Unit",  "Variant Tax Code", "Cost per item",    "Price / International",    "Compare At Price / International", "Price / Singapore",    "Compare At Price / Singapore", "Price / United States",    "Compare At Price / United States", "Status"

        };

        public sealed class ItemMap : CsvHelper.Configuration.ClassMap<ItemCsvViewModel>
        {
            public ItemMap()
            {
                Map(p => p.handle).Index(0);
                Map(p => p.title).Index(1);
                Map(p => p.vendor).Index(3);
                Map(p => p.option1Value).Index(9);
                Map(p => p.variantSKU).Index(14);
                Map(p => p.variantGrams).Index(15);
                Map(p => p.variantInventoryTracker).Index(16);
                Map(p => p.variantInventoryQty).Index(17);
                Map(p => p.variantPrice).Index(20);
                Map(p => p.variantTaxable).Index(23);
                Map(p => p.variantBarcode).Index(24);
                Map(p => p.variantWeightUnit).Index(45);
                Map(p => p.variantTaxCode).Index(46);
                Map(p => p.costPeritem).Index(47);
            }
        }

        public async Task<List<AccuItemViewModel>> MapToViewModel(List<ItemCsvViewModel> csv)
        {
            List<AccuItemViewModel> item = new List<AccuItemViewModel>();
            List<string> tempNo = new List<string>();
            foreach (var i in csv )
            {
               
                    var barcode = i.variantBarcode.Replace("'", string.Empty).Trim();
                    AccuItemViewModel ii = new AccuItemViewModel
                    {
                        itemType = "INVENTORY",
                        name = string.IsNullOrWhiteSpace(i.title) ? csv.Find(x => x.handle == i.handle).title + " - " + i.option1Value : i.title + " - " + i.option1Value,
                        unit1Name = "PCS",
                        unitPrice = string.IsNullOrWhiteSpace(i.variantPrice) ? 0 : Convert.ToDouble(i.variantPrice),
                        upcNo = barcode,
                        no = barcode,
                        serialNumberType = "UNIQUE",
                        usePPn = string.IsNullOrWhiteSpace(i.variantTaxable) ? false : (i.variantTaxable == "TRUE" ? true : false),
                        preferedVendorName = string.IsNullOrWhiteSpace(i.vendor) ? csv.Find(x => x.handle == i.handle).vendor : i.vendor,
                        vendorPrice = string.IsNullOrWhiteSpace(i.costPeritem) ? (string.IsNullOrWhiteSpace(i.variantPrice) ? 0 : Convert.ToDouble(i.variantPrice)) : Convert.ToDouble(i.costPeritem),
                        vendorUnitName = "PCS",

                    };
                if (ii.no !="")
                {

                    item.Add(ii);
                }
               
            }

            return item;
        }

        public async Task<List<AccuItem>> MapToModel(List<AccuItemViewModel> data1)
        {
            List<AccuItem> item = new List<AccuItem>();

            foreach (var i in data1)
            {
                //List<AccuItemDetailOpenBalance> itemDetailOpenBalance = new List<AccuItemDetailOpenBalance>();

                //foreach (var iii in i.detailOpenBalance)
                //{
                //    AccuItemDetailOpenBalance temp3 = new AccuItemDetailOpenBalance
                //    {
                //        AsOf = iii.asOf,
                //        Quantity = iii.quantity,
                //        ItemUnitName = iii.itemUnitName,
                //        WarehouseName = iii.warehouseName,
                //        UnitCost = iii.unitCost,
                //    };

                //    itemDetailOpenBalance.Add(temp3);
                //}

                AccuItem temp1 = new AccuItem
                {
                    ItemType = i.itemType,
                    Name = i.name,
                    No = i.no,
                    UpcNo = i.upcNo,
                    Unit1Name = i.unit1Name,
                    UnitPrice = i.unitPrice,
                    UsePPn = i.usePPn,
                    VendorPrice = i.vendorPrice,
                    VendorUnitName = i.vendorUnitName,
                    PreferedVendorName = i.preferedVendorName,
                    SerialNumberType = i.serialNumberType,
                    //DetailOpenBalance = itemDetailOpenBalance,
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

            foreach (ItemCsvViewModel item in data.Where(s=>s.variantBarcode !=""))
            {
                ErrorMessage = "";

                var x = data.Find(y => y.handle == item.handle && !string.IsNullOrWhiteSpace(y.handle));
                var name = x != null ? x.title + " - " + item.option1Value : item.title;

                var isExist = Query.Where(s => s.Name == name).ToList();

                if (x == null && string.IsNullOrWhiteSpace(x.title))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Nama Barang Tidak Boleh Kosong, ");
                }

                if (isExist.Count > 0)
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Barang Sudah Ada, ");
                }

                if (string.IsNullOrWhiteSpace(item.variantBarcode))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Barcode Tidak Boleh Kosong, ");
                }

                if (string.IsNullOrWhiteSpace(item.option1Value))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Size Tidak Boleh Kosong, ");
                }

                decimal domesticSale = 0;
                if (string.IsNullOrWhiteSpace(item.variantPrice))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Harga tidak boleh kosong, ");
                }
                else if (!decimal.TryParse(item.variantPrice, out domesticSale))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Harga harus numerik, ");
                }
                else if (domesticSale < 0)
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Harga harus lebih besar dari 0, ");
                }

                if (!string.IsNullOrEmpty(ErrorMessage))
                {
                    ErrorMessage = ErrorMessage.Remove(ErrorMessage.Length - 2);
                    var Error = new ExpandoObject() as IDictionary<string, object>;
                    Error.Add("title", item.title);
                    Error.Add("handle", item.handle);
                    Error.Add("option1Value", item.option1Value);
                    Error.Add("variantBarcode", item.variantBarcode);
                    Error.Add("variantPrice", item.variantPrice);
                    //Error.Add("variantInventoryQty", item.variantInventoryQty);
                    Error.Add("error", ErrorMessage);

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
            foreach (var i in data)
            {
                EntityExtension.FlagForCreate(i, username, USER_AGENT);

                //foreach (var iii in i.DetailOpenBalance)
                //{
                //    EntityExtension.FlagForCreate(iii, username, USER_AGENT);
                //}

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

        public async Task<int> Create(List<AccuItemViewModel> viewModel, string username)
        {
            var token = await facade.RefreshToken();
            var session = await facade.OpenDb();
            var temp = await BulkIntoTemp();
            var created = 0;

            IAccurateClientService httpClient = (IAccurateClientService)serviceProvider.GetService(typeof(IAccurateClientService));
            var url = $"{AuthCredential.Host}/accurate/api/item/save.do";

            foreach (var i in viewModel)
            {
                var dataToBeMapped = dbSet.Where(x => x.Id == i.Id).FirstOrDefault();
                var dataToBeChecked = dbSetTemp.Where(x => x.No == dataToBeMapped.No).FirstOrDefault();

                var dataToBeSerialize = new ItemUploadViewModel
                {
                    itemType = dataToBeMapped.ItemType,
                    name = dataToBeMapped.Name,
                    no = dataToBeMapped.No,
                    preferedVendorName = dataToBeMapped.PreferedVendorName,
                    serialNumberType = dataToBeMapped.SerialNumberType,
                    unit1Name = dataToBeMapped.Unit1Name,
                    unitPrice = dataToBeMapped.UnitPrice,
                    upcNo = dataToBeMapped.UpcNo,
                    usePPn = dataToBeMapped.UsePPn,
                    vendorPrice = dataToBeMapped.VendorPrice,
                    vendorUnitName = dataToBeMapped.VendorUnitName,
                };

                var dataToBeSend = JsonConvert.SerializeObject(dataToBeSerialize);

                var content = new StringContent(dataToBeSend, Encoding.UTF8, General.JsonMediaType);
                var response = await httpClient.PostAsync(url, content);
                var message = JsonConvert.DeserializeObject<AccurateResponseViewModel>(await response.Content.ReadAsStringAsync());

                if (response.IsSuccessStatusCode && message.s)
                {
                    dataToBeMapped.IsAccurate = true;
                    EntityExtension.FlagForUpdate(dataToBeMapped, username, USER_AGENT);
                }

                if(dataToBeChecked != null)
                {
                    dataToBeMapped.IsAccurate = true;
                    EntityExtension.FlagForUpdate(dataToBeMapped, username, USER_AGENT);
                }
            }

            created += await dbContext.SaveChangesAsync();

            return created;
        }
        private class ResponseVM
        {
            public bool s { get; set; }
            public List<string> d { get; set; }
            public int  pageCount { get; set; }
        }

        private async Task<ResponseVM> SearchNo(int page, DateTime date)
        {
            IAccurateClientService httpClient = (IAccurateClientService)serviceProvider.GetService(typeof(IAccurateClientService));
            var url = $"{AuthCredential.Host}/accurate/api/item/list.do";
            var list = new List<string>();
            var now = date.Date.ToShortDateString();

            list.Add(now);

            var dataToBeSerialize = new DetailSearchByDate
			{
                fields = "no",
                filter = new Filter
                {
                    lastUpdate = new Val
                    {
                        op = "LESS_THAN",
                        val = list
                    }
                },
                sp = new Sp
                {
                    page = page,
                    pageSize = 100,
                    sort = "no|asc"
                }
            };

            var dataToBeSend = JsonConvert.SerializeObject(dataToBeSerialize);

           

            var content = new StringContent(dataToBeSend, Encoding.UTF8, General.JsonMediaType);
            var response = await httpClient.SendAsync(HttpMethod.Get, url, content);
            var message = await response.Content.ReadAsStringAsync();

            JObject joResponse = JObject.Parse(message);

            var d = joResponse.GetValue("d").ToString();
            var s = Convert.ToBoolean(joResponse.GetValue("s"));
            var sp = joResponse.GetValue("sp").ToString();

            List<Dictionary<string, string>> _d = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(d);
            Dictionary<string, string> _sp = JsonConvert.DeserializeObject<Dictionary<string, string>>(sp);
            List<string> dataD = new List<string>();
            int pageCount = 0;
            foreach (var item in _d)
            {
                foreach(var detail in item)
                {
                    dataD.Add(detail.Value);
                }
            }
            foreach (var detail in _sp)
            {
                if(detail.Key == "pageCount")
                {
                    pageCount = Convert.ToInt32(detail.Value);
                }
            }
            ResponseVM data = new ResponseVM
            {
                s = s,
                d = dataD,
                pageCount = pageCount

            };
            if(s)
            {
                return data;
            }else
            {
                return null;
            }
            //var res = JsonConvert.DeserializeObject<AccurateResponseViewModel>(message);

            //if (res.s)
            //{
            //    var data = JsonConvert.DeserializeObject<ItemSearchResultViewModel>(message);
            //    return data;
            //}
            //else
            //{
            //    return null;
            //}
        }

        private async Task<int> BulkIntoTemp()
        {
            var date = DateTime.Now;
            var page = 1;
            var created = 0;
            List<AccuItemTemp> temp = new List<AccuItemTemp>();
            var st = await SearchNo(page, date);

            if(st != null)
            {
                for(var x = 1; x <= st.pageCount; x++)
                {
                    var data = await SearchNo(x, date);
                    foreach(var i in data.d)
                    {
                        temp.Add(new AccuItemTemp { No = i});
                    }
                }

                foreach(var i in temp)
                {
                    EntityExtension.FlagForCreate(i, "YOKS", USER_AGENT);
                    dbSetTemp.Add(i);
                }

                
            }

            return created += await dbContext.SaveChangesAsync();
        }
    }
}

