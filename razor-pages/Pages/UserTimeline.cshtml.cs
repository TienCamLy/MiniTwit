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
    public string Error { get; set; }
    
    public UserTimelineModel(IDBContext dbcontext)
    {
        _dbcontext = dbcontext;
    }
    
    public void OnGet(string user, string error)
    {
        Messages = _dbcontext.GetUserTimeline(30, user);
        Username = user;
        if (User.Identity?.IsAuthenticated == true)
        {
            var userObj = _dbcontext.GetUserByUsername(user);
            var sessionUser = _dbcontext.GetUserByUsername(User.Identity.Name);
            Followed = sessionUser != null && userObj != null && _dbcontext.IsFollowed(sessionUser.id, userObj.id);
        } else {
            Followed = false;
        }
        Error = error;
    }
    
    public IActionResult OnPost(string user)
    {
        if (string.IsNullOrEmpty(User.Identity.Name))
            Error = "You must be logged in to follow users";
        else
        {
            var who = _dbcontext.GetUserByUsername(User.Identity.Name);
            var whom = _dbcontext.GetUserByUsername(user);
            if (who == null || whom == null)
                Error = $"User with name '{(who == null ? User.Identity.Name : user)}' not found";
            else
            {
                if (_dbcontext.IsFollowed(who.id, whom.id))
                {
                    _dbcontext.UnfollowUser(who.id, whom.id);
                    TempData["FlashMessage"] = $"You are no longer following \"{whom.name}\"";   
                }
                else
                {
                    _dbcontext.FollowUser(who.id, whom.id);
                    TempData["FlashMessage"] = $"You are now following \"{whom.name}\"";
                }
                return Redirect($"/{user}");
            }
        }
        return Redirect($"/{user}/{Error}");
    }
}
