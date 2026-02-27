using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Core.DTOs;

namespace Web.Pages;

public class UserTimelineModel : PageModel
{
    private readonly MiniTwitContext _miniTwitContext;
    private readonly IMessageRepository _messageRepository;
    private readonly IUserRepository _userRepository;
    private readonly IFollowerRepository _followerRepository;
    
    public IEnumerable<MessageDTO> Messages { get; set; } = new List<MessageDTO>();
    public string Username { get; set; } = string.Empty;
    public bool Followed { get; set; } = false;
    public string? Error { get; set; }
    
    public UserTimelineModel(
        MiniTwitContext miniTwitContext, 
        MessageRepository messageRepository, 
        UserRepository userRepository, 
        FollowerRepository followerRepository)
    {
        _miniTwitContext = miniTwitContext;
        _messageRepository = messageRepository;
        _userRepository = userRepository;
        _followerRepository = followerRepository
    }
    

    
    public void OnGet(string user, string error)
    {
        Messages = _messageRepository.GetUserTimeline(user);
        Username = user;
        if (User.Identity?.IsAuthenticated == true && !string.IsNullOrEmpty(User.Identity.Name))
        {
            var userObj = _userRepository.GetUserByUsername(user);
            var sessionUser = _userRepository.GetUserByUsername(User.Identity.Name);
            Followed = sessionUser != null && userObj != null && _followerRepository.IsFollowed(sessionUser.id, userObj.id);
        } else {
            Followed = false;
        }
        Error = error;
    }
    
    public IActionResult OnPost(string user)
    {
        var identityName = User.Identity?.Name;
        
        if (string.IsNullOrEmpty(identityName))
            Error = "You must be logged in to follow users";
        else
        {
            var who = _userRepository.GetUserByUsername(identityName);
            var whom = _userRepository.GetUserByUsername(user);
            if (who == null || whom == null)
                Error = $"User with name '{(who == null ? identityName : user)}' not found";
            else
            {
                if (_followerRepository.IsFollowed(who.id, whom.id))
                {
                    _followerRepository.UnfollowUser(who.id, whom.id);
                    TempData["FlashMessage"] = $"You are no longer following \"{whom.name}\"";   
                }
                else
                {
                    _followerRepository.FollowUser(who.id, whom.id);
                    TempData["FlashMessage"] = $"You are now following \"{whom.name}\"";
                }
                return Redirect($"/{user}");
            }
        }
        return Redirect($"/{user}/{Error}");
    }
}
