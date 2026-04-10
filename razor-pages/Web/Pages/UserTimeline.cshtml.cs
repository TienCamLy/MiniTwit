using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Core.DTOs;
using Core.Interfaces;

namespace Web.Pages;

public class UserTimelineModel : PageModel
{
    private readonly IMessageRepository _messageRepository;
    private readonly IUserRepository _userRepository;
    private readonly IFollowerRepository _followerRepository;
    
    public IEnumerable<MessageDTO> Messages { get; set; } = new List<MessageDTO>();
    public string Username { get; set; } = string.Empty;
    public bool Followed { get; set; } = false;
    public string? Error { get; set; }

    [FromQuery(Name = "page")]
    public int PageNumber { get; set; } = 1;
    public int TotalMessages { get; set; }
    public int TotalPages { get; set; }
    
    public UserTimelineModel(
        IMessageRepository messageRepository, 
        IUserRepository userRepository, 
        IFollowerRepository followerRepository)
    {
        _messageRepository = messageRepository;
        _userRepository = userRepository;
        _followerRepository = followerRepository;
    }
    
    public void OnGet(string user, string? error = null)
    {
        var userObj = _userRepository.GetUserByUsername(user);
        if (userObj != null)
        {
            Username = user;
            Messages = _messageRepository.GetUserTimelinePage(userObj.UserName, PageNumber);

            TotalMessages = _messageRepository.GetUserTimelineCount(userObj.UserName);
            TotalPages = (int)Math.Ceiling((double)TotalMessages / 10);

            if (User.Identity?.IsAuthenticated == true && !string.IsNullOrEmpty(User.Identity.Name))
            {
                var sessionUser = _userRepository.GetUserByUsername(User.Identity.Name);
                Followed = sessionUser != null &&
                           _followerRepository.IsFollowed(sessionUser.Id, userObj.Id);
            }
            else
            {
                Followed = false;
            }
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
                if (_followerRepository.IsFollowed(who.Id, whom.Id))
                {
                    _followerRepository.UnfollowUser(who.Id, whom.Id);
                    TempData["FlashMessage"] = $"You are no longer following \"{whom.UserName}\"";   
                }
                else
                {
                    _followerRepository.FollowUser(who.Id, whom.Id);
                    TempData["FlashMessage"] = $"You are now following \"{whom.UserName}\"";
                }
                return Redirect($"/{user}");
            }
        }
        return Redirect($"/{user}/{Error}");
    }
}
