using Com.Kana.Service.Upload.Lib.Enums;
using Com.Kana.Service.Upload.Lib.Facades.Expedition;
using Com.Kana.Service.Upload.Lib.Models.Expedition;
using Com.Kana.Service.Upload.Lib.Models.UnitPaymentOrderModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.Kana.Service.Upload.Test.DataUtils.ExpeditionDataUtil
{
    public class SendToVerificationDataUtil
    {
        private readonly PurchasingDocumentExpeditionFacade Facade;

        public SendToVerificationDataUtil(PurchasingDocumentExpeditionFacade Facade)
        {
            this.Facade = Facade;
        }
        public PurchasingDocumentExpedition GetNewData(UnitPaymentOrder unitPaymentOrder = null)
        {
            List<PurchasingDocumentExpeditionItem> Items = new List<PurchasingDocumentExpeditionItem>()
            {
                new PurchasingDocumentExpeditionItem()
                {
                    ProductId = "ProductId",
                    ProductCode = "ProductCode",
                    ProductName = "ProductName",
                    Price = 10000,
                    Quantity = 5,
                    Uom = "MTR",
                    UnitId = "UnitId",
                    UnitCode = "UnitCode",
                    UnitName = "UnitName"
                }
            };

            PurchasingDocumentExpedition TestData = new PurchasingDocumentExpedition()
            {
                SendToVerificationDivisionDate = DateTimeOffset.UtcNow,
                UnitPaymentOrderNo = unitPaymentOrder == null ? Guid.NewGuid().ToString() : unitPaymentOrder.UPONo,
                UPODate = unitPaymentOrder == null ? DateTimeOffset.UtcNow : unitPaymentOrder.Date,
                DueDate = DateTimeOffset.UtcNow,
                InvoiceNo = "Invoice",
                PaymentMethod = "CASH",
                SupplierCode = "Supplier",
                SupplierName = "Supplier",
                DivisionCode = "Division",
                DivisionName = "Division",
                IncomeTax = 20000,
                Vat = 100000,
                IncomeTaxId = "IncomeTaxId",
                IncomeTaxName = "IncomeTaxName",
                IncomeTaxRate = 2,
                TotalPaid = 1000000,
                Currency = "IDR",
                Items = Items,
            };

            return TestData;
        }

        public async Task<PurchasingDocumentExpedition> GetTestData(PurchasingDocumentExpedition purchasingDocumentExpedition = null)
        {
            PurchasingDocumentExpedition model = purchasingDocumentExpedition ?? GetNewData();
            await Facade.SendToVerification(new List<PurchasingDocumentExpedition>() { model }, "Unit Test");
            return model;
        }
    }
}
