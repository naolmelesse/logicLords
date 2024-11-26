using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ContosoCrafts.WebSite.Pages.Product
{
    /// <summary>
    /// Index Page will return all the data to show the user
    /// </summary>
    public class IndexModel : PageModel
    {
        // Logger instance for logging errors and information
        private readonly ILogger<IndexModel> _logger;

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="productService"></param>
        /// <param name="logger">Logger for logging events and errors.</param>
        public IndexModel(JsonFileProductService productService, ILogger<IndexModel> logger)
        {
            ProductService = productService;
            _logger = logger;

        }

        // Data Service
        public JsonFileProductService ProductService { get; }

        // Collection of the Data
        public IEnumerable<ProductModel> Products { get; private set; }

        /// <summary>
        /// REST OnGet
        /// Return all the data
        /// </summary>
        /// <returns>A page result or a redirection to the Error page in case of an exception.</returns>
        public IActionResult OnGet()
        {
            try
            {

                Products = ProductService.GetAllData();

                if (Products == null)
                {
                    throw new Exception("Products was null");
                }
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching products.");
                TempData["ErrorMessage"] = ex.ToString();

                return RedirectToPage("/Error");
            }
        }
    }
}