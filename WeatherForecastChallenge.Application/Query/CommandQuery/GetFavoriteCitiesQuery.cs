using MediatR;
using WeatherForecastChallenge.Core.Entities;

namespace WeatherForecastChallenge.Application.Query.CommandQuery
{
    public class GetFavoriteCitiesQuery : IRequest<List<FavoriteCity>>
    {
        public string UserId { get; set; } 
    }
}
