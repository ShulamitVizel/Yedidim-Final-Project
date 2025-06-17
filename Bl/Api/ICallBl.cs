using System.Collections.Generic;
using System.Threading.Tasks;
using Dal.Models;           // Call, Volunteer

namespace Bl.Api
{
    public interface ICallBl
    {
        Task<int> CreateCallAsync(Call call);          // מחזיר ID שנוצר
        Task DeleteCallAsync(int callId);
        Task UpdateCallAsync(Call call);
        Task<Call?> GetCallByIdAsync(int callId);

        Task AssignVolunteerToCallAsync(int callId, int volunteerId);
        Task<bool> AssignNearestVolunteerAsync(int callId);
        Task<List<Volunteer>> GetMatchingVolunteersAsync(int callId);   // אופציונלי
        Task<int?> GetEstimatedArrivalTimeAsync(int volunteerId, int callId);
    }
}

