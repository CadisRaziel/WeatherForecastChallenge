using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using WeatherForecastChallenge.Application.Query.CommandQuery;
using WeatherForecastChallenge.Core.Response;

public class GetWeatherForecastFiveDayQueryHandler : IRequestHandler<GetWeatherForecastFiveDayQuery, WeatherApiFiveDayResponse>
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public GetWeatherForecastFiveDayQueryHandler(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task<WeatherApiFiveDayResponse> Handle(GetWeatherForecastFiveDayQuery request, CancellationToken cancellationToken)
    {
        var apiKey = _configuration["WeatherApi:ApiKey"];
        var city = request.City;

        var httpClient = _httpClientFactory.CreateClient("WeatherAPI");

        var requestUri = $"forecast.json?key={apiKey}&q={city}&days=5&aqi=no&alerts=no";

        var response = await httpClient.GetAsync(requestUri);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Erro ao chamar a API de previsão do tempo: " + response.ReasonPhrase);
        }

        var content = await response.Content.ReadAsStringAsync();
        var weatherApiResponse = JsonConvert.DeserializeObject<WeatherApiFiveDayResponse>(content);

        return weatherApiResponse;
    }
}
