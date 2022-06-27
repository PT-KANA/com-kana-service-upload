using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Com.Kana.Service.Upload.Lib.Facades.Expedition;
using Com.Kana.Service.Upload.Lib.ViewModels.Expedition;
using Com.Kana.Service.Upload.WebApi.Helpers;
using Com.Kana.Service.Upload.Lib.Helpers.ReadResponse;
using Com.Kana.Service.Upload.Lib.Interfaces;

namespace Com.Kana.Service.Upload.WebApi.Controllers.v1.Expedition
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/expedition/pph-bank-expenditure-notes-report")]
    [Authorize]
    public class PPHBankExpenditureNoteReportController : Controller
    {
        private string ApiVersion = "1.0.0";
        private readonly IPPHBankExpenditureNoteReportFacade pphBankExpenditureNoteReportFacade;

        public PPHBankExpenditureNoteReportController(IPPHBankExpenditureNoteReportFacade pphBankExpenditureNoteReportFacade)
        {
            this.pphBankExpenditureNoteReportFacade = pphBankExpenditureNoteReportFacade;
        }

        [HttpGet]
        public ActionResult Get(int Size, int Page, string No, string UnitPaymentOrderNo, string InvoiceNo, string SupplierCode, DateTimeOffset? DateFrom, DateTimeOffset? DateTo)
        {
            int clientTimeZoneOffset = int.Parse(Request.Headers["x-timezone-offset"].First());

            ReadResponse<object> response = this.pphBankExpenditureNoteReportFacade.GetReport(Size, Page, No, UnitPaymentOrderNo, InvoiceNo, SupplierCode, DateFrom, DateTo, clientTimeZoneOffset);

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