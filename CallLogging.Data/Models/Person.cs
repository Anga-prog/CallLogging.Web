using System;
using System.Collections.Generic;

namespace CallLogging.Data.Models;

public partial class Person
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string? PasswordHash { get; set; }

    public DateTime? LastLoginAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();

    public virtual ClientProfile? ClientProfile { get; set; }

    public virtual SystemUser? SystemUser { get; set; }
}
