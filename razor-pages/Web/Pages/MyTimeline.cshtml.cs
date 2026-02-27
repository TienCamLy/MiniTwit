using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Core.DTOs;

namespace Web.Pages;

public class MyTimelineModel : PageModel
{
    private readonly IMessageRepository _messageRepository;
    private readonly IUserRepository _userRepository;
    private readonly IFollowerRepository _followerRepository;
    
    public IEnumerable<Message> Messages { get; set; } = new List<Message>();
    [BindProperty]
    public string Text { get; set; } = "";
    private int UserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    
    public MyTimelineModel(
        MessageRepository messageRepository, 
        UserRepository userRepository, 
        FollowerRepository followerRepository)
    {
        _messageRepository = messageRepository;
        _userRepository = userRepository;
        _followerRepository = followerRepository;
    }
    public IActionResult OnGet()
    {
        if (User.Identity?.IsAuthenticated != true || User.Identity.Name == null) return Redirect("/public");
        
        Messages = _messageRepository.GetOwnTimeline(UserId);
        return Page();
    }
    
    public IActionResult OnPostCreateMessage()
    {
        _messageRepository.CreateMessage(UserId, Text);
        TempData["FlashMessage"] = "Your message was recorded";
        return RedirectToPage();
    }
}