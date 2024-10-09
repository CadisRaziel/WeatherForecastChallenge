using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WeatherForecastChallenge.Application.Commands.FavoriteCities.AddFavoriteCities;
using WeatherForecastChallenge.Application.Commands.FavoriteCities.RemoveFavoriteCities;
using WeatherForecastChallenge.Application.Query.CommandQuery;
using WeatherForecastChallenge.Core.Response;

namespace WeatherForecastChallenge.API.Controllers
{
    [Authorize] 
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteCityController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly UserManager<IdentityUser> _userManager;

        public FavoriteCityController(IMediator mediator, UserManager<IdentityUser> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        /// <summary>
        /// Add a city to your favorites
        /// </summary>
        /// <param name="cityName"></param>
        /// <returns>returns the name of the city and a string message of success or error</returns>        
        [HttpPost("AddFavoriteCitie")]
        [ProducesResponseType(typeof(FavoriteCityResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddFavoriteCity([FromBody] string cityName)
        {           
            var userId = _userManager.GetUserId(User);
            
            if (string.IsNullOrWhiteSpace(cityName))
            {
                return BadRequest("O nome da cidade não pode ser vazio.");
            }
            
            var command = new AddFavoriteCityCommand { CityName = cityName, UserId = userId };
            
            var response = await _mediator.Send(command);
          
            return Ok(response);
        }


        /// <summary>
        /// Remove a city from favorites
        /// </summary>
        /// <param name="cityName"></param>
        /// <returns>Returns the name of the city and a string message of success or error</returns>
        [HttpDelete("RemoveFavoriteCities/{cityName}")]
        [ProducesResponseType(typeof(FavoriteCityResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> RemoveFavoriteCity(string cityName)
        {
            var userId = _userManager.GetUserId(User);
            var command = new RemoveFavoriteCityCommand { CityName = cityName, UserId = userId };

            var response = await _mediator.Send(command);
            return Ok(response);
        }


        /// <summary>
        /// Displays the logged-in user's list of favorite cities
        /// </summary>
        /// <returns>returns a list of the user's favorite cities</returns>
        [HttpGet("list")]
        [ProducesResponseType(typeof(List<FavoriteCityResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFavoriteCities()
        {
            var userId = _userManager.GetUserId(User);
            var query = new GetFavoriteCitiesQuery { UserId = userId };

            var cities = await _mediator.Send(query);
            return Ok(cities);
        }
    }
}
