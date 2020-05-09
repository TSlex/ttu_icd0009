using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Contracts.BLL.App;
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
    public class IdentityController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<IdentityController> _logger;
        private readonly UserManager<Profile> _userManager;
        private readonly SignInManager<Profile> _signInManager;
        private readonly IAppBLL _bll;

        public IdentityController(ILogger<IdentityController> logger, IConfiguration configuration,
            SignInManager<Profile> signInManager, UserManager<Profile> userManager, IAppBLL bll)
        {
            _logger = logger;
            _configuration = configuration;
            _signInManager = signInManager;
            _userManager = userManager;
            _bll = bll;
        }

        [HttpPost]
        [AllowAnonymous]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JwtResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        public async Task<ActionResult<string>> Login([FromBody] LoginDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                _logger.LogInformation($"Web-Api login. Profile with credentials: {model.Email} - was not found!");
                return NotFound(new ErrorResponseDTO("User not found!"));
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (result.Succeeded)
            {
                var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(user); //get the User analog

                var jwt = IdentityExtensions.GenerateJWT(claimsPrincipal.Claims,
                    _configuration["JWT:SigningKey"],
                    _configuration["JWT:Issuer"],
                    _configuration.GetValue<int>("JWT:ExpirationInDays")
                );

                _logger.LogInformation($"Token generated for user {model.Email}");
                return Ok(new JwtResponseDTO {Token = jwt, Status = "Logged in"});
            }

            _logger.LogInformation(
                $"Web-Api login. Profile with credentials: {model.Email} - was attempted to log-in with bad password!");
            return NotFound(new ErrorResponseDTO("User not found!"));
        }

        [HttpPost]
        [AllowAnonymous]
        [Produces("application/json")]
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
                    return Ok(new OkResponseDTO {Status = "Registration was successful!"});
                }

                _bll.ProfileRanks.Add(new BLL.App.DTO.ProfileRank()
                {
                    Id = user.Id,
                    RankId = (await _bll.Ranks.FindByCodeAsync("X_00")).Id
                });
                
                await _bll.SaveChangesAsync();

                var errors = result.Errors.Select(error => error.Description).ToList();

                return BadRequest(new ErrorResponseDTO(JsonSerializer.Serialize(errors)));
            }

            return BadRequest(new ErrorResponseDTO("Invalid request!"));
        }
    }
}