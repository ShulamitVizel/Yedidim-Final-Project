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

        // GET api/volunteer/getAllVolunteers
        [HttpGet("getAllVolunteers")]
        public ActionResult<List<Bl.Models.Volunteer>> GetAllVolunteers()
        {
            return Ok(_volunteerBl.GetAllVolunteers());
        }

        // GET api/volunteer/getVolunteerById/{id}
        [HttpGet("getVolunteerById/{id}")]
        public ActionResult<Bl.Models.Volunteer> GetVolunteerById(int id)
        {
            var volunteer = _volunteerBl.GetVolunteerById(id);
            if (volunteer == null)
                return NotFound();
            return Ok(volunteer);
        }

        // GET api/volunteer/getAvailableVolunteers
        [HttpGet("getAvailableVolunteers")]
        public ActionResult<List<Bl.Models.Volunteer>> GetAvailableVolunteers()
        {
            return Ok(_volunteerBl.GetAvailableVolunteers());
        }

        // POST api/volunteer/addVolunteer
        [HttpPost("addVolunteer")]
        public IActionResult AddVolunteer([FromBody] Bl.Models.Volunteer volunteer)
        {
            _volunteerBl.AddVolunteer(volunteer);
            return Ok();
        }

        // DELETE api/volunteer/deleteVolunteer/{id}
        [HttpDelete("deleteVolunteer/{id}")]
        public IActionResult DeleteVolunteer(int id)
        {
            _volunteerBl.DeleteVolunteer(id);
            return Ok();
        }

        // PUT api/volunteer/updateVolunteer
        [HttpPut("updateVolunteer")]
        public IActionResult UpdateVolunteer([FromBody] Bl.Models.Volunteer volunteer)
        {
            _volunteerBl.UpdateVolunteer(volunteer);
            return Ok();
        }
    }
}
