using Com.Kana.Service.Upload.Lib.Enums;
using Com.Kana.Service.Upload.Lib.Facades.Expedition;
using Com.Kana.Service.Upload.Lib.Models.Expedition;
using Com.Kana.Service.Upload.Lib.Services;
using Com.Kana.Service.Upload.Lib.ViewModels.Expedition;
using Com.Kana.Service.Upload.Test.DataUtils.ExpeditionDataUtil;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Com.Kana.Service.Upload.Test.Facades.PurchasingDocumentExpeditionTest
{
 //   [Collection("ServiceProviderFixture Collection")]
    public class UnitPaymentOrderNotVerifiedReportTest
    {
        //private IServiceProvider ServiceProvider { get; set; }

        //public UnitPaymentOrderNotVerifiedReportTest(ServiceProviderFixture fixture)
        //{
        //    ServiceProvider = fixture.ServiceProvider;

        //    IdentityService identityService = (IdentityService)ServiceProvider.GetService(typeof(IdentityService));
        //    identityService.Username = "Unit Test";
        //}

        //private SendToVerificationDataUtil DataUtil
        //{
        //    get { return (SendToVerificationDataUtil)ServiceProvider.GetService(typeof(SendToVerificationDataUtil)); }
        //}

        //private UnitPaymentOrderNotVerifiedReportFacade Facade
        //{
        //    get { return (UnitPaymentOrderNotVerifiedReportFacade)ServiceProvider.GetService(typeof(UnitPaymentOrderNotVerifiedReportFacade)); }
        //}

        //private PurchasingDocumentExpeditionFacade FacadeExpedition
        //{
        //    get { return (PurchasingDocumentExpeditionFacade)ServiceProvider.GetService(typeof(PurchasingDocumentExpeditionFacade)); }
        //}


        //[Fact]
        //public async Task Should_Success_Get_Report_Data()
        //{
        //    PurchasingDocumentExpedition model = await DataUtil.GetTestData();
        //    model.Position = (ExpeditionPosition)6;
        //    model.VerifyDate = DateTimeOffset.UtcNow;
        //    DateTimeOffset tomorrow = DateTimeOffset.UtcNow.AddDays(1);
        //    var Response = this.FacadeExpedition.Read();
        //    await this.FacadeExpedition.UnitPaymentOrderVerification(model, "Unit Test");
        //    var Report = this.Facade.GetReport("", "", "", model.VerifyDate, tomorrow, 1,25, "{}", 7, "not-history");
        //    Assert.NotEmpty(Report.Item1);
        //}

        //[Fact]
        //public async Task Should_Success_Get_Report_Data_History()
        //{
        //    PurchasingDocumentExpedition model = await DataUtil.GetTestData();
        //    model.Position = (ExpeditionPosition)6;
        //    model.VerifyDate = DateTimeOffset.UtcNow;
        //    DateTimeOffset tomorrow = DateTimeOffset.UtcNow.AddDays(1);
        //    var Response = this.FacadeExpedition.Read();
        //    await this.FacadeExpedition.UnitPaymentOrderVerification(model, "Unit Test");
        //    var Report = this.Facade.GetReport("", "", "", model.VerifyDate, tomorrow, 1, 25, "{}", 7, "history");
        //    Assert.NotEmpty(Report.Item1);
        //}

        
    }
}
