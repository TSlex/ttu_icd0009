using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Contracts.BLL.App;
using Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PublicApi.v1.Identity;
using PublicApi.v1.Mappers;
using PublicApi.v1.Response;
using AppRole = Domain.Identity.AppRole;
using AppUser = Domain.Identity.AppUser;


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
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UniversalAPIMapper<AppUser, PublicApi.v1.Identity.AppUser> _dataMapper;
        private readonly IAppBLL _bll;

        /// <summary>
        /// Controller
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="configuration"></param>
        /// <param name="signInManager"></param>
        /// <param name="userManager"></param>
        /// <param name="bll"></param>
        /// <param name="roleManager"></param>
        public IdentityController(ILogger<IdentityController> logger, IConfiguration configuration,
            SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IAppBLL bll,
            RoleManager<AppRole> roleManager)
        {
            _logger = logger;
            _configuration = configuration;
            _signInManager = signInManager;
            _userManager = userManager;
            _dataMapper = new UniversalAPIMapper<AppUser, PublicApi.v1.Identity.AppUser>();
            _bll = bll;
            _roleManager = roleManager;
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
                _logger.LogInformation($"Web-Api login. AppUser with credentials: {model.Email} - was not found!");
                return NotFound(new ErrorResponseDTO(Resources.Domain.Common.ErrorUserNotFound));
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            if (!userRoles.Select(role => role.ToLower()).Contains("admin") && user.DeletedAt != null)
            {
                return NotFound(new ErrorResponseDTO(Resources.Domain.Common.ErrorUserNotFound));
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
                $"Web-Api login. AppUser with credentials: {model.Email} - was attempted to log-in with bad password!");
            return NotFound(new ErrorResponseDTO(Resources.Domain.Common.ErrorUserNotFound));
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
            var appUser = await _userManager.FindByEmailAsync(model.Email);
            if (appUser != null)
            {
                _logger.LogInformation($"WebApi register. User {model.Email} already registered!");
                return NotFound(new ErrorResponseDTO(Resources.Domain.AppUsers.AppUser.ErrorUserAlreadyExists));
            }

            if (ModelState.IsValid)
            {
                var user = new AppUser()
                {
                    Id = Guid.NewGuid(),
                    UserName = (await GenerateStudentCode(model.FirstName, model.LastName, model.Email)).ToLower(),
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Created a new account with password.");

                    await _bll.SaveChangesAsync();

                    //add user role
                    var role = await _roleManager.FindByNameAsync("User");

                    if (role != null)
                    {
                        await _userManager.AddToRoleAsync(user, role.Name);
                    }
                    else
                    {
                        _logger.LogWarning("User role - \"User\" was not found!");
                    }

                    return Ok();
                }

                var errors = result.Errors.Select(error => error.Description).ToList();

                return BadRequest(new ErrorResponseDTO(JsonSerializer.Serialize(errors)));
            }

            return BadRequest(new ErrorResponseDTO(Resources.Domain.Common.ErrorBadData));
        }

        /// <summary>
        /// Get AppUser email
        /// </summary>
        /// <returns>List of chat roles</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        public async Task<IActionResult> GetEmail()
        {
            return Ok((await _userManager.FindByIdAsync(User.UserId().ToString())).Email);
        }

        /// <summary>
        /// Get AppUser data
        /// </summary>
        /// <returns>List of chat roles</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDataDTO))]
        public async Task<IActionResult> GetAppUserData()
        {
            var user = await _userManager.FindByIdAsync(User.UserId().ToString());
            return Ok(new UserDataDTO
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber
            });
        }

        /// <summary>
        /// Updates a AppUser data (full name, work place etc...)
        /// </summary>
        /// <param name="dto">Data</param>
        /// <returns></returns>
        [HttpPut]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> UpdateAppUserData([FromBody] UserDataDTO dto)
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
                        Resources.Domain.AppUsers.AppUser.ErrorChangePhone));
                }
            }

            user = await _userManager.FindByIdAsync(User.UserId().ToString());

            //setup other
            user.FirstName = dto.FirstName!;
            user.LastName = dto.LastName!;

            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);

            return Ok(new OkResponseDTO{Status = Resources.Views.Identity.Identity.ProfileDataUpdateStatusSuccess});
            
        }

        /// <summary>
        /// Updates AppUser email
        /// </summary>
        /// <param name="dto">Data</param>
        /// <returns></returns>
        [HttpPut]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> UpdateAppUserEmail([FromBody] EmailDTO dto)
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
                            Resources.Domain.AppUsers.AppUser.ErrorChangeEmail));
                }
            }
            else
            {
                return Ok(new ErrorResponseDTO(Resources.Views.Identity.Identity.EmailUpdateStatusUnchanged));
            }

            return Ok(new ErrorResponseDTO(Resources.Views.Identity.Identity.EmailUpdateStatusSuccess));
        }

        /// <summary>
        /// Updates AppUser password
        /// </summary>
        /// <param name="dto">Data</param>
        /// <returns></returns>
        [HttpPut]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> UpdateAppUserPassword([FromBody] PasswordDTO dto)
        {
            var user = await _userManager.FindByIdAsync(User.UserId().ToString());

            var changePasswordResult =
                await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                return BadRequest(
                    new ErrorResponseDTO(Resources.Domain.AppUsers.AppUser.ErrorIncorrectPassword));
            }

            await _signInManager.RefreshSignInAsync(user);
            _logger.LogInformation("User changed their password successfully.");

            return Ok(new ErrorResponseDTO(Resources.Views.Identity.Identity.PasswordDataUpdateStatusSuccess));
        }

        /// <summary>
        /// Deletes AppUser 
        /// </summary>
        /// <param name="deleteDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> DeleteAppUser([FromBody] DeleteDTO deleteDTO)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound(new ErrorResponseDTO(Resources.Domain.Common.ErrorUserNotFound));
            }

            var requirePassword = await _userManager.HasPasswordAsync(user);
            if (requirePassword)
            {
                if (deleteDTO.Password == null || !await _userManager.CheckPasswordAsync(user, deleteDTO.Password))
                {
                    return BadRequest(
                        new ErrorResponseDTO(Resources.Domain.AppUsers.AppUser.ErrorIncorrectPassword));
                }
            }

            // TODO: soft delete depended entities
            user.DeletedAt = DateTime.UtcNow;
            user.DeletedBy = User.Identity.Name;

//            var result = await _bll.AppUsers.GetForUpdateAsync(user.Id);
//
//            if (result.DeletedAt == null)
//            {
//                throw new InvalidOperationException($"Unexpected error occurred deleting user with ID '{user.Id}'.");
//            }

            await _signInManager.SignOutAsync();

            return Ok();
        }
        
        /// <summary>
        /// generates a unique student code
        /// </summary>
        /// <returns></returns>
        private async Task<string> GenerateStudentCode(string firstName, string lastName, string email)
        {
            var fLenght = firstName.Length;
            var fNameLeft = firstName.Substring(0, fLenght / 2);
            
            var lLenght = lastName.Length;
            var lNameRight = lastName.Substring(lLenght / 2);

            var name = fNameLeft + lNameRight;
            var result = await _userManager.FindByNameAsync(name);

            if (result == null)
            {
                return name;
            }

            var counter = 1;
            
            while (true)
            {
                if (counter > 15)
                {
                    return email.Split("@")[0];
                }
                
                result = await _userManager.FindByNameAsync(name + counter);
                
                if (result == null)
                {
                    return name;
                }

                counter++;
            }
        }
    }
}