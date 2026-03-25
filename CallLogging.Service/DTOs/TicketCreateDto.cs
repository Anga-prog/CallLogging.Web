using Microsoft.AspNetCore.Http;

namespace CallLogging.Services.DTOs
{
    // DTOs/TicketCreateDto.cs
    public class TicketCreateDto
    {
        public int ClientProfileId { get; set; }
        public int ProductId { get; set; }
        public int CallTypeId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int PriorityId { get; set; }
        public int? AssignedToId { get; set; }
        public IFormFile? Attachment { get; set; } // optional
    }
}
