using ChatSupportApi.Models;

namespace ChatSupportApi.Services.Contracts
{
    public interface IChatService
    {
        Task<ChatSession> CreateChatSessionAsync(string requestedBy);
        Task<ChatSession> AssignAgentToChatAsync(int chatSessionId);

        Task<ChatSession> GetSession(int chatSessionId);
    }
}
