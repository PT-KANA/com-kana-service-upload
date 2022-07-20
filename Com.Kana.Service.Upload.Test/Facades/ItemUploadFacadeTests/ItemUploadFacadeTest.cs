using AutoMapper;
using Com.DanLiris.Service.Purchasing.Lib.ViewModels.AccuItemViewModel;
using Com.Kana.Service.Upload.Lib;
using Com.Kana.Service.Upload.Lib.Facades;
using Com.Kana.Service.Upload.Lib.Interfaces;
using Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuItemModel;
using Com.Kana.Service.Upload.Lib.Services;
using Com.Kana.Service.Upload.Lib.ViewModels;
using Com.Kana.Service.Upload.Lib.ViewModels.ItemViewModel;
using Com.Kana.Service.Upload.Test.DataUtils.ItemDataUtils;
using Com.Kana.Service.Upload.Test.Helpers;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Primitives;
using Moq;
using OfficeOpenXml;
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
using static Com.Kana.Service.Upload.Test.DataUtils.ItemDataUtils.ItemDataUtil;

namespace Com.Kana.Service.Upload.Test.Facades.ItemUploadFacadeTests
{
	public class ItemUploadFacadeTest
	{
		private const string ENTITY = "ItemUploadFacade";

		private const string USERNAME = "Unit Test";
		private IServiceProvider ServiceProvider { get; set; }
		private IIntegrationFacade IntegrationFacade { get; set; }
		private   IMapper mapper { get; set; }
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
 

			var facade = new ItemFacade(ServiceProvider, integrationProvider.Object, _dbContext("user"), mapper);


