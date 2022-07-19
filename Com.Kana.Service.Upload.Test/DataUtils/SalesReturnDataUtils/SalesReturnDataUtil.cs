using Com.Kana.Service.Upload.Lib.Facades;
using Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuSalesReturnModel;
using Com.Kana.Service.Upload.Lib.ViewModels.AccuSalesReturnViewModel;
using Com.Kana.Service.Upload.Lib.ViewModels.SalesReturnViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.Kana.Service.Upload.Test.DataUtils.SalesReturnDataUtils
{
    public class SalesReturnDataUtil
    {
        private readonly SalesReturnUploadFacade facade;

        public SalesReturnDataUtil(SalesReturnUploadFacade facade)
        {
            this.facade = facade;
        }

        public AccuSalesReturn GetNewData()
        {
            return new AccuSalesReturn
            {
                CustomerNo = "C.00004",
                BranchName = "JAKARTA",
                InvoiceNumber = "PLR.0001",
                ReturnType = "INVOICE",
                TransDate = DateTimeOffset.Now,
                TaxDate = DateTimeOffset.Now,
                TaxNumber = "TAX001",
                DetailItem = new List<AccuSalesReturnDetailItem> 
                {
                    new AccuSalesReturnDetailItem
                    {
                        ItemNo = "2201918",
                        ItemUnitName = "PCS",
                        WarehouseName = "shopify",
                        UnitPrice = 500000,
                        DetailNotes = "",
                        Quantity = 1
                    }
                }
            };
        }

        public class SalesReturnDataUtilViewModel
        {
            private readonly SalesReturnUploadFacade facade;

            public SalesReturnDataUtilViewModel(SalesReturnUploadFacade facade)
            {
                this.facade = facade;
            }

            public AccuSalesReturnViewModel GetNewDataValid()
            {
                return new AccuSalesReturnViewModel
                {
                    customerNo = "C.00004",
                    branchName = "JAKARTA",
                    invoiceNumber = "PLR.0001",
                    returnType = "INVOICE",
                    transDate = DateTimeOffset.Now,
                    taxDate = DateTimeOffset.Now,
                    taxNumber = "TAX001",
                    detailItem = new List<AccuSalesReturnDetailItemViewModel> 
                    { 
                        new AccuSalesReturnDetailItemViewModel
                        {
                            itemNo = "2201918",
                            itemUnitName = "PCS",
                            warehouseName = "shopify",
                            unitPrice = 500000,
                            detailNotes = "",
                            quantity = 1
                        }
                    }
                };
            }
        }

        public class SalesReturnDataUtilCSV
        {
            private readonly SalesReturnUploadFacade facade;

            public SalesReturnDataUtilCSV(SalesReturnUploadFacade facade)
            {
                this.facade = facade;
            }

            public SalesReturnCsvViewModel GetNewDataValid()
            {
                return new SalesReturnCsvViewModel
                {
                    customerNo = "C.00004",
                    salesOrderNo = "PLR.0001",
                    returnType = "INVOICE",
                    taxDate = DateTimeOffset.Now.Date.ToShortDateString(),
                    taxNumber = "TAX001",
                    transDate = DateTimeOffset.Now.Date.ToShortDateString(),
                    itemNo = "2201918",
                    unitPrice = "500000",
                    detailNotes = "",
                    quantity = "1"
                };
            }

            public List<SalesReturnCsvViewModel> GetNewListDataValid()
            {
                return new List<SalesReturnCsvViewModel>
                {
                    new SalesReturnCsvViewModel
                    {
                        customerNo = "C.00004",
                        salesOrderNo = "PLR.0001",
                        returnType = "INVOICE",
                        taxDate = DateTimeOffset.Now.Date.ToShortDateString(),
                        taxNumber = "TAX001",
                        transDate = DateTimeOffset.Now.Date.ToShortDateString(),
                        itemNo = "2201918",
                        unitPrice = "500000",
                        detailNotes = "",
                        quantity = "1"
                    },
                    new SalesReturnCsvViewModel
                    {
                        customerNo = "C.00004",
                        salesOrderNo = "PLR.0001",
                        returnType = "INVOICE",
                        taxDate = DateTimeOffset.Now.Date.ToShortDateString(),
                        taxNumber = "TAX002",
                        transDate = DateTimeOffset.Now.Date.ToShortDateString(),
                        itemNo = "2201919",
                        unitPrice = "500000",
                        detailNotes = "",
                        quantity = "1"
                    }
                };
            }

            public SalesReturnCsvViewModel GetNewData1()
            {
                //var datas = await Task.Run(() => garmentPurchaseOrderDataUtil.GetTestDataByTags());
                return new SalesReturnCsvViewModel
                {
                    customerNo = "",
                    salesOrderNo = "",
                    returnType = "INVOICE",
                    taxDate = DateTimeOffset.Now.Date.ToShortDateString(),
                    taxNumber = "TAX001",
                    transDate = DateTimeOffset.Now.Date.ToShortDateString(),
                    itemNo = "",
                    unitPrice = "aaa",
                    detailNotes = "",
                    quantity = "aa"
                };
            }

            public SalesReturnCsvViewModel GetNewData2()
            {
                //var datas = await Task.Run(() => garmentPurchaseOrderDataUtil.GetTestDataByTags());
                return new SalesReturnCsvViewModel
                {
                    customerNo = "",
                    salesOrderNo = "",
                    returnType = "INVOICE",
                    taxDate = DateTimeOffset.Now.Date.ToShortDateString(),
                    taxNumber = "TAX001",
                    transDate = DateTimeOffset.Now.Date.ToShortDateString(),
                    itemNo = "",
                    unitPrice = "-1",
                    detailNotes = "",
                    quantity = "-1"
                };
            }
        }

        public async Task<AccuSalesReturn> GetTestData()
        {
            var data = GetNewData();
            List<AccuSalesReturn> accuSalesReturn = new List<AccuSalesReturn>();
            accuSalesReturn.Add(data);

            await facade.UploadData(accuSalesReturn, "Unit Test");
            return data;
        }
    }
}
