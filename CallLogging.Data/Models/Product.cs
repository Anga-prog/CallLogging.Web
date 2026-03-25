using System;
using System.Collections.Generic;

namespace CallLogging.Data.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<ClientProduct> ClientProducts { get; set; } = new List<ClientProduct>();

    public virtual ICollection<Log> Logs { get; set; } = new List<Log>();
}
