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

        // Gets or sets the minimum player age provided by the user.   
        [BindProperty(SupportsGet = true)]
        public int? MinAge { get; set; }

        // Gets or sets the maximum player age provided by the user.   
        [BindProperty(SupportsGet = true)]
        public int? MaxAge { get; set; }

        // Gets or sets the minimum player age provided by the user.   
        [BindProperty(SupportsGet = true)]
        public int? MinAppearance { get; set; }

        // Gets or sets the maximum player age provided by the user.   
        [BindProperty(SupportsGet = true)]
        public int? MaxAppearance { get; set; }

        // Gets the position provided by the user.
        [BindProperty(SupportsGet = true)]
        public string Position { get; set; } 

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
        /// Filters the player list based on the search term and age range.
        /// If no search term is provided, <see cref="SelectedPlayerList"/> is set to null.
        /// </summary>
        public void OnGetSearch()
        {
            //Load players from PlayerList to SelectedPlayerList to filter
            SelectedPlayerList = PlayerList;

            // Filter based on search term if provided
            if (string.IsNullOrEmpty(SearchTerm) == false)
            {
                SelectedPlayerList = SelectedPlayerList
                    .Where(player => player.Title.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase));
            }

            // Filter based on MinAge if value provided
            if (MinAge.HasValue)
            {
                SelectedPlayerList = SelectedPlayerList
                    .Where(player => player.Age >= MinAge.Value);
            }

            // Filter based on MaxAge if value provided
            if (MaxAge.HasValue)
            {
                SelectedPlayerList = SelectedPlayerList
                    .Where(player => player.Age <= MaxAge.Value);
            }

            // Filter based on MinAppearance if value provided
            if (MinAppearance.HasValue)
            {
                SelectedPlayerList = SelectedPlayerList
                    .Where(player => player.AppearancesAsInt >= MinAppearance.Value);
            }


            // Filter based on MaxAppearance if value provided
            if (MaxAppearance.HasValue)
            {
                SelectedPlayerList = SelectedPlayerList
                    .Where(player => player.AppearancesAsInt <= MaxAppearance.Value);
            }

            // Filter based on Position if value provided
            if (!string.IsNullOrEmpty(Position))
            {
                SelectedPlayerList = SelectedPlayerList
                    .Where(player => player.Position.Equals(Position, StringComparison.OrdinalIgnoreCase));
            }

            SelectedPlayerList = SelectedPlayerList.ToList();

        }

    }
}