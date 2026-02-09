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
        var userObj = _dbcontext.GetUserByUsername(user);
        var sessionUser = _dbcontext.GetUserByUsername("a"); // TODO: get logged in user id from current session
        Followed = sessionUser != null && userObj != null && _dbcontext.IsFollowed(sessionUser.id, userObj.id);
        Error = error;
    }
    
    public IActionResult OnPost(string user)
    {
        if (string.IsNullOrEmpty(User.Identity.Name)) // TODO: get logged in user name from current session
            Error = "You must be logged in to follow users";
        else if (string.IsNullOrEmpty(user))
            Error = "You must specify a user to follow";
        else if (user == User.Identity.Name) // TODO: get logged in user name from current session
            Error = "You cannot follow yourself";
        else
        {
            var who = _dbcontext.GetUserByUsername(User.Identity.Name); // TODO: get logged in user name from current session
            var whom = _dbcontext.GetUserByUsername(user);
            if (who == null || whom == null)
                Error = "User not found";
            else
            {
                if (Followed)
                    _dbcontext.UnfollowUser(who.id, whom.id);
                else
                    _dbcontext.FollowUser(who.id, whom.id);
                return Redirect($"/Public/{user}");
            }
        }
        return Redirect($"/Public/{user}/{Error}");
    }
}
