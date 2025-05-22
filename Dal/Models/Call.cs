using System;
using System.Collections.Generic;

namespace Dal.Models;

public partial class Call
{
    public int CallId { get; set; }

    public DateTime CallTime { get; set; }

    public int ClientId { get; set; }

    public int FinalVolunteerId { get; set; }

    public string CallType { get; set; } = null!;

    public double CallLatitude { get; set; }

    public double CallLongitude { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual Volunteer FinalVolunteer { get; set; } = null!;

    public virtual ICollection<Volunteer> Volunteers { get; set; } = new List<Volunteer>();
}
