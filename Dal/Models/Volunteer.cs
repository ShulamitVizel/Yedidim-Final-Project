using System;
using System.Collections.Generic;

namespace Dal.Models;

public partial class Volunteer
{
    public int VolunteerId { get; set; }

    public string Name { get; set; } = null!;

    public string Level { get; set; } = null!;

    public bool IsAvailable { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public double VolunteerLatitude { get; set; }

    public double VolunteerLongitude { get; set; }

    public virtual ICollection<Call> Calls { get; set; } = new List<Call>();

    public virtual ICollection<Call> CallsNavigation { get; set; } = new List<Call>();
}
