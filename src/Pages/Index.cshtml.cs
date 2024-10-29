using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;

namespace ContosoCrafts.WebSite.Pages
{
    /// <summary> 
    /// This is the page model for the Index (Home) Page
    /// </summary> 
    public class IndexModel : PageModel
    {
        // Logger instance for capturing log details related to Index Page
        private readonly ILogger<IndexModel> _logger;

        /// <summary>
        /// Initializes a new instance of the IndexModel class.
        /// </summary>
        /// <param name="logger">The logger instance used for logging</param>
        /// <param name="productService">The service used to load player data</param>
        public IndexModel(ILogger<IndexModel> logger,
            JsonFileProductService productService)
        {
            _logger = logger;
            ProductService = productService;
        }

        //Getter of the JsonFileProductService
        public JsonFileProductService ProductService { get; }

        //The Players retrieved from the Product Model
        public IEnumerable<ProductModel> Products { get; private set; }


        /// <summary>
        /// Handles GET requests to the error page.
        /// Loads all data for players from the Product Service
        /// </summary>
        public void OnGet()
        {
            Products = ProductService.GetAllData();
        }
    }
}