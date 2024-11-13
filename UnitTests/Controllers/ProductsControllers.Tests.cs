using ContosoCrafts.WebSite.Controllers;
using ContosoCrafts.WebSite.Models;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests.Controllers
{

    /// <summary>
    /// Unit tests for the ProductsController class.
    /// </summary>
    [TestFixture]
    public class ProductsControllerTests
    {

        /// <summary>
        /// Static instance of ProductsController for testing.
        /// </summary>
        public static ProductsController controller;

        /// <summary>
        /// Initializes the ProductsController instance before each test with the ProductService.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            // Arrange:
            
            // Initialize the ProductsController with ProductService
            controller = new ProductsController(TestHelper.ProductService) {};

        }

        /// <summary>
        /// Test case for retrieving all products. Expects all products to be returned with correct details.
        /// </summary>
        [Test]
        public void Get_Valid_Should_Return_All_Products()
        {
            // Arrange:
            // Retrieve all products from the ProductService
            var players = TestHelper.ProductService.GetAllData();

            // Act: Call the Get method
            var result = controller.Get().ToList();

            // Assert: Verify that result is not null, is of the expected type, and has the correct count
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<IEnumerable<ProductModel>>());
            Assert.That(result.Count, Is.EqualTo(players.Count()));

            // Perform element-wise comparison of properties to ensure each product matches
            for (int i = 0; i < result.Count; i++)
            {
                Assert.That(result[i].Id, Is.EqualTo(players.ElementAt(i).Id));
                Assert.That(result[i].Title, Is.EqualTo(players.ElementAt(i).Title));
                Assert.That(result[i].Ratings, Is.EqualTo(players.ElementAt(i).Ratings));             
            }

        }

        /// <summary>
        /// Test case for updating product rating with a valid request. Expects an OkResult after updating.
        /// </summary>
        [Test]
        public void Patch_Valid_Update_Product_Rating__Should_Return_OkResult()
        {
            // Arrange:
            // Retrieve all products and select the first one for testing
            var players = TestHelper.ProductService.GetAllData();

            // Get the first player's data
            var data = players.First();

            // Add a new rating for the first player
            var ratingRequest = new ProductsController.RatingRequest
            {
                ProductId = data.Id,
                Rating = 5
            };

            // Act: Call the Patch method
            var result = controller.Patch(ratingRequest);

            // Assert: Check that result is an OkResult
            Assert.That(result, Is.InstanceOf<OkResult>());

            // Verify the product rating was updated in the data
            var updatedProduct = TestHelper.ProductService.GetAllData().First(p => p.Id == data.Id);

            // Ensure the last rating matches
            Assert.That(updatedProduct.Ratings.Last(), Is.EqualTo(5)); 

        }

        /// <summary>
        /// Patch_InvalidRequest_ReturnsBadRequest
        /// Test case for an invalid rating request with null ProductId. Expects a BadRequest result.
        /// </summary>
        [Test]
        public void Patch_InvalidRequest_ReturnsBadRequest()
        {
            // Arrange:
            // Create an invalid rating request with null ProductId
            var ratingRequest = new ProductsController.RatingRequest
            {
                ProductId = null,
                Rating = 5
            };

            // Act: Call the Patch method
            var result = controller.Patch(ratingRequest);

            // Assert: Since ProductId is null, we expect a BadRequest         
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
          
        }
    }
}