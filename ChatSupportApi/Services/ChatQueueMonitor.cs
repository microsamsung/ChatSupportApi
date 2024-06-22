using ChatSupportApi.Repository;
using ChatSupportApi.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace ChatSupportApi.Services
{
    public class ChatQueueMonitor : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private const int PollInterval = 1000; // Polling interval in milliseconds
        private const int MaxInactivePolls = 3; // Maximum allowed inactive polls

        public ChatQueueMonitor(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    //var chatSessions = await context.ChatSessions.Where(cs => cs.IsActive).ToListAsync();
                    // Implement logic to monitor and assign chat sessions

                    var chatService = scope.ServiceProvider.GetRequiredService<IChatService>();
                    await MonitorQueue(context, chatService);
                }
                await Task.Delay(1000, stoppingToken); // Poll every second
            }
        }

        private async Task MonitorQueue(AppDbContext context, IChatService chatService)
        {
            // Get active chat sessions
            var activeChatSessions = await context.ChatSessions
                .Where(cs => cs.IsActive)
                .ToListAsync();

            foreach (var session in activeChatSessions)
            {
                if (DateTime.UtcNow - session.LastPolledTime > TimeSpan.FromSeconds(PollInterval * MaxInactivePolls / 1000))
                {
                    // Mark session as inactive if it hasn't received poll requests
                    session.IsActive = false;
                }
            }

            // Get chat sessions that are waiting for an agent
            var waitingChatSessions = activeChatSessions
                .Where(cs => cs.AssignedAgentId == null)
                .OrderBy(cs => cs.CreatedAt)
                .ToList();

            foreach (var session in waitingChatSessions)
            {
                try
                {
                    await chatService.AssignAgentToChatAsync(session.Id);
                }
                catch (InvalidOperationException)
                {
                }
            }

            await context.SaveChangesAsync();
        }
    }
}
