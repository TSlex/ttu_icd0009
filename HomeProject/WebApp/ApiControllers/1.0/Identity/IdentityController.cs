using System;
using System.Linq;
using System.Text.Encodings.Web;
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
using PublicApi.DTO.v1;
using PublicApi.DTO.v1.Identity;
using PublicApi.DTO.v1.Response;

namespace WebApp.ApiControllers._1._0.Identity
{
    /// <summary>
    /// Login, Register and Managing
    /// </summary>
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

        /// <summary>
        /// Controller
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="configuration"></param>
        /// <param name="signInManager"></param>
        /// <param name="userManager"></param>
        /// <param name="bll"></param>
        public IdentityController(ILogger<IdentityController> logger, IConfiguration configuration,
            SignInManager<Profile> signInManager, UserManager<Profile> userManager, IAppBLL bll)
        {
            _logger = logger;
            _configuration = configuration;
            _signInManager = signInManager;
            _userManager = userManager;
            _bll = bll;
        }

        /// <summary>
        /// Login user
        /// </summary>
        /// <param name="model"></param>
        /// <returns>JWT if succeed</returns>
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

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Updates a profile data (full name, work place etc...)
        /// </summary>
        /// <param name="dto">Data</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> UpdateProfileData([FromBody] ProfileDataDTO dto)
        {
            var user = await _userManager.FindByIdAsync(User.UserId().ToString());

            //setup new phone
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (dto.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, dto.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    return BadRequest(new ErrorResponseDTO(
                        $"Unexpected error occurred while setting phone"));
                }
            }

            //setup new username
            var username = await _userManager.GetUserNameAsync(user);
            if (dto.Username != username)
            {
                var userCheck = await _userManager.FindByNameAsync(dto.Username);

                if (userCheck != null && !(userCheck.Equals(user)))
                {
                    return BadRequest(new ErrorResponseDTO("Error. That username is already taken!"));
                }

                var setUserNameResult = await _userManager.SetUserNameAsync(user, dto.Username);

                if (!setUserNameResult.Succeeded)
                {
                    return BadRequest(
                        new ErrorResponseDTO(
                            "Unexpected error occurred while setting username!"));
                }
            }

            //setup other
            user.ProfileFullName = dto.ProfileFullName;
            user.ProfileWorkPlace = dto.ProfileWorkPlace;
            user.ProfileAbout = dto.ProfileAbout;
            user.ProfileGender = dto.ProfileGender;
            user.ProfileGenderOwn = dto.ProfileGenderOwn;

            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);

            return NoContent();
        }

        /// <summary>
        /// Updates profile email
        /// </summary>
        /// <param name="dto">Data</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> UpdateProfileEmail([FromBody] EmailDTO dto)
        {
            var user = await _userManager.FindByIdAsync(User.UserId().ToString());

            var email = await _userManager.GetEmailAsync(user);
            if (dto.NewEmail != email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, dto.NewEmail);

                if (!setEmailResult.Succeeded)
                {
                    return BadRequest(
                        new ErrorResponseDTO(
                            "Unexpected error occurred while setting email!"));
                }
            }

            return NoContent();
        }
        
        /// <summary>
        /// Updates profile password
        /// </summary>
        /// <param name="dto">Data</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> UpdateProfilePassword([FromBody] PasswordDTO dto)
        {
            var user = await _userManager.FindByIdAsync(User.UserId().ToString());

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                return BadRequest(
                    new ErrorResponseDTO(changePasswordResult.Errors.Select(error => error.Description).ToString()));
            }

            await _signInManager.RefreshSignInAsync(user);
            _logger.LogInformation("User changed their password successfully.");

            return NoContent();
        }
    }
}