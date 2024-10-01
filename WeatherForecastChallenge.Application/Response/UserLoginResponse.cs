using WeatherForecastChallenge.Core.Entities;

namespace WeatherForecastChallenge.Application.Response
{
    public class UserLoginResponse
    {
        public string AccessToken { get; set; } 
        public double ExpiresIn { get; set; } 
        public UserToken UserToken { get; set; } 
    }
}
