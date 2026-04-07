using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TrakQ.Web.Pages;

public class IndexModel : PageModel
{
    public IActionResult OnGet() =>
        RedirectToPage("/Reports/Summary");
}
