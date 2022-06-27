using Com.Kana.Service.Upload.Lib;
using Com.Kana.Service.Upload.Lib.Facades.GarmentDeliveryOrderFacades;
using Com.Kana.Service.Upload.Lib.Facades.GarmentExternalPurchaseOrderFacades;
using Com.Kana.Service.Upload.Lib.Facades.GarmentInternalPurchaseOrderFacades;
using Com.Kana.Service.Upload.Lib.Facades.GarmentPOMasterDistributionFacades;
using Com.Kana.Service.Upload.Lib.Facades.GarmentPurchaseRequestFacades;
using Com.Kana.Service.Upload.Lib.Facades.GarmentReports;
using Com.Kana.Service.Upload.Lib.Interfaces;
using Com.Kana.Service.Upload.Lib.Models.GarmentDeliveryOrderModel;
using Com.Kana.Service.Upload.Lib.Services;
using Com.Kana.Service.Upload.Test.DataUtils.GarmentDeliveryOrderDataUtils;
using Com.Kana.Service.Upload.Test.DataUtils.GarmentExternalPurchaseOrderDataUtils;
using Com.Kana.Service.Upload.Test.DataUtils.GarmentInternalPurchaseOrderDataUtils;
using Com.Kana.Service.Upload.Test.DataUtils.GarmentPOMasterDistributionDataUtils;
using Com.Kana.Service.Upload.Test.DataUtils.GarmentPurchaseRequestDataUtils;
using Com.Kana.Service.Upload.Test.DataUtils.NewIntegrationDataUtils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xunit;

namespace Com.Kana.Service.Upload.Test.Facades.GarmentReportTests
{
    public class MonitoringROJobOrderFacadeTest
    {
        private const string ENTITY = "MonitoringROJobOrder";

        [MethodImpl(MethodImplOptions.NoInlining)]
        public string GetCurrentMethod()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);

