using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WeatherForecastChallenge.API.Extension;
using WeatherForecastChallenge.Application.Response;
using WeatherForecastChallenge.Core.Entities;

namespace WeatherForecastChallenge.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : MainController
    {
        private readonly SignInManager<IdentityUser> _signInManager; 
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppSettings _appSettings;

        public AuthController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IOptions<AppSettings> appSettings)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _appSettings = appSettings.Value;
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
           
            var user = new IdentityUser
            {
                UserName = userRegistry.Email, 
                Email = userRegistry.Email,
                EmailConfirmed = true 
            };
          
            var result = await _userManager.CreateAsync(user: user, password: userRegistry.Password);

            if (result.Succeeded)
            {
                return CustomResponse(await GenerateJwt(userRegistry.Email)); 
            }
            
            foreach (var error in result.Errors)
            {
                AddProcessingError(error.Description); 
            }

            return CustomResponse();
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
                      
            var result = await _signInManager.PasswordSignInAsync(userName: userLogin.Email, password: userLogin.Password, isPersistent: false, lockoutOnFailure: true);

            if (result.Succeeded)
                return CustomResponse(await GenerateJwt(userLogin.Email)); 
            
            if (result.IsLockedOut)
            {
                AddProcessingError("Usuario temporariamente bloqueado por tentativas invalidas.");
                return CustomResponse();
            }

            AddProcessingError("Usuario ou Senha incorretos");

            return CustomResponse();
        }
        
        private async Task<UserLoginResponse> GenerateJwt(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var claims = await _userManager.GetClaimsAsync(user);


            var identityClaims = await GetUserClaims(claims, user);
            var encodedToken = TokenEncryption(identityClaims);

            return GetTokenAnswer(encodedToken, user, claims);
        }

        private async Task<ClaimsIdentity> GetUserClaims(ICollection<Claim> claims, IdentityUser user)
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

        private string TokenEncryption(ClaimsIdentity identityClaims)
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

        private UserLoginResponse GetTokenAnswer(string encodedToken, IdentityUser user, IEnumerable<Claim> claims)
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
            
        private static long ToUnixEpochDate(DateTime date) => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
       
    }
}
