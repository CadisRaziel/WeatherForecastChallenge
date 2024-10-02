using WeatherForecastChallenge.Core.Entities;

namespace WeatherForecastChallenge.Core.Response
{
    public class WeatherApiResponse
    {
        public Location Location { get; set; }
        public Current Current { get; set; }
    }
}
