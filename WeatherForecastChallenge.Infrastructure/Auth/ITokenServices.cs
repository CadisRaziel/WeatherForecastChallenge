using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using WeatherForecastChallenge.Core.Response;

namespace WeatherForecastChallenge.Infrastructure.Auth
{
    public interface ITokenServices
    {
        Task<UserLoginResponse> GenerateJwt(string email);
        Task<ClaimsIdentity> GetUserClaims(ICollection<Claim> claims, IdentityUser user);
        string TokenEncryption(ClaimsIdentity identityClaims);
        UserLoginResponse GetTokenAnswer(string encodedToken, IdentityUser user, IEnumerable<Claim> claims);
    }
}
