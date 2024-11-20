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

        // Gets the list of top 5 players have the most goals.        
        public IEnumerable<ProductModel> TopGoalScorers { get; private set; }

        // Gets the list of top 5 players have the most assists.        
        public IEnumerable<ProductModel> TopAssisters { get; private set; }
        // Gets the list of top 5 players have the most clean sheets.        
        public IEnumerable<ProductModel> TopCleanSheets { get; private set; }

        // Gets the list of top 5 players have the most saves.        
        public IEnumerable<ProductModel> TopSaves { get; private set; }

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
        public string Height { get; set; }

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

            // Filter based on MinAge if value provided that validates if greater than MaxAge when provided
            if (MinAge.HasValue)
            {
                if (MaxAge.HasValue)
                {
                    if (MinAge > MaxAge)
                    {
                        ModelState.AddModelError(string.Empty, "Invalid Age Range: Min Age cannot be greater than Max Age");
                        return;
                    }
                }
                SelectedPlayerList = SelectedPlayerList
                    .Where(player => player.Age >= MinAge.Value);
            }

            // Filter based on MaxAge if value provided
            if (MaxAge.HasValue)
            {
                SelectedPlayerList = SelectedPlayerList
                    .Where(player => player.Age <= MaxAge.Value);
            }

            // Filter based on MinAppearance if value provided that validates if greater than MaxAppearance when provided
            if (MinAppearance.HasValue)
            {
                if (MaxAppearance.HasValue)
                {
                    if (MinAppearance > MaxAppearance)
                    {
                        ModelState.AddModelError(string.Empty, "Invalid Appearance Range: Min Appearances cannot be greater than Max Appearances");
                        return;
                    }
                }
                SelectedPlayerList = SelectedPlayerList
                    .Where(player => player.Appearances >= MinAppearance.Value);
            }


            // Filter based on MaxAppearance if value provided
            if (MaxAppearance.HasValue)
            {
                SelectedPlayerList = SelectedPlayerList
                    .Where(player => player.Appearances <= MaxAppearance.Value);
            }

            // Filter based on Position if value provided
            if (!string.IsNullOrEmpty(Position))
            {
                SelectedPlayerList = SelectedPlayerList
                    .Where(player => player.Position.Equals(Position, StringComparison.OrdinalIgnoreCase));
            }

            // Filter based on Height if value provided
            if (Height != null)
            {
                if (Height.Equals("< 155 cm"))
                {
                    SelectedPlayerList = SelectedPlayerList.Where(player => player.HeightInCMs < 155);
                }
                if (Height.Equals("155 cm - 164 cm"))
                {
                    SelectedPlayerList = SelectedPlayerList.Where(player => player.HeightInCMs >= 155 && player.HeightInCMs <= 164);
                }
                if (Height.Equals("165 cm - 174 cm"))
                {
                    SelectedPlayerList = SelectedPlayerList.Where(player => player.HeightInCMs >= 165 && player.HeightInCMs <= 174);
                }
                if (Height.Equals("175 cm - 184 cm"))
                {
                    SelectedPlayerList = SelectedPlayerList.Where(player => player.HeightInCMs >= 175 && player.HeightInCMs <= 184);
                }
                if (Height.Equals("185 cm - 194 cm"))
                {
                    SelectedPlayerList = SelectedPlayerList.Where(player => player.HeightInCMs >= 185 && player.HeightInCMs <= 194);
                }
                if (Height.Equals("> 195 cm"))
                {
                    SelectedPlayerList = SelectedPlayerList.Where(player => player.HeightInCMs > 195);
                }
            }


            SelectedPlayerList = SelectedPlayerList.ToList();

        }

        /// <summary>
        /// Sets up leader board snippets
        /// </summary>
        public void SetPlayerLeaderboardSnippet()
        {
            TopGoalScorers = PlayerService.GetAllData().OrderByDescending(p => p.Goals).Take(5);
            TopAssisters = PlayerService.GetAllData().OrderByDescending(p => p.Assists).Take(5);
            TopSaves = PlayerService.GetAllData().OrderByDescending(p => p.Saves).Take(5);
            TopCleanSheets = PlayerService.GetAllData().OrderByDescending(p => p.CleanSheets).Take(5);
        }

    }
}