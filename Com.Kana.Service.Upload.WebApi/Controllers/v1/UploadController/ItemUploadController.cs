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
using Com.Kana.Service.Upload.WebApi.Helpers;
using Com.Kana.Service.Upload.Lib.ViewModels.ItemViewModel;
using Com.DanLiris.Service.Purchasing.Lib.ViewModels.AccuItemViewModel;
using Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuItemModel;

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

                        List<ItemCsvViewModel> Data = Csv.GetRecords<ItemCsvViewModel>().ToList();

                        List<AccuItemViewModel> Data1 = await facade.MapToViewModel(Data);

                        Tuple<bool, List<object>> Validated = facade.UploadValidate(ref Data, Request.Form.ToList());

                        Reader.Close();

                        if (Validated.Item1)
                        {
                            //List<AccuItem> data = mapper.Map<List<AccuItem>>(Data1);
                            List<AccuItem> data = await facade.MapToModel(Data1);
                            await facade.UploadData(data, identityService.Username);

                            Dictionary<string, object> Result =
                               new ResultFormatter(ApiVersion, General.CREATED_STATUS_CODE, General.OK_MESSAGE)
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
                          new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, General.CSV_ERROR_MESSAGE)
                          .Fail();

                        return NotFound(Result);
                    }
                }
                else
                {
                    Dictionary<string, object> Result =
                        new ResultFormatter(ApiVersion, General.BAD_REQUEST_STATUS_CODE, General.NO_FILE_ERROR_MESSAGE)
                            .Fail();
                    return BadRequest(Result);
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
