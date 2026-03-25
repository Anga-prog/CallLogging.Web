using System;
using System.Collections.Generic;

namespace CallLogging.Data.Models;

public partial class AgentMonthlyPerformance
{
    public int SystemUserId { get; set; }

    public string FullName { get; set; } = null!;

    public int? Year { get; set; }

    public int? Month { get; set; }

    public int? TicketsTouched { get; set; }

    public int? UpdatesMade { get; set; }

    public int? TotalTimeMinutes { get; set; }

    public double? AvgMinutesPerUpdate { get; set; }

    public int? TicketsResolved { get; set; }
}
