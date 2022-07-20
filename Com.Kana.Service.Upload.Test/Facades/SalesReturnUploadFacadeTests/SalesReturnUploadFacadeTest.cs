using AutoMapper;
using Com.Kana.Service.Upload.Lib;
using Com.Kana.Service.Upload.Lib.Facades;
using Com.Kana.Service.Upload.Lib.Interfaces;
using Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuSalesReturnModel;
using Com.Kana.Service.Upload.Lib.ViewModels;
using Com.Kana.Service.Upload.Lib.ViewModels.AccuSalesReturnViewModel;
using Com.Kana.Service.Upload.Lib.ViewModels.SalesReturnViewModel;
using Com.Kana.Service.Upload.Test.DataUtils.SalesReturnDataUtils;
using Com.Kana.Service.Upload.Test.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Primitives;
using Moq;
using Newtonsoft.Json;
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
using static Com.Kana.Service.Upload.Test.DataUtils.SalesReturnDataUtils.SalesReturnDataUtil;

namespace Com.Kana.Service.Upload.Test.Facades.SalesReturnUploadFacadeTests
{
    public class SalesReturnUploadFacadeTest
    {
        private const string ENTITY = "ItemUploadFacade";

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

        private UploadDbContext _dbContext(string testName)
        {
            DbContextOptionsBuilder<UploadDbContext> optionsBuilder = new DbContextOptionsBuilder<UploadDbContext>();
            optionsBuilder
                .UseInMemoryDatabase(testName)
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));

            UploadDbContext dbContext = new UploadDbContext(optionsBuilder.Options);

