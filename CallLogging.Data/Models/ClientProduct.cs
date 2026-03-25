using System;
using System.Collections.Generic;

namespace CallLogging.Data.Models;

public partial class ClientProduct
{
    public int Id { get; set; }

    public int ClientProfileId { get; set; }

    public int ProductId { get; set; }

    public DateTime AssignedAt { get; set; }

    public virtual ClientProfile ClientProfile { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
