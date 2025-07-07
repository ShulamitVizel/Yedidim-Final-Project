namespace Server.DTOs
{
    public class VolunteerDto
    {
        public string Name { get; set; } = null!;
        public string Level { get; set; } = null!;
        public bool IsAvailable { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public double VolunteerLatitude { get; set; }
        public double VolunteerLongitude { get; set; }
    }
} 