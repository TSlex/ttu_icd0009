using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PublicApi.DTO.v1;
using PublicApi.DTO.v1.Response;

namespace WebApp.ApiControllers._1._0
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RanksController : ControllerBase
    {
        private readonly IAppBLL _bll;

        public RanksController(IAppBLL bll)
        {
            _bll = bll;
        }
        
        [AllowAnonymous]
        [HttpGet("{username}/all")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<RankDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> GetUserRanks(string username)
        {
            var user = await _bll.Profiles.FindByUsernameAsync(username);

            if (user == null)
            {
                return NotFound(new ErrorResponseDTO("User is not found!"));
            }
            
            return Ok((await _bll.ProfileRanks.AllUserAsync(user.Id)).Select(rank => new RankDTO()
            {
                RankTitle = rank.Rank.RankTitle,
                RankDescription = rank.Rank.RankDescription,
                RankIcon = rank.Rank.RankIcon,
                RankColor = rank.Rank.RankColor,
                RankTextColor = rank.Rank.RankTextColor,
                MinExperience = rank.Rank.MinExperience,
                MaxExperience = rank.Rank.MaxExperience,
            }));
        }
        
        [AllowAnonymous]
        [HttpGet("{username}/active")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RankDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> GetUserActiveRank(string username)
        {
            var user = await _bll.Profiles.FindByUsernameAsync(username);

            if (user == null)
            {
                return NotFound(new ErrorResponseDTO("User is not found!"));
            }
            
            return Ok((await _bll.ProfileRanks.AllUserAsync(user.Id))
                .Select(rank => rank.Rank)
                .OrderByDescending(rank => rank.MaxExperience)
                .Where(rank => rank.MinExperience <= user.Experience)
                .Take(1)
                .Select(rank => new RankDTO()
            {
                RankTitle = rank.RankTitle,
                RankDescription = rank.RankDescription,
                RankIcon = rank.RankIcon,
                RankColor = rank.RankColor,
                RankTextColor = rank.RankTextColor,
                MinExperience = rank.MinExperience,
                MaxExperience = rank.MaxExperience,
            }));
        }
    }
}
