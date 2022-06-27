using Com.Kana.Service.Upload.Lib.Facades.Expedition;
using Com.Kana.Service.Upload.Lib.Models.Expedition;
using Com.Kana.Service.Upload.Lib.Services;
using Com.Kana.Service.Upload.Test.DataUtils.ExpeditionDataUtil;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Com.Kana.Service.Upload.Test.Facades.PurchasingDocumentExpeditionTest
{
 //   [Collection("ServiceProviderFixture Collection")]
    public class Report
    {
        //private IServiceProvider ServiceProvider { get; set; }

        //public Report(ServiceProviderFixture fixture)
        //{
        //    ServiceProvider = fixture.ServiceProvider;

        //    IdentityService identityService = (IdentityService)ServiceProvider.GetService(typeof(IdentityService));
        //    identityService.Username = "Unit Test";
        //}

        //private SendToVerificationDataUtil DataUtil
        //{
        //    get { return (SendToVerificationDataUtil)ServiceProvider.GetService(typeof(SendToVerificationDataUtil)); }
        //}

        //private PurchasingDocumentExpeditionReportFacade Facade
        //{
        //    get { return (PurchasingDocumentExpeditionReportFacade)ServiceProvider.GetService(typeof(PurchasingDocumentExpeditionReportFacade)); }
        //}

        //[Fact]
        //public async Task Should_Success_Get_Report_Data()
        //{
        //    PurchasingDocumentExpedition model = await DataUtil.GetTestData();
        //    List<string> unitPaymentOrders = new List<string>() { model.UnitPaymentOrderNo };
        //    var Response = this.Facade.GetReport(unitPaymentOrders);
        //    Assert.NotEmpty(Response);
        //}
    }
}
