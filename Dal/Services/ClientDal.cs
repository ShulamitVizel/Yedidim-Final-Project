using Dal.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Services
{
    public class ClientDal
    {
        private readonly dbClass _context;
        public ClientDal(dbClass context)
        {
            _context = context;
        }

        public async Task<List<Client>> GetAllClientsAsync() =>
         await _context.Clients.Include(c => c.Calls).ToListAsync();


        public async Task<Client?> GetClientByIdAsync(int id) =>
    await _context.Clients.Include(c => c.Calls)
                          .FirstOrDefaultAsync(c => c.ClientId == id);


        public async Task CreateClientAsync(Client client)
        {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteClientAsync(int clientId)
        {
            var client = await _context.Clients
                                       .Include(c => c.Calls)
                                       .FirstOrDefaultAsync(c => c.ClientId == clientId);

            if (client is null)
                throw new KeyNotFoundException("Client not found");

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
        }
        //public void UpdateClient (Client client) { }


    
    }
}
