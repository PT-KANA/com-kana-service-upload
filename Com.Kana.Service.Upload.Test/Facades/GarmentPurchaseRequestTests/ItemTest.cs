using Com.Kana.Service.Upload.Lib;
using Com.Kana.Service.Upload.Lib.Facades.GarmentCorrectionNoteFacades;
using Com.Kana.Service.Upload.Lib.Facades.GarmentDeliveryOrderFacades;
using Com.Kana.Service.Upload.Lib.Facades.GarmentExternalPurchaseOrderFacades;
using Com.Kana.Service.Upload.Lib.Facades.GarmentInternalPurchaseOrderFacades;
using Com.Kana.Service.Upload.Lib.Facades.GarmentPurchaseRequestFacades;
using Com.Kana.Service.Upload.Lib.Facades.PRMasterValidationReportFacade;
using Com.Kana.Service.Upload.Lib.Helpers;
using Com.Kana.Service.Upload.Lib.Interfaces;
using Com.Kana.Service.Upload.Lib.Models.GarmentPurchaseRequestModel;
using Com.Kana.Service.Upload.Lib.Services;
using Com.Kana.Service.Upload.Lib.ViewModels.GarmentPurchaseRequestViewModel;
using Com.Kana.Service.Upload.Lib.ViewModels.NewIntegrationViewModel;
using Com.Kana.Service.Upload.Test.DataUtils.GarmentCorrectionNoteDataUtils;
using Com.Kana.Service.Upload.Test.DataUtils.GarmentDeliveryOrderDataUtils;
using Com.Kana.Service.Upload.Test.DataUtils.GarmentExternalPurchaseOrderDataUtils;
using Com.Kana.Service.Upload.Test.DataUtils.GarmentInternalPurchaseOrderDataUtils;
using Com.Kana.Service.Upload.Test.DataUtils.GarmentPurchaseRequestDataUtils;
using Com.Kana.Service.Upload.Test.DataUtils.NewIntegrationDataUtils;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Com.Kana.Service.Upload.Test.Facades.GarmentPurchaseRequestTests
{
    public class ItemTest
    {
        private const string ENTITY = "GarmentPurchaseRequestItem";

        private const string USERNAME = "Unit Test";
        private IServiceProvider ServiceProvider { get; set; }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public string GetCurrentMethod()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);

            return string.Concat(sf.GetMethod().Name, "_", ENTITY);
        }

        private PurchasingDbContext dbContext(string testName)
        {
            DbContextOptionsBuilder<PurchasingDbContext> optionsBuilder = new DbContextOptionsBuilder<PurchasingDbContext>();
            optionsBuilder
                .UseInMemoryDatabase(testName)
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));

            PurchasingDbContext dbContext = new PurchasingDbContext(optionsBuilder.Options);

            return dbContext;
        }

        private GarmentPurchaseRequestDataUtil dataUtil(GarmentPurchaseRequestFacade facade, string testName)
        {
            return new GarmentPurchaseRequestDataUtil(facade);
        }

		private Mock<IServiceProvider> GetServiceProvider()
		{
			var serviceProvider = new Mock<IServiceProvider>();

            serviceProvider
				.Setup(x => x.GetService(typeof(IdentityService)))
				.Returns(new IdentityService() { Token = "Token", Username = "Test" });

            return serviceProvider;
		}

        [Fact]
        public async Task Should_Success_Get_Items()
        {
            var ServiceProvider = GetServiceProvider().Object;

            GarmentPurchaseRequestFacade facade = new GarmentPurchaseRequestFacade(ServiceProvider, dbContext(GetCurrentMethod()));
            var model = await dataUtil(facade, GetCurrentMethod()).GetTestData();

            GarmentPurchaseRequestItemFacade itemFacade = new GarmentPurchaseRequestItemFacade(ServiceProvider, dbContext(GetCurrentMethod()));

            var Response = itemFacade.Read(Select: "new(Id)");
            Assert.NotEmpty(Response.Data);
        }

        [Fact]
        public async Task Should_Success_Patch_Item()
        {
            var ServiceProvider = GetServiceProvider().Object;

            GarmentPurchaseRequestFacade facade = new GarmentPurchaseRequestFacade(ServiceProvider, dbContext(GetCurrentMethod()));
            var model = await dataUtil(facade, GetCurrentMethod()).GetTestData();

            GarmentPurchaseRequestItemFacade itemFacade = new GarmentPurchaseRequestItemFacade(ServiceProvider, dbContext(GetCurrentMethod()));

            JsonPatchDocument<GarmentPurchaseRequestItem> jsonPatch = new JsonPatchDocument<GarmentPurchaseRequestItem>();
            jsonPatch.Replace(m => m.IsOpenPO, false);

            var ItemIDs = model.Items.Select(i => i.Id).ToArray();
            var Response = await itemFacade.Patch($"[{string.Join(",", ItemIDs)}]", jsonPatch);
            Assert.NotEqual(0, Response);
        }

        [Fact]
        public async Task Should_Error_Patch_Item()
        {
            var ServiceProvider = GetServiceProvider().Object;

            GarmentPurchaseRequestFacade facade = new GarmentPurchaseRequestFacade(ServiceProvider, dbContext(GetCurrentMethod()));
            var model = await dataUtil(facade, GetCurrentMethod()).GetTestData();

            GarmentPurchaseRequestItemFacade itemFacade = new GarmentPurchaseRequestItemFacade(ServiceProvider, dbContext(GetCurrentMethod()));

            JsonPatchDocument<GarmentPurchaseRequestItem> jsonPatch = new JsonPatchDocument<GarmentPurchaseRequestItem>();
            jsonPatch.Replace(m => m.Id, 0);

            var ItemIDs = model.Items.Select(i => i.Id).ToArray();
            var Response = Assert.ThrowsAnyAsync<Exception>(async () => await itemFacade.Patch($"[{string.Join(",", ItemIDs)}]", jsonPatch));
            Assert.NotNull(Response);
        }
    }
}
