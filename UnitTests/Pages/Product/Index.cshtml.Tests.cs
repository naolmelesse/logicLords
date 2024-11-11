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
using System.Linq;

namespace UnitTests.Pages.Product.Index
{
    public class IndexTests
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

        // The IndexModel instance being tested
        public static IndexModel pageModel;

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

            // Create a mock of the web host environment to simulate hosting environment
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            mockWebHostEnvironment.Setup(m => m.EnvironmentName).Returns("Hosting:UnitTestEnvironment");
            mockWebHostEnvironment.Setup(m => m.WebRootPath).Returns("../../../../src/bin/Debug/net7.0/wwwroot");
            mockWebHostEnvironment.Setup(m => m.ContentRootPath).Returns("./data/");

            // Create a mock logger for the IndexModel
            var MockLoggerDirect = Mock.Of<ILogger<IndexModel>>();

            // Initialize the product service with the mocked web host environment
            JsonFileProductService productService;
            productService = new JsonFileProductService(mockWebHostEnvironment.Object);
            
            // Initialize the IndexModel with the product service
            pageModel = new IndexModel(productService);
        }

        #endregion TestSetup

        #region OnGet

        /// <summary>
        /// Tests whether the OnGet method retrieves products successfully
        /// </summary>
        [Test]
        public void OnGet_Valid_Should_Return_Products()
        {

            // Arrange

            // Act
            pageModel.OnGet();
            var result = pageModel.ModelState.IsValid;

            // Assert
            Assert.That(result, Is.EqualTo(true));
            Assert.That(pageModel.Products.ToList().Any(), Is.EqualTo(true));
        }
        #endregion OnGet
    }
}