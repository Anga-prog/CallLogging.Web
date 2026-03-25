using System;
using System.Collections.Generic;

namespace CallLogging.Data.Models;

public partial class LogStatus
{
    public int Id { get; set; }

    public string StatusName { get; set; } = null!;

    public bool IsFinal { get; set; }

    public virtual ICollection<LogUpdate> LogUpdates { get; set; } = new List<LogUpdate>();

    public virtual ICollection<Log> Logs { get; set; } = new List<Log>();
}
