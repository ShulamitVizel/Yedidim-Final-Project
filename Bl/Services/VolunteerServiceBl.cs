using Bl.Api;
using Dal.Models;
using Dal.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bl.Services
{
    public class VolunteerServiceBl : IVolunteerBl
    {
        private readonly VolunteerDal _dal;
        public VolunteerServiceBl(VolunteerDal dal) => _dal = dal;

        public async Task<List<Volunteer>> GetAllVolunteersAsync() =>
            await _dal.GetAllVolunteersAsync();

        public async Task<Volunteer?> GetVolunteerByIdAsync(int id) =>
            await _dal.GetVolunteerByIdAsync(id);

        public async Task<List<Volunteer>> GetAvailableVolunteersAsync() =>
            await _dal.GetAvailableVolunteersAsync();

        public async Task AddVolunteerAsync(Volunteer v) =>
            await _dal.AddVolunteerAsync(v);

        public async Task DeleteVolunteerAsync(int id) =>
            await _dal.DeleteVolunteerAsync(id);

        public async Task UpdateVolunteerAsync(Volunteer v) =>
            await _dal.UpdateVolunteerAsync(v);

    }
}

