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
using Moq;
using NUnit.Framework;

namespace UnitTests.Pages.Product
{

    /// <summary>
    /// Unit tests for the CreateModel Page
    /// </summary>
    internal class CreateTests
    {

        // Required objects for setting up the test context
        #region TestSetup
        public static DefaultHttpContext httpContextDefault;
        public static IWebHostEnvironment webHostEnvironment;
        public static ModelStateDictionary modelState;
        public static ActionContext actionContext;
        public static EmptyModelMetadataProvider modelMetadataProvider;
        public static ViewDataDictionary viewData;
        public static TempDataDictionary tempData;
        public static PageContext pageContext;

        // The page model being tested
        public static CreateModel pageModel;

        // Service for handling JSON file operations
        public static JsonFileProductService productService;

        /// <summary>
        /// Setup method that initializes all required objects before each test.
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {

            // Initialize a default HTTP context
            httpContextDefault = new DefaultHttpContext();

            // Initialize the model state for validation tracking
            modelState = new ModelStateDictionary();

            // Setup the action context with the HTTP context and routing data
            actionContext = new ActionContext(httpContextDefault, httpContextDefault.GetRouteData(), new PageActionDescriptor(), modelState);

            // Setup metadata and view data
            modelMetadataProvider = new EmptyModelMetadataProvider();
            viewData = new ViewDataDictionary(modelMetadataProvider, modelState);
            tempData = new TempDataDictionary(httpContextDefault, Mock.Of<ITempDataProvider>());

            // Initialize page context with the action context
            pageContext = new PageContext(actionContext)
            {
                ViewData = viewData,
            };

            // Mock the IWebHostEnvironment to set up paths for testing
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            mockWebHostEnvironment.Setup(m => m.EnvironmentName).Returns("Hosting:UnitTestEnvironment");
            mockWebHostEnvironment.Setup(m => m.WebRootPath).Returns("../../../../src/bin/Debug/net7.0/wwwroot");
            mockWebHostEnvironment.Setup(m => m.ContentRootPath).Returns("./data/");

            // Initialize the product service with the mocked environment
            productService = new JsonFileProductService(mockWebHostEnvironment.Object);

            // Initialize the CreateModel page model
            pageModel = new CreateModel(productService)
            {
                PageContext = pageContext,
                TempData = tempData,
                Url = new Mock<IUrlHelper>().Object
            };
        }

        #endregion TestSetup

        #region OnPost


        /// <summary>
        /// Test case for a valid product model. 
        /// Should create the product and redirect to the Index page.
        /// </summary>
        [Test]
        public void OnPost_Valid_Model_Should_Create_And_Redirect()
        {
            // Arrange
            // Create a valid product model with sample data
            var data = new ProductModel
            {
                Title = "Ready Player",
                Dateofbirth = "15 October 1998",
                Placeofbirth = "Santiago, Chile",
                Height = "1.74 m (5 ft 8 in)",
                Position = "Goalkeeper",
                Club = "Columbus Crew",
                Appearances = 12,
                Minutesplayed = 880,
                Assists = 5,
                Image = "/data/players/ChristianBenteke.png"
            };

            pageModel.Player = data;

            // Act
            var result = pageModel.OnPost() as RedirectToPageResult;

            // Assert
            Assert.That(pageModel.ModelState.IsValid, Is.EqualTo(true));
            Assert.That(result.PageName, Is.EqualTo("/Index"));
        }

        /// <summary>
        /// Test case for an invalid product model.
        /// Should return the current page without redirecting.
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

        #endregion OnPost
    }
}