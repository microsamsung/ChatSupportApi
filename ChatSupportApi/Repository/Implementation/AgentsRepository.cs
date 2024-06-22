using ChatSupportApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatSupportApi.Repository.Implementation
{
    public class AgentsRepository : IAgentsRepository
    {
        private readonly AppDbContext _context;

        public AgentsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Agent>> GetAvailableAgentsAsync()
        {
            return await _context.Agents.Where(a => a.IsAvailable).ToListAsync();
        }

        public async Task<Agent> GetAgentByIdAsync(int id)
        {
            return await _context.Agents.FindAsync(id);
        }

        public async Task<IEnumerable<Agent>> GetOverflowAgentsAsync()
        {
            // Assuming overflow agents are marked in a specific way (e.g., a specific flag or shift)
            return await _context.Agents.Where(a => a.IsAvailableDuringPeakHours).ToListAsync();
        }
    }
}
