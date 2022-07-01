using Com.Kana.Service.Upload.Lib.Interfaces;
using Com.Kana.Service.Upload.Lib.Services;
using Com.Kana.Service.Upload.WebApi.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.Kana.Service.Upload.WebApi.Controllers.v1.IntegrationController
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/integration")]

    public class IntegrationController : Controller
    {
        private string ApiVersion = "1.0.0";
        private readonly IdentityService identityService;
        private readonly IIntegrationFacade facade;

        public IntegrationController(IServiceProvider serviceProvider, IIntegrationFacade facade)
        {
            this.identityService = (IdentityService)serviceProvider.GetService(typeof(IdentityService));
            this.facade = facade;
        }

        [HttpGet("call")]
        public IActionResult CallAccurate()
        {
            try
            {
                var data = facade.GetCode();
                if (!data.Result)
                {
                    throw new Exception("Terjadi Kesalahan");
                }
                else
                {
                    return Ok(new
                    {
                        apiVersion = ApiVersion,
                        statusCode = General.OK_STATUS_CODE,
                        message = General.OK_MESSAGE,
                    });
                }
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                   new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                   .Fail();

                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpGet("authcallback")]
        public IActionResult GetToken(string code)
        {
            try
            {
                var data = facade.RetrieveToken(code);
                if(data == null)
                {
                    throw new Exception("Terjadi Kesalahan");
                }
                else
                {
                    return Ok(new
                    {
                        apiVersion = ApiVersion,
                        statusCode = General.OK_STATUS_CODE,
                        message = General.OK_MESSAGE,
                        data = data.Result.access_token
                    });
                }
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                   new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                   .Fail();

                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

    }

    
}
