using Bl.Api;
using Dal.Models;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Post([FromBody] Client client)
        {
            await _bl.CreateClientAsync(client);
            return CreatedAtAction(nameof(Get), new { id = client.ClientId }, client);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _bl.DeleteClientAsync(id);
            return NoContent();
        }
    }

}

