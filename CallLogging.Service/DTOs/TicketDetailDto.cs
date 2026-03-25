namespace CallLogging.Services.DTOs
{
    public class TicketDetailDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Status { get; set; } = null!;
        public int StatusId { get; set; }
        public bool IsFinal { get; set; }
        public string Priority { get; set; } = null!;
        public int PriorityId { get; set; }
        public int ClientProfileId { get; set; }
        public string Product { get; set; } = null!;
        public string CallType { get; set; } = null!;
        public string ClientCompany { get; set; } = null!;
        public string ClientName { get; set; } = null!;
        public string? AssignedTo { get; set; }
        public int? AssignedToId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<LogUpdateItemDto> Updates { get; set; } = new();
    }

    public class LogUpdateItemDto
    {
        public int Id { get; set; }
        public string Description { get; set; } = null!;
        public string? Status { get; set; }
        public int? TimeSpentMinutes { get; set; }
        public string UpdatedBy { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
