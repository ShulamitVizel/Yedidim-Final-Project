using Bl.Api;
using Dal.Models;
using Microsoft.AspNetCore.Mvc;
using Server.DTOs;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IClientBl _bl;
        public ClientController(IClientBl bl) => _bl = bl;

        [HttpGet]
        public async Task<ActionResult<List<Client>>> GetAll() =>
            Ok(await _bl.GetAllClientsAsync());

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Client>> Get(int id)
        {
            var client = await _bl.GetClientByIdAsync(id);
            return client is null ? NotFound() : Ok(client);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ClientDto dto)
        {
            try
            {
                Console.WriteLine($"[ClientController] Incoming DTO: Name={dto.Name}, PhoneNumber={dto.PhoneNumber}");
                var client = new Client {
                    Name = dto.Name,
                    PhoneNumber = dto.PhoneNumber
                };
                await _bl.CreateClientAsync(client);
                return CreatedAtAction(nameof(Get), new { id = client.ClientId }, client);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ClientController] Error: {ex.Message}\n{ex.StackTrace}");
                return StatusCode(500, new { error = ex.Message, stack = ex.StackTrace });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _bl.DeleteClientAsync(id);
            return NoContent();
        }
    }

}

