using Bl.Models;
using Bl.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal;
using Dal.Models;
using Dal.Services;
using Bl.Api;
using Microsoft.Extensions.Configuration;

namespace Bl.Services
{
    public class CallServiceBl : ICallBl
    {
        private readonly CallDal _callDal;
        private readonly VolunteerDal _volunteerDal;
        private readonly IGoogleMapsService _googleMapsService;

        public CallServiceBl(CallDal callDal, VolunteerDal volunteerDal, IGoogleMapsService googleMapsService)
        {
            _volunteerDal = volunteerDal;

            _callDal = callDal;
            _googleMapsService = googleMapsService;
        }

        public void CreateCall(Models.Call call)
        {
            var dalCall = MapBlToDal(call);
            _callDal.CreateCall(dalCall);
        }

        public void DeleteCall(int callId)
        {
            _callDal.DeleteCall(callId);
        }

        public void UpdateCall(Models.Call call)
        {
            var dalCall = MapBlToDal(call);
            _callDal.UpdateCall(dalCall).Wait(); // בגלל שזה async ב-DAL
        }

        public Models.Call GetCallById(int callId)
        {
            var dalCall = _callDal.GetCallById(callId);
            return dalCall == null ? null : MapDalToBl(dalCall);
        }
        public void AssignVolunteerToCall(int callId, int volunteerId)
        {
            _callDal.AssignVolunteerToCall(callId, volunteerId);
        }

        // Mapping Functions
        private Dal.Models.Call MapBlToDal(Models.Call blCall)
        {
            return new Dal.Models.Call
            {
                CallId = blCall.CallId,
                CallTime = blCall.CallTime,
                ClientId = blCall.ClientId,
                FinalVolunteerId = blCall.FinalVolunteerId,
                CallType = blCall.CallType,
                CallLatitude = double.TryParse(blCall.CallLocation, out var lat) ? lat : 0, // אם שדה שונה
                CallLongitude = 0, // או שתמפי בהתאם
                // אין כאן Clients / Volunteers מלאים במיפוי פשוט
            };
        }

        private Models.Call MapDalToBl(Dal.Models.Call dalCall)
        {
            return new Models.Call
            {
                CallId = dalCall.CallId,
                CallTime = dalCall.CallTime,
                ClientId = dalCall.ClientId,
                FinalVolunteerId = dalCall.FinalVolunteerId,
                CallType = dalCall.CallType,
                CallLocation = $"{dalCall.CallLatitude},{dalCall.CallLongitude}", // אם את מאחדת לשדה אחד
                // אפשרות להוסיף מיפוי מתנדבים ולקוח אם צריך
            };
        }
        public async Task<bool> AssignNearestVolunteerAsync(int callId)
        {
            // שליפת הקריאה לפי מזהה
            var call = await _callDal.GetCallByIdAsync(callId);
            if (call == null || call.CallLatitude == null || call.CallLongitude == null)
                return false;

            // שליפת כל המתנדבים הזמינים
            var volunteers = await _volunteerDal.GetAvailableVolunteersAsync();
            if (volunteers == null || !volunteers.Any())
                return false;

            Volunteer closestVolunteer = null;
            double shortestDistance = double.MaxValue;

            foreach (var volunteer in volunteers)
            {
                if (volunteer.VolunteerLatitude == null || volunteer.VolunteerLongitude == null)
                    continue;

                // חישוב מרחק בין הקריאה למתנדב
                double distance = await _googleMapsService.GetDistanceInKmAsync(
                    call.CallLatitude,
                    call.CallLongitude,
                    volunteer.VolunteerLatitude,
                    volunteer.VolunteerLongitude
                );

                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    closestVolunteer = volunteer;
                }
            }

            if (closestVolunteer != null)
            {
                // עדכון הקריאה עם המתנדב שנבחר
                call.FinalVolunteerId = closestVolunteer.Id;
                await _callDal.UpdateCall(call);
                return true;
            }

            return false;
        }

        public async Task<int?> GetEstimatedArrivalTimeAsync(int volunteerId, int callId)
        {
            var call = await _callDal.GetCallByIdAsync(callId);
            var volunteer = await _volunteerDal.GetVolunteerByIdAsync(volunteerId);

            if (call == null || volunteer == null ||
                call.CallLatitude == null || call.CallLongitude == null ||
                volunteer.VolunteerLatitude == null || volunteer.VolunteerLongitude == null)
            {
                return null;
            }

            return await _googleMapsService.GetEstimatedArrivalTimeInMinutesAsync(
                volunteer.VolunteerLatitude, volunteer.VolunteerLatitude,
                call.CallLatitude, call.CallLongitude
            );
        }
    }



    }


