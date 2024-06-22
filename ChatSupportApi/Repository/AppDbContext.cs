using ChatSupportApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatSupportApi.Repository
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

        public DbSet<Agent> Agents { get; set; }
        public DbSet<ChatSession> ChatSessions { get; set; }
        //public DbSet<ChatQueue> ChatQueues { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Agent>().HasData(
                new Agent
                {
                    Id = 1,
                    Name = "Senior Test Agent",
                    IsAvailable = true,
                    Seniority = SeniorityLevel.Senior
                },
                new Agent
                {
                    Id = 2,
                    Name = "Junior Test Agent",
                    IsAvailable = true,
                    Seniority = SeniorityLevel.Junior
                }
            );
        }

    }
}
