using Bl.Api;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;



namespace Bl.Services
{
    public class GoogleMapsService : IGoogleMapsService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public GoogleMapsService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClient = httpClientFactory.CreateClient();
        }   

        private string ApiKey => _configuration["GoogleMaps:ApiKey"]!;

        public async Task<double> GetDistanceInKmAsync(
            double originLat, double originLng,
            double destLat, double destLng)
        {
            // If no API key is configured, return a simple distance calculation
            if (string.IsNullOrEmpty(ApiKey))
            {
                // Simple distance calculation using Haversine formula
                const double earthRadius = 6371; // Earth's radius in kilometers
                var latDiff = (destLat - originLat) * Math.PI / 180;
                var lngDiff = (destLng - originLng) * Math.PI / 180;
                var a = Math.Sin(latDiff / 2) * Math.Sin(latDiff / 2) +
                        Math.Cos(originLat * Math.PI / 180) * Math.Cos(destLat * Math.PI / 180) *
                        Math.Sin(lngDiff / 2) * Math.Sin(lngDiff / 2);
                var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                return earthRadius * c;
            }

            var url = $"https://maps.googleapis.com/maps/api/distancematrix/json?" +
                      $"origins={originLat},{originLng}&destinations={destLat},{destLng}&key={ApiKey}";

            var res = await _httpClient.GetAsync(url);
            res.EnsureSuccessStatusCode();

            var responseContent = await res.Content.ReadAsStringAsync();
            dynamic data = JsonConvert.DeserializeObject(responseContent)!;
            
            // Check if the API returned an error
            if (data.status != "OK")
            {
                throw new Exception($"Google Maps API error: {data.status} - {data.error_message}");
            }

            // Check if the response has the expected structure
            if (data.rows == null || data.rows.Count == 0 || 
                data.rows[0].elements == null || data.rows[0].elements.Count == 0)
            {
                throw new Exception("Invalid response structure from Google Maps API");
            }

            var element = data.rows[0].elements[0];
            if (element.status != "OK")
            {
                throw new Exception($"Google Maps API element error: {element.status}");
            }

            double meters = element.distance.value;
            return meters / 1000.0;
        }

        public async Task<int> GetEstimatedArrivalTimeInMinutesAsync(
            double originLat, double originLng,
            double destLat, double destLng)
        {
            // If no API key is configured, return a simple time estimation
            if (string.IsNullOrEmpty(ApiKey))
            {
                // Simple time estimation: assume 50 km/h average speed
                const double averageSpeedKmh = 50;
                var distance = await GetDistanceInKmAsync(originLat, originLng, destLat, destLng);
                return (int)(distance / averageSpeedKmh * 60); // Convert to minutes
            }

            var url = $"https://maps.googleapis.com/maps/api/distancematrix/json?" +
                      $"origins={originLat},{originLng}&destinations={destLat},{destLng}&key={ApiKey}";

            var res = await _httpClient.GetAsync(url);
            res.EnsureSuccessStatusCode();

            var responseContent = await res.Content.ReadAsStringAsync();
            dynamic data = JsonConvert.DeserializeObject(responseContent)!;
            
            // Check if the API returned an error
            if (data.status != "OK")
            {
                throw new Exception($"Google Maps API error: {data.status} - {data.error_message}");
            }

            // Check if the response has the expected structure
            if (data.rows == null || data.rows.Count == 0 || 
                data.rows[0].elements == null || data.rows[0].elements.Count == 0)
            {
                throw new Exception("Invalid response structure from Google Maps API");
            }

            var element = data.rows[0].elements[0];
            if (element.status != "OK")
            {
                throw new Exception($"Google Maps API element error: {element.status}");
            }

            int seconds = element.duration.value;
            return seconds / 60;
        }
    }


    // פונקציות נוספות ייכנסו לפה בעתיד...
}


