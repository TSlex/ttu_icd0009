using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.BLL.App;
using Extension;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PublicApi.DTO.v1;
using PublicApi.DTO.v1.Mappers;
using PublicApi.DTO.v1.Response;

namespace WebApp.ApiControllers._1._0
{
    /// <summary>
    /// Profiles
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProfilesController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly DTOMapper<Profile, ProfileDTO> _mapper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bll">Application Bll</param>
        public ProfilesController(IAppBLL bll)
        {
            _bll = bll;
            _mapper = new DTOMapper<Profile, ProfileDTO>();
        }

        /// <summary>
        /// Get profile
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{username}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProfileDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> GetProfile(string username)
        {
            Profile user;

            if (User.Identity.IsAuthenticated)
            {
                user = await _bll.Profiles.FindByUsernameAsync(username, User.UserId());
            }
            else
            {
                user = await _bll.Profiles.FindByUsernameAsync(username, null);
            }

            if (user == null)
            {
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorUserNotFound));
            }

            return Ok(_mapper.Map(user));
        }

        /// <summary>
        /// Subscribe to profile
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpPost("{username}/follow")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> FollowProfile(string username)
        {
            var user = await _bll.Profiles.FindByUsernameAsync(username);

            if (user == null)
            {
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorUserNotFound));
            }

            if (user.Id == User.UserId())
            {
                return BadRequest(new ErrorResponseDTO(Resourses.BLL.App.DTO.Profiles.Profiles.ErrorFollowYouSelf));
            }

            var property = await _bll.BlockedProfiles.FindAsync(user.Id, User.UserId());

            if (property != null)
            {
                return BadRequest(
                    new ErrorResponseDTO(Resourses.BLL.App.DTO.Profiles.Profiles.ErrorCannotFollowDueBlock));
            }

            var subscription = await _bll.Followers.FindAsync(User.UserId(), user.Id);

            if (subscription == null)
            {
                _bll.Followers.AddSubscription(User.UserId(), user.Id);
                await _bll.SaveChangesAsync();

                return Ok(new OkResponseDTO()
                    {Status = string.Format(Resourses.BLL.App.DTO.Profiles.Profiles.ResponseNowFollow, username)});
            }

            return Ok(new OkResponseDTO()
                {Status = string.Format(Resourses.BLL.App.DTO.Profiles.Profiles.ResponseAlreadyFollow, username)});
        }

        /// <summary>
        /// Unsubscribe from profile
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpPost("{username}/unfollow")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> UnfollowProfile(string username)
        {
            var user = await _bll.Profiles.FindByUsernameAsync(username);

            if (user == null)
            {
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorUserNotFound));
            }

            if (user.Id == User.UserId())
            {
                return BadRequest(new ErrorResponseDTO(Resourses.BLL.App.DTO.Profiles.Profiles.ErrorFollowYouSelf));
            }

            var subscription = await _bll.Followers.FindAsync(User.UserId(), user.Id);

            if (subscription != null)
            {
                _bll.Followers.Remove(subscription);
                await _bll.SaveChangesAsync();
                
                return Ok(new OkResponseDTO()
                    {Status = string.Format(Resourses.BLL.App.DTO.Profiles.Profiles.ResponseNowUnFollow, username)});
            }

            return Ok(new OkResponseDTO()
                {Status = string.Format(Resourses.BLL.App.DTO.Profiles.Profiles.ResponseNotFollow, username)});
        }

        /// <summary>
        /// Add profile to black list
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpPost("{username}/block")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> BlockProfile(string username)
        {
            var user = await _bll.Profiles.FindByUsernameAsync(username);

            if (user == null)
            {
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorUserNotFound));
            }

            if (user.Id == User.UserId())
            {
                return BadRequest(new ErrorResponseDTO(Resourses.BLL.App.DTO.Profiles.Profiles.ErrorBlockYouSelf));
            }

            var property = await _bll.BlockedProfiles.FindAsync(User.UserId(), user.Id);
            var subscription = await _bll.Followers.FindAsync(User.UserId(), user.Id);

            if (property == null)
            {
                if (subscription != null)
                {
                    _bll.Followers.Remove(subscription);
                }

                _bll.BlockedProfiles.AddBlockProperty(User.UserId(), user.Id);
                await _bll.SaveChangesAsync();
                return Ok(new OkResponseDTO() {Status = string.Format(Resourses.BLL.App.DTO.Profiles.Profiles.ResponseNowBlock, username)});
            }

            return Ok(new OkResponseDTO() {Status = string.Format(Resourses.BLL.App.DTO.Profiles.Profiles.ResponseAlreadyBlock, username)});
        }

        /// <summary>
        /// Remove profile from black list
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpPost("{username}/unblock")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> UnblockProfile(string username)
        {
            var user = await _bll.Profiles.FindByUsernameAsync(username);

            if (user == null)
            {
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorUserNotFound));
            }

            if (user.Id == User.UserId())
            {
                return BadRequest(new ErrorResponseDTO(Resourses.BLL.App.DTO.Profiles.Profiles.ErrorBlockYouSelf));
            }

            var property = await _bll.BlockedProfiles.FindAsync(User.UserId(), user.Id);

            if (property != null)
            {
                _bll.BlockedProfiles.Remove(property);
                await _bll.SaveChangesAsync();
                return Ok(new OkResponseDTO() {Status = string.Format(Resourses.BLL.App.DTO.Profiles.Profiles.ResponseNowUnBlock, username)});
            }

            return Ok(new OkResponseDTO() {Status = string.Format(Resourses.BLL.App.DTO.Profiles.Profiles.ResponseNotBlock, username)});
        }
    }
}