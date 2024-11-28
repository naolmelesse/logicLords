using System.Collections.Generic;
using System;

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using ContosoCrafts.WebSite.Pages.Shared;
using System.Reflection;

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

        // List of leaderboard snippet criteria to display different leaderboards
        public List<LeaderboardSnippetModel> LeaderboardSnippetCriterias { get; set; }

        // Gets or sets the search term provided by the user.        
        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        // Gets or sets the minimum player age provided by the user.   
        [BindProperty(SupportsGet = true)]
        public int? MinAge { get; set; }

        // Gets or sets the maximum player age provided by the user.   
        [BindProperty(SupportsGet = true)]
        public int? MaxAge { get; set; }

        // Gets or sets the minimum player appearance provided by the user.   
        [BindProperty(SupportsGet = true)]
        public int? MinAppearance { get; set; }

        // Gets or sets the maximum player appearance provided by the user.   
        [BindProperty(SupportsGet = true)]
        public int? MaxAppearance { get; set; }

        // Gets the position provided by the user.
        [BindProperty(SupportsGet = true)]
        public string Position { get; set; }

        // Gets the Height provided by the user.
        [BindProperty(SupportsGet = true)]
        public string HeightInCM { get; set; }

        /// <summary>
        /// Handles GET requests to the error page.
        /// Loads all data for players from the Product Service and applies filtering if a search term is provided.
        /// </summary>
        public void OnGet()
        {
            PlayerList = PlayerService.GetAllData();
            SetPlayerLeaderboardSnippet();
            // Filter based on search term if provided
            OnGetSearch();
        }

        /// <summary>
        /// Filters the player list based on the search term and age range.
        /// If no search term is provided, <see cref="SelectedPlayerList"/> is set to null.
        /// </summary>
        public void OnGetSearch()
        {
            // Load players from PlayerList to SelectedPlayerList to filter if not already loaded
            if (SelectedPlayerList == null)
            {
                SelectedPlayerList = PlayerList;
            }

            // Filter based on search term if provided
            if (string.IsNullOrEmpty(SearchTerm) == false)
            {
                SelectedPlayerList = SelectedPlayerList
                    .Where(player => player.Title.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase));
            }

            OnGetSearchFilterSetAge();
            OnGetSearchFilterSetAppearance();
            OnGetSearchFilterSetPosition();
            OnGetSearchFilterSetHeight();


            SelectedPlayerList = SelectedPlayerList.ToList();

        }

        /// <summary>
        /// Sets the age filter by applying minimum and maximum age constraints if valid.
        /// </summary>
        public void OnGetSearchFilterSetAge()
        {
            if (MinAge > MaxAge)
            {
                ModelState.AddModelError(string.Empty, "Invalid Age Range: Min Age cannot be greater than Max Age");
                return;
            }

            OnGetSearchFilterMinAge();
            OnGetSearchFilterMaxAge();
        }

        /// <summary>
        /// Filters players based on minimum age if provided.
        /// </summary>
        public void OnGetSearchFilterMinAge()
        {
            if (MinAge.HasValue)
            {
                SelectedPlayerList = SelectedPlayerList
                .Where(player => player.Age >= MinAge.Value);
            }
        }

        /// <summary>
        /// Filters players based on maximum age if provided.
        /// </summary>
        public void OnGetSearchFilterMaxAge()
        {
            if (MaxAge.HasValue)
            {
                SelectedPlayerList = SelectedPlayerList
                .Where(player => player.Age <= MaxAge.Value);
            }
        }

        /// <summary>
        /// Sets the Appearance filter by applying minimum and maximum appearance constraints if valid.
        /// </summary>
        public void OnGetSearchFilterSetAppearance()
        {
            if (MinAppearance > MaxAppearance)
            {
                ModelState.AddModelError(string.Empty, "Invalid Appearance Range: Min Appearances cannot be greater than Max Appearances");
                return;
            }

            OnGetSearchFilterMinAppearance();
            OnGetSearchFilterMaxAppearance();
        }

        /// <summary>
        /// Filters players based on minimum number of appearances if provided.
        /// </summary>
        public void OnGetSearchFilterMinAppearance()
        {
            if (MinAppearance.HasValue)
            {
                SelectedPlayerList = SelectedPlayerList
                   .Where(player => player.Appearances >= MinAppearance.Value);
            }
        }

        /// <summary>
        /// Filters players based on maximum number of appearances if provided.
        /// </summary>
        public void OnGetSearchFilterMaxAppearance()
        {
            if (MaxAppearance.HasValue)
            {
                SelectedPlayerList = SelectedPlayerList
                    .Where(player => player.Appearances <= MaxAppearance.Value);
            }
        }

        /// <summary>
        /// Filters players based on their position.
        /// </summary>
        public void OnGetSearchFilterSetPosition()
        {
            // Try to parse the string to an enum value
            Enum.TryParse(Position, true, out PositionEnum parsedPosition);

            if (parsedPosition.Equals(PositionEnum.Undefined))
            {
                return;
            }

            SelectedPlayerList = SelectedPlayerList
                .Where(player => player.Position == parsedPosition);
        }

        /// <summary>
        /// Filters players based on their height range provided as a string (e.g., "170-180").
        /// </summary>
        public void OnGetSearchFilterSetHeight()
        {
            if (string.IsNullOrEmpty(HeightInCM))
            {
                return;
            }

            // Split the height range string into min and max parts
            string[] heightParts = HeightInCM.Split('-');

            // Parse minimum height values
            int.TryParse(heightParts[0], out int parsedMinHeight);

            // Parse maximum height values
            int.TryParse(heightParts[1], out int parsedMaxHeight);

            SelectedPlayerList = SelectedPlayerList
              .Where(player => player.HeightInCMs <= parsedMaxHeight);
            SelectedPlayerList = SelectedPlayerList
           .Where(player => player.HeightInCMs >= parsedMinHeight);
        }

        /// <summary>
        /// Sets up leaderboard snippets
        /// </summary>
        public void SetPlayerLeaderboardSnippet()
        {
            // Get the top 5 goal scorers
            IEnumerable<ProductModel> topGoalScorers = PlayerService
                .GetAllData()
                .OrderByDescending(p => p.Goals)
                .Take(5);

            // Get the top 5 players with the most assists
            IEnumerable<ProductModel> topAssisters = PlayerService
                .GetAllData()
                .OrderByDescending(p => p.Assists)
                .Take(5);

            // Get the top 5 players with the most saves
            IEnumerable<ProductModel> topSaves = PlayerService
                .GetAllData()
                .OrderByDescending(p => p.Saves)
                .Take(5);

            // Get the top 5 players with the most clean sheets
            IEnumerable<ProductModel> topCleanSheets = PlayerService
                .GetAllData()
                .OrderByDescending(p => p.CleanSheets)
                .Take(5);

            // Initialize the leaderboard snippet criteria list
            LeaderboardSnippetCriterias = new List<LeaderboardSnippetModel>
            {
                new()
                {
                        Title = "Top Goal Scorers",
                        Players = topGoalScorers,
                        ShowMoreUrl = "/leaderboard/goalsleaderboard",
                        DisplayStat = "Goals"
                },
                new()
                    {
                        Title = "Most Assists",
                        Players = topAssisters,
                        ShowMoreUrl = "/leaderboard/assistsleaderboard",
                        DisplayStat = "Assists"
                    },
                new() {
                        Title = "Most Saves",
                        Players = topSaves,
                        ShowMoreUrl = "/leaderboard/savesleaderboard",
                        DisplayStat = "Saves"
                    },
                new() {
                        Title = "Most Clean Sheets",
                        Players = topCleanSheets,
                        ShowMoreUrl = "/leaderboard/cleansheetsleaderboard",
                        DisplayStat = "CleanSheets"
                    }
            };
        }

    }
}