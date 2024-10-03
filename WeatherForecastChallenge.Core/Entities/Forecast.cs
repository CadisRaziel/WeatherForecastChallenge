using Newtonsoft.Json;

namespace WeatherForecastChallenge.Core.Entities
{
    public class Forecast
    {
        [JsonProperty("forecastday")]
        public List<ForecastDay> ForecastDay { get; set; }
    }
}
