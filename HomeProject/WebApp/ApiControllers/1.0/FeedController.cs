using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts.BLL.App;
using Extension;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PublicApi.DTO.v1;
using PublicApi.DTO.v1.Mappers;
using PublicApi.DTO.v1.Response;
using Post = BLL.App.DTO.Post;
using Profile = Domain.Profile;

namespace WebApp.ApiControllers._1._0
{
    /// <summary>
    /// Home page posts
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FeedController : ControllerBase
    {
        private readonly IAppBLL _bll;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bll">Application Bll</param>
        public FeedController(IAppBLL bll)
        {
            _bll = bll;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("count")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CountResponseDTO))]
        public async Task<IActionResult> GetPostsCount()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Ok(new CountResponseDTO() {Count = await _bll.Feeds.GetUserCount(User.UserId())});
            }

            return Ok(new CountResponseDTO() {Count = await _bll.Feeds.GetCount()});
        }

        /// <summary>
        /// Get posts for specific user subscriptions (including own posts) or all posts of all users
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{pageNumber}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PostGetDTO>))]
        public async Task<IActionResult> GetPosts(int pageNumber)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Ok((await _bll.Feeds.GetUser10ByPage(User.UserId(), pageNumber))
                    .Posts.Select(Map));
            }

            return Ok((await _bll.Feeds.Get10ByPage(pageNumber))
                .Posts.Select(Map));
        }

        private static PostGetDTO Map(Post inObj)
        {
            return new PostGetDTO()
            {
                 Id = inObj.Id,
                 IsFavorite = inObj.IsUserFavorite,
                 PostDescription = inObj.PostDescription,
                 PostTitle = inObj.PostTitle,
                 ProfileUsername = inObj.Profile.UserName,
                 PostCommentsCount = inObj.PostCommentsCount,
                 PostFavoritesCount = inObj.PostFavoritesCount,
                 PostImageId = inObj.PostImageId,
                 PostImageUrl = inObj.PostImageUrl,
                 PostPublicationDateTime = inObj.PostPublicationDateTime
            };
        }
    }
}