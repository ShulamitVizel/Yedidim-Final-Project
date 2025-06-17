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

        public async Task<List<Volunteer>> GetAllVolunteersAsync() =>
                            await _context.Volunteers.ToListAsync();

        public async Task<Volunteer?> GetVolunteerByIdAsync(int id) =>
          await _context.Volunteers.FirstOrDefaultAsync(v => v.VolunteerId == id);
        public async Task<List<Volunteer>> GetAvailableVolunteersAsync() =>
        await _context.Volunteers.Where(v => v.IsAvailable).ToListAsync();

        public async Task AddVolunteerAsync(Volunteer volunteer)
        {
            _context.Volunteers.Add(volunteer);
            await _context.SaveChangesAsync();
        }


        public async Task DeleteVolunteerAsync(int volunteerId)
        {
            var volunteer = await _context.Volunteers
                                          .Include(v => v.Calls)
                                          .FirstOrDefaultAsync(v => v.VolunteerId == volunteerId);

            if (volunteer is null)
                throw new KeyNotFoundException("Volunteer not found");

            _context.Volunteers.Remove(volunteer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateVolunteerAsync(Volunteer volunteer)
        {
            _context.Volunteers.Update(volunteer);
            await _context.SaveChangesAsync();
        }
    }
    }


