using Microsoft.AspNetCore.Mvc.RazorPages;
using Core.DTOs;
using Infrastructure.Repositories;

namespace Web.Pages;

public class PublicTimelineModel : PageModel
{
    private readonly IMessageRepository _messageRepository;
    public IEnumerable<MessageDTO> Messages { get; set; } = new List<MessageDTO>();
    public PublicTimelineModel(MessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }
    public void OnGet()
    {
        Messages = _messageRepository.GetPublicTimeline(30);
    }
}