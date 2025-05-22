using Bl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bl.Api
{
    public interface IVolunteerBl
    {

        List<Volunteer> GetAllVolunteers();
        Volunteer? GetVolunteerById(int id);
        List<Volunteer> GetAvailableVolunteers();
        void AddVolunteer(Volunteer volunteer);
        void DeleteVolunteer(int volunteerId);
        void UpdateVolunteer(Volunteer volunteer);


    }
}
