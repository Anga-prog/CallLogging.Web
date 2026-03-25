using System;
using System.Collections.Generic;

namespace CallLogging.Data.Models;

public partial class Log
{
    public int Id { get; set; }

    public int ClientProfileId { get; set; }

    public int ProductId { get; set; }

    public int CallTypeId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int PriorityId { get; set; }

    public int StatusId { get; set; }

    public int CreatedById { get; set; }

    public int? AssignedToId { get; set; }

    public int? RelatedToCallId { get; set; }

    public int? MergedIntoCallId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual SystemUser? AssignedTo { get; set; }

    public virtual ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();

    public virtual CallType CallType { get; set; } = null!;

    public virtual ClientProfile ClientProfile { get; set; } = null!;

    public virtual SystemUser CreatedBy { get; set; } = null!;

    public virtual ICollection<Log> InverseMergedIntoCall { get; set; } = new List<Log>();

    public virtual ICollection<Log> InverseRelatedToCall { get; set; } = new List<Log>();

    public virtual ICollection<LogUpdate> LogUpdates { get; set; } = new List<LogUpdate>();

    public virtual Log? MergedIntoCall { get; set; }

    public virtual Priority Priority { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;

    public virtual Log? RelatedToCall { get; set; }

    public virtual LogStatus Status { get; set; } = null!;
}
