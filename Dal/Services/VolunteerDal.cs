using Dal.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Services
{

        public class VolunteerDal
        {
            private readonly dbClass _context;

            public VolunteerDal(dbClass context)
            {
                _context = context;
            }

            public List<Volunteer> GetAllVolunteers()
            {
                return _context.Volunteers.ToList();
            }

            public Volunteer? GetVolunteerById(int id)
            {
                return _context.Volunteers.FirstOrDefault(v => v.VolunteerId == id);
            }

            public async Task<List<Volunteer>> GetAvailableVolunteersAsync()
            {
                return await _context.Volunteers
                    .Where(v => v.IsAvailable)
                    .ToListAsync();
            }

            public void AddVolunteer(Volunteer volunteer)
            {
                _context.Volunteers.Add(volunteer);
                _context.SaveChanges();
            }

            public void DeleteVolunteer(int volunteerId)
            {
                var volunteer = _context.Volunteers
                    .Include(v => v.Calls)
                    .FirstOrDefault(v => v.VolunteerId == volunteerId);

                if (volunteer != null)
                {
                    _context.Volunteers.Remove(volunteer);
                    _context.SaveChanges();
                }
                else
                {
                    throw new Exception("The volunteer was not found in the system.");
                }
            }

            public void UpdateVolunteer(Volunteer volunteer)
            {
                var existingVolunteer = _context.Volunteers
                    .FirstOrDefault(v => v.VolunteerId == volunteer.VolunteerId);

                if (existingVolunteer != null)
                {
                    existingVolunteer.Name = volunteer.Name;
                    existingVolunteer.Level = volunteer.Level;
                    existingVolunteer.IsAvailable = volunteer.IsAvailable;
                    existingVolunteer.PhoneNumber = volunteer.PhoneNumber;
                    existingVolunteer.VolunteerLatitude = volunteer.VolunteerLatitude;
                    existingVolunteer.VolunteerLongitude = volunteer.VolunteerLongitude;

                    _context.SaveChanges();
                }
                else
                {
                    throw new Exception("The volunteer was not found for update.");
                }
            }
        }
    }


