﻿using Com.Kana.Service.Upload.Lib.Facades.ExternalPurchaseOrderFacade;
using Com.Kana.Service.Upload.Lib.Facades.ExternalPurchaseOrderFacade.Reports;
using Com.Kana.Service.Upload.Lib.Models.ExternalPurchaseOrderModel;
using Com.Kana.Service.Upload.Lib.Services;
using Com.Kana.Service.Upload.Test.DataUtils.ExternalPurchaseOrderDataUtils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Com.Kana.Service.Upload.Test.Facades.ExternalPurchaseOrderTests.Reports
{
	//[Collection("ServiceProviderFixture Collection")]
	public class TotalPurchaseByCategoriesTest
    {
		//private IServiceProvider ServiceProvider { get; set; }
		//public TotalPurchaseByCategoriesTest(ServiceProviderFixture fixture)
		//{
		//	ServiceProvider = fixture.ServiceProvider;

		//	IdentityService identityService = (IdentityService)ServiceProvider.GetService(typeof(IdentityService));
		//	identityService.Username = "Unit Test";
		//}
		//private ExternalPurchaseOrderDataUtil DataUtil
		//{
		//	get { return (ExternalPurchaseOrderDataUtil)ServiceProvider.GetService(typeof(ExternalPurchaseOrderDataUtil)); }
		//}

		//private TotalPurchaseFacade Facade
		//{
		//	get { return (TotalPurchaseFacade)ServiceProvider.GetService(typeof(TotalPurchaseFacade)); }
		//}
		//private ExternalPurchaseOrderFacade FacadeEPO
		//{
		//	get { return (ExternalPurchaseOrderFacade)ServiceProvider.GetService(typeof(ExternalPurchaseOrderFacade)); }
		//}
		//[Fact]
		//public async Task Should_Success_Get_Report_Total_Purchase_By_Categories_Data_Null_Parameter()
		//{
		//	ExternalPurchaseOrder externalPurchaseOrder = await DataUtil.GetNewData("unit-test");
		//	await FacadeEPO.Create(externalPurchaseOrder, "unit-test", 7);
		//	var Response = Facade.GetTotalPurchaseByCategoriesReport( null, null, 7);
		//	Assert.NotEqual(1, 0);
		//}
		//[Fact]
		//public async Task Should_Success_Get_Report_Total_Purchase_By_Categories_Data_Excel_Null_Parameter()
		//{
		//	ExternalPurchaseOrder externalPurchaseOrder = await DataUtil.GetNewData("unit-test");
		//	await FacadeEPO.Create(externalPurchaseOrder, "unit-test", 7);
		//	var Response = Facade.GenerateExcelTotalPurchaseByCategories( null, null, 7);
		//	Assert.IsType<System.IO.MemoryStream>(Response);
		//}
		//[Fact]
		//public void Should_Success_Get_Report_Total_Purchase_By_Categories_Null_Data_Excel()
		//{
		//	DateTime DateFrom = new DateTime(2018, 1, 1);
		//	DateTime DateTo = new DateTime(2018, 1, 1);
		//	var Response = Facade.GenerateExcelTotalPurchaseByCategories(DateFrom, DateTo, 7);
		//	Assert.IsType<System.IO.MemoryStream>(Response);
		//}
	
	}
}
