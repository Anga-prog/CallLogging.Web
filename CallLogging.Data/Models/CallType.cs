using System;
using System.Collections.Generic;

namespace CallLogging.Data.Models;

public partial class CallType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Log> Logs { get; set; } = new List<Log>();
}
