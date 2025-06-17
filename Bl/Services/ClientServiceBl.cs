using Bl.Api;
using Dal.Models;
using Dal.Services;
using System.Collections.Generic;

namespace Bl.Services
{
    public class ClientServiceBl : IClientBl
    {
        private readonly ClientDal _dal;
        public ClientServiceBl(ClientDal dal) => _dal = dal;

        public async Task CreateClientAsync(Client client) =>
            await _dal.CreateClientAsync(client);

        public async Task DeleteClientAsync(int clientId) =>
            await _dal.DeleteClientAsync(clientId);

        public async Task<List<Client>> GetAllClientsAsync() =>
            await _dal.GetAllClientsAsync();

        public async Task<Client?> GetClientByIdAsync(int id) =>
            await _dal.GetClientByIdAsync(id);
    }

}

