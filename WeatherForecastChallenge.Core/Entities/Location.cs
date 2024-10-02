using Newtonsoft.Json;

namespace WeatherForecastChallenge.Core.Entities
{
    public class Location
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }
    }
}
