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
    public MyTimelineModel(IDBContext dbcontext)
    {
        _dbcontext = dbcontext;
    }
    public IActionResult OnGet()
    {
        if (User.Identity?.IsAuthenticated != true || User.Identity.Name == null) return Redirect("/public");
        
        Messages = _dbcontext.GetUserTimeline(30, User.Identity.Name);
        return Page();

    }
    
    public IActionResult OnPostCreateMessage()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        _dbcontext.CreateMessage(userId, Text);

        Console.WriteLine("text: " + Text);
        return RedirectToPage();
    }

}