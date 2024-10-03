using MediatR;
using WeatherForecastChallenge.Core.Response;

namespace WeatherForecastChallenge.Application.Commands.FavoriteCities.AddFavoriteCities
{

    public class AddFavoriteCityCommand : IRequest<FavoriteCityResponse>
    {
        public string CityName { get; set; } 
        public string UserId { get; set; }
    }

}
