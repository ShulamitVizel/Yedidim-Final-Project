using Dal.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Dal.Services
{
    public class CallDal
    {
        private readonly dbClass _context;

        public CallDal(dbClass context)
        {
            _context = context;
        }

        //public GetAllCalls() { }
        //public GetCallByID() { }
        public Dal.Models.Call? GetCallById(int id)
        {
            return _context.Calls
                .Include(c => c.FinalVolunteer)
                .FirstOrDefault(c => c.CallId == id);
        }
        public async Task<Call> GetCallByIdAsync(int id)
        {
            return await _context.Calls.FindAsync(id);
        }

        public void CreateCall(Call call) 
        {
            _context.Calls.Add(call);
            _context.SaveChanges();
        }
        public void DeleteCall(int callId) 
        {
            var call = _context.Calls.Include(c => c.Volunteers) 
                         .FirstOrDefault(c => c.CallId == callId);

            if (call != null)
            {
                _context.Calls.Remove(call);
                _context.SaveChanges(); 
            }
            else
            {
                throw new Exception("The call was not found in the system.");
            }
        }
        public async Task UpdateCall(Call call)
        {
            _context.Calls.Update(call);
            await _context.SaveChangesAsync();
        }
        public void AssignVolunteerToCall(int callId, int volunteerId)
        {
            var call = _context.Calls.Include(c => c.Volunteers)
                                     .FirstOrDefault(c => c.CallId == callId);

            if (call == null)
            {
                throw new Exception("The call was not found in the system");
            }

            var volunteer = _context.Volunteers.FirstOrDefault(v => v.VolunteerId == volunteerId);

            if (volunteer == null)
            {
                throw new Exception("The volunteer was not found in the system");
            }

            if (call.Volunteers.Any(v => v.VolunteerId == volunteerId))
            {
                throw new Exception("The volunteer has already been assigned to this call.");
            }

            call.Volunteers.Add(volunteer);

            _context.SaveChanges(); 
        }

        public async Task<List<Volunteer>> GetAvailableVolunteersAsync()
        {
            return await _context.Volunteers
                .Where(v => v.IsAvailable)
                .ToListAsync();
        }






    }
}

//Scaffold-DbContext "Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='H:\newYedidim\Yedidim\Dal\Data\database.mdf';Integrated Security=True; Connect Timeout=30" Microsoft.EntityFrameworkCore.SqlServer -outputdir Models -context dbClass -f
