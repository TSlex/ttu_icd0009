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
        [HttpGet("/{postId}/count")]
        public async Task<IActionResult> GetPostFavoritesCount(Guid postId)
        {
            var post = _bll.Posts.GetNoIncludes(postId);

            if (post == null)
            {
                return NotFound();
            }

            return Ok(new CountResponseDTO()
                {Count = await _bll.Favorites.CountByIdAsync(nameof(Favorite.PostId), postId)});
        }
        
        [AllowAnonymous]
        [HttpGet("/{postId}/{pageNumber}")]
        public async Task<IActionResult> GetPostFavorites(Guid postId, int pageNumber)
        {
            var post = _bll.Posts.GetNoIncludes(postId);

            if (post == null)
            {
                return NotFound();
            }

            return Ok(await _bll.Favorites.AllByIdPageAsync(pageNumber, 10, nameof(Favorite.PostId), postId));
        }
    }
}