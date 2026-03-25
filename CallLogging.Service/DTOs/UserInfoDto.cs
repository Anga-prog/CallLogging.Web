namespace CallLogging.Services.DTOs
{
    // DTOs/UserInfoDto.cs
    public class UserInfoDto
    {
        public int PersonId { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool IsStaff { get; set; }
        public int? SystemUserId { get; set; }
        public string? RoleName { get; set; }
        public bool IsClient { get; set; }
        public int? ClientProfileId { get; set; }
        public string? Company { get; set; }
    }
}
