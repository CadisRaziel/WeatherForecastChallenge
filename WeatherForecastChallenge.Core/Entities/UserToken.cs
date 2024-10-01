namespace WeatherForecastChallenge.Core.Entities
{
    public class UserToken
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public IEnumerable<UserClaims> Claims { get; set; }
    }
}
