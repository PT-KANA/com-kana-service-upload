using Com.Kana.Service.Upload.Lib.Facades;
using Com.Kana.Service.Upload.Lib.Models.PurchaseRequestModel;
using Com.Kana.Service.Upload.Lib.Services;
using Com.Kana.Service.Upload.Test.DataUtils.PurchaseRequestDataUtils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Com.Kana.Service.Upload.Test.Facades.PurchaseRequestTests
{
  //  [Collection("ServiceProviderFixture Collection")]
    public class PostUnpostTest
    {
        //private IServiceProvider ServiceProvider { get; set; }

        //public PostUnpostTest(ServiceProviderFixture fixture)
        //{
        //    ServiceProvider = fixture.ServiceProvider;

        //    IdentityService identityService = (IdentityService)ServiceProvider.GetService(typeof(IdentityService));
        //    identityService.Username = "Unit Test";
        //}

        //private PurchaseRequestDataUtil DataUtil
        //{
        //    get { return (PurchaseRequestDataUtil)ServiceProvider.GetService(typeof(PurchaseRequestDataUtil)); }
        //}

        //private PurchaseRequestFacade Facade
        //{
        //    get { return (PurchaseRequestFacade)ServiceProvider.GetService(typeof(PurchaseRequestFacade)); }
        //}

        //[Fact]
        //public async Task Should_Success_PRPost()
        //{
        //    List<PurchaseRequest> modelList = new List<PurchaseRequest>();
        //    PurchaseRequest model = await DataUtil.GetTestData("Unit test");
        //    modelList.Add(model);
        //    var Response = Facade.PRPost(modelList, "Unit Test");
        //    Assert.NotEqual(0, Response);
        //}

        //[Fact]
        //public async Task Should_Success_PRUnpost()
        //{
        //    PurchaseRequest model = await DataUtil.GetTestData("Unit test");
        //    var Response = Facade.PRUnpost((int)model.Id, "Unit Test");
        //    Assert.NotEqual(0, Response);
        //}
    }
}
