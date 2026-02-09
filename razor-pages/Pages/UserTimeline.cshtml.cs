using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using razor_pages.Structs;

namespace razor_pages.Pages;

public class UserTimelineModel : PageModel
{
    public IEnumerable<Message> Messages { get; set; } = new List<Message>();
    public string Username { get; set; } = string.Empty;
    public bool Followed { get; set; } = false;

    private readonly IDBContext _dbcontext;
    
    public UserTimelineModel(IDBContext dbcontext)
    {
        _dbcontext = dbcontext;
    }
    
    public void OnGet(string user)
    {
        Messages = _dbcontext.GetUserTimeline(30, user);
        Username = user;
        Followed = User.Identity?.Name != null && _dbcontext.IsFollowed(User.Identity.Name, user);
    }
    
    public IActionResult OnPost()
    {
        if (Followed)
        {
            Followed = false;
        }
        else
        {
            Followed = true;
        }
        
        return RedirectToPage("/Public", new { username = Username });
    }
}
