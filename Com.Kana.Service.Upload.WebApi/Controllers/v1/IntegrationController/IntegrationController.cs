using Com.Kana.Service.Upload.Lib.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.Kana.Service.Upload.WebApi.Controllers.v1.IntegrationController
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/authcallback")]

    public class IntegrationController : Controller
    {
        private string ApiVersion = "1.0.0";
        private readonly IdentityService identityService;

    }
}
