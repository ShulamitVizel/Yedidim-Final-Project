using Dal.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Bl.Api
{
    public interface IClientBl
    {
        Task CreateClientAsync(Client client);
        Task DeleteClientAsync(int clientId);
        Task<List<Client>> GetAllClientsAsync();
        Task<Client?> GetClientByIdAsync(int id);
    }
}
