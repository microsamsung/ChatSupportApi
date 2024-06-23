namespace ChatSupportApi.Models
{
    /// <summary>
    /// Agent Class
    /// </summary>
    public class Agent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public SeniorityLevel Seniority { get; set; }
        public bool IsAvailable { get; set; }
        public int Shift { get; set; } // 1, 2, or 3

        public bool IsAvailableDuringPeakHours { get; set; }
    }
}
