namespace CallLogging.Models
{
    public class PersonLogin
    {
        public int PersonId { get; set; }
        public string Email { get; set; } = null!;
        public string? PasswordHash { get; set; }
        public string FullName { get; set; } = null!;
        public bool IsActive { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public int? SystemUserId { get; set; }
        public int? RoleId { get; set; }
        public string? RoleName { get; set; }
        public int? ClientProfileId { get; set; }
        public string? Company { get; set; }
        public bool IsStaff { get; set; }
        public bool IsClient { get; set; }
    }
}
