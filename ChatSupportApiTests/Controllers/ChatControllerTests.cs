using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChatSupportApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatSupportApi.DTO;
using ChatSupportApi.Models;
using ChatSupportApi.Services.Contracts;
using Moq;
using ChatSupportApi.Repository;
using ChatSupportApi.Services;
using Microsoft.EntityFrameworkCore;

namespace ChatSupportApi.Controllers.Tests
{
    [TestClass]
    public class ChatServiceTests
    {
        private Mock<IAgentsRepository> _mockAgentsRepository;
        private AppDbContext _context;
        private IChatService _chatService;

        [TestInitialize]
        public void SetUp()
        {
            _mockAgentsRepository = new Mock<IAgentsRepository>();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "ChatSupportTestDb")
                .Options;

            _context = new AppDbContext(options);

            _chatService = new ChatService(_mockAgentsRepository.Object, _context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [TestMethod]
        public async Task AssignChats_TeamWith1Snr1Jnr_Assigns4ChatsToJnr1ChatToSnr()
        {
            // Arrange
            var agents = new List<Agent>
            {
                new Agent { Id = 1, Seniority = SeniorityLevel.Senior, IsAvailable = true },
                new Agent { Id = 2, Seniority = SeniorityLevel.Junior, IsAvailable = true }
            };

            _mockAgentsRepository.Setup(repo => repo.GetAvailableAgentsAsync())
                .ReturnsAsync(agents);

            for (int i = 0; i < 5; i++)
            {
                await _chatService.CreateChatSessionAsync($"User{i + 1}");
            }

            // Act
            foreach (var session in _context.ChatSessions.ToList())
            {
                await _chatService.AssignAgentToChatAsync(session.Id);
            }
            //var Count = _context.ChatSessions.Count(s => s.AssignedAgentId == 2);
            // Assert
            Assert.AreEqual(4, _context.ChatSessions.Count(s => s.AssignedAgentId == 2));
            Assert.AreEqual(1, _context.ChatSessions.Count(s => s.AssignedAgentId == 1));
        }

        [TestMethod]
        public async Task AssignChats_TeamWith2Jnr1Mid_Assigns3ChatsToEachJnrNoneToMid()
        {
            // Arrange
            var agents = new List<Agent>
            {
                new Agent { Id = 1, Seniority = SeniorityLevel.Junior, IsAvailable = true },
                new Agent { Id = 2, Seniority = SeniorityLevel.Junior, IsAvailable = true },
                new Agent { Id = 3, Seniority = SeniorityLevel.MidLevel, IsAvailable = true }
            };

            _mockAgentsRepository.Setup(repo => repo.GetAvailableAgentsAsync())
                .ReturnsAsync(agents);

            for (int i = 0; i < 6; i++)
            {
                await _chatService.CreateChatSessionAsync($"User{i + 1}");
            }

            // Act
            foreach (var session in _context.ChatSessions.ToList())
            {
                await _chatService.AssignAgentToChatAsync(session.Id);
            }
            var a = _context.ChatSessions.Count(s => s.AssignedAgentId == 1);
            var a1 = _context.ChatSessions.Count(s => s.AssignedAgentId == 2);
            var a2 = _context.ChatSessions.Count(s => s.AssignedAgentId == 3);
            // Assert
            Assert.AreEqual(4, _context.ChatSessions.Count(s => s.AssignedAgentId == 1));
            Assert.AreEqual(2, _context.ChatSessions.Count(s => s.AssignedAgentId == 2));
            Assert.AreEqual(0, _context.ChatSessions.Count(s => s.AssignedAgentId == 3));
        }
    }
}