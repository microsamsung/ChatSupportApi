using ChatSupportApi.DTO;
using ChatSupportApi.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace ChatSupportApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : Controller
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateChatSession([FromBody] ChatRequest request)
        {
            var chatSession = await _chatService.CreateChatSessionAsync(request.RequestedBy);
            return Ok(chatSession);
        }

        [HttpPost("Status")]
        public async Task<IActionResult> Poll(int sessionId)
        {
            var session = await _chatService.GetSession(sessionId);
            if (session == null)
            {
                return NotFound();
            }

            return Ok(session);
        }
    }
}
