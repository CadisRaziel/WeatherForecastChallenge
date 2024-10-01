using MediatR;
using WeatherForecastChallenge.Core.Response;

namespace WeatherForecastChallenge.Application.Commands.UserCommands.Register
{
    public class RegisterCommand : IRequest<UserLoginResponse>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
