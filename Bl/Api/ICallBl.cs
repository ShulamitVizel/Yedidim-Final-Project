using Bl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public interface ICallBl
{
    void CreateCall(Call call);
    void DeleteCall(int callId);
    void UpdateCall(Call call);
    Call? GetCallById(int callId);
    void AssignVolunteerToCall(int callId, int volunteerId);
}
