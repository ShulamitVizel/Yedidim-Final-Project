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

        public List<Client> GetAllClients()
        {
            return _context.Clients.Include(c => c.Calls).ToList();
        }

        public Client? GetClientById(int id)
        {
            return _context.Clients.Include(c => c.Calls)
                                   .FirstOrDefault(c => c.ClientId == id);
        }
        public void CreateClient (Client client) 
        {
            _context.Clients.Add(client);
            _context.SaveChanges();

        }
        public void DeleteClient (Client client) {
            var client1 = _context.Clients.Include(c => c.Calls)
                         .FirstOrDefault(c => c.ClientId == client.ClientId);

            if (client != null)
            {
                _context.Clients.Remove(client1);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("The client was not found in the system.");
            }
        }
        //public void UpdateClient (Client client) { }

        
    }
}
