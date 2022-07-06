using AutoMapper;
using Com.Kana.Service.Upload.Lib.Facades;
using Com.Kana.Service.Upload.Lib.Interfaces.SalesUploadInterface;
using Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuSalesInvoiceModel;
using Com.Kana.Service.Upload.Lib.Services;
using Com.Kana.Service.Upload.Lib.ViewModels.AccuSalesViewModel;
using Com.Kana.Service.Upload.Lib.ViewModels.SalesViewModel;
using Com.Kana.Service.Upload.WebApi.Helpers;
using CsvHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Com.Kana.Service.Upload.WebApi.Controllers.v1.UploadController
{
	[Produces("application/json")]
	[ApiVersion("1.0")]
	[Route("v{version:apiVersion}/sales")]
	[Authorize]
	public class SalesUploadController :Controller
	{
		private string ApiVersion = "1.0.0";
		private readonly IMapper mapper;
		private readonly ISalesUpload facade;
		private readonly IdentityService identityService;
		private readonly string ContentType = "application/vnd.openxmlformats";
		private readonly string FileName = string.Concat("Error Log - ", typeof(AccuSalesInvoice).Name, " ", DateTime.Now.ToString("dd MMM yyyy"), ".csv");
		public SalesUploadController(IMapper mapper, ISalesUpload facade, IServiceProvider serviceProvider) //: base(facade, ApiVersion)
		{
			this.mapper = mapper;
			this.facade = facade;
			this.identityService = (IdentityService)serviceProvider.GetService(typeof(IdentityService));
		}

	  
		[HttpPost("upload")]
		public async Task<IActionResult> PostCSVFileAsync(double source, string sourcec, string sourcen, double destination, string destinationc, string destinationn, DateTimeOffset date)
		// public async Task<IActionResult> PostCSVFileAsync(double source, double destination,  DateTime date)
		{
			try
			{
				identityService.Username = User.Claims.Single(p => p.Type.Equals("username")).Value;
				identityService.Token = Request.Headers["Authorization"].FirstOrDefault().Replace("Bearer ", "");
				identityService.TimezoneOffset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
				if (Request.Form.Files.Count > 0)
				{
					//VerifyUser();
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
						Csv.Configuration.RegisterClassMap<Lib.Facades.SalesUploadFacade.SalesInvoiceMap>();
						Csv.Configuration.HeaderValidated = null;

						List<SalesCsvViewModel> Data = Csv.GetRecords<SalesCsvViewModel>().ToList();
						List<AccuSalesViewModel> Data1 = await facade.MapToViewModel(Data);
						 

						Tuple<bool, List<object>> Validated = facade.UploadValidate(ref Data, Request.Form.ToList());

						Reader.Close();

						if (Validated.Item1) /* If Data Valid */
						{ 
							List<AccuSalesInvoice> data = await facade.MapToModel(Data1);
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
								using (CsvWriter csvWriter = new CsvWriter(streamWriter))
								{
									csvWriter.WriteRecords(Validated.Item2);
								}

								return File(memoryStream.ToArray(), ContentType, FileName);
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
		[HttpPost("post")]
		public async Task<IActionResult> Post([FromBody] List<AccuSalesViewModel> ViewModel)
		{
			try
			{
				identityService.Username = User.Claims.Single(p => p.Type.Equals("username")).Value;
				 
				await facade.Create(ViewModel,identityService.Username );

				Dictionary<string, object> Result =
					new ResultFormatter(ApiVersion, General.CREATED_STATUS_CODE, General.OK_MESSAGE)
					.Ok();
				return Created(String.Concat(Request.Path, "/", 0), Result);
			}
			catch (Exception e)
			{
				Dictionary<string, object> Result =
					new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
					.Fail();
				return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
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

				var newData = mapper.Map<List<AccuSalesViewModel>>(Data.Item1);

				List<object> listData = new List<object>();
				listData.AddRange(
					newData.AsQueryable().Select(s => new
					{
						s.orderDownPaymentNumber,
						s.transDate, 
						s.customerNo,
						s.branchName
					}).ToList()
				);

				return Ok(new
				{
					apiVersion = ApiVersion,
					statusCode = General.OK_STATUS_CODE,
					message = General.OK_MESSAGE,
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
					new ResultFormatter(ApiVersion, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
					.Fail();
				return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, Result);
			}
		}

	}
}
