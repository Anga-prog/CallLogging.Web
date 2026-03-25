using System;
using System.Collections.Generic;

namespace CallLogging.Data.Models;

public partial class SystemUser
{
    public int Id { get; set; }

    public int PersonId { get; set; }

    public int RoleId { get; set; }

    public virtual ICollection<Log> LogAssignedTos { get; set; } = new List<Log>();

    public virtual ICollection<Log> LogCreatedBies { get; set; } = new List<Log>();

    public virtual ICollection<LogUpdate> LogUpdates { get; set; } = new List<LogUpdate>();

    public virtual Person Person { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
}
