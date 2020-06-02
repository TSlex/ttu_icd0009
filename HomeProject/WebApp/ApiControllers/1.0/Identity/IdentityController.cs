using System;
using System.Collections.Generic;
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
using PublicApi.DTO.v1.Enums;
using PublicApi.DTO.v1.Identity;
using PublicApi.DTO.v1.Mappers;
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
        private readonly DTOMapper<BLL.App.DTO.Profile, ProfileDataDTO> _dataMapper;
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
            _dataMapper = new DTOMapper<BLL.App.DTO.Profile, ProfileDataDTO>();
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
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorUserNotFound));
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
                return Ok(new JwtResponseDTO {Token = jwt, Status = "OK"});
            }

            _logger.LogInformation(
                $"Web-Api login. Profile with credentials: {model.Email} - was attempted to log-in with bad password!");
            return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorUserNotFound));
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
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Profiles.Profiles.ErrorUserAlreadyExists));
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
                    return Ok(new OkResponseDTO {Status = "OK"});
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

            return BadRequest(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorBadData));
        }

        /// <summary>
        /// Get profile email
        /// </summary>
        /// <returns>List of chat roles</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public async Task<IActionResult> GetEmail()
        {
            return Ok((await _bll.Profiles.GetForUpdateAsync(User.UserId())).Email);
        }

        /// <summary>
        /// Get profile data
        /// </summary>
        /// <returns>List of chat roles</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProfileDataDTO))]
        public async Task<IActionResult> GetProfileData()
        {
            return Ok(_dataMapper.Map((await _bll.Profiles.GetForUpdateAsync(User.UserId()))));
        }

        /// <summary>
        /// Updates a profile data (full name, work place etc...)
        /// </summary>
        /// <param name="dto">Data</param>
        /// <returns></returns>
        [HttpPut]
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
                        Resourses.BLL.App.DTO.Profiles.Profiles.ErrorChangePhone));
                }
            }

            //setup new username
            var username = await _userManager.GetUserNameAsync(user);
            if (dto.Username != username)
            {
                var userCheck = await _userManager.FindByNameAsync(dto.Username);

                if (userCheck != null && !(userCheck.Equals(user)))
                {
                    return BadRequest(new ErrorResponseDTO(Resourses.BLL.App.DTO.Profiles.Profiles.ErrorUsernameAlreadyExists));
                }

                var setUserNameResult = await _userManager.SetUserNameAsync(user, dto.Username);

                if (!setUserNameResult.Succeeded)
                {
                    return BadRequest(
                        new ErrorResponseDTO(
                            Resourses.BLL.App.DTO.Profiles.Profiles.ErrorChangeUsername));
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
        [HttpPut]
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
                await _userManager.SetEmailAsync(user, dto.NewEmail);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var setEmailResult = await _userManager.ConfirmEmailAsync(user, code);

                if (!setEmailResult.Succeeded)
                {
                    return BadRequest(
                        new ErrorResponseDTO(
                            Resourses.BLL.App.DTO.Profiles.Profiles.ErrorChangeEmail));
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Updates profile password
        /// </summary>
        /// <param name="dto">Data</param>
        /// <returns></returns>
        [HttpPut]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> UpdateProfilePassword([FromBody] PasswordDTO dto)
        {
            var user = await _userManager.FindByIdAsync(User.UserId().ToString());

            var changePasswordResult =
                await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                return BadRequest(
                    new ErrorResponseDTO(changePasswordResult.Errors.Select(error => error.Description).ToArray()));
            }

            await _signInManager.RefreshSignInAsync(user);
            _logger.LogInformation("User changed their password successfully.");

            return NoContent();
        }

        /// <summary>
        /// Deletes message
        /// </summary>
        /// <param name="deleteDTO"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> DeleteProfile([FromBody] ProfileDeleteDTO deleteDTO)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorUserNotFound));
            }

            var requirePassword = await _userManager.HasPasswordAsync(user);
            if (requirePassword)
            {
                if (deleteDTO.Password == null || !await _userManager.CheckPasswordAsync(user, deleteDTO.Password))
                {
                    return BadRequest(new ErrorResponseDTO(Resourses.BLL.App.DTO.Profiles.Profiles.ErrorIncorrectPassword));
                }
            }

            var profile = await _bll.Profiles.GetForUpdateAsync(user.Id);
            _bll.Profiles.Remove(profile);
            await _bll.SaveChangesAsync();

            await _signInManager.SignOutAsync();

            return Ok(new OkResponseDTO() {Status = "OK"});
        }
    }
}