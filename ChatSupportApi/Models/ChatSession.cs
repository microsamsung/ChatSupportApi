namespace ChatSupportApi.Models
{
    /// <summary>
    /// Class ChatSession
    /// </summary>
    public class ChatSession
    {
        public int Id { get; set; }
        public string RequestedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public bool IsOverflow { get; set; }
        public DateTime LastPolledTime { get; set; }
        public int? AssignedAgentId { get; set; }
        public Agent AssignedAgent { get; set; }

    }
}
