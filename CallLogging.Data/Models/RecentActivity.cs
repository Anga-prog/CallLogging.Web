using System;
using System.Collections.Generic;

namespace CallLogging.Data.Models;

public partial class RecentActivity
{
    public int SystemUserId { get; set; }

    public string FullName { get; set; } = null!;

    public int TicketId { get; set; }

    public string TicketTitle { get; set; } = null!;

    public string? StatusAfterUpdate { get; set; }

    public string UpdateDescription { get; set; } = null!;

    public int? TimeSpentMinutes { get; set; }

    public DateTime ActivityAt { get; set; }

    public int? MinutesAgo { get; set; }
}
