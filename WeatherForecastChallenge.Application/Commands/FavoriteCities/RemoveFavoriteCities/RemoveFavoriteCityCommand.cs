using MediatR;
using WeatherForecastChallenge.Core.Response;

namespace WeatherForecastChallenge.Application.Commands.FavoriteCities.RemoveFavoriteCities
{
    public class RemoveFavoriteCityCommand : IRequest<FavoriteCityResponse>
    {
        public string CityName { get; set; } 
        public string UserId { get; set; } 
    }
}
