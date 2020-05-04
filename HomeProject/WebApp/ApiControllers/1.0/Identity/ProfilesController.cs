using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Domain;
using Extension;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PublicApi.DTO.v1.Identity;
using PublicApi.DTO.v1.Response;

namespace WebApp.ApiControllers._1._0.Identity
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProfilesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ProfilesController> _logger;
        private readonly UserManager<Profile> _userManager;
        private readonly SignInManager<Profile> _signInManager;

        public ProfilesController(ILogger<ProfilesController> logger, IConfiguration configuration, SignInManager<Profile> signInManager, UserManager<Profile> userManager)
        {
            _logger = logger;
            _configuration = configuration;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        
        [HttpPost]
        [AllowAnonymous]
        [Produces( "application/json" )]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JwtResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        public async Task<ActionResult<string>> Login([FromBody] LoginDTO model)
        {
            var profile = await _userManager.FindByEmailAsync(model.Email);
            
            if (profile == null)
            {
                _logger.LogInformation($"Web-Api login. Profile with credentials: {model.Email} - was not found!");
                return NotFound(new ErrorResponseDTO("User not found!"));
            }
            
            var result = await _signInManager.CheckPasswordSignInAsync(profile, model.Password, false);
            
            if (result.Succeeded)
            {
                var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(profile); //get the User analog
                
                var jwt = IdentityExtensions.GenerateJWT(claimsPrincipal.Claims,
                    _configuration["JWT:SigningKey"],
                    _configuration["JWT:Issuer"],
                    _configuration.GetValue<int>("JWT:ExpirationInDays")
                );
                
                _logger.LogInformation($"Token generated for user {model.Email}");
                return Ok(new JwtResponseDTO{Token = jwt, Status = "Logged in"});
            }
            
            _logger.LogInformation($"Web-Api login. Profile with credentials: {model.Email} - was attempted to log-in with bad password!");
            return NotFound(new ErrorResponseDTO("User not found!"));
        }
        
        [HttpPost]
        [AllowAnonymous]
        [Produces( "application/json" )]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<ActionResult<string>> Register([FromBody] RegisterDTO model)
        {
            var profile = await _userManager.FindByEmailAsync(model.Email);
            if (profile != null)
            {
                _logger.LogInformation($"WebApi register. User {model.Email} already registered!");
                return NotFound(new ErrorResponseDTO("User already registered!"));
            }
            
            if (ModelState.IsValid)
            {
                var user = new Profile()
                {
                    UserName = model.Username, 
                    Email = model.Email, 
                    EmailConfirmed = true
                };
                
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Created a new account with password.");
                    return Ok(new OkResponseDTO{Status = "Registration was successful!"});
                }
                
                var errors = result.Errors.Select(error => error.Description).ToList();
                
                return BadRequest(new ErrorResponseDTO(JsonSerializer.Serialize(errors)));
            }
            
            return BadRequest(new ErrorResponseDTO("Invalid request!"));
        }
    }
}
