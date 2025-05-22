using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bl.Models
{
    public partial class Volunteer
    {
       
        public int VolunteerId { get; set; }
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only letters are allowed")]
        public string Name { get; set; } = null!;

        public string Level { get; set; } = null!;

        public bool IsAvailable { get; set; }
        [Phone]
        public string PhoneNumber { get; set; } = null!;

        public string VolunteerLocation { get; set; } = null!;

        public virtual ICollection<Call> Calls { get; set; } = new List<Call>();

        public virtual ICollection<Call> CallsNavigation { get; set; } = new List<Call>();

    }
}