            return dbContext;
        }
        private SalesReturnDataUtil dataUtil(SalesReturnUploadFacade facade, string testName)
        {
            var salesRFacade = new SalesReturnUploadFacade(ServiceProvider, IntegrationFacade, _dbContext(testName), mapper);

            return new SalesReturnDataUtil(salesRFacade);
        }
        private SalesReturnDataUtilCSV dataUtilCSV(SalesReturnUploadFacade facade, string testName)
        {
            var salesRFacade = new SalesReturnUploadFacade(ServiceProvider, IntegrationFacade, _dbContext(testName), mapper);

            return new SalesReturnDataUtilCSV(salesRFacade);
        }
        private SalesReturnDataUtilViewModel dataUtilViewModel(SalesReturnUploadFacade facade, string testName)
        {
            var salesRFacade = new SalesReturnUploadFacade(ServiceProvider, IntegrationFacade, _dbContext(testName), mapper);

            return new SalesReturnDataUtilViewModel(salesRFacade);
        }
        private Mock<IServiceProvider> GetServiceProvider(string testname)
        {
            var serviceProvider = new Mock<IServiceProvider>();
            var accurateProvider = new Mock<IAccurateClientService>();

            serviceProvider
                .Setup(x => x.GetService(typeof(IHttpClientService)))
                .Returns(new HttpClientTestService());
            
            accurateProvider
                .Setup(x => x.SendAsync(HttpMethod.Get, It.Is<string>(s => s.Contains("accurate/api/customer/list.do")), It.IsAny<HttpContent>()))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(JsonConvert.SerializeObject(new AccurateSearchCustomerViewModel { s = true, d = new List<AccurateCustomerViewModel> { new AccurateCustomerViewModel { customerNo = "C.00004", name = "PELANGGAN SHOPIFY", branch = new Dictionary<string, string> { { "name", "JAKARTA" } } } } })) });

            accurateProvider
                .Setup(x => x.PostAsync(It.Is<string>(s => s.Contains("accurate/api/sales-return/save.do")), It.IsAny<HttpContent>()))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{\"s\":true, \"d\":[\"Proses Berhasil Dilakukan\"]}") });

            serviceProvider
                .Setup(x => x.GetService(typeof(IAccurateClientService)))
                .Returns(accurateProvider.Object);

            return serviceProvider;
        }

        [Fact]
        public async Task Should_Success_Upload_Data()
        {
            SalesReturnUploadFacade facade = new SalesReturnUploadFacade(ServiceProvider, IntegrationFacade, _dbContext("user"), mapper);
            var model = await dataUtil(facade, GetCurrentMethod()).GetTestData();

            List<AccuSalesReturn> data = new List<AccuSalesReturn>();
            data.Add(model);

            var Response = facade.UploadData(data, USERNAME);
            Assert.NotNull(Response);
        }

        [Fact]
        public void Should_Success_Validate_UploadData()
        {
            SalesReturnUploadFacade facade = new SalesReturnUploadFacade(ServiceProvider, IntegrationFacade, _dbContext("user"), mapper);
            var viewModel = dataUtilCSV(facade, GetCurrentMethod()).GetNewDataValid();
            var viewModel2 = dataUtilCSV(facade, GetCurrentMethod()).GetNewData1();
            var viewModel3 = dataUtilCSV(facade, GetCurrentMethod()).GetNewData2();

            List<SalesReturnCsvViewModel> data = new List<SalesReturnCsvViewModel>();
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
            SalesReturnUploadFacade facade = new SalesReturnUploadFacade(ServiceProvider, IntegrationFacade, _dbContext("user"), mapper);
            var viewModel = dataUtilCSV(facade, GetCurrentMethod()).GetNewDataValid();
            var viewModel2 = dataUtilCSV(facade, GetCurrentMethod()).GetNewDataValid();
            var model = dataUtil(facade, GetCurrentMethod()).GetTestData();

            List<AccuSalesReturn> AccuItem = new List<AccuSalesReturn>();
            AccuItem.Add(model.Result);

            var Response = facade.UploadData(AccuItem, USERNAME);

            List<SalesReturnCsvViewModel> data = new List<SalesReturnCsvViewModel>();
            data.Add(viewModel);

            List<KeyValuePair<string, StringValues>> Body = new List<KeyValuePair<string, StringValues>>();

            var Response1 = facade.UploadValidate(ref data, Body);
            Assert.True(Response1.Item2.Count() > 0);
        }

        [Fact]
        public async Task ShouldSuccesReadForUpload()
        {
            SalesReturnUploadFacade facade = new SalesReturnUploadFacade(ServiceProvider, IntegrationFacade, _dbContext("user"), mapper);
            var model = await dataUtil(facade, GetCurrentMethod()).GetTestData();

            List<AccuSalesReturn> data = new List<AccuSalesReturn>();
            data.Add(model);

            var upload = facade.UploadData(data, USERNAME);
            var Response = facade.ReadForUpload(1, 25, "{}", "", "");

            Assert.NotNull(Response);
        }

        [Fact]
        public async Task Should_Success_Map_ViewModel()
        {
            SalesReturnUploadFacade facade = new SalesReturnUploadFacade(ServiceProvider, IntegrationFacade, _dbContext("user"), mapper);
            var viewModel = dataUtilCSV(facade, GetCurrentMethod()).GetNewListDataValid();

            //List<SalesReturnCsvViewModel> data = new List<SalesReturnCsvViewModel>();
            //data.Add(viewModel);

            var Response = await facade.MapToViewModel(viewModel);

            Assert.NotNull(Response);
        }

        [Fact]
        public async Task Should_Success_Map_ToModel()
        {
            SalesReturnUploadFacade facade = new SalesReturnUploadFacade(ServiceProvider, IntegrationFacade, _dbContext("user"), mapper);
            var model = dataUtilViewModel(facade, GetCurrentMethod()).GetNewDataValid();

            List<AccuSalesReturnViewModel> data = new List<AccuSalesReturnViewModel>();
            data.Add(model);

            var Response = facade.MapToModel(data);

            Assert.NotNull(Response);
        }

        [Fact]
        public async Task Should_Success_Post_Data()
        {
            var mockIntegrationFacade = new Mock<IIntegrationFacade>();
            mockIntegrationFacade
                .Setup(x => x.RefreshToken())
                .ReturnsAsync( new AccurateTokenViewModel { access_token = "2201921"});

            mockIntegrationFacade
                .Setup(x => x.OpenDb())
                .ReturnsAsync(new AccurateSessionViewModel { session = "1201201", host = "https://zeus.accurate.co.id" });

            SalesReturnUploadFacade facade = new SalesReturnUploadFacade(GetServiceProvider(GetCurrentMethod()).Object, mockIntegrationFacade.Object, _dbContext(GetCurrentMethod()), mapper);

            var model = await dataUtil(facade, GetCurrentMethod()).GetTestData();
            var viewModel = dataUtilViewModel(facade, GetCurrentMethod()).GetNewDataValid();

            viewModel.Id = 1;
            foreach(var i in viewModel.detailItem)
            {
                i.Id = 1;
            }

            List<AccuSalesReturnViewModel> data = new List<AccuSalesReturnViewModel>();
            data.Add(viewModel);

            var response = facade.Create(data, USERNAME);
            Assert.NotNull(response);
        }

    }
}
