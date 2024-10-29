using System.Collections.Generic;
using System;

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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
            PlayerService = productService;
        }

        //Getter of the JsonFileProductService
        public JsonFileProductService PlayerService { get; }

        //The Players retrieved from the Product Model
        public IEnumerable<ProductModel> PlayerList { get; private set; }


        
        // Gets or sets the search term provided by the user.        
        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }


        /// <summary>
        /// Handles GET requests to the error page.
        /// Loads all data for players from the Product Service
        /// </summary>
        public void OnGet()
        {
            PlayerList = PlayerService.GetAllData();
            // Filter based on search term if provided
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                PlayerList = PlayerList
                    .Where(player => player.Title.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase));
            }
        }

        /// <summary>
        /// Handles GET requests specifically for search functionality.
        /// Reuses the same filtering logic defined in the <see cref="OnGet"/> method.
        /// </summary>
        public void OnGetSearch()
        {
            // This method will handle search-specific requests
            OnGet(); 
        }

    }
}