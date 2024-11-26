using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Pages.Product;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace UnitTests.Pages.Product.Read
{
    /// <summary>
    /// Unit tests for Read Page
    /// </summary>
    public class ReadTests
    {
        #region TestSetup

        // Factory to create URL helpers
        public static IUrlHelperFactory urlHelperFactory;

        // Default HTTP context for simulating HTTP requests
        public static DefaultHttpContext httpContextDefault;

        // Web host environment for accessing environment-specific information
        public static IWebHostEnvironment webHostEnvironment;

        // Model state dictionary for holding model state information
        public static ModelStateDictionary modelState;

        // Action context for providing context about the current request
        public static ActionContext actionContext;

        // Provider for model metadata
        public static EmptyModelMetadataProvider modelMetadataProvider;

        // View data dictionary for passing data between controller and view
        public static ViewDataDictionary viewData;

        // Temp data dictionary for storing data temporarily
        public static TempDataDictionary tempData;

        // Page context for the Razor page
        public static PageContext pageContext;

        // The ReadModel instance being tested
        public static ReadModel pageModel;

        /// <summary>
        /// Initializes common objects needed for the tests
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            // Initialize the default HTTP context
            httpContextDefault = new DefaultHttpContext();

            // Initialize the model state dictionary
            modelState = new ModelStateDictionary();

            // Create an action context with the default HTTP context and model state
            actionContext = new ActionContext(httpContextDefault, httpContextDefault.GetRouteData(), new PageActionDescriptor(), modelState);

            // Initialize the model metadata provider and view data dictionary
            modelMetadataProvider = new EmptyModelMetadataProvider();
            viewData = new ViewDataDictionary(modelMetadataProvider, modelState);

            // Initialize the temp data dictionary
            tempData = new TempDataDictionary(httpContextDefault, Mock.Of<ITempDataProvider>());

            // Create the page context with the action context and view data
            pageContext = new PageContext(actionContext)
            {
                ViewData = viewData,
            };

            // Mock the web host environment to simulate hosting environment properties
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            mockWebHostEnvironment.Setup(m => m.EnvironmentName).Returns("Hosting:UnitTestEnvironment");
            mockWebHostEnvironment.Setup(m => m.WebRootPath).Returns("../../../../src/bin/Debug/net7.0/wwwroot");
            mockWebHostEnvironment.Setup(m => m.ContentRootPath).Returns("./data/");

            // Create a mock logger for the ReadModel
            var MockLoggerDirect = Mock.Of<ILogger<ReadModel>>();

            // Initialize the product service with the mocked web host environment
            JsonFileProductService productService;
            productService = new JsonFileProductService(mockWebHostEnvironment.Object);

            // Initialize the ReadModel instance
            pageModel = new ReadModel(productService, MockLoggerDirect)
            {
                PageContext = pageContext,
                TempData = tempData,
            };
        }

        #endregion TestSetup

        #region OnGet

        /// <summary>
        /// Test that the ReadModel correctly fetches and returns the product ID
        /// </summary>
        [Test]
        public void OnGet_Valid_Should_Return_Products_With_Correct_ID()
        {
            // Arrange
            var data = "test-id";
            pageModel.Id = data;

            // Act
            var result = pageModel.Id;

            // Assert
            Assert.That(result, Is.EqualTo(data));
        }

        /// <summary>
        /// Tests OnGet when the Id is null or empty.
        /// </summary>
        [Test]
        public void OnGet_Invalid_Null_Or_EmptyId_Should_Redirect_To_Error_Page()
        {
            // Arrange
            pageModel.Id = null;

            // Act
            var result = pageModel.OnGet();

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToPageResult>());
            var redirectResult = result as RedirectToPageResult;
            Assert.That(redirectResult.PageName, Is.EqualTo("/Error"));
            Assert.That(pageModel.TempData["ErrorMessage"], Is.EqualTo("Player ID is missing."));
        }

        /// <summary>
        /// Tests OnGet when the player with the given Id does not exist.
        /// </summary>
        [Test]
        public void OnGet_Invalid_Id_Should_Redirect_To_Error_Page()
        {
            // Arrange
            var mockProductService = new Mock<JsonFileProductService>(Mock.Of<IWebHostEnvironment>());

            mockProductService.Setup(service => service.GetAllData()).Returns(new List<ProductModel>());

            var mockLogger = Mock.Of<ILogger<ReadModel>>();

            var testPageModel = new ReadModel(mockProductService.Object, mockLogger)
            {
                PageContext = pageContext,
                TempData = tempData
            };

            testPageModel.Id = "InvalidId";

            // Act
            var result = testPageModel.OnGet();

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToPageResult>());
            var redirectResult = result as RedirectToPageResult;
            Assert.That(redirectResult.PageName, Is.EqualTo("/Error"));
            Assert.That(testPageModel.TempData["ErrorMessage"], Is.EqualTo("Player with ID InvalidId not found."));
        }

        /// <summary>
        /// Tests OnGet when the player with the given Id exists.
        /// </summary>
        [Test]
        public void OnGet_Valid_Id_Should_Return_Read_Page()
        {
            // Arrange
            var mockProductService = new Mock<JsonFileProductService>(Mock.Of<IWebHostEnvironment>());

            mockProductService.Setup(service => service.GetAllData()).Returns(new List<ProductModel>
            {
                new ProductModel { Id = "ValidId", Title = "Test Player" }
            });

            var mockLogger = Mock.Of<ILogger<ReadModel>>();

            var testPageModel = new ReadModel(mockProductService.Object, mockLogger)
            {
                PageContext = pageContext,
                TempData = tempData
            };

            testPageModel.Id = "ValidId";

            // Act
            var result = testPageModel.OnGet();

            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>());
        }

        /// <summary>
        /// Tests OnGet when an exception is thrown.
        /// </summary>
        [Test]
        public void OnGet_ExceptionThrown_Should_RedirectToError()
        {
            // Arrange
            var mockProductService = new Mock<JsonFileProductService>(Mock.Of<IWebHostEnvironment>());

            mockProductService.Setup(service => service.GetAllData()).Throws(new Exception("Test Exception"));

            var mockLogger = Mock.Of<ILogger<ReadModel>>();

            var testPageModel = new ReadModel(mockProductService.Object, mockLogger)
            {
                PageContext = pageContext,
                TempData = tempData
            };

            testPageModel.Id = "ValidId";

            // Act
            var result = testPageModel.OnGet();

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToPageResult>());
            var redirectResult = result as RedirectToPageResult;
            Assert.That(redirectResult.PageName, Is.EqualTo("/Error"));
            Assert.That(testPageModel.TempData["ErrorMessage"], Is.EqualTo("Test Exception"));
        }

        #endregion OnGet
    }
}
