
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;

using Moq;

using ContosoCrafts.WebSite.Services;

namespace UnitTests
{
    /// <summary>
    /// Helper to hold the web start settings
    ///
    /// HttpClient
    /// 
    /// Action Contect
    /// 
    /// The View Data and Teamp Data
    /// 
    /// The Product Service
    /// </summary>
    public static class TestHelper
    {
        // Mock instance of IWebHostEnvironment for testing
        public static Mock<IWebHostEnvironment> MockWebHostEnvironment;

        // Factory for creating URL helpers
        public static IUrlHelperFactory UrlHelperFactory;

        // Default HTTP context for handling HTTP requests in tests
        public static DefaultHttpContext HttpContextDefault;

        // Instance of IWebHostEnvironment for environment-specific settings
        public static IWebHostEnvironment WebHostEnvironment;

        // Dictionary to store the model validation state
        public static ModelStateDictionary ModelState;

        // Action context holding details like route data, action descriptor, and model state
        public static ActionContext ActionContext;

        // Provider for model metadata used in views and data binding
        public static EmptyModelMetadataProvider ModelMetadataProvider;

        // Dictionary for storing and passing view data to views
        public static ViewDataDictionary ViewData;

        // Dictionary for holding temporary data between requests
        public static TempDataDictionary TempData;

        // Page context containing action context, view data, and HTTP context
        public static PageContext PageContext;

        // Service for managing product data from JSON files
        public static JsonFileProductService ProductService;


        /// <summary>
        /// Default Constructor
        /// </summary>
        static TestHelper()
        {
            MockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            MockWebHostEnvironment.Setup(m => m.EnvironmentName).Returns("Hosting:UnitTestEnvironment");
            MockWebHostEnvironment.Setup(m => m.WebRootPath).Returns(TestFixture.DataWebRootPath);
            MockWebHostEnvironment.Setup(m => m.ContentRootPath).Returns(TestFixture.DataContentRootPath);

            HttpContextDefault = new DefaultHttpContext()
            {
                TraceIdentifier = "trace",
            };
            HttpContextDefault.HttpContext.TraceIdentifier = "trace";

            ModelState = new ModelStateDictionary();

            ActionContext = new ActionContext(HttpContextDefault, HttpContextDefault.GetRouteData(), new PageActionDescriptor(), ModelState);

            ModelMetadataProvider = new EmptyModelMetadataProvider();
            ViewData = new ViewDataDictionary(ModelMetadataProvider, ModelState);
            TempData = new TempDataDictionary(HttpContextDefault, Mock.Of<ITempDataProvider>());

            PageContext = new PageContext(ActionContext)
            {
                ViewData = ViewData,
                HttpContext = HttpContextDefault
            };

            ProductService = new JsonFileProductService(MockWebHostEnvironment.Object);

            JsonFileProductService productService;

            productService = new JsonFileProductService(TestHelper.MockWebHostEnvironment.Object);
        }
    }
}