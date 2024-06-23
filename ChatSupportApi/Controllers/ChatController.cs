using ChatSupportApi.DTO;
using ChatSupportApi.Models;
using ChatSupportApi.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChatSupportApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ChatController : Controller
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost("Create")]
        [SwaggerOperation(Summary = "Creates a new chat session and queues it.")]
        [SwaggerResponse(200, "Chat session created successfully", typeof(ChatSession))]
        public async Task<IActionResult> CreateChatSession([FromBody] ChatRequest request)
        {
            var chatSession = await _chatService.CreateChatSessionAsync(request.RequestedBy);
            return Ok(chatSession);
        }

        [HttpPost("Status")]
        [SwaggerOperation(Summary = "Polls the status of a chat session.")]
        [SwaggerResponse(200, "Chat session status retrieved successfully", typeof(ChatSession))]
        [SwaggerResponse(404, "Chat session not found")]
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
