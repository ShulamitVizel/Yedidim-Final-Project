using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bl.Models
{
    public partial class Client
    {
       
        public int ClientId { get; set; }
        [RegularExpression(@"^[a-zA-Z]+$",ErrorMessage ="Only letters are allowed")]
        public string Name { get; set; } = null!;
        [Phone]
        public string PhoneNumber { get; set; } = null!;

        public virtual ICollection<Call> Calls { get; set; } = new List<Call>();
    }
}
