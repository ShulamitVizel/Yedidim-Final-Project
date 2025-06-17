using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bl.Api
{
    public interface IGoogleMapsService
    {
        Task<double> GetDistanceInKmAsync(
            double originLat, double originLng,
            double destLat, double destLng);

        Task<int> GetEstimatedArrivalTimeInMinutesAsync(
            double originLat, double originLng,
            double destLat, double destLng);
    }
}
