using MediatR;
using WeatherForecastChallenge.Core.Response;

namespace WeatherForecastChallenge.Application.Query.CommandQuery
{
    public class GetWeatherForecastQuery : IRequest<WeatherApiResponse>
    {
        public string City { get; }

        public GetWeatherForecastQuery(string city)
        {
            City = city;
        }
    }
}
