using Com.Kana.Service.Upload.Lib;
using Com.Kana.Service.Upload.Lib.Enums;
using Com.Kana.Service.Upload.Lib.Facades;
using Com.Kana.Service.Upload.Lib.Facades.Expedition;
using Com.Kana.Service.Upload.Lib.Facades.ExternalPurchaseOrderFacade;
using Com.Kana.Service.Upload.Lib.Facades.InternalPO;
using Com.Kana.Service.Upload.Lib.Facades.UnitReceiptNoteFacade;
using Com.Kana.Service.Upload.Lib.Interfaces;
using Com.Kana.Service.Upload.Lib.Services;
using Com.Kana.Service.Upload.Lib.Utilities.CacheManager;
using Com.Kana.Service.Upload.Lib.Utilities.CacheManager.CacheData;
using Com.Kana.Service.Upload.Lib.Utilities.Currencies;
using Com.Kana.Service.Upload.Lib.ViewModels.Expedition;
using Com.Kana.Service.Upload.Lib.ViewModels.NewIntegrationViewModel;
using Com.Kana.Service.Upload.Test.DataUtils.DeliveryOrderDataUtils;
using Com.Kana.Service.Upload.Test.DataUtils.ExpeditionDataUtil;
using Com.Kana.Service.Upload.Test.DataUtils.ExternalPurchaseOrderDataUtils;
using Com.Kana.Service.Upload.Test.DataUtils.InternalPurchaseOrderDataUtils;
using Com.Kana.Service.Upload.Test.DataUtils.PurchaseRequestDataUtils;
using Com.Kana.Service.Upload.Test.DataUtils.UnitPaymentOrderDataUtils;
using Com.Kana.Service.Upload.Test.DataUtils.UnitReceiptNoteDataUtils;
using Com.Kana.Service.Upload.Test.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Com.Kana.Service.Upload.Test.Facades
{
    public class UnitPaymentOrderExpeditionServiceTest
    {
        private const string ENTITY = "UnitPaymentOrder";

        private const string USERNAME = "Unit Test";
        private IServiceProvider ServiceProvider { get; set; }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public string GetCurrentMethod()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);

            return string.Concat(sf.GetMethod().Name, "_", ENTITY);
        }

        private UploadDbContext _dbContext(string testName)
        {
            DbContextOptionsBuilder<UploadDbContext> optionsBuilder = new DbContextOptionsBuilder<UploadDbContext>();
            optionsBuilder
                .UseInMemoryDatabase(testName)
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));

            UploadDbContext dbContext = new UploadDbContext(optionsBuilder.Options);

            return dbContext;
        }

        private Mock<IServiceProvider> GetServiceProvider(string testname)
        {
            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider
                .Setup(x => x.GetService(typeof(IdentityService)))
                .Returns(new IdentityService() { Token = "Token", Username = "Test" });

            serviceProvider
                .Setup(x => x.GetService(typeof(IHttpClientService)))
                .Returns(new HttpClientTestService());

            serviceProvider
                .Setup(x => x.GetService(typeof(InternalPurchaseOrderFacade)))
                .Returns(new InternalPurchaseOrderFacade(serviceProvider.Object, _dbContext(testname)));

            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProviders = services.BuildServiceProvider();
            var memoryCache = serviceProviders.GetService<IMemoryCache>();

            serviceProvider
                .Setup(x => x.GetService(typeof(IMemoryCacheManager)))
                .Returns(new MemoryCacheManager(memoryCache));

            Mock<IDistributedCache> mockDistributedCache = new Mock<IDistributedCache>();
            mockDistributedCache.Setup(s => s.Get(It.Is<string>(i => i == MemoryCacheConstant.Categories)))
                .Returns(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(new List<IdCOAResult>())));
            mockDistributedCache.Setup(s => s.Get(It.Is<string>(i => i == MemoryCacheConstant.Units)))
                .Returns(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(new List<IdCOAResult>())));
            mockDistributedCache.Setup(s => s.Get(It.Is<string>(i => i == MemoryCacheConstant.Divisions)))
                .Returns(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(new List<CategoryCOAResult>())));
            mockDistributedCache.Setup(s => s.Get(It.Is<string>(i => i == MemoryCacheConstant.IncomeTaxes)))
                .Returns(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(new List<IncomeTaxCOAResult>())));
            serviceProvider
                .Setup(x => x.GetService(typeof(IDistributedCache)))
                .Returns(mockDistributedCache.Object);

            var opts = Options.Create(new MemoryDistributedCacheOptions());
            var cache = new MemoryDistributedCache(opts);

            serviceProvider
                .Setup(x => x.GetService(typeof(IDistributedCache)))
                .Returns(cache);

            var mockCurrencyProvider = new Mock<ICurrencyProvider>();
            mockCurrencyProvider
                .Setup(x => x.GetCurrencyByCurrencyCode(It.IsAny<string>()))
                .ReturnsAsync((Currency)null);
            serviceProvider
                .Setup(x => x.GetService(typeof(ICurrencyProvider)))
                .Returns(mockCurrencyProvider.Object);

            return serviceProvider;
        }

        private UnitPaymentOrderDataUtil _dataUtil(UnitPaymentOrderFacade facade, UploadDbContext dbContext, string testname)
        {

            PurchaseRequestFacade purchaseRequestFacade = new PurchaseRequestFacade(GetServiceProvider(testname).Object, dbContext);
            PurchaseRequestItemDataUtil purchaseRequestItemDataUtil = new PurchaseRequestItemDataUtil();
            PurchaseRequestDataUtil purchaseRequestDataUtil = new PurchaseRequestDataUtil(purchaseRequestItemDataUtil, purchaseRequestFacade);

            InternalPurchaseOrderFacade internalPurchaseOrderFacade = new InternalPurchaseOrderFacade(GetServiceProvider(testname).Object, dbContext);
            InternalPurchaseOrderItemDataUtil internalPurchaseOrderItemDataUtil = new InternalPurchaseOrderItemDataUtil();
            InternalPurchaseOrderDataUtil internalPurchaseOrderDataUtil = new InternalPurchaseOrderDataUtil(internalPurchaseOrderItemDataUtil, internalPurchaseOrderFacade, purchaseRequestDataUtil);

            ExternalPurchaseOrderFacade externalPurchaseOrderFacade = new ExternalPurchaseOrderFacade(GetServiceProvider(testname).Object, dbContext);
            ExternalPurchaseOrderDetailDataUtil externalPurchaseOrderDetailDataUtil = new ExternalPurchaseOrderDetailDataUtil();
            ExternalPurchaseOrderItemDataUtil externalPurchaseOrderItemDataUtil = new ExternalPurchaseOrderItemDataUtil(externalPurchaseOrderDetailDataUtil);
            ExternalPurchaseOrderDataUtil externalPurchaseOrderDataUtil = new ExternalPurchaseOrderDataUtil(externalPurchaseOrderFacade, internalPurchaseOrderDataUtil, externalPurchaseOrderItemDataUtil);

            DeliveryOrderFacade deliveryOrderFacade = new DeliveryOrderFacade(dbContext, GetServiceProvider(testname).Object);
            DeliveryOrderDetailDataUtil deliveryOrderDetailDataUtil = new DeliveryOrderDetailDataUtil();
            DeliveryOrderItemDataUtil deliveryOrderItemDataUtil = new DeliveryOrderItemDataUtil(deliveryOrderDetailDataUtil);
            DeliveryOrderDataUtil deliveryOrderDataUtil = new DeliveryOrderDataUtil(deliveryOrderItemDataUtil, deliveryOrderDetailDataUtil, externalPurchaseOrderDataUtil, deliveryOrderFacade);

            UnitReceiptNoteFacade unitReceiptNoteFacade = new UnitReceiptNoteFacade(GetServiceProvider(testname).Object, dbContext);
            UnitReceiptNoteItemDataUtil unitReceiptNoteItemDataUtil = new UnitReceiptNoteItemDataUtil();
            UnitReceiptNoteDataUtil unitReceiptNoteDataUtil = new UnitReceiptNoteDataUtil(unitReceiptNoteItemDataUtil, unitReceiptNoteFacade, deliveryOrderDataUtil);

            return new UnitPaymentOrderDataUtil(unitReceiptNoteDataUtil, facade);
        }

        [Fact]
        public async Task Should_Success_GetReport()
        {
            var dbContext = _dbContext(GetCurrentMethod());
            var unitPaymentOrderFacade = new UnitPaymentOrderFacade(GetServiceProvider(GetCurrentMethod()).Object, dbContext);
            var modelLocalSupplier = await _dataUtil(unitPaymentOrderFacade, dbContext, GetCurrentMethod()).GetNewData();
            var responseLocalSupplier = await unitPaymentOrderFacade.Create(modelLocalSupplier, USERNAME, false);

            var reportService = new UnitPaymentOrderExpeditionReportService(dbContext);
            var dateTo = DateTime.UtcNow.AddDays(1);
            var dateFrom = dateTo.AddDays(-30);
            var results = await reportService.GetReport(modelLocalSupplier.UPONo, modelLocalSupplier.SupplierCode, modelLocalSupplier.DivisionCode, modelLocalSupplier.Position, dateFrom, dateTo, "{'Date': 'desc'}", 1, 25);



            Assert.NotNull(results);
        }

        [Fact]
        public async Task Should_Success_GetReport_Excel()
        {
            var dbContext = _dbContext(GetCurrentMethod());
            var unitPaymentOrderFacade = new UnitPaymentOrderFacade(GetServiceProvider(GetCurrentMethod()).Object, dbContext);
            var modelLocalSupplier = await _dataUtil(unitPaymentOrderFacade, dbContext, GetCurrentMethod()).GetNewData();
            var responseLocalSupplier = await unitPaymentOrderFacade.Create(modelLocalSupplier, USERNAME, false);

            var purchasingDocumentExpeditionFacade = new PurchasingDocumentExpeditionFacade(GetServiceProvider(GetCurrentMethod()).Object, dbContext);
            var sendToVerificationDataUtil = new SendToVerificationDataUtil(purchasingDocumentExpeditionFacade);
            var purchasingDocumentExpedition = sendToVerificationDataUtil.GetNewData(modelLocalSupplier);
            await sendToVerificationDataUtil.GetTestData(purchasingDocumentExpedition);

            var reportService = new UnitPaymentOrderExpeditionReportService(dbContext);
            var dateTo = modelLocalSupplier.Date;
            var dateFrom = modelLocalSupplier.Date;
            var results = await reportService.GetExcel(modelLocalSupplier.UPONo, modelLocalSupplier.SupplierCode, modelLocalSupplier.DivisionCode, modelLocalSupplier.Position, dateFrom, dateTo, "{'Date': 'desc'}");

            Assert.NotNull(results);
        }

        [Fact]
        public void Should_Success_InstantiateReport()
        {
            var report = new UnitPaymentOrderExpeditionReportViewModel()
            {
                BankExpenditureNoteNo = "",
                CashierDivisionDate = DateTime.Now,
                Category = new CategoryViewModel()
                {
                    Code = "Code",
                    Name = "Name"
                },
                Currency = "IDR",
                Date = DateTime.Now,
                Division = new DivisionViewModel()
                {
                    Code = "Code",
                    Id = "Id",
                    Name = "Name"
                },
                DPP = 0,
                DueDate = DateTime.Now,
                InvoiceNo = "InvoiceNo",
                No = "no",
                Position = ExpeditionPosition.CASHIER_DIVISION,
                PPn = 0,
                PPh = 0,
                SendDate = DateTime.Now,
                SendToVerificationDivisionDate = DateTime.Now,
                Supplier = new NewSupplierViewModel()
                {
                    code = "code",
                    contact = "contact",
                    import = false,
                    name = "name",
                    PIC = "pic",
                    _id = 1
                },
                TotalDay = 0,
                TotalTax = 0,
                Unit = new UnitViewModel()
                {
                    Code = "Code",
                    Name = "Name"
                },
                VerificationDivisionDate = DateTime.Now,
                VerifyDate = DateTime.Now
            };

            Assert.NotNull(report);
        }

        [Fact]
        public void Should_Fail_Validate_VM_UPOVerificationVM()
        {
            UnitPaymentOrderVerificationViewModel vm = new UnitPaymentOrderVerificationViewModel();
            Assert.NotEmpty(vm.Validate(null));

            vm.VerifyDate = DateTimeOffset.UtcNow.AddDays(1);
            Assert.NotEmpty(vm.Validate(null));

            vm.VerifyDate = DateTimeOffset.UtcNow.AddDays(-1);
            Assert.NotEmpty(vm.Validate(null));

            vm.UnitPaymentOrderNo = "no";
            Assert.NotEmpty(vm.Validate(null));

            vm.SubmitPosition = ExpeditionPosition.CASHIER_DIVISION;
            Assert.Empty(vm.Validate(null));
        }
    }
}
