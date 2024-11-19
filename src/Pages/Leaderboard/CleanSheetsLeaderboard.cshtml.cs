using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace ContosoCrafts.WebSite.Pages.Leaderboard
{

    /// <summary>
    /// Class for methods and properties related to Clean Sheets Leaderboard
    /// </summary>
    public class CleanSheetsLeaderboardModel : PageModel
    {
        // Logger instance for logging information and errors
        private readonly ILogger<CleanSheetsLeaderboardModel> _logger;


        /// <summary>
        /// Constructor for initializing the CleanSheetsLeaderboardModel with logger and player service dependencies.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="playerService">Service for handling player data retrieval.</param>
        public CleanSheetsLeaderboardModel(ILogger<CleanSheetsLeaderboardModel> logger,
           JsonFileProductService playerService)
        {
            _logger = logger;
            PlayerService = playerService;
        }

        // Service to retrieve player data
        public JsonFileProductService PlayerService { get; }

        // List of players retrieved from the PlayerService
        public IEnumerable<ProductModel> PlayerList { get; set; }

        /// <summary>
        /// Handles GET requests to populate the player list, selected players, and dropdown items for player selection.
        /// </summary>
        public void OnGet()
        {
            // Retrieve all player data from the PlayerService
            PlayerList = PlayerService.GetAllData().OrderByDescending(p => p.CleanSheets);
        }
    }
}