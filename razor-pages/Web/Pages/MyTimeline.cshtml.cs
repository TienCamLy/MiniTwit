using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Core.DTOs;
using Core.Interfaces;

namespace Web.Pages;

public class MyTimelineModel : PageModel
{
    private readonly IMessageRepository _messageRepository;
    private readonly IUserRepository _userRepository;
    private readonly IFollowerRepository _followerRepository;
    
    public IEnumerable<MessageDTO> Messages { get; set; } = new List<MessageDTO>();
    
    [BindProperty]
    public string Text { get; set; } = "";
    
    [FromQuery(Name = "page")]
    public int Page { get; set; } = 1;
    
    public int TotalPages { get; set; }
    
    private int UserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    private string Username => User.FindFirst(ClaimTypes.Name)!.Value;
    public MyTimelineModel(
        IMessageRepository messageRepository, 
        IUserRepository userRepository, 
        IFollowerRepository followerRepository)
    {
        _messageRepository = messageRepository;
        _userRepository = userRepository;
        _followerRepository = followerRepository;
    }
    public IActionResult OnGet()
    {
        if (User.Identity?.IsAuthenticated != true || User.Identity.Name == null) return Redirect("/public");

        Messages = _messageRepository.GetUserPrivateTimeline(Username,Page);
        var totalMessages = _messageRepository.GetUserTimelineCount(Username);
        TotalPages = (int)Math.Ceiling((double)totalMessages / 10);

        return Page();
    }
    
    public IActionResult OnPostCreateMessage()
    {
        _messageRepository.CreateMessage(UserId, Text);
        TempData["FlashMessage"] = "Your message was recorded";
        return RedirectToPage();
    }
}