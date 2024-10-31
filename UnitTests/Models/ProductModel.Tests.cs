using NUnit.Framework;
using ContosoCrafts.WebSite.Models;
using System.Text.Json;

namespace UnitTests.Models
{
    public class ProductModelTests
    {
        #region TestSetup

        public static ProductModel productModel;

        [SetUp]
        public void TestInitialize()
        {
            // Initialize the ProductModel before each test
            productModel = new ProductModel
            {
                Id = "AitanaBonmati",
                Title = "Aitana Bonmati",
            };
        }

        #endregion TestSetup

        #region ToString Method Tests

        [Test]
        public void ToString_Valid_ProductModel_Should_Return_JsonString()
        {
            // Arrange
            var expectedJson = JsonSerializer.Serialize(productModel);

            // Act
            var result = productModel.ToString();

            // Assert
            Assert.That(result, Is.EqualTo(expectedJson), "The ToString method should return a JSON representation of the product model.");
        }

        [Test]
        public void ToString_Empty_ProductModel_Should_Return_JsonString()
        {
            // Arrange
            var emptyProductModel = new ProductModel();
            var expectedJson = JsonSerializer.Serialize(emptyProductModel);

            // Act
            var result = emptyProductModel.ToString();

            // Assert
            Assert.That(result, Is.EqualTo(expectedJson), "The ToString method should return a JSON representation of an empty product model.");
        }

        #endregion ToString Method Tests
    }
}
