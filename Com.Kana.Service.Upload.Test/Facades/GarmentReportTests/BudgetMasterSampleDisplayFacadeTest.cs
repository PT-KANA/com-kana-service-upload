using Com.Kana.Service.Upload.Lib;
using Com.Kana.Service.Upload.Lib.Facades.GarmentPurchaseRequestFacades;
using Com.Kana.Service.Upload.Lib.Facades.GarmentReports;
using Com.Kana.Service.Upload.Lib.Interfaces;
using Com.Kana.Service.Upload.Lib.Services;
using Com.Kana.Service.Upload.Test.DataUtils.GarmentPurchaseRequestDataUtils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xunit;

namespace Com.Kana.Service.Upload.Test.Facades.GarmentReportTests
{
    public class BudgetMasterSampleDisplayFacadeTest
    {
        private const string ENTITY = "BudgetMasterSampleDisplay";

        public string GetCurrentMethod()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);

            return string.Concat(sf.GetMethod().Name, "_", ENTITY);
        }

        private Mock<IServiceProvider> GetMockServiceProvider()
        {
            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider
                .Setup(x => x.GetService(typeof(IdentityService)))
                .Returns(new IdentityService { Username = "Username" });

            return mockServiceProvider;
        }

        private UploadDbContext GetDbContext(string testName)
        {
            DbContextOptionsBuilder<UploadDbContext> optionsBuilder = new DbContextOptionsBuilder<UploadDbContext>();
            optionsBuilder
                .UseInMemoryDatabase(testName)
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .EnableSensitiveDataLogging();

            UploadDbContext dbContext = new UploadDbContext(optionsBuilder.Options);

            return dbContext;
        }

        private GarmentPurchaseRequestDataUtil GetDataUtil(GarmentPurchaseRequestFacade facade)
        {
            return new GarmentPurchaseRequestDataUtil(facade);
        }

        [Fact]
        public async void Should_Success_Get_Monitoring()
        {
            var mockServiceProvider = GetMockServiceProvider();

            var dbContext = GetDbContext(GetCurrentMethod());

            var garmentPurchaseRequestFacade = new GarmentPurchaseRequestFacade(mockServiceProvider.Object, dbContext);
            var dataUtil = GetDataUtil(garmentPurchaseRequestFacade);
            var dataGarmentPurchaseRequest = await dataUtil.GetTestData();

            var facade = new BudgetMasterSampleDisplayFacade(mockServiceProvider.Object, dbContext);

            var Response = facade.GetMonitoring(dataGarmentPurchaseRequest.Id, "{}");
            Assert.NotEmpty(Response.Item1);
        }

        [Fact]
        public async void Should_Success_Get_Excel()
        {
            var mockServiceProvider = GetMockServiceProvider();

            var dbContext = GetDbContext(GetCurrentMethod());

            var garmentPurchaseRequestFacade = new GarmentPurchaseRequestFacade(mockServiceProvider.Object, dbContext);
            var dataUtil = GetDataUtil(garmentPurchaseRequestFacade);
            var dataGarmentPurchaseRequest = await dataUtil.GetTestData();

            var facade = new BudgetMasterSampleDisplayFacade(mockServiceProvider.Object, dbContext);

            var Response = facade.GenerateExcel(dataGarmentPurchaseRequest.Id);
            Assert.NotNull(Response);
        }
    }
}
