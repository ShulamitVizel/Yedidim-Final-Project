using System;
using System.Collections.Generic;

namespace Dal.Models;

public partial class Client
{
    public int ClientId { get; set; }

    public string Name { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public virtual ICollection<Call> Calls { get; set; } = new List<Call>();
}
