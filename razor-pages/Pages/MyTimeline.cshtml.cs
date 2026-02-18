using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using razor_pages.Structs;

namespace razor_pages.Pages;

public class MyTimelineModel : PageModel
{
    public IEnumerable<Message> Messages { get; set; } = new List<Message>();
    [BindProperty]
    public string Text { get; set; } = "";

    private readonly IDBContext _dbcontext;
    private int UserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    public MyTimelineModel(IDBContext dbcontext)
    {
        _dbcontext = dbcontext;
    }
    public IActionResult OnGet()
    {
        if (User.Identity?.IsAuthenticated != true || User.Identity.Name == null) return Redirect("/public");
        
        Messages = _dbcontext.GetOwnTimeline(30, UserId);
        return Page();
    }
    
    public IActionResult OnPostCreateMessage()
    {
        _dbcontext.CreateMessage(UserId, Text);
        TempData["FlashMessage"] = "Your message was recorded";
        return RedirectToPage();
    }
}