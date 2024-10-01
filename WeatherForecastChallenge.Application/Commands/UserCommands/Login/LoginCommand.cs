using MediatR;
using WeatherForecastChallenge.Core.Response;

namespace WeatherForecastChallenge.Application.Commands.UserCommands.Login
{
    public class LoginCommand : IRequest<UserLoginResponse>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}

