using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;
using WeatherForecastChallenge.Application.Commands.UserCommands.Login;
using WeatherForecastChallenge.Core.Response;
using WeatherForecastChallenge.Infrastructure.Auth;

public class UserLoginCommandHandler : IRequestHandler<LoginCommand, UserLoginResponse>
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly ITokenServices _tokenServices;

    public UserLoginCommandHandler(SignInManager<IdentityUser> signInManager, ITokenServices tokenServices)
    {
        _signInManager = signInManager;
        _tokenServices = tokenServices;
    }

    public async Task<UserLoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, false, true);

        if (result.Succeeded)
        {
            return await _tokenServices.GenerateJwt(request.Email);
        }

        throw new Exception("Login falhou: usuário ou senha incorretos.");
    }
}
