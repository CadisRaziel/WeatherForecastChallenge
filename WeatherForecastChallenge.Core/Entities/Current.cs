using Newtonsoft.Json;

namespace WeatherForecastChallenge.Core.Entities
{
    public class Current
    {
        [JsonProperty("temp_c")]
        public float TempC { get; set; }

        [JsonProperty("wind_kph")]
        public float WindKph { get; set; }

        [JsonProperty("humidity")]
        public float Humidity { get; set; }

        [JsonProperty("condition")]
        public Condition Condition { get; set; }
    }
}   