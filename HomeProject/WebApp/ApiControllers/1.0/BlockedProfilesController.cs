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
    /// User blacklist controller 
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BlockedProfilesController : ControllerBase
    {
        private readonly IAppBLL _bll;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bll">Application Bll</param>
        public BlockedProfilesController(IAppBLL bll)
        {
            _bll = bll;
        }

        /// <summary>
        /// Get Profiles count that User follows
        /// </summary>
        /// <returns>Profiles count that User follows</returns>
        [HttpGet("count")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CountResponseDTO))]
        public async Task<IActionResult> GetUserFollowedCount()
        {
            return Ok(new CountResponseDTO()
                {Count = await _bll.BlockedProfiles.CountByIdAsync(User.UserId())});
        }

        /// <summary>
        /// Get Profiles that User follows
        /// </summary>
        /// <param name="pageNumber">Page number for page system</param>
        /// <returns>Profiles that User follows</returns>
        [HttpGet("{pageNumber}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BlockedProfileDTO>))]
        public async Task<IActionResult> GetUserFollowed(int pageNumber)
        {
            return Ok((await _bll.BlockedProfiles.AllByIdPageAsync(User.UserId(), pageNumber, 10)).Select(favorite =>
                new BlockedProfileDTO
                {
                    UserName = favorite.BProfile!.UserName,
                    ProfileAvatarUrl = favorite.BProfile!.ProfileAvatarUrl
                }));
        }
    }
}