using Bl.Api;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IClientBl _clientBl;

        public ClientController(IClientBl clientBl)
        {
            _clientBl = clientBl;
        }

        // GET api/client/getAllClients
        [HttpGet("getAllClients")]
        public ActionResult<List<Bl.Models.Client>> GetAllClients()
        {
            return Ok(_clientBl.GetAllClients());
        }

        // GET api/client/getClientById/{id}
        [HttpGet("getClientById/{id}")]
        public ActionResult<Bl.Models.Client> GetClientById(int id)
        {
            var client = _clientBl.GetClientById(id);
            if (client == null)
                return NotFound();
            return Ok(client);
        }

        // POST api/client/createClient
        [HttpPost("createClient")]
        public IActionResult CreateClient([FromBody] Bl.Models.Client client)
        {
            _clientBl.CreateClient(client);
            return CreatedAtAction(nameof(GetClientById), new { id = client.ClientId }, client);
        }

        // DELETE api/client/deleteClient/{id}
        [HttpDelete("deleteClient/{id}")]
        public IActionResult DeleteClient(int id)
        {
            var client = _clientBl.GetClientById(id);
            if (client == null)
                return NotFound();

            _clientBl.DeleteClient(client);
            return NoContent();
        }
    }
}

