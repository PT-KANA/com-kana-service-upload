using AutoMapper;
using Com.Kana.Service.Upload.Lib.Helpers;
using Com.Kana.Service.Upload.Lib.Interfaces;
using Com.Kana.Service.Upload.Lib.Interfaces.SalesUploadInterface;
using Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuSalesInvoiceModel;
using Com.Kana.Service.Upload.Lib.Services;
using Com.Kana.Service.Upload.Lib.ViewModels.AccuSalesViewModel;
using Com.Kana.Service.Upload.Test.Helpers;
using Com.Kana.Service.Upload.WebApi.Controllers.v1.UploadController;
using Com.Moonlay.NetCore.Lib.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace Com.Kana.Service.Upload.Test.Controllers.SalesControllerTests
{
	public class SalesControllerTest
	{
		private AccuSalesViewModel ViewModel
		{
			get
			{
				return new AccuSalesViewModel
				{
					number = "143554",
					orderDownPaymentNumber = "name",
					detailItem = new List<AccuSalesInvoiceDetailItemViewModel>
					{
						new AccuSalesInvoiceDetailItemViewModel
						{
							quantity=10
						}

					}

				};

			}
		}
		private Mock<IServiceProvider> GetServiceProvider()
		{
			var serviceProvider = new Mock<IServiceProvider>();
			serviceProvider
				.Setup(x => x.GetService(typeof(IdentityService)))
				.Returns(new IdentityService() { Token = "Token", Username = "Test" });

			serviceProvider
				.Setup(x => x.GetService(typeof(IHttpClientService)))
				.Returns(new HttpClientTestService());

			return serviceProvider;
		}

		private ServiceValidationExeption GetServiceValidationExeption()
		{
			Mock<IServiceProvider> serviceProvider = new Mock<IServiceProvider>();
			List<ValidationResult> validationResults = new List<ValidationResult>();
			System.ComponentModel.DataAnnotations.ValidationContext validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(ViewModel, serviceProvider.Object, null);
			return new ServiceValidationExeption(validationContext, validationResults);
		}

		protected int GetStatusCode(IActionResult response)
		{
			return (int)response.GetType().GetProperty("StatusCode").GetValue(response, null);
		}

		private SalesUploadController GetController(Mock<ISalesUpload> facadeM, Mock<IValidateService> validateM, Mock<IMapper> mapper)
		{
			var user = new Mock<ClaimsPrincipal>();
			var claims = new Claim[]
			{
				new Claim("username", "unittestusername")
			};
			user.Setup(u => u.Claims).Returns(claims);

			var servicePMock = GetServiceProvider();
			if (validateM != null)
			{
				servicePMock
					.Setup(x => x.GetService(typeof(IValidateService)))
					.Returns(validateM.Object);
			}

			SalesUploadController controller = new SalesUploadController(mapper.Object, facadeM.Object, servicePMock.Object)
			{
				ControllerContext = new ControllerContext()
				{
					HttpContext = new DefaultHttpContext()
					{
						User = user.Object
					}
				}
			};
			controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = "Bearer unittesttoken";
			controller.ControllerContext.HttpContext.Request.Path = new PathString("/v1/unit-test");
			controller.ControllerContext.HttpContext.Request.Headers["x-timezone-offset"] = "7";

			return controller;
		}

		[Fact]
		public void Should_OK_Get_()
		{
			var validateMock = new Mock<IValidateService>();
			validateMock.Setup(s => s.Validate(It.IsAny<AccuSalesViewModel>())).Verifiable();

			var mockFacade = new Mock<ISalesUpload>();

			mockFacade.Setup(x => x.ReadForUpload(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), null, It.IsAny<string>()))
				.Returns(Tuple.Create(new List<AccuSalesInvoice>(), 0, new Dictionary<string, string>()));

			var mockMapper = new Mock<IMapper>();
			mockMapper.Setup(x => x.Map<List<AccuSalesViewModel>>(It.IsAny<List<AccuSalesInvoice>>()))
				.Returns(new List<AccuSalesViewModel> { ViewModel });

			SalesUploadController controller = GetController(mockFacade, validateMock, mockMapper);
			var response = controller.Get();
			Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(response));
		}
		[Fact]
		public void Should_Error_Get_()
		{
			var validateMock = new Mock<IValidateService>();
			validateMock.Setup(s => s.Validate(It.IsAny<AccuSalesViewModel>())).Verifiable();

			var mockFacade = new Mock<ISalesUpload>();

			mockFacade.Setup(x => x.ReadForApproved(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), null, It.IsAny<string>()))
				.Returns(Tuple.Create(new List<AccuSalesInvoice>(), 0, new Dictionary<string, string>()));

			var mockMapper = new Mock<IMapper>();
			mockMapper.Setup(x => x.Map<List<AccuSalesViewModel>>(It.IsAny<List<AccuSalesInvoice>>()))
				.Returns(new List<AccuSalesViewModel> { ViewModel });

			SalesUploadController controller = GetController(mockFacade, validateMock, mockMapper);
			var response = controller.Get();
			Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
		}
		[Fact]
		public void Should_OK_Get_Approve()
		{
			var validateMock = new Mock<IValidateService>();
			validateMock.Setup(s => s.Validate(It.IsAny<AccuSalesViewModel>())).Verifiable();

			var mockFacade = new Mock<ISalesUpload>();

			mockFacade.Setup(x => x.ReadForApproved(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), null, It.IsAny<string>()))
				.Returns(Tuple.Create(new List<AccuSalesInvoice>(), 0, new Dictionary<string, string>()));

			var mockMapper = new Mock<IMapper>();
			mockMapper.Setup(x => x.Map<List<AccuSalesViewModel>>(It.IsAny<List<AccuSalesInvoice>>()))
				.Returns(new List<AccuSalesViewModel> { ViewModel });

			SalesUploadController controller = GetController(mockFacade, validateMock, mockMapper);
			var response = controller.GetApproved();
			Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(response));
		}
		[Fact]
		public void Should_Error_Get_Approve()
		{
			var validateMock = new Mock<IValidateService>();
			validateMock.Setup(s => s.Validate(It.IsAny<AccuSalesViewModel>())).Verifiable();

			var mockFacade = new Mock<ISalesUpload>();

			mockFacade.Setup(x => x.ReadForUpload(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), null, It.IsAny<string>()))
				.Returns(Tuple.Create(new List<AccuSalesInvoice>(), 0, new Dictionary<string, string>()));

			var mockMapper = new Mock<IMapper>();
			mockMapper.Setup(x => x.Map<List<AccuSalesViewModel>>(It.IsAny<List<AccuSalesInvoice>>()))
				.Returns(new List<AccuSalesViewModel> { ViewModel });

			SalesUploadController controller = GetController(mockFacade, validateMock, mockMapper);
			var response = controller.GetApproved();
			Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
		}
		[Fact]
		public void Should_error_PostCSVFileAsync()
		{
			var validateMock = new Mock<IValidateService>();
			validateMock.Setup(s => s.Validate(It.IsAny<AccuSalesViewModel>())).Verifiable();

			var mockFacade = new Mock<ISalesUpload>();

			mockFacade.Setup(x => x.ReadForUpload(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), null, It.IsAny<string>()))
				.Returns(Tuple.Create(new List<AccuSalesInvoice>(), 0, new Dictionary<string, string>()));

			var mockMapper = new Mock<IMapper>();
			mockMapper.Setup(x => x.Map<List<AccuSalesViewModel>>(It.IsAny<List<AccuSalesInvoice>>()))
				.Returns(new List<AccuSalesViewModel> { ViewModel });

			SalesUploadController controller = GetController(mockFacade, validateMock, mockMapper);
			var response = controller.PostCSVFileAsync();
			Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response.Result));
		}

		[Fact]
		public void Should_created_POst_()
		{
			var validateMock = new Mock<IValidateService>();
			validateMock.Setup(s => s.Validate(It.IsAny<AccuSalesViewModel>())).Verifiable();

			var mockFacade = new Mock<ISalesUpload>();
 
			var mockMapper = new Mock<IMapper>();
			mockMapper.Setup(x => x.Map<List<AccuSalesViewModel>>(It.IsAny<List<AccuSalesInvoice>>()))
				.Returns(new List<AccuSalesViewModel> { ViewModel });
			List<AccuSalesViewModel> AccuSalesViewModels = new List<AccuSalesViewModel>
			{
				new AccuSalesViewModel
				{

				}
			};
			SalesUploadController controller = GetController(mockFacade, validateMock, mockMapper);
			var response = controller.Post(AccuSalesViewModels);
			Assert.Equal((int)HttpStatusCode.Created, GetStatusCode(response.Result));
		}
		[Fact]
		public void Should_created_POst_Approved()
		{
			var validateMock = new Mock<IValidateService>();
			validateMock.Setup(s => s.Validate(It.IsAny<AccuSalesViewModel>())).Verifiable();

			var mockFacade = new Mock<ISalesUpload>();
 
			var mockMapper = new Mock<IMapper>();
			mockMapper.Setup(x => x.Map<List<AccuSalesViewModel>>(It.IsAny<List<AccuSalesInvoice>>()))
				.Returns(new List<AccuSalesViewModel> { ViewModel });
			List<AccuSalesViewModel> AccuSalesViewModels = new List<AccuSalesViewModel>
			{
				new AccuSalesViewModel
				{

				}
			};
			SalesUploadController controller = GetController(mockFacade, validateMock, mockMapper);
			var response = controller.PostApproved(AccuSalesViewModels);
			Assert.Equal((int)HttpStatusCode.Created, GetStatusCode(response.Result));
		}
		[Fact]
		public void ss()
		{
			 

			var validateMock = new Mock<IValidateService>();
			validateMock.Setup(s => s.Validate(It.IsAny<AccuSalesViewModel>())).Verifiable();

			var mockFacade = new Mock<ISalesUpload>();

			var mockMapper = new Mock<IMapper>();
			var httpContext = new DefaultHttpContext();

			httpContext.Request.Headers.Add("Content-Type", "multipart/form-data");
			httpContext.Request.Headers.Add("ContentDisposition", "form-data");
			
			var file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.xls");
			var content = new StringContent(file.ToString(), Encoding.UTF8, General.JsonMediaType);

			httpContext.Request.Form = new FormCollection(new Dictionary<string, StringValues>(), new FormFileCollection { file });
 
			httpContext.Request.Form.Files[0].OpenReadStream();

			var actx = new ActionContext(httpContext, new RouteData(), new ControllerActionDescriptor());
			SalesUploadController controller = GetController(mockFacade, validateMock, mockMapper);
			var response = controller.PostCSVFileAsync();
			Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response.Result));

		}
	}
}
