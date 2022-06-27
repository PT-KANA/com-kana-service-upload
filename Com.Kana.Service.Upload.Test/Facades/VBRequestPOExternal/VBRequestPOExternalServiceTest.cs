using Com.Kana.Service.Upload.Lib;
using Com.Kana.Service.Upload.Lib.Enums;
using Com.Kana.Service.Upload.Lib.Facades;
using Com.Kana.Service.Upload.Lib.Facades.ExternalPurchaseOrderFacade;
using Com.Kana.Service.Upload.Lib.Facades.GarmentExternalPurchaseOrderFacades;
using Com.Kana.Service.Upload.Lib.Facades.GarmentInternalPurchaseOrderFacades;
using Com.Kana.Service.Upload.Lib.Facades.GarmentInternNoteFacades;
using Com.Kana.Service.Upload.Lib.Facades.GarmentPurchaseRequestFacades;
using Com.Kana.Service.Upload.Lib.Facades.InternalPO;
using Com.Kana.Service.Upload.Lib.Facades.VBRequestPOExternal;
using Com.Kana.Service.Upload.Lib.Interfaces;
using Com.Kana.Service.Upload.Lib.Models.GarmentInternNoteModel;
using Com.Kana.Service.Upload.Lib.Services;
using Com.Kana.Service.Upload.Lib.Utilities.CacheManager;
using Com.Kana.Service.Upload.Lib.Utilities.CacheManager.CacheData;
using Com.Kana.Service.Upload.Lib.Utilities.Currencies;
using Com.Kana.Service.Upload.Test.DataUtils.ExternalPurchaseOrderDataUtils;
using Com.Kana.Service.Upload.Test.DataUtils.GarmentExternalPurchaseOrderDataUtils;
using Com.Kana.Service.Upload.Test.DataUtils.GarmentInternalPurchaseOrderDataUtils;
using Com.Kana.Service.Upload.Test.DataUtils.GarmentInternNoteDataUtils;
using Com.Kana.Service.Upload.Test.DataUtils.GarmentPurchaseRequestDataUtils;
using Com.Kana.Service.Upload.Test.DataUtils.InternalPurchaseOrderDataUtils;
using Com.Kana.Service.Upload.Test.DataUtils.PurchaseRequestDataUtils;
using Com.Kana.Service.Upload.Test.DataUtils.UnitPaymentOrderDataUtils;
using Com.Kana.Service.Upload.Test.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xunit;

namespace Com.Kana.Service.Upload.Test.Facades.VBRequestPOExternal
{
    public class VBRequestPOExternalServiceTest
    {

        protected string GetCurrentAsyncMethod([CallerMemberName] string methodName = "")
        {
            var method = new StackTrace()
                .GetFrames()
                .Select(frame => frame.GetMethod())
                .FirstOrDefault(item => item.Name == methodName);

            return method.Name;

        }

        protected UploadDbContext GetDbContext(string testName)
        {
            string databaseName = testName;
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var optionsBuilder = new DbContextOptionsBuilder<UploadDbContext>();
            optionsBuilder
                .UseInMemoryDatabase(databaseName)
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .UseInternalServiceProvider(serviceProvider);

            UploadDbContext DbContex = new UploadDbContext(optionsBuilder.Options);
            return DbContex;
        }

        private Mock<IServiceProvider> GetServiceProvider()
        {

            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider
                .Setup(x => x.GetService(typeof(IdentityService)))
                .Returns(new IdentityService() { Token = "Token", Username = "Test" });

            serviceProvider
               .Setup(x => x.GetService(typeof(IHttpClientService)))
               .Returns(new HttpClientTestService());

            Mock<ICurrencyProvider> currencyProvider = new Mock<ICurrencyProvider>();
            currencyProvider.Setup(x => x.GetCurrencyByCurrencyCode(It.IsAny<string>())).ReturnsAsync(new Currency() { Rate = 1, Code = "IDR", Date = DateTime.Now });

            serviceProvider
               .Setup(x => x.GetService(typeof(ICurrencyProvider)))
               .Returns(currencyProvider.Object);

            return serviceProvider;
        }

