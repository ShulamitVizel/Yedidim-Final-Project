using Dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bl.Api
{
    public interface IClientBl
    {
        void CreateClient(Models.Client client);
        void DeleteClient(Models.Client client);
        List<Models.Client> GetAllClients();
        Models.Client? GetClientById(int id);
    }

}
