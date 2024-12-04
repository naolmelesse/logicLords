using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace ContosoCrafts.WebSite.Pages.Product
{
    /// <summary>
    /// Class for CreateModel of create page
    /// </summary>
    public class CreateModel : PageModel
    {
        // Logger instance for logging errors and information
        private readonly ILogger<CreateModel> _logger;

        //A list for getting the postion enums 
        public SelectList PositionOptions { get; set; }

        //The player to be created. 
        [BindProperty]
        public ProductModel Player { get; set; }

        // The service used to handle player data operations.
        public JsonFileProductService PlayerService { get; }

        /// <summary>
        /// Constructor for the CreateModel class.
        /// Initializes the PlayerService used to manage player data.
        /// </summary>
        /// <param name="playerService">The service for handling player data operations.</param>
        /// <param name="logger">Logger instance for logging errors.</param>
        public CreateModel(JsonFileProductService playerService, ILogger<CreateModel> logger)
        {
            PlayerService = playerService;
            _logger = logger;
        }

        /// <summary>
        /// Handles GET requests to initialize the page.
        /// Populates the PositionOptions for the dropdown.
        /// </summary>
        public void OnGet()
        {
            PositionOptions = new SelectList(
                Enum.GetValues(typeof(PositionEnum))
                    .Cast<PositionEnum>()
                    .Where(e => e != PositionEnum.Undefined)
                    .Select(e => new { Value =(int)e, Text = e.DisplayName() }),
                "Value",
                "Text");
        }

        /// <summary>
        /// Handles POST requests to create a product.
        /// Checks if Model's state is valid before using PlayerService.
        /// </summary>
        /// <returns>
        /// Redirects to Index Page if Model is valid and createData is successful, 
        /// else it redirects to the Error page if exception.
        /// </returns>
        public IActionResult OnPost()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    PositionOptions = new SelectList(Enum.GetValues(typeof(PositionEnum))
                        .Cast<PositionEnum>()
                        .Where(e => e != PositionEnum.Undefined)
                        .Select(e => new { Value = (int)e, Text = e.DisplayName() }),
                    "Value",
                    "Text",
                    (int)Player.Position);

                    return Page();
                }

                if (Player == null)
                {
                    throw new Exception("Player data is missing");
                }

                PlayerService.CreateData(Player);

                return RedirectToPage("/Product/Index");
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An error occurred on the Create page during OnPost.");
                TempData["ErrorMessage"] = ex.Message;

                return RedirectToPage("/Error");
            }
        }
    }
}