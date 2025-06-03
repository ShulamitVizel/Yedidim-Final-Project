using Dal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bl.Models
{
    public partial class Call
    {

        public int CallId { get; set; }

        public DateTime CallTime { get; set; }
        [StringLength(10)]
        public int ClientId { get; set; }
        [StringLength(10)]
        public int FinalVolunteerId { get; set; }

        public string CallType { get; set; } = null!;

        public string CallLocation { get; set; } = null!;

        public virtual Client Client { get; set; } = null!;

        public virtual Volunteer FinalVolunteer { get; set; } = null!;

        public virtual ICollection<Volunteer> Volunteers { get; set; } = new List<Volunteer>();

    }
}
