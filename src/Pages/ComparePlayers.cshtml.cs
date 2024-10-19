using ContosoCrafts.WebSite.Models;
using ContosoCrafts.WebSite.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace ContosoCrafts.WebSite.Pages
{
    public class ComparePlayersModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public ComparePlayersModel(ILogger<IndexModel> logger,
           JsonFileProductService playerService)
        {
            _logger = logger;
            PlayerService = playerService;
        }

        public JsonFileProductService PlayerService { get; }
        public IEnumerable<ProductModel> PlayerList { get; private set; }
        public IEnumerable<SelectListItem> PlayerDropDownItems { get; private set; }

        public ProductModel SelectedPlayerLeft { get; private set; }
        public ProductModel SelectedPlayerRight { get; private set; }

        public void OnGet()
        {
            PlayerList = PlayerService.GetAllData();
            PlayerDropDownItems = PlayerService.GetAllData().Select(p => new SelectListItem
            {
                Value = p.Id,
                Text = p.Title
            }).ToList();

        }
    }
}
