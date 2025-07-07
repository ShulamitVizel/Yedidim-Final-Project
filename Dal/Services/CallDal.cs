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
        private readonly VolunteerDal _volunteerDal;

        public CallDal(dbClass context, VolunteerDal volunteerDal)
        {
            _context = context;
            _volunteerDal = volunteerDal;
        }

        //public GetAllCalls() { }
        //public GetCallByID() { }

        public async Task<Call?> GetCallByIdAsync(int id) =>
            await _context.Calls
                      .Include(c => c.FinalVolunteer)
                      .FirstOrDefaultAsync(c => c.CallId == id);


        //public async Task<Call> GetCallByIdAsync(int id)
        //{
        //    return await _context.Calls.FindAsync(id);
        //}

        public async Task<int> CreateCallAsync(Call call)
        {
            _context.Calls.Add(call);
            await _context.SaveChangesAsync();
            return call.CallId;
        }
        public async Task DeleteCallAsync(int callId)
        {
            var call = await _context.Calls
                                 .Include(c => c.Volunteers)
                                 .FirstOrDefaultAsync(c => c.CallId == callId)
                             ?? throw new Exception("this call does not exist in the system");
            _context.Calls.Remove(call);
            await _context.SaveChangesAsync();
        }
    

        public async Task UpdateCallAsync(Call call)
        {
            var existingCall = await _context.Calls.FindAsync(call.CallId);
            if (existingCall == null)
            {
                throw new Exception("Call not found in the system");
            }

            existingCall.CallTime = call.CallTime;
            existingCall.ClientId = call.ClientId;
            existingCall.FinalVolunteerId = call.FinalVolunteerId;
            existingCall.CallType = call.CallType;
            existingCall.CallLatitude = call.CallLatitude;
            existingCall.CallLongitude = call.CallLongitude;

            await _context.SaveChangesAsync();
        }
        public async Task AssignVolunteerToCallAsync(int callId, int volunteerId)
        {
            // Check if call exists
            var call = await _context.Calls.FindAsync(callId);
            if (call == null)
            {
                throw new Exception("The call was not found in the system");
            }

            // Check if volunteer exists
            var volunteer = await _volunteerDal.GetVolunteerByIdAsync(volunteerId);
            if (volunteer == null)
            {
                throw new Exception("The volunteer was not found in the system");
            }

            // Simply update the call's FinalVolunteerId instead of using many-to-many
            call.FinalVolunteerId = volunteerId;
            await _context.SaveChangesAsync();
        }

        //public async Task<List<Volunteer>> GetAvailableVolunteersAsync()
        //{
        //    return await _context.Volunteers
        //        .Where(v => v.IsAvailable)
        //        .ToListAsync();
        //}

        public async Task<int> GetMaxCallIdAsync()
        {
            var maxId = await _context.Calls.MaxAsync(c => (int?)c.CallId) ?? 0;
            return maxId;
        }

        public async Task<List<Call>> GetAllCallsAsync()
        {
            return await _context.Calls
                .Include(c => c.FinalVolunteer)
                .Include(c => c.Client)
                .Include(c => c.Volunteers)
                .ToListAsync();
        }
    }
}

//Scaffold-DbContext "Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='H:\newYedidim\Yedidim\Dal\Data\database.mdf';Integrated Security=True; Connect Timeout=30" Microsoft.EntityFrameworkCore.SqlServer -outputdir Models -context dbClass -f
