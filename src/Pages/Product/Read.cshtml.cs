using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoCrafts.WebSite.Pages.Product
{
    /// <summary>
    /// Page model for the Read Page
    /// </summary>
    public class ReadModel : PageModel
    {
        // Id of the player to be displayed
        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }
    }
}