        public GarmentExternalPurchaseOrderDataUtil _dataUtil(GarmentExternalPurchaseOrderFacade facade, GarmentInternalPurchaseOrderDataUtil garmentPurchaseOrderDataUtil)
        {
            return new GarmentExternalPurchaseOrderDataUtil(facade, garmentPurchaseOrderDataUtil);
        }

        public GarmentInternNoteDataUtil dataUtil(GarmentInternNoteFacades facades)
        {
            return new GarmentInternNoteDataUtil(facades);
        }

        public UnitPaymentOrderDataUtil dataUtil(UploadDbContext dbContext)
        {
            return new UnitPaymentOrderDataUtil(dbContext);
        }

        public ExternalPurchaseOrderDataUtil dataUtil(ExternalPurchaseOrderFacade facade, InternalPurchaseOrderDataUtil internalPurchaseOrderDataUtil, ExternalPurchaseOrderItemDataUtil externalPurchaseOrderItemDataUtil)
        {
            return new ExternalPurchaseOrderDataUtil(facade, internalPurchaseOrderDataUtil, externalPurchaseOrderItemDataUtil);
        }


        [Fact]
        public async Task ReadPOExternal_with_Garment_Return_Success()
        {
            //Setup
            UploadDbContext dbContext = GetDbContext(GetCurrentAsyncMethod());
            var serviceProviderMock = GetServiceProvider();

            GarmentExternalPurchaseOrderFacade facade = new GarmentExternalPurchaseOrderFacade(serviceProviderMock.Object, dbContext);
            GarmentPurchaseRequestFacade garmentPurchaseRequestFacade = new GarmentPurchaseRequestFacade(serviceProviderMock.Object, dbContext);
            GarmentPurchaseRequestDataUtil garmentPurchaseRequestDataUtil = new GarmentPurchaseRequestDataUtil(garmentPurchaseRequestFacade);

            GarmentInternalPurchaseOrderFacade internalPurchaseOrderFacade = new GarmentInternalPurchaseOrderFacade(dbContext);
            GarmentInternalPurchaseOrderDataUtil garmentPurchaseOrderDataUtil = new GarmentInternalPurchaseOrderDataUtil(internalPurchaseOrderFacade, garmentPurchaseRequestDataUtil);

            var data = await _dataUtil(facade, garmentPurchaseOrderDataUtil).GetTestData_VBRequestPOExternal();

            //Act
            VBRequestPOExternalService service = new VBRequestPOExternalService(dbContext, serviceProviderMock.Object);
            var result = service.ReadPOExternal("PO700100001", "GARMENT", "IDR");

            //Assert
            Assert.NotNull(result);

        }

        [Fact]
        public async Task ShouldSuccess_ReadPOExternal_with_NoGarment()
        {
            //Setup
            UploadDbContext dbContext = GetDbContext(GetCurrentAsyncMethod());
            var serviceProviderMock = GetServiceProvider();

            var purchaseRequestItemDataUtil = new PurchaseRequestItemDataUtil();
            var purchaseRequestFacade = new PurchaseRequestFacade(serviceProviderMock.Object, dbContext);
            var purchaserequestDataUtil = new PurchaseRequestDataUtil(purchaseRequestItemDataUtil, purchaseRequestFacade);

            var internalPurchaseOrderItemDataUtil = new InternalPurchaseOrderItemDataUtil();
            var internalPurchaseOrderFacade = new InternalPurchaseOrderFacade(serviceProviderMock.Object, dbContext);
            var internalPurchaseOrderDataUtil = new InternalPurchaseOrderDataUtil(internalPurchaseOrderItemDataUtil, internalPurchaseOrderFacade, purchaserequestDataUtil);

            var externalPurchaseOrderFacade = new ExternalPurchaseOrderFacade(serviceProviderMock.Object, dbContext);
            var externalPurchaseOrderDetailDataUtil = new ExternalPurchaseOrderDetailDataUtil();
            var externalPurchaseOrderItemDataUtil = new ExternalPurchaseOrderItemDataUtil(externalPurchaseOrderDetailDataUtil);
            var data = await dataUtil(externalPurchaseOrderFacade, internalPurchaseOrderDataUtil, externalPurchaseOrderItemDataUtil).GetTestData("user");

            //Act
            VBRequestPOExternalService service = new VBRequestPOExternalService(dbContext, serviceProviderMock.Object);
            var result = service.ReadPOExternal("PO700100001", "NO_GARMENT", "IDR");

            //Assert
            Assert.NotNull(result);

        }


