using MediatR; 
using Microsoft.AspNetCore.Identity;
using WeatherForecastChallenge.Application.Commands.UserCommands.Register;
using WeatherForecastChallenge.Core.Response;
using WeatherForecastChallenge.Infrastructure.Auth;

public class UserRegistryCommandHandler : IRequestHandler<RegisterCommand, UserLoginResponse>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ITokenServices _tokenServices;

    public UserRegistryCommandHandler(UserManager<IdentityUser> userManager, ITokenServices tokenServices)
    {
        _userManager = userManager;
        _tokenServices = tokenServices;
    }

    public async Task<UserLoginResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = new IdentityUser
        {
            UserName = request.Email,
            Email = request.Email,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            return await _tokenServices.GenerateJwt(request.Email);
        }

        throw new Exception("Registro falhou: " + string.Join(", ", result.Errors.Select(e => e.Description)));
    }
}
