using ContosoCrafts.WebSite.Controllers;
using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Pages;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UnitTests.Controllers
{
    [TestFixture]
    public class ProductsControllerTests
    {

        public static ProductsController controller;

        [SetUp]
        public void Setup()
        {
            // Arrange:             
            controller = new ProductsController(TestHelper.ProductService) 
            {
            };
        }

        [Test]
        public void Get_ReturnsAllProducts()
        {
            // Arrange:
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

        [Test]
        public void Patch_UpdatesProductRating_ReturnsOkResult()
        {
            // Arrange: 
            var players = TestHelper.ProductService.GetAllData();
            var data = players.First();
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
            Assert.That(updatedProduct.Ratings.Last(), Is.EqualTo(5)); // Ensure the last rating matches


        }

        [Test]
        public void Patch_InvalidRequest_ReturnsBadRequest()
        {
            // Arrange:
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