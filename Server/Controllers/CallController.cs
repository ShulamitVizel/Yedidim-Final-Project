using Dal.Models;
using Bl.Api;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CallController : ControllerBase
{
    private readonly ICallBl _bl;
    public CallController(ICallBl bl) => _bl = bl;

    // GET api/call/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Call>> Get(int id)
    {
        var call = await _bl.GetCallByIdAsync(id);
        return call is null ? NotFound(new { error = "Call not found." }) : Ok(call);
    }

    // POST api/call
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Call call)
    {
        if (call is null)
            return BadRequest(new { error = "Call data is missing." });

        var id = await _bl.CreateCallAsync(call);
        return CreatedAtAction(nameof(Get), new { id }, call);
    }

    // PUT api/call/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] Call call)
    {
        if (call is null || id != call.CallId)
            return BadRequest(new { error = "Invalid call ID or missing data." });

        await _bl.UpdateCallAsync(call);
        return NoContent();
    }

    // DELETE api/call/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _bl.DeleteCallAsync(id);
        return NoContent();
    }

    // POST api/call/{callId}/volunteers/{volunteerId}
    [HttpPost("{callId:int}/volunteers/{volunteerId:int}")]
    public async Task<IActionResult> AssignVolunteer(int callId, int volunteerId)
    {
        try
        {
            await _bl.AssignVolunteerToCallAsync(callId, volunteerId);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    // POST api/call/{callId}/assign-best
    [HttpPost("{callId:int}/assign-best")]
    public async Task<IActionResult> AssignBest(int callId)
    {
        bool assigned = await _bl.AssignNearestVolunteerAsync(callId);
        return assigned ? Ok(new { message = "Volunteer assigned successfully." })
                        : NotFound(new { error = "Call or suitable volunteer not found." });
    }

    // GET api/call/{callId}/matching-volunteers
    [HttpGet("{callId:int}/matching-volunteers")]
    public async Task<IActionResult> GetMatching(int callId)
    {
        var volunteers = await _bl.GetMatchingVolunteersAsync(callId);
        return (volunteers is null || volunteers.Count == 0) ? NotFound(new { error = "No matching volunteers found." })
                                                             : Ok(volunteers);
    }

    // GET api/call/{callId}/eta?volunteerId=5
    [HttpGet("{callId:int}/eta")]
    public async Task<IActionResult> GetEta(int callId, [FromQuery] int volunteerId)
    {
        var eta = await _bl.GetEstimatedArrivalTimeAsync(volunteerId, callId);
        return eta is null ? NotFound(new { error = "Call or volunteer location missing." })
                           : Ok(new { estimatedArrivalMinutes = eta });
    }
}
