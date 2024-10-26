using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContosoCrafts.WebSite.Pages
{
    public class ContactUsModel : PageModel
    {
        public string PhoneNumber { get; } = "(123) 456-7890";
        public string Email { get;} = "info@dribblekings.com";
        public string AddressLine1 { get;} = "Seattle University";
        public string City { get;} = "Seattle";
        public string State { get;} = "WA";
    }
}
