using Microsoft.AspNetCore.Http;

namespace CallLogging.Services.DTOs
{
    // DTOs/LogUpdateCreateDto.cs (for adding updates/comments)
    public class LogUpdateCreateDto
    {
        public int CallLogId { get; set; }
        public string Description { get; set; } = null!;
        public int? StatusId { get; set; }
        public int? TimeSpentMinutes { get; set; }
        public IFormFile? Attachment { get; set; }
    }
}
