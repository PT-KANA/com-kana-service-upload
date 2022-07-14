using AutoMapper;
using Com.DanLiris.Service.Purchasing.Lib.ViewModels.AccuItemViewModel;
using Com.Kana.Service.Upload.Lib;
using Com.Kana.Service.Upload.Lib.Facades;
using Com.Kana.Service.Upload.Lib.Interfaces;
using Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuItemModel;
using Com.Kana.Service.Upload.Lib.Services;
using Com.Kana.Service.Upload.Lib.ViewModels.ItemViewModel;
using Com.Kana.Service.Upload.Test.DataUtils.ItemDataUtils;
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
	}
}
