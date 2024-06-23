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
        private readonly ILogger<ChatController> _logger;

        public ChatController(IChatService chatService, ILogger<ChatController> logger)
        {
            _chatService = chatService;
            _logger = logger;
        }

        /// <summary>
        /// Method to Creates a new chat session and queues it
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        [SwaggerOperation(Summary = "Creates a new chat session and queues it.")]
        [SwaggerResponse(200, "Chat session created successfully", typeof(ChatSession))]
        [SwaggerResponse(400, "Invalid request")]
        [SwaggerResponse(500, "Internal server error")]        
        public async Task<IActionResult> CreateChatSession([FromBody] ChatRequest request)
        {
            try
            {
                if (request == null || string.IsNullOrEmpty(request.RequestedBy))
                {
                    _logger.LogWarning("Invalid ChatRequest received");
                    return BadRequest("Invalid request");
                }

                var chatSession = await _chatService.CreateChatSessionAsync(request.RequestedBy);
                return Ok(chatSession);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating chat session");
                return StatusCode(500, "An error occurred while creating the chat session");
            }
        }

        /// <summary>
        /// Method to Polls the status of a chat session
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        /// 
        [HttpPost("Status")]
        [SwaggerOperation(Summary = "Polls the status of a chat session.")]
        [SwaggerResponse(200, "Chat session status retrieved successfully", typeof(ChatSession))]
        [SwaggerResponse(404, "Chat session not found")]
        [SwaggerResponse(500, "Internal server error")]
        public async Task<IActionResult> Poll(int sessionId)
        {
            try
            {
                var session = await _chatService.GetSession(sessionId);
                if (session == null)
                {
                    _logger.LogWarning($"Chat session with ID {sessionId} not found");
                    return NotFound();
                }

                return Ok(session);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while polling chat session with ID {sessionId}");
                return StatusCode(500, "An error occurred while retrieving the chat session status");
            }
        }
    }
}