            return string.Concat(sf.GetMethod().Name, "_", ENTITY);
        }

        private Mock<IHttpClientService> GetMockHttpClientService()
        {
            var mockHttpClientService = new Mock<IHttpClientService>();

            mockHttpClientService
                .Setup(x => x.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{\"apiVersion\":\"1.0\",\"statusCode\":200,\"message\":\"Ok\",\"data\":[{\"Id\":7,\"code\":\"USD\",\"rate\":13700.0,\"date\":\"2018/10/20\"}],\"info\":{\"count\":1,\"page\":1,\"size\":1,\"total\":2,\"order\":{\"date\":\"desc\"},\"select\":[\"Id\",\"code\",\"rate\",\"date\"]}}") });
            mockHttpClientService
               .Setup(x => x.GetAsync(It.Is<string>(s => s.Contains("master/garment-suppliers"))))
               .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(new SupplierDataUtil().GetResultFormatterOkString()) });
            mockHttpClientService
               .Setup(x => x.GetAsync(It.Is<string>(s => s.Contains("cost-calculation-garments"))))
               .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(new CostCalculationGarmentDataUtil().GetResultFormatterOkString()) });
            mockHttpClientService
               .Setup(x => x.GetAsync(It.Is<string>(s => s.Contains("master/garmentProducts"))))
               .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(new GarmentProductDataUtil().GetMultipleResultFormatterOkString()) });

            return mockHttpClientService;
        }

        private Mock<IServiceProvider> GetMockServiceProvider()
        {
            var httpClientService = GetMockHttpClientService();

            var mockGarmentDeliveryOrderFacade = new Mock<IGarmentDeliveryOrderFacade>();
            mockGarmentDeliveryOrderFacade
                .Setup(x => x.ReadById(It.IsAny<int>()))
                .Returns(new GarmentDeliveryOrder());

            var mockServiceProvider = new Mock<IServiceProvider>();
            mockServiceProvider
                .Setup(x => x.GetService(typeof(IdentityService)))
                .Returns(new IdentityService { Username = "Username" });
            mockServiceProvider
                .Setup(x => x.GetService(typeof(IHttpClientService)))
                .Returns(httpClientService.Object);
            mockServiceProvider
                .Setup(x => x.GetService(typeof(IGarmentDeliveryOrderFacade)))
                .Returns(mockGarmentDeliveryOrderFacade.Object);


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

        private BasicDataUtil dataUtil(GarmentPOMasterDistributionFacade facade, UploadDbContext dbContext)
        {
            var mockServiceProvider = GetMockServiceProvider();
            var garmentPurchaseRequestFacade = new GarmentPurchaseRequestFacade(mockServiceProvider.Object, dbContext);
            var garmentPurchaseRequestDataUtil = new GarmentPurchaseRequestDataUtil(garmentPurchaseRequestFacade);

            var garmentInternalPurchaseOrderFacade = new GarmentInternalPurchaseOrderFacade(dbContext);
            var garmentInternalPurchaseOrderDataUtil = new GarmentInternalPurchaseOrderDataUtil(garmentInternalPurchaseOrderFacade, garmentPurchaseRequestDataUtil);

            var garmentExternalPurchaseOrderFacade = new GarmentExternalPurchaseOrderFacade(mockServiceProvider.Object, dbContext);
            var garmentExternalPurchaseOrderDataUtil = new GarmentExternalPurchaseOrderDataUtil(garmentExternalPurchaseOrderFacade, garmentInternalPurchaseOrderDataUtil);

            var garmentDeliveryOrderFacade = new GarmentDeliveryOrderFacade(mockServiceProvider.Object, dbContext);
            var garmentDeliveryOrderDataUtil = new GarmentDeliveryOrderDataUtil(garmentDeliveryOrderFacade, garmentExternalPurchaseOrderDataUtil);

            return new BasicDataUtil(facade, garmentDeliveryOrderDataUtil);
        }

        //[Fact]
        //public async Task Should_Success_Get_Monitoring()
        //{
        //    var mockServiceProvider = GetMockServiceProvider();

        //    var dbContext = GetDbContext(GetCurrentMethod());

        //    var garmentPOMasterDistributionFacade  = new GarmentPOMasterDistributionFacade(mockServiceProvider.Object, dbContext);
        //    var dataGarmentPOMasterDistribution = await dataUtil(garmentPOMasterDistributionFacade, dbContext).GetTestData();

        //    var costCalculationGarmentDataUtil = new CostCalculationGarmentDataUtil();
        //    var ccData = costCalculationGarmentDataUtil.GetNewData();
        //    ccData.CostCalculationGarment_Materials.First().PO_SerialNumber = dataGarmentPOMasterDistribution.Items.First().Details.First().POSerialNumber;

        //    var mockHttpClientService = GetMockHttpClientService();
        //    mockHttpClientService
        //       .Setup(x => x.GetAsync(It.Is<string>(s => s.Contains("cost-calculation-garments"))))
        //       .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(costCalculationGarmentDataUtil.GetResultFormatterOkString(ccData)) });
        //    mockServiceProvider
        //        .Setup(x => x.GetService(typeof(IHttpClientService)))
        //        .Returns(mockHttpClientService.Object);

        //    var facade = new MonitoringROJobOrderFacade(mockServiceProvider.Object, dbContext);

        //    var Response = await facade.GetMonitoring(dataGarmentPOMasterDistribution.Id);
        //    Assert.NotEmpty(Response);

        //    Assert.NotEqual(0, Response.Sum(d => d.Items.Count()));
        //}

        //[Fact]
        //public async Task Should_Error_Get_Monitoring_Failed_Get_CostCalculationGarment()
        //{
        //    var httpClientService = GetMockHttpClientService();
        //    httpClientService
        //       .Setup(x => x.GetAsync(It.Is<string>(s => s.Contains("cost-calculation-garments"))))
        //       .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content = new StringContent(string.Empty) });

        //    var mockServiceProvider = GetMockServiceProvider();
        //    mockServiceProvider
        //        .Setup(x => x.GetService(typeof(IHttpClientService)))
        //        .Returns(httpClientService.Object);

        //    var serviceProvider = mockServiceProvider.Object;
        //    var dbContext = GetDbContext(GetCurrentMethod());

        //    var garmentPOMasterDistributionFacade  = new GarmentPOMasterDistributionFacade(serviceProvider, dbContext);
        //    var data = await dataUtil(garmentPOMasterDistributionFacade, dbContext).GetTestData();

        //    var facade = new MonitoringROJobOrderFacade(serviceProvider, dbContext);

        //    Exception e = await Assert.ThrowsAsync<Exception>(async () => await facade.GetMonitoring(data.Id));
        //    Assert.NotNull(e.Message);
        //}

        //[Fact]
        //public async Task Should_Error_Get_Monitoring_Failed_Get_GarmentProduct()
        //{
        //    var httpClientService = GetMockHttpClientService();
        //    httpClientService
        //       .Setup(x => x.GetAsync(It.Is<string>(s => s.Contains("master/garmentProducts"))))
        //       .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content = new StringContent(string.Empty) });

        //    var mockServiceProvider = GetMockServiceProvider();
        //    mockServiceProvider
        //        .Setup(x => x.GetService(typeof(IHttpClientService)))
        //        .Returns(httpClientService.Object);

        //    var serviceProvider = mockServiceProvider.Object;
        //    var dbContext = GetDbContext(GetCurrentMethod());

        //    var garmentPOMasterDistributionFacade  = new GarmentPOMasterDistributionFacade(serviceProvider, dbContext);
        //    var data = await dataUtil(garmentPOMasterDistributionFacade, dbContext).GetTestData();

        //    var facade = new MonitoringROJobOrderFacade(serviceProvider, dbContext);

        //    Exception e = await Assert.ThrowsAsync<Exception>(async () => await facade.GetMonitoring(data.Id));
        //    Assert.NotNull(e.Message);
        //}

        //[Fact]
        //public async Task Should_Success_Get_Excel()
        //{
        //    var mockServiceProvider = GetMockServiceProvider();

        //    var dbContext = GetDbContext(GetCurrentMethod());

        //    var garmentPOMasterDistributionFacade = new GarmentPOMasterDistributionFacade(mockServiceProvider.Object, dbContext);
        //    var dataGarmentPOMasterDistribution = await dataUtil(garmentPOMasterDistributionFacade, dbContext).GetTestData();

        //    var costCalculationGarmentDataUtil = new CostCalculationGarmentDataUtil();
        //    var ccData = costCalculationGarmentDataUtil.GetNewData();
        //    ccData.CostCalculationGarment_Materials.First().PO_SerialNumber = dataGarmentPOMasterDistribution.Items.First().Details.First().POSerialNumber;
        //    var ccData2 = costCalculationGarmentDataUtil.GetNewData();
        //    ccData.CostCalculationGarment_Materials.Add(ccData2.CostCalculationGarment_Materials.First());

        //    var mockHttpClientService = GetMockHttpClientService();
        //    mockHttpClientService
        //       .Setup(x => x.GetAsync(It.Is<string>(s => s.Contains("cost-calculation-garments"))))
        //       .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(costCalculationGarmentDataUtil.GetResultFormatterOkString(ccData)) });
        //    mockServiceProvider
        //        .Setup(x => x.GetService(typeof(IHttpClientService)))
        //        .Returns(mockHttpClientService.Object);

        //    var facade = new MonitoringROJobOrderFacade(mockServiceProvider.Object, dbContext);

        //    var Response = await facade.GetExcel(dataGarmentPOMasterDistribution.Id);
        //    Assert.NotNull(Response.Item2);
        //}

    }
}
