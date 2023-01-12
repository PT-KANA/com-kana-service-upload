using AutoMapper;
using Com.Kana.Service.Upload.Lib.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Com.Kana.Service.Upload.Lib.Interfaces.ItemInterface;
using CsvHelper;
using WebApiHelpers = Com.Kana.Service.Upload.WebApi.Helpers;
using Com.Kana.Service.Upload.Lib.ViewModels.ItemViewModel;
using Com.DanLiris.Service.Purchasing.Lib.ViewModels.AccuItemViewModel;
using Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuItemModel;
using LibHelpers = Com.Kana.Service.Upload.Lib.Helpers;

namespace Com.Kana.Service.Upload.WebApi.Controllers.v1.UploadController
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/master/item")]
    [Authorize]
    public class ItemUploadController : Controller
    {
        private string ApiVersion = "1.0.0";
        private readonly IMapper mapper;
        private readonly IItemFacade facade;
        private readonly IdentityService identityService;

        private readonly string ContentType = "application/vnd.openxmlformats";
        private readonly string FileName = string.Concat("Error Log - Upload Item - ", DateTime.Now.ToString("dd MMM yyyy"), ".csv");

        public ItemUploadController(IServiceProvider serviceProvider, IItemFacade facade, IMapper mapper)
        {
            this.mapper = mapper;
            this.facade = facade;
            this.identityService = (IdentityService)serviceProvider.GetService(typeof(IdentityService));
        }

        [HttpPost("upload")]
        public async Task<IActionResult> PostCSVFileAsync()
        {
            try
            {
                identityService.Username = User.Claims.Single(p => p.Type.Equals("username")).Value;
                identityService.Token = Request.Headers["Authorization"].FirstOrDefault().Replace("Bearer ", "");
                identityService.TimezoneOffset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);

                if (Request.Form.Files.Count > 0)
                {
                    var UploadedFile = Request.Form.Files[0];
                    StreamReader Reader = new StreamReader(UploadedFile.OpenReadStream());
                    List<string> FileHeader = new List<string>(Reader.ReadLine().Replace("\"", string.Empty).Split(","));
                    var ValidHeader = facade.CsvHeader.SequenceEqual(FileHeader, StringComparer.OrdinalIgnoreCase);

                    if (ValidHeader)
                    {
                        Reader.DiscardBufferedData();
                        Reader.BaseStream.Seek(0, SeekOrigin.Begin);
                        Reader.BaseStream.Position = 0;
                        CsvReader Csv = new CsvReader(Reader);
                        Csv.Configuration.IgnoreQuotes = false;
                        Csv.Configuration.Delimiter = ",";
                        Csv.Configuration.RegisterClassMap<Lib.Facades.ItemFacade.ItemMap>();
                        Csv.Configuration.HeaderValidated = null;

                        List<ItemCsvViewModel> viewModel = Csv.GetRecords<ItemCsvViewModel>().ToList();

                        List<AccuItemViewModel> model = await facade.MapToViewModel(viewModel);

                        Tuple<bool, List<object>> Validated = facade.UploadValidate(ref viewModel, Request.Form.ToList());

                        Reader.Close();

                        if (Validated.Item1)
                        {
                            //List<AccuItem> data = mapper.Map<List<AccuItem>>(Data1);
                            List<AccuItem> data = await facade.MapToModel(model);
                            await facade.UploadData(data, identityService.Username);

                            Dictionary<string, object> Result =
                               new WebApiHelpers.ResultFormatter(ApiVersion, WebApiHelpers.General.CREATED_STATUS_CODE, WebApiHelpers.General.OK_MESSAGE)
                               .Ok();
                            return Created(HttpContext.Request.Path, Result);
                        }
                        else
                        {
                            using (MemoryStream memoryStream = new MemoryStream())
                            {
                                using (StreamWriter streamWriter = new StreamWriter(memoryStream))
                                {
                                    using (CsvWriter csvWriter = new CsvWriter(streamWriter))
                                    {
                                        csvWriter.WriteRecords(Validated.Item2);
                                    }

                                    return File(memoryStream.ToArray(), ContentType, FileName);

                                }
                            }
                        }
                    }
                    else
                    {
                        Dictionary<string, object> Result =
                          new WebApiHelpers.ResultFormatter(ApiVersion, WebApiHelpers.General.INTERNAL_ERROR_STATUS_CODE, WebApiHelpers.General.CSV_ERROR_MESSAGE)
                          .Fail();

                        return NotFound(Result);
                    }
                }
                else
                {
                    Dictionary<string, object> Result =
                        new WebApiHelpers.ResultFormatter(ApiVersion, WebApiHelpers.General.BAD_REQUEST_STATUS_CODE, WebApiHelpers.General.NO_FILE_ERROR_MESSAGE)
                            .Fail();
                    return BadRequest(Result);
                }
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                   new WebApiHelpers.ResultFormatter(ApiVersion, WebApiHelpers.General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                   .Fail();

                return StatusCode(WebApiHelpers.General.INTERNAL_ERROR_STATUS_CODE, Result);

            }
        }

        [HttpPost("post")]
        public async Task<IActionResult> Post([FromBody] List<AccuItemViewModel> ViewModel)
       {
            try
            {
                identityService.Username = User.Claims.Single(p => p.Type.Equals("username")).Value;
                identityService.Token = Request.Headers["Authorization"].FirstOrDefault().Replace("Bearer ", "");

                if (LibHelpers.AuthCredential.AccessToken == null)
                {
                    Dictionary<string, object> TokenNotFoundResult =
                          new WebApiHelpers.ResultFormatter(ApiVersion, WebApiHelpers.General.NOT_FOUND_STATUS_CODE, WebApiHelpers.General.NO_ACCESS_TOKEN)
                          .Fail();

                    return NotFound(TokenNotFoundResult);
                }
                else
                {
                    var res = await facade.Create(ViewModel, identityService.Username);

                    Dictionary<string, object> Result =
                        new WebApiHelpers.ResultFormatter(ApiVersion, WebApiHelpers.General.CREATED_STATUS_CODE, WebApiHelpers.General.OK_MESSAGE)
                        .Ok(res);
                    return Created(String.Concat(Request.Path, "/", 0), Result);
                }

                
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new WebApiHelpers.ResultFormatter(ApiVersion, WebApiHelpers.General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(WebApiHelpers.General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

        [HttpGet]
        public IActionResult Get(int page = 1, int size = 25, string order = "{}", string keyword = null, string filter = "{}")
        {
            identityService.Username = User.Claims.Single(p => p.Type.Equals("username")).Value;

            try
            {
                string filterUser = string.Concat("'CreatedBy':'", identityService.Username, "'");
                if (filter == null || !(filter.Trim().StartsWith("{") && filter.Trim().EndsWith("}")) || filter.Replace(" ", "").Equals("{}"))
                {
                    filter = string.Concat("{", filterUser, "}");
                }
                else
                {
                    filter = filter.Replace("}", string.Concat(", ", filterUser, "}"));
                }

                var Data = facade.ReadForUpload(page, size, order, keyword, filter);

                var newData = mapper.Map<List<AccuItemViewModel>>(Data.Item1);

                List<object> listData = new List<object>();
                listData.AddRange(
                    newData.AsQueryable().Select(s => new
                    {
                        s.Id,
                        s.no,
                        s.name,
                        s.itemType,
                        s.isAccurate
                    }).ToList()
                );

                return Ok(new
                {
                    apiVersion = ApiVersion,
                    statusCode = WebApiHelpers.General.OK_STATUS_CODE,
                    message = WebApiHelpers.General.OK_MESSAGE,
                    data = listData,
                    info = new Dictionary<string, object>
                    {
                        { "count", listData.Count },
                        { "total", Data.Item2 },
                        { "order", Data.Item3 },
                        { "page", page },
                        { "size", size }
                    },
                });
            }
            catch (Exception e)
            {
                Dictionary<string, object> Result =
                    new WebApiHelpers.ResultFormatter(ApiVersion, WebApiHelpers.General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(WebApiHelpers.General.INTERNAL_ERROR_STATUS_CODE, Result);
            }
        }

    }
}
