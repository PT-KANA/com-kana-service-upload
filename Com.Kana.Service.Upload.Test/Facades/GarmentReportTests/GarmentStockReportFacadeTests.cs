using Com.Kana.Service.Upload.Lib;
using Com.Kana.Service.Upload.Lib.Facades.GarmentDeliveryOrderFacades;
using Com.Kana.Service.Upload.Lib.Facades.GarmentExternalPurchaseOrderFacades;
using Com.Kana.Service.Upload.Lib.Facades.GarmentInternalPurchaseOrderFacades;
using Com.Kana.Service.Upload.Lib.Facades.GarmentPurchaseRequestFacades;
using Com.Kana.Service.Upload.Lib.Facades.GarmentReports;
using Com.Kana.Service.Upload.Lib.Facades.GarmentUnitDeliveryOrderFacades;
using Com.Kana.Service.Upload.Lib.Facades.GarmentUnitReceiptNoteFacades;
using Com.Kana.Service.Upload.Lib.Interfaces;
using Com.Kana.Service.Upload.Lib.Services;
using Com.Kana.Service.Upload.Test.DataUtils.GarmentDeliveryOrderDataUtils;
using Com.Kana.Service.Upload.Test.DataUtils.GarmentExternalPurchaseOrderDataUtils;
using Com.Kana.Service.Upload.Test.DataUtils.GarmentInternalPurchaseOrderDataUtils;
using Com.Kana.Service.Upload.Test.DataUtils.GarmentPurchaseRequestDataUtils;
using Com.Kana.Service.Upload.Test.DataUtils.GarmentUnitDeliveryOrderDataUtils;
using Com.Kana.Service.Upload.Test.DataUtils.GarmentUnitReceiptNoteDataUtils;
using Com.Kana.Service.Upload.Test.DataUtils.NewIntegrationDataUtils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Com.Kana.Service.Upload.Test.Facades.GarmentReportTests
{
	public class GarmentStockReportFacadeTests
	{ 
        private const string ENTITY = "GarmentStockReport";

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

		private Mock<IServiceProvider> GetServiceProvider()
		{
			HttpResponseMessage message = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
			message.Content = new StringContent("{\"apiVersion\":\"1.0\",\"statusCode\":200,\"message\":\"Ok\",\"data\":[{\"Id\":7,\"Composition\":\"compos\",\"Width\":\"11\",\"Const\":\"aa\",\"Yarn\":\"yarn\",\"codeRequirement\":\"BB\",\"code\":\"BB\",\"rate\":13700.0,\"Name\":\"FABRIC\",\"date\":\"2018/10/20\"}],\"info\":{\"count\":1,\"page\":1,\"size\":1,\"total\":2,\"order\":{\"date\":\"desc\"},\"select\":[\"Id\",\"code\",\"rate\",\"date\"]}}");
			var HttpClientService = new Mock<IHttpClientService>();
			HttpClientService
				.Setup(x => x.GetAsync(It.IsAny<string>()))
				.ReturnsAsync(message);

			HttpClientService
			  .Setup(x => x.SendAsync(It.IsAny<HttpMethod>(), It.Is<string>(s => s.Contains("master/garmentProducts/byCode")), It.IsAny<HttpContent>()))
			  .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(new GarmentProductDataUtil().GetMultipleResultFormatterOkString()) });

			var serviceProvider = new Mock<IServiceProvider>();
			serviceProvider
				.Setup(x => x.GetService(typeof(IdentityService)))
				.Returns(new IdentityService() { Token = "Token", Username = "Test" });

			serviceProvider
				.Setup(x => x.GetService(typeof(IHttpClientService)))
				.Returns(HttpClientService.Object);



			return serviceProvider;
		}

		private GarmentUnitReceiptNoteDataUtil garmentUnitReceiptNoteDataUtil(GarmentUnitReceiptNoteFacade garmentUnitReceiptNoteFacade, string testName)
		{
			var garmentPurchaseRequestFacade = new GarmentPurchaseRequestFacade(GetServiceProvider().Object, _dbContext(testName));
			var garmentPurchaseRequestDataUtil = new GarmentPurchaseRequestDataUtil(garmentPurchaseRequestFacade);

			var garmentInternalPurchaseOrderFacade = new GarmentInternalPurchaseOrderFacade(_dbContext(testName));
			var garmentInternalPurchaseOrderDataUtil = new GarmentInternalPurchaseOrderDataUtil(garmentInternalPurchaseOrderFacade, garmentPurchaseRequestDataUtil);

			var garmentExternalPurchaseOrderFacade = new GarmentExternalPurchaseOrderFacade(GetServiceProvider().Object, _dbContext(testName));
			var garmentExternalPurchaseOrderDataUtil = new GarmentExternalPurchaseOrderDataUtil(garmentExternalPurchaseOrderFacade, garmentInternalPurchaseOrderDataUtil);

			var garmentDeliveryOrderFacade = new GarmentDeliveryOrderFacade(GetServiceProvider().Object, _dbContext(testName));
			var garmentDeliveryOrderDataUtil = new GarmentDeliveryOrderDataUtil(garmentDeliveryOrderFacade, garmentExternalPurchaseOrderDataUtil);

			return new GarmentUnitReceiptNoteDataUtil(garmentUnitReceiptNoteFacade, garmentDeliveryOrderDataUtil, null);
		}

		private GarmentUnitDeliveryOrderDataUtil UnitDOdataUtil(GarmentUnitDeliveryOrderFacade garmentUnitDeliveryOrderFacade, string testName)
		{
			var garmentUnitReceiptNoteFacade = new GarmentUnitReceiptNoteFacade(GetServiceProvider().Object, _dbContext(testName));
			var garmentUnitReceiptNoteDataUtil = this.garmentUnitReceiptNoteDataUtil(garmentUnitReceiptNoteFacade, testName);

			return new GarmentUnitDeliveryOrderDataUtil(garmentUnitDeliveryOrderFacade, garmentUnitReceiptNoteDataUtil);
		}
		 
		[Fact]
		public async Task Should_Success_GENERATEEXCENotSample()
		{
			var dbContext = _dbContext(GetCurrentMethod());
			var facadeGarmentstockReport = new GarmentStockReportFacade(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
			var facade = new GarmentUnitReceiptNoteFacade(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
			var data = await garmentUnitReceiptNoteDataUtil(facade, GetCurrentMethod()).GetTestDataMonitoringFlow();
			var Response = facadeGarmentstockReport.GenerateExcelStockReport("BB","FABRIC","SAMPLE","SMP1",  DateTime.Now.AddDays(-1), DateTime.Now.AddDays(4),7);

			Assert.NotNull(Response);

		}

		[Fact]
		public async Task Should_Success_Get_Monitoring()
		{
			var dbContext = _dbContext(GetCurrentMethod());
			var facadeGarmentstockReport= new GarmentStockReportFacade(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
			var facade = new GarmentUnitReceiptNoteFacade(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
			 var data = await garmentUnitReceiptNoteDataUtil(facade, GetCurrentMethod()).GetTestDataMonitoringFlow();
			var Response = facadeGarmentstockReport.GetStockReport(7,"SMP1", "BB", 1, 25, "{}", DateTime.Now.AddDays(-1), DateTime.Now.AddDays(4));
			Assert.NotNull(Response.Item1);
		}
		 
	}
}
