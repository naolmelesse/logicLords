using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Pages;
using ContosoCrafts.WebSite.Services;


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

            // Act
            pageModel.OnGet();

            // Assert
            Assert.That(pageModel.ModelState.IsValid, Is.True);
            var expectedCount = productService.GetAllData().Count();
            Assert.That(pageModel.PlayerList.Count(), Is.EqualTo(expectedCount));
            Assert.That(pageModel.SelectedPlayerList.Count(), Is.EqualTo(expectedCount));
        }

        [Test]
        public void OnGet_Valid_SearchTerm_Should_Return_FilteredPlayers()
        {
            // Arrange
            pageModel.SearchTerm = "Lionel";

            // Act
            pageModel.OnGet();

            // Assert
            Assert.That(pageModel.ModelState.IsValid, Is.True);
            Assert.That(pageModel.SelectedPlayerList.Count(), Is.EqualTo(1));
            Assert.That(pageModel.SelectedPlayerList.First().Title, Is.EqualTo("Lionel Messi"));
        }

        [Test]
        public void OnGet_Valid_MinAge_Should_Return_Players_Above_MinAge()
        {
            // Arrange
            pageModel.MinAge = 30;

            // Act
            pageModel.OnGet();

            // Assert
            var expectedPlayers = productService.GetAllData()
                .Where(p => p.Age >= 30)
                .Select(p => p.Title)
                .ToList();

            Assert.That(pageModel.SelectedPlayerList.Select(p => p.Title), Is.EquivalentTo(expectedPlayers));
        }

        [Test]
        public void OnGet_Valid_MaxAge_Should_Return_PlayersBelowMaxAge()
        {
            // Arrange
            pageModel.MaxAge = 25;

            // Act
            pageModel.OnGet();

            // Assert
            var expectedPlayers = productService.GetAllData()
                .Where(p => p.Age <= 25)
                .Select(p => p.Title)
                .ToList();

            Assert.That(pageModel.SelectedPlayerList.Select(p => p.Title), Is.EquivalentTo(expectedPlayers));
        }

        [Test]
        public void OnGet_Valid_MinAndMaxAge_Should_Return_Players_Within_AgeRange()
        {
            // Arrange
            pageModel.MinAge = 26;
            pageModel.MaxAge = 30;

            // Act
            pageModel.OnGet();

            // Assert
            var expectedPlayers = productService.GetAllData()
                .Where(p => p.Age >= 26 && p.Age <= 30)
                .Select(p => p.Title)
                .ToList();

            Assert.That(pageModel.SelectedPlayerList.Select(p => p.Title), Is.EquivalentTo(expectedPlayers));
        }

        [Test]
        public void OnGet_Valid_MinAppearance_Should_Return_Players_Above_MinAppearance()
        {
            // Arrange
            pageModel.MinAppearance = 30;

            // Act
            pageModel.OnGet();

            // Assert
            var expectedPlayers = productService.GetAllData()
                .Where(p => p.AppearancesAsInt >= 30)
                .Select(p => p.Title)
                .ToList();

            Assert.That(pageModel.SelectedPlayerList.Select(p => p.Title), Is.EquivalentTo(expectedPlayers));
        }

        [Test]
        public void OnGet_Valid_MaxAppearance_Should_Return_Players_Below_MaxAppearance()
        {
            // Arrange
            pageModel.MaxAppearance = 25;

            // Act
            pageModel.OnGet();

            // Assert
            var expectedPlayers = productService.GetAllData()
                .Where(p => p.AppearancesAsInt <= 25)
                .Select(p => p.Title)
                .ToList();

            Assert.That(pageModel.SelectedPlayerList.Select(p => p.Title), Is.EquivalentTo(expectedPlayers));
        }



        [Test]
        public void OnGet_Valid_Height_Should_Return_PlayersWithinHeightRange_165cm_174cm()
        {
            // Arrange
            pageModel.Height = "165 cm - 174 cm";

            // Act
            pageModel.OnGet();

            // Assert
            var expectedPlayers = productService.GetAllData()
                .Where(p => p.HeightInCMs >= 165 && p.HeightInCMs <= 174)
                .Select(p => p.Title)
                .ToList();

            Assert.That(pageModel.SelectedPlayerList.Select(p => p.Title), Is.EquivalentTo(expectedPlayers));
        }

        [Test]
        public void OnGet_Valid_Height_Should_Return_Players_Within_Height_Range_155_164()
        {
            // Arrange
            pageModel.Height = "155 cm - 164 cm";

            // Act
            pageModel.OnGet();

            // Assert
            var expectedPlayers = productService.GetAllData()
                .Where(p => p.HeightInCMs >= 155 && p.HeightInCMs <= 164)
                .Select(p => p.Title)
                .ToList();

            Assert.That(pageModel.SelectedPlayerList.Select(p => p.Title), Is.EquivalentTo(expectedPlayers));
        }

        [Test]
        public void OnGet_Valid_Height_Should_Return_Players_Within_Height_Range_175cm_184cm()
        {
            // Arrange
            pageModel.Height = "175 cm - 184 cm";

            // Act
            pageModel.OnGet();

            // Assert
            var expectedPlayers = productService.GetAllData()
                .Where(p => p.HeightInCMs >= 175 && p.HeightInCMs <= 184)
                .Select(p => p.Title)
                .ToList();

            Assert.That(pageModel.SelectedPlayerList.Select(p => p.Title), Is.EquivalentTo(expectedPlayers));

            // Ensure the specific line in OnGetSearch is executed
            Assert.That(expectedPlayers.Count, Is.GreaterThan(0));
        }

        [Test]
        public void OnGet_Valid_Height_Less_Than_155_Should_Return_Players_Below_155cm()
        {
            // Arrange
            pageModel.Height = "< 155 cm";

            // Act
            pageModel.OnGet();

            // Assert
            var expectedPlayers = productService.GetAllData()
                .Where(p => p.HeightInCMs < 155)
                .Select(p => p.Title)
                .ToList();

            Assert.That(pageModel.SelectedPlayerList.Select(p => p.Title), Is.EquivalentTo(expectedPlayers));
        }

        [Test]
        public void OnGet_Valid_Height_Between_185_And_194_Should_Return_Players_Within_Range()
        {
            // Arrange
            pageModel.Height = "185 cm - 194 cm";

            // Act
            pageModel.OnGet();

            // Assert
            var expectedPlayers = productService.GetAllData()
                .Where(p => p.HeightInCMs >= 185 && p.HeightInCMs <= 194)
                .Select(p => p.Title)
                .ToList();

            Assert.That(pageModel.SelectedPlayerList.Select(p => p.Title), Is.EquivalentTo(expectedPlayers));

            // Ensure the specific line in OnGetSearch is executed
            Assert.That(expectedPlayers.Count, Is.GreaterThan(0));
        }

        [Test]
        public void OnGet_Valid_Height_Greater_Than_195_Should_Return_Players_Above_195cm()
        {
            // Arrange
            pageModel.Height = "> 195 cm";

            // Act
            pageModel.OnGet();

            // Assert
            var expectedPlayers = productService.GetAllData()
                .Where(p => p.HeightInCMs > 195)
                .Select(p => p.Title)
                .ToList();

            Assert.That(pageModel.SelectedPlayerList.Select(p => p.Title), Is.EquivalentTo(expectedPlayers));
        }

        [Test]
        public void OnGet_Valid_All_Filters_Should_Return_PlayersMatchingAllCriteria()
        {
            // Arrange
            pageModel.SearchTerm = "Messi";
            pageModel.MinAge = 30;
            pageModel.MaxAge = 40;
            pageModel.MinAppearance = 15;
            pageModel.MaxAppearance = 25;
            pageModel.Position = "Forward";
            pageModel.Height = "165 cm - 174 cm";

            // Act
            pageModel.OnGet();

            // Assert
            var expectedPlayers = productService.GetAllData()
                .Where(p =>
                    p.Title.Contains("Messi", StringComparison.OrdinalIgnoreCase) &&
                    p.Age >= 30 && p.Age <= 40 &&
                    p.AppearancesAsInt >= 15 && p.AppearancesAsInt <= 25 &&
                    p.Position.Equals("Forward", StringComparison.OrdinalIgnoreCase) &&
                    p.HeightInCMs >= 165 && p.HeightInCMs <= 174)
                .Select(p => p.Title)
                .ToList();

            Assert.That(pageModel.SelectedPlayerList.Count(), Is.EqualTo(expectedPlayers.Count));
            Assert.That(pageModel.SelectedPlayerList.Select(p => p.Title), Is.EquivalentTo(expectedPlayers));
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
