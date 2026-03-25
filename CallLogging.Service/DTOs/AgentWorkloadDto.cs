namespace CallLogging.Services.DTOs
{
    // DTOs/AgentWorkloadDto.cs
    public class AgentWorkloadDto
    {
        public int SystemUserId { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
        public int TotalAssigned { get; set; }
        public int OpenTickets { get; set; }
        public int AwaitingClient { get; set; }
        public int HighPlusTickets { get; set; }
        public string WorkloadLevel { get; set; } = null!;
    }
}
