using System;
using System.Collections.Generic;

namespace CallLogging.Data.Models;

public partial class Priority
{
    public int Id { get; set; }

    public string PriorityName { get; set; } = null!;

    public int SortOrder { get; set; }

    public virtual ICollection<Log> Logs { get; set; } = new List<Log>();
}