        [Fact]
        public async Task ShouldSuccess_ReadSPB_Garment()
        {
            //Setup
            UploadDbContext dbContext = GetDbContext(GetCurrentAsyncMethod());
            var serviceProviderMock = GetServiceProvider();

            GarmentExternalPurchaseOrderFacade facade = new GarmentExternalPurchaseOrderFacade(serviceProviderMock.Object, dbContext);
            GarmentPurchaseRequestFacade garmentPurchaseRequestFacade = new GarmentPurchaseRequestFacade(serviceProviderMock.Object, dbContext);
            GarmentPurchaseRequestDataUtil garmentPurchaseRequestDataUtil = new GarmentPurchaseRequestDataUtil(garmentPurchaseRequestFacade);

            GarmentInternalPurchaseOrderFacade internalPurchaseOrderFacade = new GarmentInternalPurchaseOrderFacade(dbContext);
            GarmentInternalPurchaseOrderDataUtil garmentPurchaseOrderDataUtil = new GarmentInternalPurchaseOrderDataUtil(internalPurchaseOrderFacade, garmentPurchaseRequestDataUtil);

            var data = await _dataUtil(facade, garmentPurchaseOrderDataUtil).GetTestData_VBRequestPOExternal();

            //Act
            VBRequestPOExternalService service = new VBRequestPOExternalService(dbContext, serviceProviderMock.Object);
            var result = service.ReadSPB("P", "GARMENT", new List<long>(), "IDR", "UMUM");

            //Assert
            Assert.NotNull(result);

        }

        [Fact]
        public async Task ShouldSuccess_ReadSPB_Garment_WithEpoIds()
        {
            //Setup
            UploadDbContext dbContext = GetDbContext(GetCurrentAsyncMethod());
            var serviceProviderMock = GetServiceProvider();

            GarmentExternalPurchaseOrderFacade facade = new GarmentExternalPurchaseOrderFacade(serviceProviderMock.Object, dbContext);
            GarmentPurchaseRequestFacade garmentPurchaseRequestFacade = new GarmentPurchaseRequestFacade(serviceProviderMock.Object, dbContext);
            GarmentPurchaseRequestDataUtil garmentPurchaseRequestDataUtil = new GarmentPurchaseRequestDataUtil(garmentPurchaseRequestFacade);

            GarmentInternalPurchaseOrderFacade internalPurchaseOrderFacade = new GarmentInternalPurchaseOrderFacade(dbContext);
            GarmentInternalPurchaseOrderDataUtil garmentPurchaseOrderDataUtil = new GarmentInternalPurchaseOrderDataUtil(internalPurchaseOrderFacade, garmentPurchaseRequestDataUtil);

            var data = await _dataUtil(facade, garmentPurchaseOrderDataUtil).GetTestData_VBRequestPOExternal();

            //Act
            VBRequestPOExternalService service = new VBRequestPOExternalService(dbContext, serviceProviderMock.Object);
            var result = service.ReadSPB("P", "GARMENT", new List<long> { 1L, 2L, 3L }, "IDR", "UMUM");

            //Assert
            Assert.NotNull(result);

        }

