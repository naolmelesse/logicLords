using System.Collections.Generic;
using System;

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Buffers.Text;
using System.Numerics;

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

        
        // Gets the list of players that match the search term.        
        public IEnumerable<ProductModel> SelectedPlayerList { get; private set; }



        // Gets or sets the search term provided by the user.        
        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }


        /// <summary>
        /// Handles GET requests to the error page.
        /// Loads all data for players from the Product Service and applies filtering if a search term is provided.

        /// </summary>
        public void OnGet()
        {
            PlayerList = PlayerService.GetAllData();

            // Filter based on search term if provided
            OnGetSearch();
        }

        /// <summary>
        /// Filters the player list based on the search term.
        /// If no search term is provided, <see cref="SelectedPlayerList"/> is set to null.
        /// </summary>
        public void OnGetSearch()
        {
            {
                if (string.IsNullOrEmpty(SearchTerm))
                {
                    SelectedPlayerList = null;
                    return;
                }
                SelectedPlayerList = PlayerList
                        .Where(player => player.Title.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase));
            }
        }

    }
}