using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.BLL.App;
using Extension;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PublicApi.DTO.v1;
using PublicApi.DTO.v1.Mappers;
using PublicApi.DTO.v1.Response;
using Post = BLL.App.DTO.Post;

namespace WebApp.ApiControllers._1._0
{
    /// <summary>
    /// Posts
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PostsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly DTOMapper<Post, PostGetDTO> _postGetMapper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bll">Application Bll</param>
        public PostsController(IAppBLL bll)
        {
            _bll = bll;
            _postGetMapper = new DTOMapper<Post, PostGetDTO>();
        }

        /// <summary>
        /// Get post favorites count
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{id}/fav_count")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CountResponseDTO))]
        public async Task<IActionResult> GetFavoritesCount(Guid id)
        {
            var post = await _bll.Posts.GetForUpdateAsync(id);

            if (post == null)
            {
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorNotFound));
            }

            return Ok(new CountResponseDTO() {Count = await _bll.Posts.GetFavoritesCount(id)});
        }

        /// <summary>
        /// Get post comments count
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{id}/comments_count")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CountResponseDTO))]
        public async Task<IActionResult> GetCommentsCount(Guid id)
        {
            var post = await _bll.Posts.GetForUpdateAsync(id);

            if (post == null)
            {
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorNotFound));
            }

            return Ok(new CountResponseDTO() {Count = await _bll.Posts.GetCommentsCount(id)});
        }

        /// <summary>
        /// Get user posts count
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{username}/count")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CountResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> GetUserPostCount(string username)
        {
            var user = await _bll.Profiles.FindByUsernameAsync(username);

            if (user == null)
            {
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorUserNotFound));
            }

            return Ok(new CountResponseDTO() {Count = await _bll.Posts.GetByUserCount(user.Id)});
        }

        //crud========================================================

        /// <summary>
        /// Get user posts
        /// </summary>
        /// <param name="username"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{username}/{pageNumber}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PostGetDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> GetUserPosts(string username, int pageNumber)
        {
            var user = await _bll.Profiles.FindByUsernameAsync(username);

            if (user == null)
            {
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorUserNotFound));
            }

            if (User.Identity.IsAuthenticated)
            {
                return Ok((await _bll.Posts.GetUser10ByPage(user.Id, pageNumber, User.UserId()))
                    .Select(Map));
            }

            return Ok((await _bll.Posts.GetUser10ByPage(user.Id, pageNumber, null))
                .Select(Map));
        }

        /// <summary>
        /// Get post by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PostGetDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> GetPost(Guid id)
        {
            Post post;
            if (User.Identity.IsAuthenticated)
            {
                post = await _bll.Posts.GetNoIncludes(id, User.UserId());
            }
            else
            {
                post = await _bll.Posts.GetNoIncludes(id, null);
            }

            if (post == null)
            {
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorNotFound));
            }

            return Ok(Map(post));
        }

        /// <summary>
        /// Creates a post
        /// </summary>
        /// <param name="post">Post body</param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PostGetDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> PostPost([FromBody] PostCreateDTO post)
        {
            if (TryValidateModel(post))
            {
                var result = _bll.Posts.Add(new Post()
                {
                    Id = post.Id,
                    PostTitle = post.PostTitle,
                    PostDescription = post.PostDescription,
                    PostImageId = post.PostImageId,
                    ProfileId = User.UserId(),
                });
                await _bll.SaveChangesAsync();

                return CreatedAtAction("GetPost", _postGetMapper.Map(result));
            }

            return BadRequest(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorBadData));
        }

        /// <summary>
        /// Updates a post
        /// </summary>
        /// <param name="id">Post id</param>
        /// <param name="post">Post body</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> PutPost(Guid id, [FromBody] PostEditDTO post)
        {
            var record = await _bll.Posts.GetForUpdateAsync(id);

            if (post.Id != id)
            {
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorIdMatch));
            }

            if (record == null)
            {
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorNotFound));
            }

            if (record.ProfileId != User.UserId())
            {
                return BadRequest(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorAccessDenied));
            }

            if (TryValidateModel(post))
            {
                record.PostTitle = post.PostTitle;
                record.PostDescription = post.PostDescription;

                await _bll.Posts.UpdateAsync(record);
                await _bll.SaveChangesAsync();

                return NoContent();
            }

            return BadRequest(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorBadData));
        }

        /// <summary>
        /// Deletes post
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> DeletePost(Guid id)
        {
            var record = await _bll.Posts.GetForUpdateAsync(id);

            if (!ValidateUserAccess(record))
            {
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorNotFound));
            }

            _bll.Posts.Remove(id);
            await _bll.SaveChangesAsync();

            return Ok(new OkResponseDTO() {Status = Resourses.BLL.App.DTO.Common.SuccessDeleted});
        }

        private bool ValidateUserAccess(Post? record)
        {
            return record != null && record.ProfileId == User.UserId();
        }

        //============================================================

        /// <summary>
        /// Add post to favorites
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("{id}/favorite")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> AddToFavorites(Guid id)
        {
            var userId = User.UserId();
            var post = await _bll.Posts.GetForUpdateAsync(id);

            if (post == null)
            {
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorNotFound));
            }

            var favorite = await _bll.Favorites.FindAsync(post.Id, userId);

            if (favorite == null)
            {
                _bll.Favorites.Create(post.Id, userId);
                await _bll.SaveChangesAsync();

                return Ok(new OkResponseDTO() {Status = Resourses.BLL.App.DTO.Posts.Posts.ResponseNowFavorited});
            }

            return Ok(new OkResponseDTO() {Status = Resourses.BLL.App.DTO.Posts.Posts.ResponseAlreadyFavorited});
        }

        /// <summary>
        /// Removes post from favorites
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("{id}/unfavorite")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> RemoveFromFavorite(Guid id)
        {
            var userId = User.UserId();
            var post = await _bll.Posts.GetForUpdateAsync(id);

            if (post == null)
            {
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorNotFound));
            }

            var favorite = await _bll.Favorites.FindAsync(post.Id, userId);

            if (favorite != null)
            {
                await _bll.Favorites.RemoveAsync(post.Id, userId);
                await _bll.SaveChangesAsync();

                return Ok(new OkResponseDTO() {Status = Resourses.BLL.App.DTO.Posts.Posts.ResponseNowUnFavorited});
            }

            return Ok(new OkResponseDTO() {Status = Resourses.BLL.App.DTO.Posts.Posts.ResponseNotFavorited});
        }

        private static PostGetDTO Map(Post inObj)
        {
            return new PostGetDTO()
            {
                Id = inObj.Id,
                IsFavorite = inObj.IsUserFavorite,
                PostDescription = inObj.PostDescription,
                PostTitle = inObj.PostTitle,
                ProfileUsername = inObj.Profile!.UserName,
                PostCommentsCount = inObj.PostCommentsCount,
                PostFavoritesCount = inObj.PostFavoritesCount,
                PostImageId = inObj.PostImageId,
                PostPublicationDateTime = inObj.PostPublicationDateTime
            };
        }
    }
}