using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.BLL.App;
using DAL;
using Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PublicApi.DTO.v1;
using PublicApi.DTO.v1.Response;

namespace WebApp.ApiControllers._1._0
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FollowersController : ControllerBase
    {
        private readonly IAppBLL _bll;

        public FollowersController(IAppBLL bll)
        {
            _bll = bll;
        }

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
                return NotFound(new ErrorResponseDTO("User was not found!"));
            }

            return Ok(new CountResponseDTO()
                {Count = await _bll.Followers.CountByIdAsync(user.Id, true)});
        }

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
                return NotFound(new ErrorResponseDTO("User was not found!"));
            }

            return Ok((await _bll.Followers.AllByIdPageAsync(user.Id, true, pageNumber, 10)).Select(favorite =>
                new FollowerProfileDTO
                {
                    UserName = favorite.Profile.UserName,
                    ProfileAvatarUrl = favorite.Profile.ProfileAvatarUrl
                }));
        }

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
                return NotFound(new ErrorResponseDTO("User was not found!"));
            }

            return Ok(new CountResponseDTO()
                {Count = await _bll.Followers.CountByIdAsync(user.Id, false)});
        }

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
                return NotFound(new ErrorResponseDTO("User was not found!"));
            }

            return Ok((await _bll.Followers.AllByIdPageAsync(user.Id, false, pageNumber, 10)).Select(favorite =>
                new FollowerProfileDTO
                {
                    UserName = favorite.Profile.UserName,
                    ProfileAvatarUrl = favorite.Profile.ProfileAvatarUrl
                }));
        }
    }
}