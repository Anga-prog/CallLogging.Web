using System;
using System.Collections.Generic;

namespace CallLogging.Data.Models;

public partial class OpenTicketsDashboard
{
    public int TicketId { get; set; }

    public string Title { get; set; } = null!;

    public string CallType { get; set; } = null!;

    public string Priority { get; set; } = null!;

    public int PrioritySortOrder { get; set; }

    public string Status { get; set; } = null!;

    public string Product { get; set; } = null!;

    public string ClientCompany { get; set; } = null!;

    public string ClientName { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int? AgeHours { get; set; }

    public string? AssignedTo { get; set; }

    public int? AssignedToId { get; set; }

    public string CreatedBy { get; set; } = null!;

    public int IsAwaitingClient { get; set; }
}
