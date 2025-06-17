using Bl.Api;
using Dal.Models;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VolunteerController : ControllerBase
    {
        private readonly IVolunteerBl _bl;

        public VolunteerController(IVolunteerBl bl) => _bl = bl;

        // GET /api/volunteer
        [HttpGet]
        public async Task<ActionResult<List<Volunteer>>> GetAll()
            => Ok(await _bl.GetAllVolunteersAsync());

        // GET /api/volunteer/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Volunteer>> Get(int id)
        {
            var vol = await _bl.GetVolunteerByIdAsync(id);
            return vol is null ? NotFound() : Ok(vol);
        }

        // GET /api/volunteer/available
        [HttpGet("available")]
        public async Task<ActionResult<List<Volunteer>>> GetAvailable()
            => Ok(await _bl.GetAvailableVolunteersAsync());

        // POST /api/volunteer
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Volunteer v)
        {
            await _bl.AddVolunteerAsync(v);
            return CreatedAtAction(nameof(Get), new { id = v.VolunteerId }, v);
        }

        // PUT /api/volunteer/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] Volunteer v)
        {
            if (id != v.VolunteerId) return BadRequest("ID in route and body mismatch");
            await _bl.UpdateVolunteerAsync(v);
            return NoContent();
        }

        // DELETE /api/volunteer/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _bl.DeleteVolunteerAsync(id);
            return NoContent();
        }

    }
}
