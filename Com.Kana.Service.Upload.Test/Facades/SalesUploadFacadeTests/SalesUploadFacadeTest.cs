using AutoMapper;
using Com.Kana.Service.Upload.Lib;
using Com.Kana.Service.Upload.Lib.Facades;
using Com.Kana.Service.Upload.Lib.Interfaces;
using Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuSalesInvoiceModel;
using Com.Kana.Service.Upload.Lib.Services;
using Com.Kana.Service.Upload.Lib.ViewModels;
using Com.Kana.Service.Upload.Lib.ViewModels.AccuSalesViewModel;
using Com.Kana.Service.Upload.Lib.ViewModels.ItemViewModel;
using Com.Kana.Service.Upload.Lib.ViewModels.SalesViewModel;
using Com.Kana.Service.Upload.Test.DataUtils.SalesDataUtils;
using Com.Kana.Service.Upload.Test.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Primitives;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static Com.Kana.Service.Upload.Test.DataUtils.SalesDataUtils.SalesDataUtil;

namespace Com.Kana.Service.Upload.Test.Facades.SalesUploadFacadeTests
{
	public class SalesUploadFacadeTest
	{
		private const string ENTITY = "SalesUploadFacade";

		private const string USERNAME = "Unit Test";
		private IServiceProvider ServiceProvider { get; set; }
		private IIntegrationFacade IntegrationFacade { get; set; }
		private IMapper mapper { get; set; }
		[MethodImpl(MethodImplOptions.NoInlining)]
		public string GetCurrentMethod()
		{
			StackTrace st = new StackTrace();
			StackFrame sf = st.GetFrame(1);

			return string.Concat(sf.GetMethod().Name, "_", ENTITY);
		}
		 
