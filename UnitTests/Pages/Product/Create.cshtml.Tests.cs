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
using System;
using System.Linq;

namespace UnitTests.Pages.Product
{

    /// <summary>
    /// Unit tests for the CreateModel Page
    /// </summary>
    internal class CreateTests
    {

        // Required objects for setting up the test context
        #region TestSetup
        // Default HTTP context used in the tests
        public static DefaultHttpContext httpContextDefault;

        // Web host environment interface instance
        public static IWebHostEnvironment webHostEnvironment;

        // Model state dictionary for managing model validation state
        public static ModelStateDictionary modelState;

        // Action context containing the route data, action descriptor, and model state
        public static ActionContext actionContext;

        // Metadata provider for models
        public static EmptyModelMetadataProvider modelMetadataProvider;

        // Dictionary for storing view data
        public static ViewDataDictionary viewData;

        // Dictionary for storing temporary data between requests
        public static TempDataDictionary tempData;

        // Page context containing action context, view data, and HTTP context
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

            // Mock the logger required by the UpdateModel constructor
            var mockLogger = new Mock<ILogger<CreateModel>>();

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
            pageModel = new CreateModel(productService, mockLogger.Object)
            {
                PageContext = pageContext,
                TempData = tempData,
                Url = new Mock<IUrlHelper>().Object
            };
        }

        #endregion TestSetup

        #region OnGet

        /// <summary>
        /// Tests that the PositionOptions are populated correctly in OnGet
        /// </summary>
        [Test]
        public void OnGet_Valid_Should_Populate_Position_Dropdown_With_Valid_Positions()
        {
            // Arrange

            // Act
            pageModel.OnGet();

            // Get the options from PositionOptions
            var options = pageModel.PositionOptions.ToList();
            var positionValues = options.Select(option => int.Parse(option.Value)).ToList();

            // Assert
            Assert.That(options, Is.Not.Null);
            Assert.That(options.Count, Is.EqualTo(6)); // Number of positions excluding Undefined
            Assert.That(positionValues, Does.Not.Contain((int)PositionEnum.Undefined));
            Assert.That(positionValues, Is.EquivalentTo(
                new[]
                {
                    (int)PositionEnum.Striker,
                    (int)PositionEnum.Forward,
                    (int)PositionEnum.Winger,
                    (int)PositionEnum.Midfielder,
                    (int)PositionEnum.Defender,
                    (int)PositionEnum.Goalkeeper
                }
            ));
        }

        #endregion OnGet        

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
                HeightInCMs = 174,
                Position = (PositionEnum)60,
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
            Assert.That(result.PageName, Is.EqualTo("/Product/Index"));
        }

        /// <summary>
        /// Test case for an invalid product model.
        /// Should return the current page without redirecting.
        /// </summary>
        [Test]
        public void OnPost_Invalid_Model_Should_Return_Page()
        {
            // Arrange
            pageModel.Player = new ProductModel
            {
                Position = PositionEnum.Midfielder
            };
            pageModel.ModelState.AddModelError("Player.Title", "Title is required");

            // Act
            var result = pageModel.OnPost();

            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>());
            Assert.That(pageModel.ModelState.IsValid, Is.False);
        }

        /// <summary>
        /// Test that OnPost with a player having an null Id returns the Error page 
        /// </summary>
        [Test]
        public void OnPost_Invalid_Null_Player_Should_Return_Error_Page()
        {
            // Arrange
            pageModel.Player = null;

            // Act
            var result = pageModel.OnPost() as RedirectToPageResult;

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToPageResult>());
            Assert.That(result.PageName, Is.EqualTo("/Error"));
            Assert.That(pageModel.TempData["ErrorMessage"], Is.EqualTo("Player data is missing"));
        }

        [Test]
        public void OnPost_Invalid_ModelState_Should_Return_Page_And_Populate_Position_Dropdown()
        {
            // Arrange
            pageModel.Player = new ProductModel
            {
                Position = PositionEnum.Midfielder
                // Leave out required properties to make the model invalid
            };

            pageModel.ModelState.AddModelError("Player.Title", "The Title field is required.");

            // Act
            var result = pageModel.OnPost();
            var options = pageModel.PositionOptions.ToList();
            var positionValues = options.Select(option => int.Parse(option.Value)).ToList();

            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>());
            Assert.That(pageModel.ModelState.IsValid, Is.False);
            Assert.That(pageModel.PositionOptions, Is.Not.Null);
            Assert.That(options.Count, Is.EqualTo(6));
            Assert.That(positionValues, Does.Not.Contain((int)PositionEnum.Undefined));
            Assert.That(positionValues, Is.EquivalentTo(
                new[]
                {
            (int)PositionEnum.Striker,
            (int)PositionEnum.Forward,
            (int)PositionEnum.Winger,
            (int)PositionEnum.Midfielder,
            (int)PositionEnum.Defender,
            (int)PositionEnum.Goalkeeper
                }
            ));
            Assert.That(pageModel.PositionOptions.SelectedValue, Is.EqualTo(((int)PositionEnum.Midfielder)));
        }


        #endregion OnPost
    }
}