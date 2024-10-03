
using WeatherForecastChallenge.Core.Entities;

namespace WeatherForecastChallenge.Core.Response
{
    public class WeatherApiFiveDayResponse
    {
        public Location Location { get; set; }
        public Forecast Forecast { get; set; }
    }
}
