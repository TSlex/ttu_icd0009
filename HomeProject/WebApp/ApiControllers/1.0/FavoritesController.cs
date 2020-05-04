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
using PublicApi.DTO.v1.Mappers;
using PublicApi.DTO.v1.Response;

namespace WebApp.ApiControllers._1._0
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FavoritesController : ControllerBase
    {
        private readonly IAppBLL _bll;

        public FavoritesController(IAppBLL bll)
        {
            _bll = bll;
        }

        [AllowAnonymous]
        [HttpGet("{postId}/count")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CountResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> GetPostFavoritesCount(Guid postId)
        {
            var post = _bll.Posts.GetNoIncludes(postId);

            if (post == null)
            {
                return NotFound(new ErrorResponseDTO("Post was not found!"));
            }

            return Ok(new CountResponseDTO()
                {Count = await _bll.Favorites.CountByIdAsync(postId)});
        }

        [AllowAnonymous]
        [HttpGet("{postId}/{pageNumber}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<FavoriteProfileDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> GetPostFavorites(Guid postId, int pageNumber)
        {
            var post = _bll.Posts.GetNoIncludes(postId);

            if (post == null)
            {
                return NotFound(new ErrorResponseDTO("Post was not found!"));
            }

            return Ok((await _bll.Favorites.AllByIdPageAsync(postId, pageNumber, 10)).Select(favorite =>
                new FavoriteProfileDTO
                {
                    UserName = favorite.Profile.UserName,
                    ProfileAvatarUrl = favorite.Profile.ProfileAvatarUrl
                }));
        }
    }
}