        [Fact]
        public async Task ShouldSuccess_ReadSPB_Garment_WithGarmentTypePurchasing()
        {
            //Setup
            UploadDbContext dbContext = GetDbContext(GetCurrentAsyncMethod());
            var serviceProviderMock = GetServiceProvider();

            GarmentExternalPurchaseOrderFacade facade = new GarmentExternalPurchaseOrderFacade(serviceProviderMock.Object, dbContext);
            GarmentPurchaseRequestFacade garmentPurchaseRequestFacade = new GarmentPurchaseRequestFacade(serviceProviderMock.Object, dbContext);
            GarmentPurchaseRequestDataUtil garmentPurchaseRequestDataUtil = new GarmentPurchaseRequestDataUtil(garmentPurchaseRequestFacade);

            GarmentInternalPurchaseOrderFacade internalPurchaseOrderFacade = new GarmentInternalPurchaseOrderFacade(dbContext);
            GarmentInternalPurchaseOrderDataUtil garmentPurchaseOrderDataUtil = new GarmentInternalPurchaseOrderDataUtil(internalPurchaseOrderFacade, garmentPurchaseRequestDataUtil);

            var data = await _dataUtil(facade, garmentPurchaseOrderDataUtil).GetTestData_VBRequestPOExternal();

            //Act
            VBRequestPOExternalService service = new VBRequestPOExternalService(dbContext, serviceProviderMock.Object);
            var result = service.ReadSPB("P", "GARMENT", new List<long> { 1L, 2L, 3L }, "IDR", "GARMENT");

            //Assert
            Assert.NotNull(result);

        }

        [Fact]
        public async Task ShouldSuccess_ReadSPB_NonGarment()
        {
            //Setup
            UploadDbContext dbContext = GetDbContext(GetCurrentAsyncMethod());
            var serviceProviderMock = GetServiceProvider();

            GarmentExternalPurchaseOrderFacade facade = new GarmentExternalPurchaseOrderFacade(serviceProviderMock.Object, dbContext);
            GarmentPurchaseRequestFacade garmentPurchaseRequestFacade = new GarmentPurchaseRequestFacade(serviceProviderMock.Object, dbContext);
            GarmentPurchaseRequestDataUtil garmentPurchaseRequestDataUtil = new GarmentPurchaseRequestDataUtil(garmentPurchaseRequestFacade);

            GarmentInternalPurchaseOrderFacade internalPurchaseOrderFacade = new GarmentInternalPurchaseOrderFacade(dbContext);
            GarmentInternalPurchaseOrderDataUtil garmentPurchaseOrderDataUtil = new GarmentInternalPurchaseOrderDataUtil(internalPurchaseOrderFacade, garmentPurchaseRequestDataUtil);

            var data = await _dataUtil(facade, garmentPurchaseOrderDataUtil).GetTestData_VBRequestPOExternal();

            //Act
            VBRequestPOExternalService service = new VBRequestPOExternalService(dbContext, serviceProviderMock.Object);
            var result = service.ReadSPB("P", "NON_GARMENT", new List<long>(), "IDR", "UMUM");

            //Assert
            Assert.NotNull(result);

        }

        //[Fact]
        //public async Task ShouldSuccess_UpdateSPB_with_GarmentDivision()
        //{
        //    //Setup
        //    PurchasingDbContext dbContext = GetDbContext(GetCurrentAsyncMethod());
        //    var serviceProviderMock = GetServiceProvider();

        //    GarmentInternNoteFacades facades = new GarmentInternNoteFacades(dbContext, serviceProviderMock.Object);

        //    var data = await dataUtil(facades).GetTestData_VBRequestPOExternal();

        //    //Act
        //    VBRequestPOExternalService service = new VBRequestPOExternalService(dbContext, serviceProviderMock.Object);
        //    var result = service.UpdateSPB("GARMENT", 1);

        //    //Assert
        //    Assert.NotEqual(0, result);

        //}

