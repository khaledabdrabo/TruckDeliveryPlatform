using TruckDeliveryPlatform.Models;

namespace TruckDeliveryPlatform.Services
{
    public class DistanceCalculator
    {
        public static double CalculateDistance(Location from, Location to)
        {
            var d1 = from.Latitude * (Math.PI / 180.0);
            var d2 = to.Latitude * (Math.PI / 180.0);
            var num1 = from.Longitude * (Math.PI / 180.0);
            var num2 = to.Longitude * (Math.PI / 180.0);
            var d3 = num2 - num1;
            var d4 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + 
                     Math.Cos(d1) * Math.Cos(d2) * 
                     Math.Pow(Math.Sin(d3 / 2.0), 2.0);

            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d4), Math.Sqrt(1.0 - d4))) / 1000; // Result in kilometers
        }
    }
} 