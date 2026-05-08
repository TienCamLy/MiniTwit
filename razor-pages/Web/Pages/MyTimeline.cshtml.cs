using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Core.DTOs;
using Core.Interfaces;

namespace Web.Pages;

public class MyTimelineModel : PageModel
{
    private readonly IMessageRepository _messageRepository;

    private const int MessagesPerPage = 10;

    public IEnumerable<MessageDTO> Messages { get; set; } = new List<MessageDTO>();

    [BindProperty]
    public string Text { get; set; } = "";

    [FromQuery(Name = "page")]
    public int PageNumber { get; set; } = 1;
    public int TotalMessages { get; set; }
    public int TotalPages { get; set; }

    private int UserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    private string Username => User.FindFirst(ClaimTypes.Name)!.Value;
    public MyTimelineModel(IMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }
    public IActionResult OnGet()
    {
        if (User.Identity?.IsAuthenticated != true || User.Identity.Name == null) return Redirect("/public");

        Messages = _messageRepository.GetMyTimeline(UserId, PageNumber);

        TotalMessages = _messageRepository.GetMyTimelineCount(UserId);
        TotalPages = (int)Math.Ceiling((double)TotalMessages / MessagesPerPage);

        return Page();
    }

    public IActionResult OnPostCreateMessage()
    {
        if (string.IsNullOrEmpty(Text)) return RedirectToPage();
        try
        {
            _messageRepository.CreateMessage(UserId, Text);
            TempData["FlashMessage"] = "Your message was recorded";
        }
        catch (ArgumentException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }
        
        return RedirectToPage();
    }
}