        [Fact]
        public void ShouldSuccess_UpdateSPB_with_NonGarmentDivision()
        {
            //Setup
            UploadDbContext dbContext = GetDbContext(GetCurrentAsyncMethod());
            GarmentInternNote garmentInternNote = new GarmentInternNote();
            garmentInternNote.Id = 1;
            garmentInternNote.IsCreatedVB = false;

            dbContext.GarmentInternNotes.Add(garmentInternNote);
            var serviceProviderMock = GetServiceProvider();

            var data = dataUtil(dbContext).GetTestData_VBRequestPOExternal();

            //Act
            VBRequestPOExternalService service = new VBRequestPOExternalService(dbContext, serviceProviderMock.Object);
            var result = service.UpdateSPB("", 1);
            var result2 = service.UpdateSPB("GARMENT", 1);

            //Assert
            Assert.NotEqual(0, result);
            Assert.NotEqual(0, result2);

        }





        
        //[Fact]
        //public async Task Should_Success_AutoJournalVBRequest()
        //{
        //    //Setup
        //    PurchasingDbContext dbContext = GetDbContext(GetCurrentAsyncMethod());

        //    var serviceProviderMock = GetServiceProvider();
        //    var memoryCacheManagerMock = new Mock<IMemoryCacheManager>();
        //    memoryCacheManagerMock
        //        .Setup(x => x.Get(MemoryCacheConstant.Categories, It.IsAny<Func<ICacheEntry, List<CategoryCOAResult>>>()))
        //        .Returns(new List<CategoryCOAResult>());

        //    memoryCacheManagerMock
        //        .Setup(x => x.Get(MemoryCacheConstant.Units, It.IsAny<Func<ICacheEntry, List<IdCOAResult>>>()))
        //        .Returns(new List<IdCOAResult>());

        //    memoryCacheManagerMock
        //        .Setup(x => x.Get(MemoryCacheConstant.Divisions, It.IsAny<Func<ICacheEntry, List<IdCOAResult>>>()))
        //        .Returns(new List<IdCOAResult>());

        //    memoryCacheManagerMock
        //        .Setup(x => x.Get(MemoryCacheConstant.IncomeTaxes, It.IsAny<Func<ICacheEntry, List<IncomeTaxCOAResult>>>()))
        //        .Returns(new List<IncomeTaxCOAResult>());

        //    serviceProviderMock
        //        .Setup(x => x.GetService(typeof(IMemoryCacheManager)))
        //        .Returns(memoryCacheManagerMock.Object);


        //    var purchaseRequestItemDataUtil = new PurchaseRequestItemDataUtil();
        //    var purchaseRequestFacade = new PurchaseRequestFacade(serviceProviderMock.Object, dbContext);
        //    var purchaserequestDataUtil = new PurchaseRequestDataUtil(purchaseRequestItemDataUtil, purchaseRequestFacade);

        //    var internalPurchaseOrderItemDataUtil = new InternalPurchaseOrderItemDataUtil();
        //    var internalPurchaseOrderFacade = new InternalPurchaseOrderFacade(serviceProviderMock.Object, dbContext);
        //    var internalPurchaseOrderDataUtil = new InternalPurchaseOrderDataUtil(internalPurchaseOrderItemDataUtil, internalPurchaseOrderFacade, purchaserequestDataUtil);

        //    var externalPurchaseOrderFacade = new ExternalPurchaseOrderFacade(serviceProviderMock.Object, dbContext);
        //    var externalPurchaseOrderDetailDataUtil = new ExternalPurchaseOrderDetailDataUtil();
        //    var externalPurchaseOrderItemDataUtil = new ExternalPurchaseOrderItemDataUtil(externalPurchaseOrderDetailDataUtil);
        //    var data = await dataUtil(externalPurchaseOrderFacade, internalPurchaseOrderDataUtil, externalPurchaseOrderItemDataUtil).GetTestData("user");

        //    //Act
        //    VBRequestPOExternalService service = new VBRequestPOExternalService(dbContext, serviceProviderMock.Object);

        //    VBFormDto dto = new VBFormDto()
        //    {
        //        EPOIds = new List<long>()
        //        {
        //            1
        //        }
        //    };
        //    var result = await service.AutoJournalVBRequest(dto);

