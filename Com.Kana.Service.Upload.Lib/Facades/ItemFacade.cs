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
using System.ComponentModel;
using System.Dynamic;
using System.Text;
using System.Threading.Tasks;

namespace Com.Kana.Service.Upload.Lib.Facades
{
    public class ItemFacade : IItemFacade
    {
        private string USER_AGENT = "Facade";

        private readonly UploadDbContext dbContext;
        private readonly DbSet<AccuItem> dbSet;
        public readonly IServiceProvider serviceProvider;
        public object Request { get; private set; }
        public object ApiVersion { get; private set; }

        public ItemFacade(IServiceProvider serviceProvider, UploadDbContext dbContext)
        {
            this.serviceProvider = serviceProvider;
            this.dbContext = dbContext;
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
                    unitPrice = i.variantPrice,
                    detailGroup = new List<AccuItemDetailGroupViewModel>()
                    {
                        new AccuItemDetailGroupViewModel()
                        {
                            quantity = i.variantInventoryQty,
                        }
                    },
                    detailOpenBalance = new List<AccuItemDetailOpenBalanceViewModel>()
                    {
                        new AccuItemDetailOpenBalanceViewModel()
                        {
                            quantity = i.variantInventoryQty,
                            warehouseName = i.variantInventoryTracker,
                            detailSerialNumber = new List<AccuItemDetailSerialNumberViewModel>()
                            {
                                new AccuItemDetailSerialNumberViewModel
                                {
                                    quantity = i.variantInventoryQty
                                }
                            }
                        }
                    }
                };

                item.Add(ii);
            }

            return item;
        }

        public Tuple<bool, List<object>> UploadValidate(ref List<ItemCsvViewModel> data, List<KeyValuePair<string, StringValues>> list)
        {
            List<object> ErrorList = new List<object>();
            string ErrorMessage;
            bool Valid = true;

            foreach(ItemCsvViewModel item in data)
            {
                ErrorMessage = "";

                if (string.IsNullOrWhiteSpace(item.title))
                {
                    ErrorMessage = string.Concat(ErrorMessage, "Nama Barang Tidak Boleh Kosong, ");
                }

                if (!string.IsNullOrEmpty(ErrorMessage))
                {
                    ErrorMessage = ErrorMessage.Remove(ErrorMessage.Length - 2);
                    var Error = new ExpandoObject() as IDictionary<string, object>;
                    Error.Add("title", item.title);

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
                dbSet.Add(i);
            }
            var result = await dbContext.SaveChangesAsync();
        }
    }
}
