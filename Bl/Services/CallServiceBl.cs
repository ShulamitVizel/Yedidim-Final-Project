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

namespace Bl.Services
{
    public class CallBl : ICallBl
    {
        private readonly CallDal _callDal;

        public CallBl(CallDal callDal)
        {
            _callDal = callDal;
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
    }
}