		private Mock<IServiceProvider> GetServiceProvider()
		{
			HttpResponseMessage message = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
			message.Content = new StringContent("{\"apiVersion\":\"1.0\",\"statusCode\":200,\"message\":\"Ok\",\"data\":[{\"Id\":7,\"code\":\"USD\",\"rate\":13700.0,\"date\":\"2018/10/20\"}],\"info\":{\"count\":1,\"page\":1,\"size\":1,\"total\":2,\"order\":{\"date\":\"desc\"},\"select\":[\"Id\",\"code\",\"rate\",\"date\"]}}");
			HttpResponseMessage messagePost = new HttpResponseMessage();

			var serviceProvider = new Mock<IServiceProvider>();
			var integrationProvider = new Mock<IIntegrationFacade>();
			var accurateProvider = new Mock<IAccurateClientService>();
				var HttpClientService = new Mock<IHttpClientService>();
			serviceProvider
				.Setup(x => x.GetService(typeof(IHttpClientService)))
				.Returns(new HttpClientTestService());

			//integrationProvider
			//	.Setup(x => x.RefreshToken().Result)
			//	.Returns(new AccurateTokenViewModel { access_token = "1212121" });

			//integrationProvider
			//	.Setup(x => x.OpenDb().Result)
			//.Returns(new AccurateSessionViewModel { host = "http://public.accurate.co.id", session = "1212112" });

			var SalesUploadFacade = new SalesUploadFacade(ServiceProvider, _dbContext("user"), integrationProvider.Object, mapper);

		
			HttpClientService
				.Setup(x => x.GetAsync(It.IsAny<string>()))
				.ReturnsAsync(message);
			HttpClientService
				.Setup(x => x.GetAsync(It.Is<string>(s => s.Contains("https://account.accurate.id/api/open-db.do?id=578154"))))
				.ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(new SalesDataUtil(SalesUploadFacade).GetResultFormatterOkString()) });
			HttpClientService
				.Setup(x => x.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
				.ReturnsAsync(messagePost);
			 accurateProvider
			  .Setup(x => x.SendAsync(HttpMethod.Get, It.Is<string>(s => s.Contains("https://account.accurate.id/api/open-db.do?id=578154")), It.IsAny<HttpContent>()))
			  .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(new SalesDataUtil(SalesUploadFacade).GetResultFormatterOkString()) });

			accurateProvider
			  .Setup(x => x.SendAsync(HttpMethod.Get, It.Is<string>(s => s.Contains("accurate/api/customer/list.do")), It.IsAny<HttpContent>()))
			  .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("") });

			accurateProvider
				.Setup(x => x.PostAsync(It.Is<string>(s => s.Contains("accurate/api/sales-return/save.do")), It.IsAny<HttpContent>()))
				.ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{\"s\":true, \"d\":[\"Proses Berhasil Dilakukan\"]}") });

			accurateProvider
				.Setup(x => x.SendAsync(HttpMethod.Post, It.Is<string>(s => s.Contains("accurate/api/sales-invoice/save.do")), It.IsAny<HttpContent>()))
				.ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(new SalesDataUtil(SalesUploadFacade).GetResponseOkString()) });

			accurateProvider
			  .Setup(x => x.SendAsync(HttpMethod.Get, It.Is<string>(s => s.Contains("accurate/api/customer/list.do")), It.IsAny<HttpContent>()))
			  .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(new SalesDataUtil(SalesUploadFacade).GetResponseOkString()) });

			serviceProvider
				.Setup(x => x.GetService(typeof(IIntegrationFacade)))
				.Returns(integrationProvider.Object);

			serviceProvider
				.Setup(x => x.GetService(typeof(IAccurateClientService)))
				.Returns(accurateProvider.Object);
			serviceProvider
				.Setup(x => x.GetService(typeof(IdentityService)))
				.Returns(new IdentityService() { Token = "Token", Username = "Test" });

			serviceProvider
				.Setup(x => x.GetService(typeof(IHttpClientService)))
				.Returns(HttpClientService.Object);

			return serviceProvider;
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
		private SalesDataUtil dataUtil(SalesUploadFacade facade, string testName)
		{
			var SalesUploadFacade = new SalesUploadFacade(ServiceProvider, _dbContext(testName), IntegrationFacade, mapper);

			return new SalesDataUtil(facade);
		}

		private SalesDataUtilCSV dataUtilCSV(SalesUploadFacade facade, string testName)
		{
			var SalesUploadFacade = new SalesUploadFacade(ServiceProvider, _dbContext(testName), IntegrationFacade, mapper);


			return new SalesDataUtilCSV(facade);
		}
		private SalesDataUtilViewModel dataUtilViewModel(SalesUploadFacade facade, string testName)
		{
			var SalesUploadFacade = new SalesUploadFacade(ServiceProvider, _dbContext(testName), IntegrationFacade, mapper);

			return new SalesDataUtilViewModel(facade);
		}
		[Fact]
		public async Task Should_Success_Upload_Data()
		{
			SalesUploadFacade facade = new SalesUploadFacade(ServiceProvider, _dbContext("user"), IntegrationFacade, mapper);
			var model = await dataUtil(facade, GetCurrentMethod()).GetTestData();
			List<AccuSalesInvoice> data = new List<AccuSalesInvoice>();
			data.Add(model);
			var Response = facade.UploadData(data, USERNAME);
			Assert.NotNull(Response);
		}

		[Fact]
		public void Should_Success_Validate_UploadData()
		{
			SalesUploadFacade facade = new SalesUploadFacade(ServiceProvider, _dbContext("user"), IntegrationFacade, mapper);
			var viewModel = dataUtilCSV(facade, GetCurrentMethod()).GetNewDataValid();
			var viewModel2 = dataUtilCSV(facade, GetCurrentMethod()).GetNewData1();
			var viewModel3 = dataUtilCSV(facade, GetCurrentMethod()).GetNewData2();
			List<SalesCsvViewModel> data = new List<SalesCsvViewModel>();
			data.Add(viewModel);
			data.Add(viewModel2);
			data.Add(viewModel3);
			List<KeyValuePair<string, StringValues>> Body = new List<KeyValuePair<string, StringValues>>();

			var Response = facade.UploadValidate(ref data, Body);
			Assert.True(Response.Item2.Count() > 0);
		}


		[Fact]
		public void Should_Success_Validate_UploadDataIsExist()
		{
			SalesUploadFacade facade = new SalesUploadFacade(ServiceProvider, _dbContext("user"), IntegrationFacade, mapper);
			var viewModel = dataUtilCSV(facade, GetCurrentMethod()).GetNewDataValid();
			var viewModel2 = dataUtilCSV(facade, GetCurrentMethod()).GetNewDataValid();


			var model = dataUtil(facade, GetCurrentMethod()).GetTestData();
			List<AccuSalesInvoice> AccuItem = new List<AccuSalesInvoice>();
			AccuItem.Add(model.Result);
			var Response = facade.UploadData(AccuItem, USERNAME);

			List<SalesCsvViewModel> data = new List<SalesCsvViewModel>();
			data.Add(viewModel);


			List<KeyValuePair<string, StringValues>> Body = new List<KeyValuePair<string, StringValues>>();


			var Response1 = facade.UploadValidate(ref data, Body);
			Assert.True(Response1.Item2.Count() > 0);
		}
		[Fact]
		public async Task ShouldSuccesReadForUpload()
		{
			SalesUploadFacade facade = new SalesUploadFacade(ServiceProvider, _dbContext("user"), IntegrationFacade, mapper);
			var model = await dataUtil(facade, GetCurrentMethod()).GetTestData();
			List<AccuSalesInvoice> data = new List<AccuSalesInvoice>();
			data.Add(model);
			var upload = facade.UploadData(data, USERNAME);

			var Response = facade.ReadForUpload(1, 25, "{}", "", "");

			Assert.NotNull(Response);
		}
		[Fact]
		public async Task ShouldSuccesReadForApproved()
		{
			SalesUploadFacade facade = new SalesUploadFacade(ServiceProvider, _dbContext("user"), IntegrationFacade, mapper);
			var model = await dataUtil(facade, GetCurrentMethod()).GetTestData();
			List<AccuSalesInvoice> data = new List<AccuSalesInvoice>();
			data.Add(model);
			var upload = facade.UploadData(data, USERNAME);

			var Response = facade.ReadForApproved(1, 25, "{}", "", "");

			Assert.NotNull(Response);
		}
		[Fact]
		public async Task Should_Success_Map_ViewModel()
		{
			SalesUploadFacade facade = new SalesUploadFacade(ServiceProvider, _dbContext("user"), IntegrationFacade, mapper);
			var viewModel = dataUtilCSV(facade, GetCurrentMethod()).GetNewDataValid();

			List<SalesCsvViewModel> data = new List<SalesCsvViewModel>();
			data.Add(viewModel);

			var Response = await facade.MapToViewModel(data);

			Assert.NotNull(Response);
		}

		[Fact]
		public async Task ShouldSuccesMap_ToModel()
		{
			SalesUploadFacade facade = new SalesUploadFacade(ServiceProvider, _dbContext("user"), IntegrationFacade, mapper);
			var model = dataUtilViewModel(facade, GetCurrentMethod()).GetNewDataValid();
			List<AccuSalesViewModel> data = new List<AccuSalesViewModel>();
			data.Add(model);
			var Response = facade.MapToModel(data);

			Assert.NotNull(Response);
		}

		[Fact]
		public async Task ShouldSuccesObenDP()
		{
			var HttpClientService = new Mock<IHttpClientService>();
			var mockIntegrationFacade = new Mock<IIntegrationFacade>();
			SalesUploadFacade facade = new SalesUploadFacade(GetServiceProvider().Object, _dbContext("user"), mockIntegrationFacade.Object, mapper);
			mockIntegrationFacade
				.Setup(x => x.RefreshToken())
				.ReturnsAsync(new AccurateTokenViewModel { access_token = "2201921" });

			mockIntegrationFacade
				.Setup(x => x.OpenDb())
				.ReturnsAsync(new AccurateSessionViewModel { session = "1201201", host = "https://zeus.accurate.co.id" });

			HttpClientService
				.Setup(x => x.SendAsync(HttpMethod.Post,It.Is<string>(s => s.Contains("https://zeus.accurate.co.id/accurate/api/sales-invoice/save.do")), It.IsAny<HttpContent>()))
				.ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(new SalesDataUtil(facade).GetResponseOkString()) });


			var model = dataUtilViewModel(facade, GetCurrentMethod()).GetNewDataValid();
			List<AccuSalesViewModel> data = new List<AccuSalesViewModel>();
			data.Add(model);
			var Response = facade.Create(data, "user");

			Assert.NotNull(Response);
		}

		[Fact]
		public async Task ShouldSuccesApprove()
		{
			var HttpClientService = new Mock<IHttpClientService>();
			var mockIntegrationFacade = new Mock<IIntegrationFacade>();
			var accurateProvider = new Mock<IAccurateClientService>();
			SalesUploadFacade facade = new SalesUploadFacade(GetServiceProvider().Object, _dbContext("user"), mockIntegrationFacade.Object, mapper);
			mockIntegrationFacade
				.Setup(x => x.RefreshToken())
				.ReturnsAsync(new AccurateTokenViewModel { access_token = "2201921" });

			mockIntegrationFacade
				.Setup(x => x.OpenDb())
				.ReturnsAsync(new AccurateSessionViewModel { session = "1201201", host = "https://zeus.accurate.co.id" });

			var model = dataUtilViewModel(facade, GetCurrentMethod()).GetNewDataValid();
			List<AccuSalesViewModel> data = new List<AccuSalesViewModel>();
			data.Add(model);
			var Response = facade.CreateSalesReceipt(data, "user");

			Assert.NotNull(Response);
		}

	}
}
