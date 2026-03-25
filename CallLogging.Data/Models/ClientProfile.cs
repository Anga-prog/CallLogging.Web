using System;
using System.Collections.Generic;

namespace CallLogging.Data.Models;

public partial class ClientProfile
{
    public int Id { get; set; }

    public int PersonId { get; set; }

    public string Company { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<ClientProduct> ClientProducts { get; set; } = new List<ClientProduct>();

    public virtual ICollection<Log> Logs { get; set; } = new List<Log>();

    public virtual Person Person { get; set; } = null!;
}
