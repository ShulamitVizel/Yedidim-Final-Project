using Bl.Services;
using Bl.Services;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallController : ControllerBase
    {
        private readonly CallBl _callBl;

        public CallController(CallBl callBl)
        {
            _callBl = callBl;
        }

        // GET api/call/{id}
        [HttpGet("getCall/{id}")]
        public ActionResult<Bl.Models.Call> GetCallById(int id)
        {
            var call = _callBl.GetCallById(id); // הפונקציה הזו צריכה להיות לא אסינכרונית גם ב-BL
            if (call == null)
            {
                return NotFound();
            }
            return Ok(call);
        }


        // POST api/call
        [HttpPost]
        public ActionResult CreateCall([FromBody] Bl.Models.Call call)
        {
            _callBl.CreateCall(call);
            return CreatedAtAction(nameof(GetCallById), new { id = call.CallId }, call);
        }

        // PUT api/call/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateCall(int id, [FromBody] Bl.Models.Call call)
        {
            if (id != call.CallId)
            {
                return BadRequest();
            }
            _callBl.UpdateCall(call);
            return NoContent();
        }

        // DELETE api/call/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteCall(int id)
        {
            _callBl.DeleteCall(id);
            return NoContent();
        }

        // POST api/call/{id}/assign-volunteer
        [HttpPost("{id}/assign-volunteer")]
        public IActionResult AssignVolunteerToCall(int id, [FromBody] int volunteerId)
        {
            _callBl.AssignVolunteerToCall(id, volunteerId);
            return NoContent();
        }
    }

}
