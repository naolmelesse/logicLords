using ContosoCrafts.WebSite.Pages;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace UnitTests.Pages.Privacy
{

    /// <summary>
    /// Unit tests for the PrivacyModel Page.
    /// This class tests the behavior of the Privacy page.
    /// </summary>
    public class PrivacyTests
    {
        
        // The page model being tested
        #region TestSetup
        public static PrivacyModel pageModel;

        /// <summary>
        /// Setup method that initializes required objects before each test case.
        /// This is run before every test to ensure a fresh testing context.
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            // Mock the ILogger for PrivacyModel
            var MockLoggerDirect = Mock.Of<ILogger<PrivacyModel>>();

            // Initialize the PrivacyModel page with the mocked logger
            pageModel = new PrivacyModel(MockLoggerDirect)
            {
                // Set the PageContext and TempData using helper methods
                PageContext = TestHelper.PageContext,
                TempData = TestHelper.TempData,
            };
        }

        #endregion TestSetup

        #region OnGet

        /// <summary>
        /// Test case for the OnGet method.
        /// This test verifies that when the OnGet method is called, the ModelState is valid.
        /// </summary>
        [Test]
        public void OnGet_Valid_Activity_Set_Should_Return_RequestId()
        {
            // Arrange

            // Act
            // Call the OnGet method of the page model
            pageModel.OnGet();

            // Reset

            // Assert
            // Verify that the ModelState is valid after the OnGet method is called
            Assert.That(pageModel.ModelState.IsValid, Is.EqualTo(true));


        }

        #endregion OnGet
    }
}