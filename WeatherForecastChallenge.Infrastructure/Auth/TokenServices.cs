
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WeatherForecastChallenge.Core.Entities;
using WeatherForecastChallenge.Core.Response;
using WeatherForecastChallenge.Infrastructure.Extension;

namespace WeatherForecastChallenge.Infrastructure.Auth
{
    public class TokenServices : ITokenServices
    {        
            private readonly UserManager<IdentityUser> _userManager;
            private readonly AppSettings _appSettings;

        public TokenServices(UserManager<IdentityUser> userManager, IOptions<AppSettings> appSettings)
        {
            _userManager = userManager;
            _appSettings = appSettings.Value;
        }

        public async Task<UserLoginResponse> GenerateJwt(string email)
            {
                var user = await _userManager.FindByEmailAsync(email);
                var claims = await _userManager.GetClaimsAsync(user);

                var identityClaims = await GetUserClaims(claims, user);
                var encodedToken = TokenEncryption(identityClaims);

                return GetTokenAnswer(encodedToken, user, claims);
            }

            public async Task<ClaimsIdentity> GetUserClaims(ICollection<Claim> claims, IdentityUser user)
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
                claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
                claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
                claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));

                foreach (var userRole in userRoles)
                {
                    claims.Add(new Claim("role", userRole));
                }

                var identityClaims = new ClaimsIdentity();
                identityClaims.AddClaims(claims);

                return identityClaims;
            }

            public string TokenEncryption(ClaimsIdentity identityClaims)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

                var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
                {
                    Issuer = _appSettings.Issuer,
                    Audience = _appSettings.ValidOn,
                    Subject = identityClaims,
                    Expires = DateTime.UtcNow.AddHours(_appSettings.ExpirationHours),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                });

                return tokenHandler.WriteToken(token);
            }

            public UserLoginResponse GetTokenAnswer(string encodedToken, IdentityUser user, IEnumerable<Claim> claims)
            {
                return new UserLoginResponse
                {
                    AccessToken = encodedToken,
                    ExpiresIn = TimeSpan.FromHours(_appSettings.ExpirationHours).TotalSeconds,
                    UserToken = new UserToken
                    {
                        Id = user.Id,
                        Email = user.Email,
                        Claims = claims.Select(c => new UserClaims { Type = c.Type, Value = c.Value })
                    }
                };
            }

            private static long ToUnixEpochDate(DateTime date) =>
                (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
        }   
}
