using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WeatherForecastChallenge.Application.Commands.UserCommands.Login;
using WeatherForecastChallenge.Application.Commands.UserCommands.Register;
using WeatherForecastChallenge.Core.Entities;
using WeatherForecastChallenge.Core.Response;
using WeatherForecastChallenge.Infrastructure.Auth;
using WeatherForecastChallenge.Infrastructure.Extension;

namespace WeatherForecastChallenge.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : MainController
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Create a new user account
        /// </summary>
        /// <param name="userRegistry"></param>
        /// <returns>It will return a Data with the AccessToken and the expiration time</returns>
        [HttpPost("Register")]
        [ProducesResponseType(typeof(UserLoginResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult> Register(UserRegistry userRegistry)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var command = new RegisterCommand
            {
                Email = userRegistry.Email,
                Password = userRegistry.Password
            };

            try
            {
                var response = await _mediator.Send(command);
                return CustomResponse(response);
            }
            catch (Exception ex)
            {
                AddProcessingError(ex.Message);
                return CustomResponse();
            }
        }



        /// <summary>
        /// User login
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns>It will return a Data with the AccessToken and the expiration time</returns>
        [HttpPost("Login")]
        [ProducesResponseType(typeof(UserLoginResponse), StatusCodes.Status200OK)]        
        public async Task<ActionResult> Login(UserLogin userLogin)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            var command = new LoginCommand
            {
                Email = userLogin.Email,
                Password = userLogin.Password
            };

            try
            {
                var response = await _mediator.Send(command);
                return CustomResponse(response);
            }
            catch (Exception ex)
            {
                AddProcessingError(ex.Message);
                return CustomResponse();
            }
        }
    }
}
