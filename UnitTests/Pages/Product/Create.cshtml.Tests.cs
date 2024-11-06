using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Pages.Product;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests.Pages.Product
{
    internal class CreateTests
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

        public static CreateModel pageModel;
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

            pageModel = new CreateModel(productService)
            {
                PageContext = pageContext,
                TempData = tempData,
                Url = new Mock<IUrlHelper>().Object
            };
        }

        #endregion TestSetup

        #region OnPost

        [Test]
        public void OnPost_Valid_Model_Should_Create_And_Redirect()
        {
            // Arrange
            var data = new ProductModel
            {
                Title = "Ready Player",
                Dateofbirth = "15 October 1998",
                Placeofbirth = "Santiago, Chile",
                Height = "1.74 m (5 ft 8 in)",
                Position = "Goalkeeper",
                Club = "Columbus Crew",
                Appearances = "12",
                Minutesplayed = "880",
                Assists = "5",
                Image = "/data/players/ChristianBenteke.png"
            };
            pageModel.Player = data;

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

        #endregion OnPost
    }
}
