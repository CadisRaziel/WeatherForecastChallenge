using Newtonsoft.Json;

namespace WeatherForecastChallenge.Core.Entities
{
    public class Condition
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }
    }
}
