using Bl.Api;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VolunteerController : ControllerBase
    {
        private readonly IVolunteerBl _volunteerBl;

        public VolunteerController(IVolunteerBl volunteerBl)
        {
            _volunteerBl = volunteerBl;
        }

        [HttpGet]
        public ActionResult<List<Bl.Models.Volunteer>> GetAllVolunteers()
        {
            return Ok(_volunteerBl.GetAllVolunteers());
        }

        [HttpGet("{id}")]
        public ActionResult<Bl.Models.Volunteer> GetVolunteerById(int id)
        {
            var volunteer = _volunteerBl.GetVolunteerById(id);
            if (volunteer == null)
                return NotFound();
            return Ok(volunteer);
        }

        [HttpGet("available")]
        public ActionResult<List<Bl.Models.Volunteer>> GetAvailableVolunteers()
        {
            return Ok(_volunteerBl.GetAvailableVolunteers());
        }

        [HttpPost]
        public IActionResult AddVolunteer(Bl.Models.Volunteer volunteer)
        {
            _volunteerBl.AddVolunteer(volunteer);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteVolunteer(int id)
        {
            _volunteerBl.DeleteVolunteer(id);
            return Ok();
        }

        [HttpPut]
        public IActionResult UpdateVolunteer(Bl.Models.Volunteer volunteer)
        {
            _volunteerBl.UpdateVolunteer(volunteer);
            return Ok();
        }
    }

}