        //    //Assert
        //    Assert.NotEqual(0, result);

        //}


        //[Fact]
        //public async Task Should_Success_AutoJournalVBRequest_when_MemoryCacheResult_Exist()
        //{
        //    //Setup
        //    PurchasingDbContext dbContext = GetDbContext(GetCurrentAsyncMethod());

        //    var serviceProviderMock = GetServiceProvider();
        //    var memoryCacheManagerMock = new Mock<IMemoryCacheManager>();
        //    memoryCacheManagerMock
        //        .Setup(x => x.Get(MemoryCacheConstant.Categories, It.IsAny<Func<ICacheEntry, List<CategoryCOAResult>>>()))
        //        .Returns(new List<CategoryCOAResult>() {
        //            new CategoryCOAResult()
        //            {
        //                Id=1,
        //            }
        //        });

        //    memoryCacheManagerMock
        //        .Setup(x => x.Get(MemoryCacheConstant.Units, It.IsAny<Func<ICacheEntry, List<IdCOAResult>>>()))
        //        .Returns(new List<IdCOAResult>() {
        //            new IdCOAResult()
        //            {
        //                Id=1,
        //            }
        //        });

        //    memoryCacheManagerMock
        //        .Setup(x => x.Get(MemoryCacheConstant.Divisions, It.IsAny<Func<ICacheEntry, List<IdCOAResult>>>()))
        //        .Returns(new List<IdCOAResult>(){
        //            new IdCOAResult()
        //            {
        //                  Id=1,
        //            }
        //        });

        //    memoryCacheManagerMock
        //        .Setup(x => x.Get(MemoryCacheConstant.IncomeTaxes, It.IsAny<Func<ICacheEntry, List<IncomeTaxCOAResult>>>()))
        //        .Returns(new List<IncomeTaxCOAResult>() {
        //             new IncomeTaxCOAResult()
        //             {
        //                 Id=1,
        //             }
        //        });

        //    serviceProviderMock
        //        .Setup(x => x.GetService(typeof(IMemoryCacheManager)))
        //        .Returns(memoryCacheManagerMock.Object);

        //    var purchaseRequestItemDataUtil = new PurchaseRequestItemDataUtil();
        //    var purchaseRequestFacade = new PurchaseRequestFacade(serviceProviderMock.Object, dbContext);
        //    var purchaserequestDataUtil = new PurchaseRequestDataUtil(purchaseRequestItemDataUtil, purchaseRequestFacade);

        //    var internalPurchaseOrderItemDataUtil = new InternalPurchaseOrderItemDataUtil();
        //    var internalPurchaseOrderFacade = new InternalPurchaseOrderFacade(serviceProviderMock.Object, dbContext);
        //    var internalPurchaseOrderDataUtil = new InternalPurchaseOrderDataUtil(internalPurchaseOrderItemDataUtil, internalPurchaseOrderFacade, purchaserequestDataUtil);

        //    var externalPurchaseOrderFacade = new ExternalPurchaseOrderFacade(serviceProviderMock.Object, dbContext);
        //    var externalPurchaseOrderDetailDataUtil = new ExternalPurchaseOrderDetailDataUtil();
        //    var externalPurchaseOrderItemDataUtil = new ExternalPurchaseOrderItemDataUtil(externalPurchaseOrderDetailDataUtil);
        //    var data = await dataUtil(externalPurchaseOrderFacade, internalPurchaseOrderDataUtil, externalPurchaseOrderItemDataUtil).GetTestData("user");

        //    //Act
        //    VBRequestPOExternalService service = new VBRequestPOExternalService(dbContext, serviceProviderMock.Object);

        //    VBFormDto dto = new VBFormDto()
        //    {
        //        EPOIds = new List<long>()
        //        {
        //            1
        //        }
        //    };
        //    var result = await service.AutoJournalVBRequest(dto);

        //    //Assert
        //    Assert.NotEqual(0, result);

        //}

    }
}
