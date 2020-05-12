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
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PostsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly DTOMapper<Post, PostGetDTO> _postGetMapper;

        public PostsController(IAppBLL bll)
        {
            _bll = bll;
            _postGetMapper = new DTOMapper<Post, PostGetDTO>();
        }

        [AllowAnonymous]
        [HttpGet("{id}/fav_count")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CountResponseDTO))]
        public async Task<IActionResult> GetFavoritesCount(Guid id)
        {
            var post = await _bll.Posts.GetForUpdateAsync(id);

            if (post == null)
            {
                return NotFound(new ErrorResponseDTO("Post was not found!"));
            }

            return Ok(new CountResponseDTO() {Count = await _bll.Posts.GetFavoritesCount(id)});
        }

        [AllowAnonymous]
        [HttpGet("{id}/comments_count")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CountResponseDTO))]
        public async Task<IActionResult> GetCommentsCount(Guid id)
        {
            var post = await _bll.Posts.GetForUpdateAsync(id);

            if (post == null)
            {
                return NotFound(new ErrorResponseDTO("Post was not found!"));
            }

            return Ok(new CountResponseDTO() {Count = await _bll.Posts.GetCommentsCount(id)});
        }

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
                return NotFound(new ErrorResponseDTO("User was not found!"));
            }

            return Ok(new CountResponseDTO() {Count = await _bll.Posts.GetByUserCount(user.Id)});
        }

        //crud========================================================

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
                return NotFound(new ErrorResponseDTO("User was not found!"));
            }
            
            if (User.Identity.IsAuthenticated)
            {
                return Ok((await _bll.Posts.GetUser10ByPage(user.Id, pageNumber, User.UserId()))
                    .Select(Map));
            }

            return Ok((await _bll.Posts.GetUser10ByPage(user.Id, pageNumber, null))
                .Select(Map));
        }

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
                return NotFound(new ErrorResponseDTO("Post was not found!"));
            }

            return Ok(Map(post));
        }

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
                    PostImageId = null,
                    ProfileId = User.UserId(),
                    PostTitle = post.PostTitle,
                    PostDescription = post.PostDescription,
                    PostImageUrl = post.PostImageUrl
                });

                await _bll.SaveChangesAsync();

                return CreatedAtAction("GetPost", _postGetMapper.Map(result));
            }

            return BadRequest(new ErrorResponseDTO("Post is invalid"));
        }

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
                return NotFound(new ErrorResponseDTO("Ids should math!"));
            }

            if (record == null)
            {
                return NotFound(new ErrorResponseDTO("Post was not found!"));
            }

            if (record.ProfileId != User.UserId())
            {
                return BadRequest(new ErrorResponseDTO("Access denied!"));
            }

            if (TryValidateModel(post))
            {
                record.PostTitle = post.PostTitle;
                record.PostDescription = post.PostDescription;

                await _bll.Posts.UpdateAsync(record);
                await _bll.SaveChangesAsync();

                return NoContent();
            }

            return BadRequest(new ErrorResponseDTO("Post is invalid"));
        }

        [HttpDelete("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PostGetDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> DeletePost(Guid id)
        {
            var post = await _bll.Posts.GetForUpdateAsync(id);

            if (post == null)
            {
                return NotFound(new ErrorResponseDTO("Post was not found!"));
            }

            throw new NotImplementedException();
        }

        //============================================================

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
                return NotFound(new ErrorResponseDTO("Post was not found!"));
            }

            var favorite = await _bll.Favorites.FindAsync(post.Id, userId);

            if (favorite == null)
            {
                _bll.Favorites.Create(post.Id, userId);
                await _bll.SaveChangesAsync();

                return Ok(new OkResponseDTO() {Status = "Post in now favorited"});
            }

            return Ok(new OkResponseDTO() {Status = "Post in already favorited"});
        }

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
                return NotFound(new ErrorResponseDTO("Post was not found!"));
            }

            var favorite = await _bll.Favorites.FindAsync(post.Id, userId);

            if (favorite != null)
            {
                await _bll.Favorites.RemoveAsync(post.Id, userId);
                await _bll.SaveChangesAsync();

                return Ok(new OkResponseDTO() {Status = "Post is no longer favorited"});
            }

            return Ok(new OkResponseDTO() {Status = "Post is not favorited"});
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