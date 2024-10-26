using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Pages.Product;
using ContosoCrafts.WebSite.Services;

namespace UnitTests.Pages.Product.Update
{
    public class UpdateTests
    {
        #region TestSetup
        public static DefaultHttpContext httpContextDefault;
        public static IWebHostEnvironment webHostEnvironment;
        public static ModelStateDictionary modelState;
        public static ActionContext actionContext;
        public static EmptyModelMetadataProvider modelMetadataProvider;
        public static ViewDataDictionary viewData;
        public static TempDataDictionary tempData;
        public static PageContext pageContext;

        public static UpdateModel pageModel;
        public static JsonFileProductService productService;

        [SetUp]
        public void TestInitialize()
        {
            httpContextDefault = new DefaultHttpContext();

            modelState = new ModelStateDictionary();

            actionContext = new ActionContext(httpContextDefault, httpContextDefault.GetRouteData(), new PageActionDescriptor(), modelState);

            modelMetadataProvider = new EmptyModelMetadataProvider();
            viewData = new ViewDataDictionary(modelMetadataProvider, modelState);
            tempData = new TempDataDictionary(httpContextDefault, Mock.Of<ITempDataProvider>());

            pageContext = new PageContext(actionContext)
            {
                ViewData = viewData,
            };

            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            mockWebHostEnvironment.Setup(m => m.EnvironmentName).Returns("Hosting:UnitTestEnvironment");
            mockWebHostEnvironment.Setup(m => m.WebRootPath).Returns("../../../../src/bin/Debug/net7.0/wwwroot");
            mockWebHostEnvironment.Setup(m => m.ContentRootPath).Returns("./data/");

            productService = new JsonFileProductService(mockWebHostEnvironment.Object);

            pageModel = new UpdateModel(productService)
            {
                PageContext = pageContext,
                TempData = tempData,
                Url = new Mock<IUrlHelper>().Object
            };
        }

        #endregion TestSetup

        #region OnGet Tests

        [Test]
        public void OnGet_Valid_Id_Should_Return_Product()
        {
            // Arrange
            var productId = "alonso-martinez";

            // Act
            pageModel.Id = productId;
            pageModel.OnGet();

            // Assert
            Assert.That(pageModel.Player, Is.Not.Null);
            Assert.That(pageModel.Player.Id, Is.EqualTo(productId));
        }

        [Test]
        public void OnGet_Invalid_Id_Should_Return_Null()
        {
            // Arrange
            var invalidProductId = "invalid-id";

            // Act
            pageModel.Id = invalidProductId;
            pageModel.OnGet();

            // Assert
            Assert.That(pageModel.Player, Is.Null);
        }

        [Test]
        public void OnGet_Invalid_No_Id_Provided_Should_Return_Null()
        {
            // Act
            pageModel.Id = null;
            pageModel.OnGet();

            // Assert
            Assert.That(pageModel.Player, Is.Null);
        }

        #endregion OnGet Tests

        #region OnPost Tests

        [Test]
        public void OnPost_Valid_Model_Should_Update_And_Redirect()
        {
            // Arrange
            var productId = "alonso-martinez";
            var productToUpdate = new ProductModel { Id = productId, Title = "Updated Alonso Martinez" };
            pageModel.Player = productToUpdate;

            // Act
            var result = pageModel.OnPost() as RedirectToPageResult;

            // Assert
            Assert.That(pageModel.ModelState.IsValid, Is.EqualTo(true));
            Assert.That(result.PageName, Is.EqualTo("/Index"));
        }

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
