using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace ContosoCrafts.WebSite.Pages.Product
{
    /// <summary>
    /// Page model for the Read Page
    /// </summary>
    public class ReadModel : PageModel
    {
        private readonly ILogger<ReadModel> _logger;

        // Data Service
        public JsonFileProductService ProductService { get; }

        // Id of the player to be displayed
        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        /// <summary>
        /// Constructor for ReadModel.
        /// </summary>
        /// <param name="logger">Logger instance for logging errors.</param>
        public ReadModel(JsonFileProductService productService, ILogger<ReadModel> logger)
        {
            ProductService = productService;
            _logger = logger;
        }

        /// <summary>
        /// Handles GET requests to the Read page.
        /// </summary>
        /// <returns>A page result or a redirection to the Error page in case of an exception.</returns>
        public IActionResult OnGet()
        {
            try
            {
                // Validate the Id
                if (string.IsNullOrEmpty(Id))
                {
                    throw new Exception("Player ID is missing.");
                }

                // You can add logic here to retrieve player data based on the Id
                // For example:
                 var player = ProductService.GetAllData().FirstOrDefault(x => x.Id.Equals(Id));
                 if (player == null)
                {
                    throw new Exception($"Player with ID {Id} not found.");
                }

                return Page();
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An error occurred on the Read page.");

                // Store the error message in TempData to display on the Error page
                TempData["ErrorMessage"] = ex.Message;

                // Redirect to the Error page
                return RedirectToPage("/Error");
            }
        }
    }
}
