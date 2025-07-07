using System;

namespace Server.Models
{
    public class CreateCallDto
    {
        public DateTime CallTime { get; set; }
        public int ClientId { get; set; }
        public int? FinalVolunteerId { get; set; }
        public string CallType { get; set; } = string.Empty;
        public double CallLatitude { get; set; }
        public double CallLongitude { get; set; }
    }
} 