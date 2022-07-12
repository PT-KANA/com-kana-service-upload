using AutoMapper;
using Com.DanLiris.Service.Purchasing.Lib.ViewModels.AccuItemViewModel;
using Com.Kana.Service.Upload.Lib.Interfaces;
using Com.Kana.Service.Upload.Lib.Interfaces.ItemInterface;
using Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuItemModel;
using Com.Kana.Service.Upload.Lib.Services;
using Com.Kana.Service.Upload.Lib.ViewModels.AccuItemViewModel;
using Com.Kana.Service.Upload.Test.Helpers;
using Com.Kana.Service.Upload.WebApi.Controllers.v1.UploadController;
using Com.Moonlay.NetCore.Lib.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace Com.Kana.Service.Upload.Test.Controllers.AccuControllerTests
{
	public class ItemUploadControllerTest
	{
		private AccuItemViewModel ViewModel
		{
			get
			{
				return new AccuItemViewModel
				{
					no="143554",
					name="name",
					detailGroup= new List<AccuItemDetailGroupViewModel>
					{
						new AccuItemDetailGroupViewModel
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

		private ItemUploadController GetController(Mock<IItemFacade> facadeM, Mock<IValidateService> validateM, Mock<IMapper> mapper)
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

			ItemUploadController controller = new ItemUploadController( servicePMock.Object, facadeM.Object, mapper.Object)
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
		public void Should_Error_Get_()
		{
			var validateMock = new Mock<IValidateService>();
			validateMock.Setup(s => s.Validate(It.IsAny<AccuItemViewModel>())).Verifiable();

			var mockFacade = new Mock<IItemFacade>();

			mockFacade.Setup(x => x.ReadForUpload(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), null, It.IsAny<string>()))
				.Returns(Tuple.Create(new List<AccuItem>(), 0, new Dictionary<string, string>()));

			var mockMapper = new Mock<IMapper>();
			mockMapper.Setup(x => x.Map<List<AccuItemViewModel>>(It.IsAny<List<AccuItem>>()))
				.Returns(new List<AccuItemViewModel> { ViewModel });

			ItemUploadController controller = GetController(mockFacade, validateMock, mockMapper);
			var response = controller.Get();
			Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(response));
		}
	}
}
