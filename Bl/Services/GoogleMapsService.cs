using Bl.Api;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Bl.Services
{
    public class GoogleMapsService:IGoogleMapsService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public GoogleMapsService(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
        }

        public async Task<double> GetDistanceInKmAsync(double originLat, double originLng, double destLat, double destLng)
        {
            string apiKey = _configuration["GoogleMaps:ApiKey"];
            string url = $"https://maps.googleapis.com/maps/api/distancematrix/json?origins={originLat},{originLng}&destinations={destLat},{destLng}&key={apiKey}";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                throw new Exception("Google Maps API call failed");

            var json = await response.Content.ReadAsStringAsync();
            dynamic data = JsonConvert.DeserializeObject(json);

            try
            {
                double distanceInMeters = data.rows[0].elements[0].distance.value;
                return distanceInMeters / 1000.0;
            }
            catch
            {
                throw new Exception("Invalid response from Google Maps API");
            }
        }
        public async Task<int> GetEstimatedArrivalTimeInMinutesAsync(double originLat, double originLng, double destLat, double destLng)
        {
            string apiKey = _configuration["GoogleMaps:ApiKey"];
            string url = $"https://maps.googleapis.com/maps/api/distancematrix/json?" +
                         $"origins={originLat},{originLng}&destinations={destLat},{destLng}&key={apiKey}";

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode) return -1;

            var json = await response.Content.ReadAsStringAsync();
            dynamic data = JsonConvert.DeserializeObject(json);

            var durationInSeconds = data.rows[0].elements[0].duration.value;
            return (int)durationInSeconds / 60; // דקות
        }

        // פונקציות נוספות ייכנסו לפה בעתיד...
    }

}
