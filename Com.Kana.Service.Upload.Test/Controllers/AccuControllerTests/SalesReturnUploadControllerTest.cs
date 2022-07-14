using AutoMapper;
using Com.Kana.Service.Upload.Lib.Interfaces;
using Com.Kana.Service.Upload.Lib.Interfaces.SalesReturnInterface;
using Com.Kana.Service.Upload.Lib.Models.AccurateIntegration.AccuSalesReturnModel;
using Com.Kana.Service.Upload.Lib.Services;
using Com.Kana.Service.Upload.Lib.ViewModels.AccuSalesReturnViewModel;
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
    public class SalesReturnUploadControllerTest
    {
        private AccuSalesReturnViewModel viewModel
        {
            get
            {
                return new AccuSalesReturnViewModel
                {
                    customerNo = "C.00004",
                    branchName = "JAKARTA",
                    invoiceNumber = "PLR.0001",
                    detailItem = new List<AccuSalesReturnDetailItemViewModel>
                    {
                        new AccuSalesReturnDetailItemViewModel
                        {
                            itemNo = "BANDERSNATC",
                            quantity = 10
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
            System.ComponentModel.DataAnnotations.ValidationContext validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(viewModel, serviceProvider.Object, null);
            return new ServiceValidationExeption(validationContext, validationResults);
        }

        protected int GetStatusCode(IActionResult response)
        {
            return (int)response.GetType().GetProperty("StatusCode").GetValue(response, null);
        }

        private SalesReturnUploadController GetController(Mock<ISalesReturnUpload> facade, Mock<IValidateService> validate, Mock<IMapper> mapper)
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };

            user.Setup(u => u.Claims).Returns(claims);
            var serviceProvider = GetServiceProvider();
            if(validate != null)
            {
                serviceProvider
                    .Setup(x => x.GetService(typeof(IValidateService)))
                    .Returns(validate.Object);
            }

            SalesReturnUploadController controller = new SalesReturnUploadController(mapper.Object, facade.Object, serviceProvider.Object)
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
            var mockValidate = new Mock<IValidateService>();
            mockValidate.Setup(s => s.Validate(It.IsAny<AccuSalesReturnViewModel>())).Verifiable();

            var mockFacade = new Mock<ISalesReturnUpload>();

            mockFacade.Setup(x => x.ReadForUpload(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), null, It.IsAny<string>()))
                .Returns(Tuple.Create(new List<AccuSalesReturn>(), 0, new Dictionary<string, string>()));

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<List<AccuSalesReturnViewModel>>(It.IsAny<List<AccuSalesReturn>>()))
                .Returns(new List<AccuSalesReturnViewModel> { viewModel });

            SalesReturnUploadController controller = GetController(mockFacade, mockValidate, mockMapper);
            var response = controller.Get();
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(response));
        }

        [Fact]
        public void Should_Error_PostCSVFileAsync()
        {
            var mockValidate = new Mock<IValidateService>();
            mockValidate.Setup(s => s.Validate(It.IsAny<AccuSalesReturnViewModel>())).Verifiable();

            var mockFacade = new Mock<ISalesReturnUpload>();

            mockFacade.Setup(x => x.ReadForUpload(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), null, It.IsAny<string>()))
                .Returns(Tuple.Create(new List<AccuSalesReturn>(), 0, new Dictionary<string, string>()));

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<List<AccuSalesReturnViewModel>>(It.IsAny<List<AccuSalesReturn>>()))
                .Returns(new List<AccuSalesReturnViewModel> { viewModel });

            SalesReturnUploadController controller = GetController(mockFacade, mockValidate, mockMapper);
            var response = controller.PostCSVFileAsync();
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response.Result));
        }

        [Fact]
        public void Should_Created_Post_()
        {
            var mockValidate = new Mock<IValidateService>();
            mockValidate.Setup(s => s.Validate(It.IsAny<AccuSalesReturnViewModel>())).Verifiable();

            var mockFacade = new Mock<ISalesReturnUpload>();

            mockFacade.Setup(x => x.ReadForUpload(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), null, It.IsAny<string>()))
                .Returns(Tuple.Create(new List<AccuSalesReturn>(), 0, new Dictionary<string, string>()));

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<List<AccuSalesReturnViewModel>>(It.IsAny<List<AccuSalesReturn>>()))
                .Returns(new List<AccuSalesReturnViewModel> { viewModel });

            List<AccuSalesReturnViewModel> accuSalesReturnViewModel = new List<AccuSalesReturnViewModel>
            {
                new AccuSalesReturnViewModel
                { }
            };

            SalesReturnUploadController controller = GetController(mockFacade, mockValidate, mockMapper);
            var response = controller.Post(accuSalesReturnViewModel);

            Assert.Equal((int)HttpStatusCode.Created, GetStatusCode(response.Result));
        }
    }
}
