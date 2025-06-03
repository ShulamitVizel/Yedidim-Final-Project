using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bl.Api
{
    public interface IGoogleMapsService
    {
        /// <summary>
        /// מחזיר מרחק בק"מ בין שתי כתובות
        /// </summary>
        Task<double> GetDistanceInKmAsync(double originLat, double originLng, double destLat, double destLng);

        /// <summary>
        /// מחזיר זמן נסיעה משוער בדקות בין שתי כתובות
        /// </summary>

        Task<int> GetEstimatedArrivalTimeInMinutesAsync(double originLat, double originLng, double destLat, double destLng);
    }
}
