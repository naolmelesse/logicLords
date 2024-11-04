using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ContosoCrafts.WebSite.Pages
{   
    /// <summary>
    /// Class for methods and properties related to player comparison
    /// </summary>
    public class ComparePlayersModel : PageModel
    {
        // Logger instance for logging information and errors
        private readonly ILogger<ComparePlayersModel> _logger;

        /// <summary>
        /// Constructor for initializing the ComparePlayersModel with logger and player service dependencies.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="playerService">Service for handling player data retrieval.</param>
        
        public ComparePlayersModel(ILogger<ComparePlayersModel> logger,
           JsonFileProductService playerService)
        {
            _logger = logger;
            PlayerService = playerService;
        }

        // Service to retrieve player data
        public JsonFileProductService PlayerService { get; }

        // List of players retrieved from the PlayerService
        public IEnumerable<ProductModel> PlayerList { get; set; }

        // List of items to display in a dropdown for selecting players
        public IEnumerable<SelectListItem> PlayerDropDownItems { get; set; }

        // Player selected on the left side for comparison
        public ProductModel SelectedPlayerLeft { get; set; }
        [BindProperty(SupportsGet = true)]

        // ID of the selected player on the left side
        public string SelectedPlayerLeftId { get; set; }

        // Player selected on the right side for comparison
        public ProductModel SelectedPlayerRight { get; set; }

        [BindProperty(SupportsGet = true)]
        // ID of the selected player on the right side
        public string SelectedPlayerRightId { get; set; }

        /// <summary>
        /// Handles GET requests to populate the player list, selected players, and dropdown items for player selection.
        /// </summary>
        public void OnGet()
        {
            // Retrieve all player data from the PlayerService
            PlayerList = PlayerService.GetAllData();

            // Set the left player based on SelectedPlayerLeftId
            if (SelectedPlayerLeftId != null)
            {
                SelectedPlayerLeft = PlayerList.FirstOrDefault(p => p.Id.ToString() == SelectedPlayerLeftId);
            }

            // Set the right player based on SelectedPlayerRightId
            if (SelectedPlayerRightId != null)
            {
                SelectedPlayerRight = PlayerList.FirstOrDefault(p => p.Id.ToString() == SelectedPlayerRightId);
            }

            // Populate dropdown items with player names for selection
            PlayerDropDownItems = PlayerList.Select(p => new SelectListItem
            {
                Value = p.Id,
                Text = p.Title
            }).ToList();
        }
    }
}
