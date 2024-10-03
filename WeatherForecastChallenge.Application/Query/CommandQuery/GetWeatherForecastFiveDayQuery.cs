using MediatR;
using WeatherForecastChallenge.Core.Response;

namespace WeatherForecastChallenge.Application.Query.CommandQuery
{
    public class GetWeatherForecastFiveDayQuery : IRequest<WeatherApiFiveDayResponse>
    {
        public string City { get; }

        public GetWeatherForecastFiveDayQuery(string city)
        {
            City = city;
        }
    }
}