			HttpClientService
				.Setup(x => x.GetAsync(It.IsAny<string>()))
				.ReturnsAsync(message);
			HttpClientService
				.Setup(x => x.GetAsync(It.Is<string>(s => s.Contains("https://account.accurate.id/api/open-db.do?id=578154"))))
				.ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(new ItemDataUtil(facade).GetResultFormatterOkString()) });
			HttpClientService
				.Setup(x => x.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
				.ReturnsAsync(messagePost);
			accurateProvider
				.Setup(x => x.PostAsync(It.Is<string>(s => s.Contains("item/save.do")), It.IsAny<HttpContent>()))
				.ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(new ItemDataUtil(facade).GetResultFormatterResponseOkString()) });
			 
			accurateProvider
			 .Setup(x => x.SendAsync(HttpMethod.Get, It.Is<string>(s => s.Contains("https://account.accurate.id/api/open-db.do?id=578154")), It.IsAny<HttpContent>()))
			  .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(new ItemDataUtil(facade).GetResultFormatterOkString()) });


 
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
		private ItemDataUtil dataUtil(ItemFacade facade, string testName)
		{
			var itemfacade = new ItemFacade(ServiceProvider,IntegrationFacade, _dbContext(testName),mapper);
			 
			return new ItemDataUtil(facade);
		}

		private ItemDataUtilCSV dataUtilCSV(ItemFacade facade, string testName)
		{
			var itemfacade = new ItemFacade(ServiceProvider, IntegrationFacade, _dbContext(testName), mapper);


			return new ItemDataUtilCSV(facade);
		}
		private ItemDataUtilViewModel dataUtilViewModel (ItemFacade facade, string testName)
		{
			var itemfacade = new ItemFacade(ServiceProvider, IntegrationFacade, _dbContext(testName), mapper);

			return new ItemDataUtilViewModel(facade);
		}
		[Fact]
		public async Task Should_Success_Upload_Data()
		{
			ItemFacade facade = new ItemFacade(ServiceProvider, IntegrationFacade, _dbContext("user"), mapper);
			var model = await dataUtil(facade, GetCurrentMethod()).GetTestData();
			List<AccuItem> data = new List<AccuItem>();
			data.Add(model);
			var Response = facade.UploadData(data, USERNAME);
			Assert.NotNull(Response);
		}

		[Fact]
		public void Should_Success_Validate_UploadData()
		{
			ItemFacade facade = new ItemFacade(ServiceProvider, IntegrationFacade, _dbContext("user"), mapper);
			var viewModel = dataUtilCSV(facade, GetCurrentMethod()).GetNewDataValid();
			var viewModel2 = dataUtilCSV(facade, GetCurrentMethod()).GetNewData1();
			var viewModel3 = dataUtilCSV(facade, GetCurrentMethod()).GetNewData2();
			List<ItemCsvViewModel> data = new List<ItemCsvViewModel>();
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
			ItemFacade facade = new ItemFacade(ServiceProvider, IntegrationFacade, _dbContext("user"), mapper);
			var viewModel = dataUtilCSV(facade, GetCurrentMethod()).GetNewDataValid();
			var viewModel2 = dataUtilCSV(facade, GetCurrentMethod()).GetNewDataValid();


			var model =   dataUtil(facade, GetCurrentMethod()).GetTestData();
			List<AccuItem> AccuItem = new List<AccuItem>();
			AccuItem.Add(model.Result);
			var Response = facade.UploadData(AccuItem, USERNAME);

			List<ItemCsvViewModel> data = new List<ItemCsvViewModel>();
			data.Add(viewModel);
			 
			 
			List<KeyValuePair<string, StringValues>> Body = new List<KeyValuePair<string, StringValues>>();

			 
			var Response1 = facade.UploadValidate(ref data, Body);
			Assert.True(Response1.Item2.Count() > 0);
		}
		[Fact]
		public async Task ShouldSuccesReadForUpload()
		{
			ItemFacade facade = new ItemFacade(ServiceProvider, IntegrationFacade, _dbContext("user"), mapper);
			var model = await dataUtil(facade, GetCurrentMethod()).GetTestData();
			List<AccuItem> data = new List<AccuItem>();
			data.Add(model);
			 var upload=  facade.UploadData(data, USERNAME);

			var Response = facade.ReadForUpload(1, 25, "{}", "", "");

			Assert.NotNull(Response);
		}

		[Fact]
		public async Task Should_Success_Map_ViewModel()
		{
			ItemFacade facade = new ItemFacade(ServiceProvider, IntegrationFacade, _dbContext("user"), mapper);
			var viewModel = dataUtilCSV(facade, GetCurrentMethod()).GetNewDataValid();

			List<ItemCsvViewModel> data = new List<ItemCsvViewModel>();
			data.Add(viewModel);

			var Response = await facade.MapToViewModel(data);

			Assert.NotNull(Response);
		}

		[Fact]
		public async Task ShouldSuccesMap_ToModel()
		{
			ItemFacade facade = new ItemFacade(ServiceProvider, IntegrationFacade, _dbContext("user"), mapper);
			var model = dataUtilViewModel(facade, GetCurrentMethod()).GetNewDataValid();
			List<AccuItemViewModel> data = new List<AccuItemViewModel>();
			data.Add(model);
			var Response = facade.MapToModel(data);

			Assert.NotNull(Response);
		}
		[Fact]
		public async Task ShouldSuccesUploadToAccurate()
		{
			var HttpClientService = new Mock<IHttpClientService>();
			var mockIntegrationFacade = new Mock<IIntegrationFacade>();
			ItemFacade facade = new ItemFacade(GetServiceProvider().Object,  mockIntegrationFacade.Object, _dbContext("user"), mapper);
			mockIntegrationFacade
				.Setup(x => x.RefreshToken())
				.ReturnsAsync(new AccurateTokenViewModel { access_token = "2201921" });

			mockIntegrationFacade
				.Setup(x => x.OpenDb())
				.ReturnsAsync(new AccurateSessionViewModel { session = "1201201", host = "https://zeus.accurate.co.id" });



			var model = dataUtilViewModel(facade, GetCurrentMethod()).GetNewDataValid();
			List<AccuItemViewModel> data = new List<AccuItemViewModel>();
			data.Add(model);
			 
			var model2 = await dataUtil(facade, GetCurrentMethod()).GetTestData();
			List<AccuItem> data2 = new List<AccuItem>();
			data2.Add(model2);
			data2.Add(model2);

			var ResponseCreated = facade.UploadData(data2, USERNAME);
			var Response = facade.Create(data, "user");

			Assert.NotNull(Response);
		}


	}
}
