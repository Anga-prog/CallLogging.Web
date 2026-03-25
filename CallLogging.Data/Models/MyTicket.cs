using System;
using System.Collections.Generic;

namespace CallLogging.Data.Models;

public partial class MyTicket
{
    public int TicketId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string CallType { get; set; } = null!;

    public string Priority { get; set; } = null!;

    public int PrioritySortOrder { get; set; }

    public string Status { get; set; } = null!;

    public bool IsFinal { get; set; }

    public string Product { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int ClientProfileId { get; set; }

    public string? AssignedTo { get; set; }

    public int? UpdateCount { get; set; }

    public DateTime? LastUpdatedAt { get; set; }
}
