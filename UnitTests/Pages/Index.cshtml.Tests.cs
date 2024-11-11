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

        // The IndexModel instance being tested
        public static IndexModel pageModel;

        // The product service used to retrieve player data
        public static JsonFileProductService productService;

        // The web host environment for accessing environment-specific information
        public static IWebHostEnvironment webHostEnvironment;

        /// <summary>
        /// Initializes common objects needed for the tests
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            // Create a mock logger for the IndexModel
            var MockLoggerDirect = Mock.Of<ILogger<IndexModel>>();

            // Create a mock of the IWebHostEnvironment
            var webHostEnvironmentMock = new Mock<IWebHostEnvironment>();

            // Get the path to the wwwroot directory
            var webRootPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "wwwroot");

            // Set up the mock to return the wwwroot path when WebRootPath is accessed
            webHostEnvironmentMock.Setup(m => m.WebRootPath).Returns(webRootPath);

            // Assign the mock IWebHostEnvironment to the webHostEnvironment variable
            webHostEnvironment = webHostEnvironmentMock.Object;

            // Initialize the product service with the mocked web host environment
            productService = new JsonFileProductService(webHostEnvironment);

            // Initialize the IndexModel with the mock logger and product service
            pageModel = new IndexModel(MockLoggerDirect, productService);
        }

        #endregion TestSetup

        #region OnGet

        /// <summary>
        /// Test that OnGet retrieves all players when no filters are applied
        /// </summary>
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

        /// <summary>
        /// Test that OnGet with a search term returns the filtered players
        /// </summary>
        [Test]
        public void OnGet_Valid_SearchTerm_Should_Return_FilteredPlayers()
        {
            // Arrange
            pageModel.SearchTerm = "Lionel";
            var data = "Lionel Messi";

            // Act
            pageModel.OnGet();
            var result = pageModel.SelectedPlayerList.First().Title;

            // Assert
            Assert.That(pageModel.ModelState.IsValid, Is.True);
            Assert.That(pageModel.SelectedPlayerList.Count(), Is.EqualTo(1));
            Assert.That(result, Is.EqualTo(data));
        }

        /// <summary>
        /// Test that OnGet with MinAge returns players above the specified age
        /// </summary>
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

        /// <summary>
        /// Test that OnGet with MaxAge returns players below the specified age
        /// </summary>
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

        /// <summary>
        /// Test that OnGet with both MinAge and MaxAge returns players within the age range
        /// </summary>
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

        /// <summary>
        /// Test that OnGet with Min Appearance returns players with appearances above the specified minimum
        /// </summary>
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

        /// <summary>
        /// Test that OnGet with Max Appearance returns players with appearances below the specified maximum
        /// </summary>
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

        /// <summary>
        /// Test that OnGet with specific height range returns players within 165 cm to 174 cm
        /// </summary>
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

        /// <summary>
        /// Test that OnGet with height range 155 cm to 164 cm returns correct players
        /// </summary>
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

        /// <summary>
        /// Test that OnGet with height range 175 cm to 184 cm returns correct players
        /// </summary>
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

        /// <summary>
        /// Test that OnGet with height less than 155 cm returns players below 155 cm
        /// </summary>
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

        /// <summary>
        /// Test that OnGet with height between 185 cm and 194 cm returns correct players
        /// </summary>
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

        /// <summary>
        /// Test that OnGet with height greater than 195 cm returns players above 195 cm
        /// </summary>
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

        /// <summary>
        /// Test that OnGet with all filters applied returns players matching all criteria
        /// </summary>
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

        #endregion OnGet
    }
}
