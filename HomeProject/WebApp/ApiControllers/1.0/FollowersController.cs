using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.BLL.App;
using DAL;
using Domain;
using Extension;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PublicApi.DTO.v1;
using PublicApi.DTO.v1.Response;

namespace WebApp.ApiControllers._1._0
{
    /// <summary>
    /// Profile followers and followed
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FollowersController : ControllerBase
    {
        private readonly IAppBLL _bll;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bll">Application Bll</param>
        public FollowersController(IAppBLL bll)
        {
            _bll = bll;
        }

        /// <summary>
        /// Get users count the profile is subscribed to
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{username}/followed/count")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CountResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> GetUserFollowedCount(string username)
        {
            var user = await _bll.Profiles.FindByUsernameAsync(username);

            if (user == null)
            {
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorUserNotFound));
            }

            return Ok(new CountResponseDTO()
                {Count = await _bll.Followers.CountByIdAsync(user.Id, true)});
        }

        /// <summary>
        /// Get users the profile is subscribed to
        /// </summary>
        /// <param name="username">Profile username</param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{username}/followed/{pageNumber}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<FollowerProfileDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> GetUserFollowed(string username, int pageNumber)
        {
            var user = await _bll.Profiles.FindByUsernameAsync(username);

            if (user == null)
            {
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorUserNotFound));
            }

            return Ok((await _bll.Followers.AllByIdPageAsync(user.Id, true, pageNumber, 10)).Select(favorite =>
                new FollowerProfileDTO
                {
                    UserName = favorite.Profile!.UserName,
                    ProfileAvatarId = favorite.Profile.ProfileAvatarId
                }));
        }

        /// <summary>
        /// Get users count who follow that profile
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{username}/followers/count")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CountResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> GetUserFollowersCount(string username)
        {
            var user = await _bll.Profiles.FindByUsernameAsync(username);

            if (user == null)
            {
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorUserNotFound));
            }

            return Ok(new CountResponseDTO()
                {Count = await _bll.Followers.CountByIdAsync(user.Id, false)});
        }

        /// <summary>
        /// Get users who follow that profile
        /// </summary>
        /// <param name="username"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{username}/followers/{pageNumber}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<FollowerProfileDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> GetUserFollowers(string username, int pageNumber)
        {
            var user = await _bll.Profiles.FindByUsernameAsync(username);

            if (user == null)
            {
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorUserNotFound));
            }

            return Ok((await _bll.Followers.AllByIdPageAsync(user.Id, false, pageNumber, 10)).Select(favorite =>
                new FollowerProfileDTO
                {
                    UserName = favorite.FollowerProfile!.UserName,
                    ProfileAvatarId = favorite.FollowerProfile.ProfileAvatarId
                }));
        }

        /// <summary>
        /// Deletes a followed
        /// </summary>
        /// <param name="id">Followed id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> DeleteFollowed(Guid id)
        {
            var record = await _bll.Followers.GetForUpdateAsync(id);

            if (record == null)
            {
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorNotFound));
            }

            if (record.FollowerProfileId != User.UserId())
            {
                return BadRequest(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorAccessDenied));
            }

            _bll.Followers.Remove(id);
            await _bll.SaveChangesAsync();

            return NotFound(new OkResponseDTO() {Status = Resourses.BLL.App.DTO.Common.SuccessDeleted});
        }
    }
}