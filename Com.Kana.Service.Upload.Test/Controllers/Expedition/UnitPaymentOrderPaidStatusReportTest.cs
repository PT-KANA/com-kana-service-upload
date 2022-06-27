using Com.Kana.Service.Upload.Lib.Facades.Expedition;
using Com.Kana.Service.Upload.Lib.Helpers.ReadResponse;
using Com.Kana.Service.Upload.Lib.Interfaces;
using Com.Kana.Service.Upload.WebApi.Controllers.v1.Expedition;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Xunit;

namespace Com.Kana.Service.Upload.Test.Controllers.Expedition
{
    public class UnitPaymentOrderPaidStatusReportTest
    {
        protected int GetStatusCode(IActionResult response)
        {
            return (int)response.GetType().GetProperty("StatusCode").GetValue(response, null);
        }

        [Fact]
        public void Should_Success_Get_Report()
        {
            Mock<IUnitPaymentOrderPaidStatusReportFacade> mockFacade = new Mock<IUnitPaymentOrderPaidStatusReportFacade>();
            mockFacade.Setup(p => p.GetReport(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTimeOffset?>(), It.IsAny<DateTimeOffset?>(), It.IsAny<DateTimeOffset?>(), It.IsAny<DateTimeOffset?>(), It.IsAny<int>()))
                .Returns(new ReadResponse<object>(new List<object>(), 0, new Dictionary<string, string>()));

            UnitPaymentOrderPaidStatusReportController controller = new UnitPaymentOrderPaidStatusReportController(mockFacade.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
            };

            controller.ControllerContext.HttpContext.Request.Headers["x-timezone-offset"] = "1";

            var response = controller.Get(1, 1, null, null, null, null, null, null, null, null, null, null, null);
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(response));
        }
    }
}
