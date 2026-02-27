using Microsoft.AspNetCore.Mvc.RazorPages;
using Core.DTOs;
using Core.Interfaces;

namespace Web.Pages;

public class PublicTimelineModel : PageModel
{
    private readonly IMessageRepository _messageRepository;
    public IEnumerable<MessageDTO> Messages { get; set; } = new List<MessageDTO>();
    public PublicTimelineModel(IMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }
    public void OnGet()
    {
        Messages = _messageRepository.GetPublicTimeline();
    }
}