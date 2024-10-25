using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace ContosoCrafts.WebSite.Pages.Product
{
    public class UpdateModel : PageModel
    {

        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        [BindProperty]
        public ProductModel Player { get; set; }

        public JsonFileProductService PlayerService { get; }

        public UpdateModel(JsonFileProductService playerService)
        {
            PlayerService = playerService;
        }
        public void OnGet()
        {
            Player = PlayerService.GetAllData().FirstOrDefault(p => p.Id == Id);
        }

        public IActionResult OnPost()
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            PlayerService.UpdateData(Player);

            return RedirectToPage("/Index");
        }
    }
}
