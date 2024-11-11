using ContosoCrafts.WebSite.Pages;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;


// THIS IS THE UNIT TEST FOR THE HOME PAGE

namespace UnitTests.Pages.Index
{
    public class IndexTests
    {
        #region TestSetup

        public static IndexModel pageModel;
        public static JsonFileProductService productService;
        public static IWebHostEnvironment webHostEnvironment;

        [SetUp]
        public void TestInitialize()
        {
            var MockLoggerDirect = Mock.Of<ILogger<IndexModel>>();

            var webHostEnvironmentMock = new Mock<IWebHostEnvironment>();

            var webRootPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "wwwroot");

            webHostEnvironmentMock.Setup(m => m.WebRootPath).Returns(webRootPath);

            webHostEnvironment = webHostEnvironmentMock.Object;

            productService = new JsonFileProductService(webHostEnvironment);

            pageModel = new IndexModel(MockLoggerDirect, productService);
        }

        #endregion TestSetup

        #region OnGet

        [Test]
        public void OnGet_Valid_Should_Return_AllPlayers()
        {
            // Arrange
            var data = productService.GetAllData().Count();

            // Act
            pageModel.OnGet();

            var result = pageModel.PlayerList.Count();

            // Assert
            Assert.That(pageModel.ModelState.IsValid, Is.EqualTo(true));
            Assert.That(result, Is.EqualTo(data));
            Assert.That(pageModel.SelectedPlayerList.Count(), Is.EqualTo(data));
        }

        [Test]
        public void OnGet_Valid_SearchTerm_Should_Return_FilteredPlayers()
        {
            // Arrange
            pageModel.SearchTerm = "Lionel";

            // Act
            pageModel.OnGet();
            var result = pageModel.SelectedPlayerList.First().Title;

            // Assert
            Assert.That(pageModel.ModelState.IsValid, Is.True);
            Assert.That(pageModel.SelectedPlayerList.Count(), Is.EqualTo(1));
            Assert.That(result, Is.EqualTo("Lionel Messi"));
        }

        [Test]
        public void OnGet_Valid_MinAge_Should_Return_Players_Above_MinAge()
        {
            // Arrange
            pageModel.MinAge = 30;
            var data = productService.GetAllData()
                .Where(p => p.Age >= 30)
                .Select(p => p.Title)
                .ToList();

            // Act
            pageModel.OnGet();
            var result = pageModel.SelectedPlayerList.Select(p => p.Title);

            // Assert
            Assert.That(result, Is.EquivalentTo(data));
        }

        [Test]
        public void OnGet_Valid_MaxAge_Should_Return_PlayersBelowMaxAge()
        {
            // Arrange
            pageModel.MaxAge = 25;
            var data = productService.GetAllData()
                .Where(p => p.Age <= 25)
                .Select(p => p.Title)
                .ToList();

            // Act
            pageModel.OnGet();
            var result = pageModel.SelectedPlayerList.Select(p => p.Title);

            // Assert
            Assert.That(result, Is.EquivalentTo(data));
        }

        [Test]
        public void OnGet_Valid_MinAndMaxAge_Should_Return_Players_Within_AgeRange()
        {
            // Arrange
            pageModel.MinAge = 26;
            pageModel.MaxAge = 30;
            var data = productService.GetAllData()
                .Where(p => p.Age >= 26 && p.Age <= 30)
                .Select(p => p.Title)
                .ToList();


            // Act
            pageModel.OnGet();
            var result = pageModel.SelectedPlayerList.Select(p => p.Title);

            // Assert
            Assert.That(result, Is.EquivalentTo(data));
        }

        [Test]
        public void OnGet_Valid_MinAppearance_Should_Return_Players_Above_MinAppearance()
        {
            // Arrange
            pageModel.MinAppearance = 30;
            var data = productService.GetAllData()
                .Where(p => p.Appearances >= 30)
                .Select(p => p.Title)
                .ToList();

            // Act
            pageModel.OnGet();
            var result = pageModel.SelectedPlayerList.Select(p => p.Title);

            // Assert
            Assert.That(result, Is.EquivalentTo(data));
        }

        [Test]
        public void OnGet_Valid_MaxAppearance_Should_Return_Players_Below_MaxAppearance()
        {
            // Arrange
            pageModel.MaxAppearance = 25;
            var data = productService.GetAllData()
                .Where(p => p.Appearances <= 25)
                .Select(p => p.Title)
                .ToList();

            // Act
            pageModel.OnGet();
            var result = pageModel.SelectedPlayerList.Select(p => p.Title);

            // Assert
            Assert.That(result, Is.EquivalentTo(data));
        }



        [Test]
        public void OnGet_Valid_Height_Should_Return_Players_Within_Height_Range_165cm_174cm()
        {
            // Arrange
            pageModel.Height = "165 cm - 174 cm";
            var data = productService.GetAllData()
                .Where(p => p.HeightInCMs >= 165 && p.HeightInCMs <= 174)
                .Select(p => p.Title)
                .ToList();

            // Act
            pageModel.OnGet();
            var result = pageModel.SelectedPlayerList.Select(p => p.Title);

            // Assert
            Assert.That(result, Is.EquivalentTo(data));
        }

