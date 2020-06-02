using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using PublicApi.DTO.v1;
using PublicApi.DTO.v1.Mappers;
using PublicApi.DTO.v1.Response;

namespace WebApp.ApiControllers._1._0.Admin
{
    /// <summary>
    /// Profiles admin controller
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Area("Admin")]
    [Route("api/v{version:apiVersion}/[area]/[controller]")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public class AdminProfilesController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly DTOMapper<Profile, ProfileAdminDTO> _mapper;
        private readonly UserManager<Domain.Profile> _userManager;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bll">Application Bll</param>
        /// <param name="userManager"></param>
        public AdminProfilesController(IAppBLL bll)
        {
            _bll = bll;
            _userManager = HttpContext.RequestServices.GetService<UserManager<Domain.Profile>>();
            _mapper = new DTOMapper<Profile, ProfileAdminDTO>();
        }
        
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProfileAdminDTO>))]
        public async Task<IActionResult> Index()
        {
            return Ok((await _bll.Profiles.AllAdminAsync()).Select(record => _mapper.Map(record)));
        }
        
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProfileAdminDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> Details(Guid id)
        {
            var record = await _bll.Profiles.FindAsync(id);

            if (record == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map(record));
        }
        
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ProfileAdminDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> Create([FromBody] ProfileAdminDTO model)
        {
            if (TryValidateModel(model))
            {
                model.Id = Guid.NewGuid();
                _bll.Profiles.Add(_mapper.MapReverse(model));
                await _bll.SaveChangesAsync();

                return CreatedAtAction("Create", model);
            }

            return BadRequest(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorBadData));
        }
        
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> Edit(Guid id, [FromBody] ProfileAdminDTO model)
        {
            if (id != model.Id)
            {
                return BadRequest(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorIdMatch));
            }
            
            var record = await _userManager.FindByIdAsync(id.ToString());

            if (record == null)
            {
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorNotFound));
            }

            if (ModelState.IsValid)
            {
                if (record.UserName != model.UserName)
                {
                    var result = await _userManager.SetUserNameAsync(record, model.UserName);
                    if (!result.Succeeded)
                    {

                        return BadRequest(Resourses.BLL.App.DTO.Profiles.Profiles.ErrorChangeUsername);
                    }
                }
                
                if (record.Email != model.Email)
                {
                    var result = await _userManager.SetEmailAsync(record, model.Email);
                    if (!result.Succeeded)
                    {

                        return BadRequest(Resourses.BLL.App.DTO.Profiles.Profiles.ErrorChangeEmail);
                    }
                }
                
                if (model.Password != null)
                {
                    var resetToken = await _userManager.GeneratePasswordResetTokenAsync(record);
                    var changePasswordResult = await _userManager.ResetPasswordAsync(record, resetToken,model.Password);
                    if (!changePasswordResult.Succeeded)
                    {
                        foreach (var error in changePasswordResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        
                        return BadRequest(Resourses.BLL.App.DTO.Profiles.Profiles.ErrorSetPassword);
                    }
                }
                
                record.ProfileFullName = model.ProfileFullName;
                record.ProfileWorkPlace = model.ProfileWorkPlace;
                record.Experience = model.Experience;
                record.ProfileAbout = model.ProfileAbout;
                record.ProfileGender = model.ProfileGender;
                record.ProfileGenderOwn = model.ProfileGenderOwn;
                record.ProfileStatus = model.ProfileStatus;
                record.PhoneNumber = model.PhoneNumber;
                record.PhoneNumberConfirmed = model.PhoneNumberConfirmed;
                record.LockoutEnabled = model.LockoutEnabled;
                record.EmailConfirmed = model.EmailConfirmed;
                record.AccessFailedCount = model.AccessFailedCount;

                await _userManager.UpdateAsync(record);
                
                return NoContent();
            }

            return BadRequest(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorBadData));
        }
        
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkResponseDTO))]
        public async Task<IActionResult> Delete(Guid id)
        {
            _bll.Profiles.Remove(id);
            await _bll.SaveChangesAsync();

            return Ok(new OkResponseDTO() {Status = Resourses.BLL.App.DTO.Common.SuccessDeleted});
        }
        
        [HttpPost("{restore}/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkResponseDTO))]
        public async Task<IActionResult> Restore(Guid id)
        {
            var record = await _bll.Profiles.GetForUpdateAsync(id);
            _bll.Profiles.Restore(record);
            await _bll.SaveChangesAsync();

            return Ok(new OkResponseDTO() {Status = Resourses.BLL.App.DTO.Common.SuccessRestored});
        }
    }
}