using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeatherForecastChallenge.Application.Query.CommandQuery;
using WeatherForecastChallenge.Core.Response;


[ApiController]
[Authorize]
[Route("api/[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly IMediator _mediator;

    public WeatherForecastController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Weather forecast api
    /// </summary>
    /// <param name="city"></param>
    /// <returns>Weather forecast api that fetches data from WeatherAPI and returns location - temperature - humidity - weather condition - wind speed</returns>
    [HttpGet("{city}")]    
    [ProducesResponseType(typeof(WeatherApiResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetWeatherForecast(string city)
    {
        var query = new GetWeatherForecastQuery(city);
        var result = await _mediator.Send(query);

        return Ok(result);
    }
}
