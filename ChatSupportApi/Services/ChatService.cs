using ChatSupportApi.Models;
using ChatSupportApi.Repository;
using ChatSupportApi.Services.Contracts;
using ChatSupportApi.Utility;
using Microsoft.EntityFrameworkCore;

namespace ChatSupportApi.Services
{
    public class ChatService : IChatService
    {
        private readonly IAgentsRepository _agentsRepository;
        private readonly AppDbContext _context;

        public ChatService(IAgentsRepository agentsRepository, AppDbContext context)
        {
            _agentsRepository = agentsRepository;
            _context = context;
        }

        public async Task<ChatSession> CreateChatSessionAsync(string requestedBy)
        {
            // Calculate the current capacity and queue size
            var agents = await _agentsRepository.GetAvailableAgentsAsync();
            var totalCapacity = CalculateTotalCapacity(agents);
            var maxQueueSize = (int)(totalCapacity * 1.5);
            var currentQueueSize = await _context.ChatSessions.CountAsync(cs => cs.IsActive && !cs.IsOverflow);

            if (currentQueueSize >= maxQueueSize)
            {
                if (TimeHelper.IsOfficeHours())
                {
                    var overflowAgents = await _agentsRepository.GetOverflowAgentsAsync();
                    if (overflowAgents.Any())
                    {
                        // Handle overflow
                        return await CreateOverflowChatSessionAsync(requestedBy);
                    }
                }

                // Queue is full and it's not during office hours or no overflow available
                throw new InvalidOperationException("Chat queue is full.");
            }

            var chatSession = new ChatSession
            {
                RequestedBy = requestedBy,
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                IsOverflow = false,
                LastPolledTime = DateTime.UtcNow
            };
            _context.ChatSessions.Add(chatSession);
            await _context.SaveChangesAsync();
            return chatSession;
        }

        private async Task<ChatSession> CreateOverflowChatSessionAsync(string requestedBy)
        {
            var chatSession = new ChatSession
            {
                RequestedBy = requestedBy,
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                IsOverflow = true,
                LastPolledTime = DateTime.UtcNow
            };
            _context.ChatSessions.Add(chatSession);
            await _context.SaveChangesAsync();
            return chatSession;
        }

        public async Task<ChatSession> AssignAgentToChatAsync(int chatSessionId)
        {
            var chatSession = await _context.ChatSessions.FindAsync(chatSessionId);
            if (chatSession == null || !chatSession.IsActive)
            {
                throw new InvalidOperationException("Chat session is invalid or inactive.");
            }

            var availableAgents = await _agentsRepository.GetAvailableAgentsAsync();
            if (!availableAgents.Any())
            {
                throw new InvalidOperationException("No available agents.");
            }

            // Calculate capacity for each agent
            var agentCapacities = availableAgents
                .Select(agent => new
                {
                    Agent = agent,
                    Capacity = CalculateCapacity(agent)
                })
                .OrderBy(a => a.Agent.Seniority) // Order by seniority
                .ThenBy(a => a.Agent.Id) // Ensure consistent ordering
                .ToList();

            // Assign to the first available agent based on capacity and load balancing
            foreach (var agentCapacity in agentCapacities)
            {
                var assignedChatsCount = await _context.ChatSessions
                    .CountAsync(cs => cs.AssignedAgentId == agentCapacity.Agent.Id && cs.IsActive);
                if (assignedChatsCount < agentCapacity.Capacity)
                {
                    chatSession.AssignedAgentId = agentCapacity.Agent.Id;
                    await _context.SaveChangesAsync();
                    return chatSession;
                }
            }

            throw new InvalidOperationException("All agents are at full capacity.");
        }

        private int CalculateCapacity(Agent agent)
        {
            // Concurrency multipliers based on seniority
            var concurrencyMultiplier = agent.Seniority switch
            {
                SeniorityLevel.Junior => 0.4,
                SeniorityLevel.MidLevel => 0.6,
                SeniorityLevel.Senior => 0.8,
                SeniorityLevel.TeamLead => 0.5,
                _ => 0.4
            };

            return (int)(10 * concurrencyMultiplier); // Assuming max concurrency is 10
        }

        private int CalculateTotalCapacity(IEnumerable<Agent> agents)
        {
            return agents.Sum(agent => CalculateCapacity(agent));
        }

        public async Task<ChatSession> GetSession(int chatSessionId)
        {
            return await _context.ChatSessions
                .Include(a => a.AssignedAgent)
                .SingleOrDefaultAsync(a => a.Id == chatSessionId);
        }
    }
}
