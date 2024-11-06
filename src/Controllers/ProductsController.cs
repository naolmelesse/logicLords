using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;

using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;

namespace ContosoCrafts.WebSite.Controllers
{
    /// <summary>
    /// API Controller that manages product-related operations.
    /// Provides endpoints to retrieve products and update product ratings.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {


        /// <summary>
        /// Initializes a new instance of the <see cref="ProductsController"/> class.
        /// Injects the <see cref="JsonFileProductService"/> to manage product data.
        /// </summary>
        /// <param name="productService">Service to access and manipulate product data.</param>
        public ProductsController(JsonFileProductService productService)
        {
            ProductService = productService;
        }

        /// <summary>
        /// Gets the product service instance to access product data.
        /// </summary>
        public JsonFileProductService ProductService { get; }


        /// <summary>
        /// Retrieves all product data.
        /// </summary>
        /// <returns>A collection of <see cref="ProductModel"/> representing the products.</returns>
        [HttpGet]
        public IEnumerable<ProductModel> Get()
        {
            return ProductService.GetAllData();
        }

        /// <summary>
        /// Updates the rating of a product based on the product ID and rating provided.
        /// </summary>
        /// <param name="request">An object containing the product ID and new rating.</param>
        /// <returns>An <see cref="ActionResult"/> indicating the status of the operation.</returns>
        [HttpPatch]
        public ActionResult Patch([FromBody] RatingRequest request)
        {
            // Validate that ProductId is not null or empty
            if (string.IsNullOrEmpty(request.ProductId))
            {
                return BadRequest("ProductId cannot be null or empty.");
            }

            ProductService.AddRating(request.ProductId, request.Rating);
            
            return Ok();
        }


        /// <summary>
        /// Represents a request to update the rating of a product.
        /// Contains the product ID and the new rating value.
        /// </summary>
        public class RatingRequest
        {

            /// <summary>
            /// Gets or sets the product ID for which the rating is being updated.
            /// </summary>
            public string ProductId { get; set; }

            /// <summary>
            /// Gets or sets the rating value to be applied to the product.
            /// </summary>
            public int Rating { get; set; }
        }
    }
}