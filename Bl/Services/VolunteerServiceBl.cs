using Bl.Api;
using Bl.Models;
using Dal.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Bl.Services
{
    public class VolunteerServiceBl : IVolunteerBl
    {
        private readonly VolunteerDal _volunteerDal;

        public VolunteerServiceBl(VolunteerDal volunteerDal)
        {
            _volunteerDal = volunteerDal;
        }

        public List<Volunteer> GetAllVolunteers()
        {
            var dalVolunteers = _volunteerDal.GetAllVolunteers();
            return dalVolunteers.Select(MapDalToBl).ToList();
        }

        public Volunteer? GetVolunteerById(int id)
        {
            var dalVolunteer = _volunteerDal.GetVolunteerById(id);
            return dalVolunteer == null ? null : MapDalToBl(dalVolunteer);
        }

        public List<Volunteer> GetAvailableVolunteers()
        {
            var dalVolunteers = _volunteerDal.GetAvailableVolunteersAsync().Result;
            return dalVolunteers.Select(MapDalToBl).ToList();
        }

        public void AddVolunteer(Volunteer volunteer)
        {
            var dalVolunteer = MapBlToDal(volunteer);
            _volunteerDal.AddVolunteer(dalVolunteer);
        }

        public void DeleteVolunteer(int volunteerId)
        {
            _volunteerDal.DeleteVolunteer(volunteerId);
        }

        public void UpdateVolunteer(Volunteer volunteer)
        {
            var dalVolunteer = MapBlToDal(volunteer);
            _volunteerDal.UpdateVolunteer(dalVolunteer);
        }

        // מיפוי מ-BL ל-DAL
        private Dal.Models.Volunteer MapBlToDal(Volunteer bl)
        {
            var split = bl.VolunteerLocation.Split(',');
            return new Dal.Models.Volunteer
            {
                VolunteerId = bl.VolunteerId,
                Name = bl.Name,
                PhoneNumber = bl.PhoneNumber,
                Level = bl.Level,
                IsAvailable = bl.IsAvailable,
                VolunteerLatitude = double.Parse(split[0]),
                VolunteerLongitude = double.Parse(split[1])
            };
        }

        // מיפוי מ-DAL ל-BL
        private Volunteer MapDalToBl(Dal.Models.Volunteer dal)
        {
            return new Volunteer
            {
                VolunteerId = dal.VolunteerId,
                Name = dal.Name,
                PhoneNumber = dal.PhoneNumber,
                Level = dal.Level,
                IsAvailable = dal.IsAvailable,
                VolunteerLocation = $"{dal.VolunteerLatitude},{dal.VolunteerLongitude}"
            };
        }
    }

}