        [Test]
        public void OnGet_Valid_Height_Should_Return_Players_Within_Height_Range_155_164()
        {
            // Arrange
            pageModel.Height = "155 cm - 164 cm";
            var data = productService.GetAllData()
                .Where(p => p.HeightInCMs >= 155 && p.HeightInCMs <= 164)
                .Select(p => p.Title)
                .ToList();

            // Act
            pageModel.OnGet();
            var result = pageModel.SelectedPlayerList.Select(p => p.Title);

            // Assert
            Assert.That(result, Is.EquivalentTo(data));
        }

        [Test]
        public void OnGet_Valid_Height_Should_Return_Players_Within_Height_Range_175cm_184cm()
        {
            // Arrange
            pageModel.Height = "175 cm - 184 cm";
            var data = productService.GetAllData()
                .Where(p => p.HeightInCMs >= 175 && p.HeightInCMs <= 184)
                .Select(p => p.Title)
                .ToList();


            // Act
            pageModel.OnGet();
            var result = pageModel.SelectedPlayerList.Select(p => p.Title);

            // Assert
            Assert.That(result, Is.EquivalentTo(data));
        }

        [Test]
        public void OnGet_Valid_Height_Less_Than_155_Should_Return_Players_Below_155cm()
        {
            // Arrange
            pageModel.Height = "< 155 cm";
            var data = productService.GetAllData()
                .Where(p => p.HeightInCMs < 155)
                .Select(p => p.Title)
                .ToList();

            // Act
            pageModel.OnGet();
            var result = pageModel.SelectedPlayerList.Select(p => p.Title);

            // Assert


            Assert.That(result, Is.EquivalentTo(data));
        }

        [Test]
        public void OnGet_Valid_Height_Between_185_And_194_Should_Return_Players_Within_Range()
        {
            // Arrange
            pageModel.Height = "185 cm - 194 cm";
            var data = productService.GetAllData()
                .Where(p => p.HeightInCMs >= 185 && p.HeightInCMs <= 194)
                .Select(p => p.Title)
                .ToList();

            // Act
            pageModel.OnGet();
            var result = pageModel.SelectedPlayerList.Select(p => p.Title);

            // Assert
            Assert.That(result, Is.EquivalentTo(data));
        }

        [Test]
        public void OnGet_Valid_Height_Greater_Than_195_Should_Return_Players_Above_195cm()
        {
            // Arrange
            pageModel.Height = "> 195 cm";
            var data = productService.GetAllData()
                .Where(p => p.HeightInCMs > 195)
                .Select(p => p.Title)
                .ToList();

            // Act
            pageModel.OnGet();
            var result = pageModel.SelectedPlayerList.Select(p => p.Title);

            // Assert
            Assert.That(result, Is.EquivalentTo(data));
        }

        [Test]
        public void OnGet_Valid_All_Filters_Should_Return_Players_Matching_All_Criteria()
        {
            // Arrange
            pageModel.SearchTerm = "Messi";
            pageModel.MinAge = 30;
            pageModel.MaxAge = 40;
            pageModel.MinAppearance = 15;
            pageModel.MaxAppearance = 25;
            pageModel.Position = "Forward";
            pageModel.Height = "165 cm - 174 cm";
            
            var data = productService.GetAllData()
                .Where(p =>
                    p.Title.Contains("Messi", StringComparison.OrdinalIgnoreCase) &&
                    p.Age >= 30 && p.Age <= 40 &&
                    p.Appearances >= 15 && p.Appearances <= 25 &&
                    p.Position.Equals("Forward", StringComparison.OrdinalIgnoreCase) &&
                    p.HeightInCMs >= 165 && p.HeightInCMs <= 174)
                .Select(p => p.Title)
                .ToList();

            // Act
            pageModel.OnGet();
            var result = pageModel.SelectedPlayerList.Select(p => p.Title);

            // Assert
            Assert.That(result, Is.EquivalentTo(data));
            Assert.That(pageModel.SelectedPlayerList.Count(), Is.EqualTo(data.Count));
        }

        [Test]
        public void OnGet_Invalid_Height_Null_Should_NotFilterByHeight()
        {
            // Arrange
            pageModel.Height = null;

            // Act
            pageModel.OnGet();

            // Assert
            Assert.That(pageModel.SelectedPlayerList.Count(), Is.EqualTo(pageModel.PlayerList.Count()));
        }

        [Test]
        public void OnGet_Invalid_Height_Range_Should_Not_Filter_Players()
        {
            // Arrange
            pageModel.Height = "Invalid Range";

            // Act
            pageModel.OnGet();

            // Assert
            Assert.That(pageModel.SelectedPlayerList.Count(), Is.EqualTo(pageModel.PlayerList.Count()));
        }


        #endregion OnGet
    }
}