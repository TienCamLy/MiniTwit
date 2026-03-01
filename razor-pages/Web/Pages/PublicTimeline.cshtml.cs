using Microsoft.AspNetCore.Mvc.RazorPages;
using Core.DTOs;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Web.Pages;

public class PublicTimelineModel : PageModel
{
    private readonly IMessageRepository _messageRepository;
    private const int _messagesPerPage = 10;

    public IEnumerable<MessageDTO> Messages { get; set; } = new List<MessageDTO>();

    [FromQuery(Name = "page")]
    public int Page { get; set; } = 1; 
    public int TotalMessages { get; set; }
    public int TotalPages { get; set; }

    public PublicTimelineModel(IMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }

    public void OnGet()
    {
        Messages = _messageRepository.GetPublicTimeline(Page);

        TotalMessages = _messageRepository.GetPublicTimelineCount();
        TotalPages = (int)Math.Ceiling((double)TotalMessages / _messagesPerPage);
    }
}