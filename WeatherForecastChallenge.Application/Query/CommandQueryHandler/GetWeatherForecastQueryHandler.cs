using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using WeatherForecastChallenge.Application.Query.CommandQuery;
using WeatherForecastChallenge.Core.Response;

public class GetWeatherForecastQueryHandler : IRequestHandler<GetWeatherForecastQuery, WeatherApiResponse>
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public GetWeatherForecastQueryHandler(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task<WeatherApiResponse> Handle(GetWeatherForecastQuery request, CancellationToken cancellationToken)
    {
        var apiKey = _configuration["WeatherApi:ApiKey"];
        var city = request.City;
       
        var httpClient = _httpClientFactory.CreateClient("WeatherAPI");
       
        var requestUri = $"current.json?key={apiKey}&q={city}&aqi=no"; 
        
        var response = await httpClient.GetAsync(requestUri);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Erro ao chamar a API de previsão do tempo: " + response.ReasonPhrase);
        }

        var content = await response.Content.ReadAsStringAsync();
        var weatherApiResponse = JsonConvert.DeserializeObject<WeatherApiResponse>(content);

        return weatherApiResponse;
    }
}
