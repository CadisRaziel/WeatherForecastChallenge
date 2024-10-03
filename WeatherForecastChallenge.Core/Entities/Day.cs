using Newtonsoft.Json;

namespace WeatherForecastChallenge.Core.Entities
{
    public class Day
    {
        [JsonProperty("maxtemp_c")]
        public float MaxTempC { get; set; }

        [JsonProperty("mintemp_c")]
        public float MinTempC { get; set; }

        [JsonProperty("avgtemp_c")]
        public float AvgTempC { get; set; }

        public Condition Condition { get; set; }
    }
}
