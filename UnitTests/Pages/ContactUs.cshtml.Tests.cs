using NUnit.Framework;
using ContosoCrafts.WebSite.Pages;

namespace UnitTests.Pages.ContactUs
{
    /// <summary>
    /// Class for unit tests of contact us page data
    /// </summary>
    public class ContactUsTests
    {
        #region TestSetup

        // The ContactUsModel instance being tested
        public static ContactUsModel pageModel;

        /// <summary>
        /// Initializes the ContactUsModel before each test
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            // Initialize the ContactUsModel before each test
            pageModel = new ContactUsModel();
        }

        #endregion TestSetup

        #region Property Tests

        /// <summary>
        /// Test that the PhoneNumber property returns the correct phone number
        /// </summary>
        [Test]
        public void PhoneNumber_Valid_DefaultValue_Should_Return_CorrectPhoneNumber()
        {
            // Arrange
            var data = "(123) 456-7890";

            // Act
            var result = pageModel.PhoneNumber;

            // Assert
            Assert.That(result, Is.EqualTo(data));
        }

        /// <summary>
        /// Test that the Email property returns the correct email address
        /// </summary>
        [Test]
        public void Email_Valid_DefaultValue_Should_Return_CorrectEmail()
        {
            // Arrange
            var data = "info@dribblekings.com";

            // Act
            var result = pageModel.Email;

            // Assert
            Assert.That(result, Is.EqualTo(data));
        }

        /// <summary>
        /// Test that the AddressLine1 property returns the correct address line
        /// </summary>
        [Test]
        public void AddressLine1_Valid_DefaultValue_Should_Return_CorrectAddress()
        {
            // Arrange
            var data = "Seattle University";

            // Act
            var result = pageModel.AddressLine1;

            // Assert
            Assert.That(result, Is.EqualTo(data));
        }

        /// <summary>
        /// Test that the City property returns the correct city
        /// </summary>
        [Test]
        public void City_Valid_DefaultValue_Should_Return_CorrectCity()
        {
            // Arrange
            var data = "Seattle";

            // Act
            var result = pageModel.City;

            // Assert
            Assert.That(result, Is.EqualTo(data));
        }

        /// <summary>
        /// Test that the State property returns the correct state abbreviation
        /// </summary>
        [Test]
        public void State_Valid_DefaultValue_Should_Return_CorrectState()
        {
            // Arrange
            var data = "WA";

            // Act
            var result = pageModel.State;

            // Assert
            Assert.That(result, Is.EqualTo(data));
        }

        #endregion Property Tests
    }
}
