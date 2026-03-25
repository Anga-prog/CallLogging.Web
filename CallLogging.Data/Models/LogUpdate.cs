using System;
using System.Collections.Generic;

namespace CallLogging.Data.Models;

public partial class LogUpdate
{
    public int Id { get; set; }

    public int CallLogId { get; set; }

    public int UpdatedById { get; set; }

    public string Description { get; set; } = null!;

    public int? StatusId { get; set; }

    public int? TimeSpentMinutes { get; set; }

    public bool HasAttachment { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();

    public virtual Log CallLog { get; set; } = null!;

    public virtual LogStatus? Status { get; set; }

    public virtual SystemUser UpdatedBy { get; set; } = null!;
}
