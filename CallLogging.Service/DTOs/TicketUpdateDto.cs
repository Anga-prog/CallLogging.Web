namespace CallLogging.Services.DTOs
{
    // DTOs/TicketUpdateDto.cs
    public class TicketUpdateDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? PriorityId { get; set; }
        public int? StatusId { get; set; }
        public int? AssignedToId { get; set; }
        public int? RelatedToCallId { get; set; }
        public int? MergedIntoCallId { get; set; }
    }
}
