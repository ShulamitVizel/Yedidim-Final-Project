using Bl.Api;
using Dal.Models;
using Dal.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bl.Services
{
    public class ClientServiceBl : IClientBl
    {
        private readonly ClientDal _clientDal;


        public ClientServiceBl(ClientDal clientDal)
        {
            _clientDal = clientDal;
        }

        public void CreateClient(Models.Client client)
        {
            var dalClient = MapBlToDal(client);
            _clientDal.CreateClient(dalClient);
        }

        public void DeleteClient(Models.Client client)
        {
            var dalClient = MapBlToDal(client);
            _clientDal.DeleteClient(dalClient);
        }

        public List<Models.Client> GetAllClients()
        {
            return _clientDal.GetAllClients().Select(MapDalToBl).ToList();
        }

        public Models.Client? GetClientById(int id)
        {
            var dalClient = _clientDal.GetClientById(id);
            return dalClient == null ? null : MapDalToBl(dalClient);
        }

        // מיפויים בין BL ל-DAL
        private Client MapBlToDal(Models.Client blClient)
        {
            return new Client
            {
                ClientId = blClient.ClientId,
                Name = blClient.Name,
                PhoneNumber = blClient.PhoneNumber
            };
        }

        private Models.Client MapDalToBl(Client dalClient)
        {
            return new Models.Client
            {
                ClientId = dalClient.ClientId,
                Name = dalClient.Name,
                PhoneNumber = dalClient.PhoneNumber
            };
        }
    }

}
