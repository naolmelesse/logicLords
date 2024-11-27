using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Pages.Product;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace UnitTests.Pages.Product.Update
{
    /// <summary>
    /// Unit tests for Update page
    /// </summary>
    public class UpdateTests
    {
        #region TestSetup

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

        // The UpdateModel instance being tested
        public static UpdateModel pageModel;

        // The product service used to retrieve and update product data
        public static JsonFileProductService productService;

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

            // Initialize the product service with the mocked web host environment
            productService = new JsonFileProductService(mockWebHostEnvironment.Object);

            // Mock the logger required by the UpdateModel constructor
            var mockLogger = new Mock<ILogger<UpdateModel>>();

            // Initialize the UpdateModel with the product service
            pageModel = new UpdateModel(productService, mockLogger.Object)
            {
                PageContext = pageContext,
                TempData = tempData,
                Url = new Mock<IUrlHelper>().Object
            };
        }

        #endregion TestSetup

        #region OnGet Tests

        /// <summary>
        /// Test that OnGet with a valid Id retrieves the correct product
        /// </summary>
        [Test]
        public void OnGet_Valid_Id_Should_Return_Product()
        {
            // Arrange
            var data = "alonso-martinez";

            // Act
            pageModel.Id = data;
            pageModel.OnGet();
            var result = pageModel.Player.Id;

            // Assert
            Assert.That(result, Is.EqualTo(data));
        }

        /// <summary>
        /// Test that OnGet with an invalid Id returns null
        /// </summary>
        [Test]
        public void OnGet_Invalid_Id_Should_Return_Null()
        {
            // Arrange
            var data = "invalid-id";

            // Act
            pageModel.Id = data;
            pageModel.OnGet();
            var result = pageModel.Player;

            // Assert
            Assert.That(result, Is.Null);
        }

        /// <summary>
        /// Test that OnGet without providing an Id returns null
        /// </summary>
        [Test]
        public void OnGet_Invalid_No_Id_Provided_Should_Return_Null()
        {
            // Arrange

            // Act
            pageModel.Id = null;
            pageModel.OnGet();
            var result = pageModel.Player;

            // Assert
            Assert.That(result, Is.Null);
        }

        #endregion OnGet Tests

        #region OnPost Tests

        /// <summary>
        /// Test that OnPost with a valid model updates the product and redirects to the Index page
        /// </summary>
        [Test]
        public void OnPost_Valid_Model_Should_Update_And_Redirect()
        {
            // Arrange
            var data = "alonso-martinez";
            var productToUpdate = new ProductModel { Id = data, Title = "Updated Alonso Martinez" };
            pageModel.Player = productToUpdate;

            // Act
            var page = pageModel.OnPost() as RedirectToPageResult;
            var result = page.PageName;

            // Assert
            Assert.That(result, Is.EqualTo("/Product/Index"));
            Assert.That(pageModel.ModelState.IsValid, Is.EqualTo(true));
        }

        /// <summary>
        /// Test that OnPost with an invalid model returns the same page with validation errors
        /// </summary>
        [Test]
        public void OnPost_Invalid_Model_Should_Return_Page()
        {
            // Arrange
            pageModel.ModelState.AddModelError("Player.Title", "Title is required");

            // Act
            var result = pageModel.OnPost();

            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>());
            Assert.That(pageModel.ModelState.IsValid, Is.False);
        }

        /// <summary>
        /// Test that OnPost with a player having an empty Id returns the same page with validation errors
        /// </summary>
        [Test]
        public void OnPost_Invalid_Player_With_EmptyId_Should_Return_Page()
        {
            // Arrange
            pageModel.Player = new ProductModel { Id = string.Empty, Title = "Test Product" };
            pageModel.ModelState.AddModelError("Player.Id", "Id is required");

            // Act
            var result = pageModel.OnPost();

            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>());
            Assert.That(pageModel.ModelState.IsValid, Is.False);
        }

        #endregion OnPost Tests
    }
}
