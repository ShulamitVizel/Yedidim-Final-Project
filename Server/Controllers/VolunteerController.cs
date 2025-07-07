using Bl.Api;
using Dal.Models;
using Microsoft.AspNetCore.Mvc;
using Server.DTOs;

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
        public async Task<IActionResult> Post([FromBody] VolunteerDto dto)
        {
            try
            {
                Console.WriteLine($"[VolunteerController] Incoming DTO: Name={dto.Name}, Level={dto.Level}, IsAvailable={dto.IsAvailable}, PhoneNumber={dto.PhoneNumber}, Lat={dto.VolunteerLatitude}, Lng={dto.VolunteerLongitude}");
                var v = new Volunteer {
                    Name = dto.Name,
                    Level = dto.Level,
                    IsAvailable = dto.IsAvailable,
                    PhoneNumber = dto.PhoneNumber,
                    VolunteerLatitude = dto.VolunteerLatitude,
                    VolunteerLongitude = dto.VolunteerLongitude
                };
                await _bl.AddVolunteerAsync(v);
                return CreatedAtAction(nameof(Get), new { id = v.VolunteerId }, v);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[VolunteerController] Error: {ex.Message}\n{ex.StackTrace}");
                return StatusCode(500, new { error = ex.Message, stack = ex.StackTrace });
            }
        }

        // PUT /api/volunteer/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] VolunteerDto dto)
        {
            if (id <= 0) return BadRequest("Invalid ID");
            var v = new Volunteer {
                VolunteerId = id,
                Name = dto.Name,
                Level = dto.Level,
                IsAvailable = dto.IsAvailable,
                PhoneNumber = dto.PhoneNumber,
                VolunteerLatitude = dto.VolunteerLatitude,
                VolunteerLongitude = dto.VolunteerLongitude
            };
            await _bl.UpdateVolunteerAsync(v);
            return NoContent();
        }

        // DELETE /api/volunteer/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _bl.DeleteVolunteerAsync(id);
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

    }
}
