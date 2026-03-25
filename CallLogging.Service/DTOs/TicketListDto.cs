namespace CallLogging.Services.DTOs
{
    // DTOs/TicketListDto.cs (used in dashboard lists)
    public class TicketListDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string CallType { get; set; } = null!;
        public string Priority { get; set; } = null!;
        public int PrioritySortOrder { get; set; }
        public string Status { get; set; } = null!;
        public bool IsFinal { get; set; }
        public string Product { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? AssignedTo { get; set; }
        public int UpdateCount { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        // Client specific
        public int ClientProfileId { get; set; }
        // Staff specific
        public string ClientCompany { get; set; } = null!;
        public string ClientName { get; set; } = null!;
        public int AgeHours { get; set; }
        public bool IsAwaitingClient { get; set; }
    }
}
