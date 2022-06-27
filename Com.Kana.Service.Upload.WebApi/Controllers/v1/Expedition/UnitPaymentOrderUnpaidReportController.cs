using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Com.Kana.Service.Upload.Lib.Helpers.ReadResponse;
using Com.Kana.Service.Upload.WebApi.Helpers;
using Com.Kana.Service.Upload.Lib.Interfaces;
using System.Threading.Tasks;

namespace Com.Kana.Service.Upload.WebApi.Controllers.v1.Expedition
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/expedition/unit-payment-order-unpaid-report")]
    [Authorize]
    public class UnitPaymentOrderUnpaidReportController : Controller
    {
        private string ApiVersion = "1.0.0";
        private readonly IUnitPaymentOrderUnpaidReportFacade unitPaymentOrderUnpaidReportFacade;

        public UnitPaymentOrderUnpaidReportController(IUnitPaymentOrderUnpaidReportFacade unitPaymentOrderUnpaidReportFacade)
        {
            this.unitPaymentOrderUnpaidReportFacade = unitPaymentOrderUnpaidReportFacade;
        }

        [HttpGet]
        public async Task<ActionResult> Get(int Size, int Page, string Order, string UnitPaymentOrderNo, string SupplierCode, DateTimeOffset? DateFrom, DateTimeOffset? DateTo)
        {
            int clientTimeZoneOffset = int.Parse(Request.Headers["x-timezone-offset"].First());

            ReadResponse<object> response = await this.unitPaymentOrderUnpaidReportFacade.GetReport(Size, Page, Order, UnitPaymentOrderNo, SupplierCode, DateFrom, DateTo, clientTimeZoneOffset);

            return Ok(new
            {
                apiVersion = ApiVersion,
                data = response.Data,
                info = new Dictionary<string, object>
                {
                    { "count", response.Data.Count },
                    { "total", response.TotalData },
                    { "order", response.Order },
                    { "page", Page },
                    { "size", Size }
                },
                message = General.OK_MESSAGE,
                statusCode = General.OK_STATUS_CODE
            });
        }
    }
}