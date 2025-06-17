using Dal.Models;
using Bl.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal.Services;
using Bl.Api;
using Microsoft.Extensions.Configuration;

namespace Bl.Services
{
    public class CallServiceBl : ICallBl
    {
        private readonly CallDal _callDal;
        private readonly VolunteerDal _volunteerDal;
        private readonly IGoogleMapsService _maps;

        public CallServiceBl(CallDal callDal,
                             VolunteerDal volunteerDal,
                             IGoogleMapsService maps)
            => (_callDal, _volunteerDal, _maps) = (callDal, volunteerDal, maps);

        /* ---------- CRUD בסיסי ---------- */

        public async Task<int> CreateCallAsync(Call call)
        {
            await _callDal.CreateCallAsync(call);
            return call.CallId;                    // ה-ID הופק ע״י EF
        }

        public async Task DeleteCallAsync(int callId) =>
            await _callDal.DeleteCallAsync(callId);

        public async Task UpdateCallAsync(Call call) =>
            await _callDal.UpdateCallAsync(call);

        public async Task<Call?> GetCallByIdAsync(int callId) =>
            await _callDal.GetCallByIdAsync(callId);

        /* ---------- שיוך מתנדבים ---------- */

        public async Task AssignVolunteerToCallAsync(int callId, int volunteerId) =>
            await _callDal.AssignVolunteerToCallAsync(callId, volunteerId);

        /* ---------- לוגיקה מתקדמת ---------- */

        public async Task<bool> AssignNearestVolunteerAsync(int callId)
        {
            var call = await _callDal.GetCallByIdAsync(callId);
            if (call is null) return false;

            var volunteers = await _volunteerDal.GetAvailableVolunteersAsync();
            if (volunteers.Count == 0) return false;

            Volunteer? closest = null;
            double shortest = double.MaxValue;

            foreach (var v in volunteers)
            {
                double dist = await _maps.GetDistanceInKmAsync(
                    call.CallLatitude, call.CallLongitude,
                    v.VolunteerLatitude, v.VolunteerLongitude);

                if (dist < shortest)
                {
                    shortest = dist;
                    closest = v;
                }
            }

            if (closest is null) return false;

            call.FinalVolunteerId = closest.VolunteerId;
            await _callDal.UpdateCallAsync(call);
            return true;
        }

        public async Task<int?> GetEstimatedArrivalTimeAsync(int volunteerId, int callId)
        {
            var (call, volunteer) = (await _callDal.GetCallByIdAsync(callId),
                                     await _volunteerDal.GetVolunteerByIdAsync(volunteerId));

            if (call is null || volunteer is null) return null;

            return await _maps.GetEstimatedArrivalTimeInMinutesAsync(
                volunteer.VolunteerLatitude, volunteer.VolunteerLongitude,
                call.CallLatitude, call.CallLongitude);
        }

        /* אופציונלי – התאמת רשימת מתנדבים */
        public async Task<List<Volunteer>> GetMatchingVolunteersAsync(int callId)
        {
            var call = await _callDal.GetCallByIdAsync(callId)
                       ?? throw new KeyNotFoundException("Call not found");

            return await _volunteerDal.GetAvailableVolunteersAsync();
            // אפשר לסנן כאן עפ״י רדיוס/רמה/סוג-קריאה
        }

    }
}


