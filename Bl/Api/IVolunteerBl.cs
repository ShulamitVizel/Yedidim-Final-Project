using System.Collections.Generic;
using System.Threading.Tasks;
using Dal.Models;

namespace Bl.Api
{
    public interface IVolunteerBl
    {
        Task<List<Volunteer>> GetAllVolunteersAsync();
        Task<Volunteer?> GetVolunteerByIdAsync(int id);
        Task<List<Volunteer>> GetAvailableVolunteersAsync();

        Task AddVolunteerAsync(Volunteer volunteer);
        Task DeleteVolunteerAsync(int volunteerId);
        Task UpdateVolunteerAsync(Volunteer volunteer);
    }
}
