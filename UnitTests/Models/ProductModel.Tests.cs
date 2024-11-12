using NUnit.Framework;
using ContosoCrafts.WebSite.Models;
using System.Text.Json;

namespace UnitTests.Models
{

    /// <summary>
    /// Unit tests for the ProductModel class.
    /// </summary>
    public class ProductModelTests
    {
        #region TestSetup

        // Static instance of ProductModel for testing.
        public static ProductModel productModel;

        /// <summary>
        /// Initializes the ProductModel instance before testing.
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            // Initialize the ProductModel with sample data
            productModel = new ProductModel
            {
                Id = "AitanaBonmati",
                Title = "Aitana Bonmati",
            };
        }

        #endregion TestSetup

        #region ToString Method Tests

        /// <summary>
        /// Test case for a valid ProductModel instance, expecting ToString to return a JSON string representation of the model.
        /// </summary>
        [Test]
        public void ToString_Valid_ProductModel_Should_Return_JsonString()
        {
            // Arrange

            // Expected JSON string from serializing the productModel instance
            var expectedJson = JsonSerializer.Serialize(productModel);

            // Act

            // Call ToString to get the JSON string representation
            var result = productModel.ToString();

            // Assert
            Assert.That(result, Is.EqualTo(expectedJson), "The ToString method should return a JSON representation of the product model.");
        }

        /// <summary>
        /// Test case for an empty ProductModel instance, expecting ToString to return a JSON string representation of an empty model.
        /// </summary>
        [Test]
        public void ToString_Empty_ProductModel_Should_Return_JsonString()
        {
            // Arrange

            // Initialize an empty ProductModel instance
            var emptyProductModel = new ProductModel();

            // Expected JSON string from serializing the emptyProductModel instance
            var expectedJson = JsonSerializer.Serialize(emptyProductModel);

            // Act

            // Call ToString to get the JSON string representation
            var result = emptyProductModel.ToString();

            // Assert
            Assert.That(result, Is.EqualTo(expectedJson), "The ToString method should return a JSON representation of an empty product model.");
        }

        #endregion ToString Method Tests
    }
}
