using Bl.Services;
using Bl.Services;
using Dal.Services;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallController : ControllerBase
    {
        private readonly CallServiceBl _callBl;

        public CallController(CallServiceBl callBl)
        {
            _callBl = callBl;
        }

        // GET api/call/getCallById/{id}
        [HttpGet("getCallById/{id}")]
        public ActionResult<Bl.Models.Call> GetCallById(int id)
        {
            var call = _callBl.GetCallById(id);
            if (call == null)
            {
                return NotFound();
            }
            return Ok(call);
        }

        // POST api/call/createCall
        [HttpPost("createCall")]
        public ActionResult CreateCall([FromBody] Bl.Models.Call call)
        {
            _callBl.CreateCall(call);
            return CreatedAtAction(nameof(GetCallById), new { id = call.CallId }, call);
        }

        // PUT api/call/updateCall/{id}
        [HttpPut("updateCall/{id}")]
        public IActionResult UpdateCall(int id, [FromBody] Bl.Models.Call call)
        {
            if (id != call.CallId)
            {
                return BadRequest();
            }
            _callBl.UpdateCall(call);
            return NoContent();
        }

        // DELETE api/call/deleteCall/{id}
        [HttpDelete("deleteCall/{id}")]
        public IActionResult DeleteCall(int id)
        {
            _callBl.DeleteCall(id);
            return NoContent();
        }

        // POST api/call/assignVolunteerToCall/{id}
        [HttpPost("assignVolunteerToCall/{id}")]
        public IActionResult AssignVolunteerToCall(int id, [FromBody] int volunteerId)
        {
            _callBl.AssignVolunteerToCall(id, volunteerId);
            return NoContent();
        }

        [HttpPost("calls/{callId}/assign-best-volunteer")]
        public async Task<IActionResult> AssignBestVolunteer(int callId)
        {
            bool assigned = await _callBl.AssignNearestVolunteerAsync(callId);
            if (assigned)
                return Ok(new { message = "Volunteer assigned successfully" });
            else
                return NotFound(new { message = "Call or suitable volunteer not found" });
        }
        [HttpGet("calls/{callId}/matching-volunteers")]
        public async Task<IActionResult> GetMatchingVolunteers(int callId)
        {
            var call = await _callDal.GetCallByIdAsync(callId);
            if (call == null) return NotFound();

            var volunteers = await _callBl.AssignNearestVolunteerAsync(call);
            return Ok(volunteers);
        }

        [HttpGet("estimated-arrival")]
        public async Task<IActionResult> GetEstimatedArrivalTime([FromQuery] int volunteerId, [FromQuery] int callId)
        {
            var etaMinutes = await _callBl.GetEstimatedArrivalTimeAsync(volunteerId, callId);
            if (etaMinutes == null)
                return NotFound("Call or volunteer not found or missing location");

            return Ok(new { EstimatedArrivalTimeMinutes = etaMinutes });
        }

    }
}

