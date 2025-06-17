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
            var url = $"https://maps.googleapis.com/maps/api/distancematrix/json?" +
                      $"origins={originLat},{originLng}&destinations={destLat},{destLng}&key={ApiKey}";

            var res = await _httpClient.GetAsync(url);
            res.EnsureSuccessStatusCode();

            dynamic data = JsonConvert.DeserializeObject(await res.Content.ReadAsStringAsync())!;
            double meters = data.rows[0].elements[0].distance.value;
            return meters / 1000.0;
        }

        public async Task<int> GetEstimatedArrivalTimeInMinutesAsync(
            double originLat, double originLng,
            double destLat, double destLng)
        {
            var url = $"https://maps.googleapis.com/maps/api/distancematrix/json?" +
                      $"origins={originLat},{originLng}&destinations={destLat},{destLng}&key={ApiKey}";

            var res = await _httpClient.GetAsync(url);
            res.EnsureSuccessStatusCode();

            dynamic data = JsonConvert.DeserializeObject(await res.Content.ReadAsStringAsync())!;
            int seconds = data.rows[0].elements[0].duration.value;
            return seconds / 60;
        }
    }


    // פונקציות נוספות ייכנסו לפה בעתיד...
}


