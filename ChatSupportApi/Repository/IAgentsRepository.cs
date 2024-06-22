using ChatSupportApi.Models;

namespace ChatSupportApi.Repository
{
    public interface IAgentsRepository
    {
        Task<IEnumerable<Agent>> GetAvailableAgentsAsync();
        Task<Agent> GetAgentByIdAsync(int id);
        Task<IEnumerable<Agent>> GetOverflowAgentsAsync();
    }
}
