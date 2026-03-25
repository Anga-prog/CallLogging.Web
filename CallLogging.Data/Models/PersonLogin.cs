using System;
using System.Collections.Generic;

namespace CallLogging.Data.Models;

public partial class PersonLogin
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

    public int IsStaff { get; set; }

    public int IsClient { get; set; }
}
