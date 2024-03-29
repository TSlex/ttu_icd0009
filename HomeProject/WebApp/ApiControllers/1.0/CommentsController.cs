using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.BLL.App;
using Extension;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PublicApi.DTO.v1;
using PublicApi.DTO.v1.Mappers;
using PublicApi.DTO.v1.Response;

namespace WebApp.ApiControllers._1._0
{
    /// <summary>
    /// Comments
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CommentsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly IConfiguration _configuration;
        private readonly DTOMapper<Comment, CommentGetDTO> _mapper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bll">Application Bll</param>
        /// <param name="configuration"></param>
        public CommentsController(IAppBLL bll, IConfiguration configuration)
        {
            _bll = bll;
            _configuration = configuration;
            _mapper = new DTOMapper<Comment, CommentGetDTO>();
        }

        /// <summary>
        /// Get Post Comments
        /// </summary>
        /// <param name="postId">Post id</param>
        /// <param name="pageNumber">Number of page</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{postId}/{pageNumber}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CommentGetDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> GetPostsComments(Guid postId, int pageNumber)
        {
            var post = await _bll.Posts.FindAsync(postId);

            if (post == null)
            {
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorNotFound));
            }

            return Ok((await _bll.Comments.AllByIdPageAsync(post.Id, pageNumber, 20)).Select(comment =>
                new CommentGetDTO
                {
                    Id = comment.Id,
                    ProfileAvatarId = comment.Profile!.ProfileAvatarId,
                    UserName = comment.Profile!.UserName,
                    CommentValue = comment.CommentValue,
                    CommentDateTime = comment.CommentDateTime
                }));
        }
        
        /// <summary>
        /// Get comment by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommentGetDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> GetComment(Guid id)
        {
            var comment = await _bll.Comments.FindAsync(id);

            if (comment == null)
            {
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorNotFound));
            }

            return Ok(new CommentGetDTO
            {
                Id = comment.Id,
                ProfileAvatarId = comment.Profile!.ProfileAvatarId,
                UserName = comment.Profile!.UserName,
                CommentValue = comment.CommentValue,
                CommentDateTime = comment.CommentDateTime
            });
        }

        /// <summary>
        /// Creates a new post
        /// </summary>
        /// <param name="comment">Comment body</param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CommentGetDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> PostComment([FromBody] CommentCreateDTO comment)
        {
            var post = await _bll.Posts.FindAsync(comment.PostId);

            if (post == null)
            {
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorNotFound));
            }
            
            if (TryValidateModel(comment))
            {
                var result = _bll.Comments.Add(new Comment()
                {
                    ProfileId = User.UserId(),
                    CommentValue = comment.CommentValue,
                    PostId = comment.PostId
                });

                await _bll.SaveChangesAsync();
                
                await _bll.Ranks.IncreaseUserExperience(User.UserId(),
                    _configuration.GetValue<int>("Rank:CommentExperience"));

                return CreatedAtAction("GetPostsComments", _mapper.Map(result));
            }

            return BadRequest(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorBadData));
        }
        
        /// <summary>
        /// Updates a comment
        /// </summary>
        /// <param name="id">Comment id</param>
        /// <param name="comment">Comment body</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> PutComment(Guid id, [FromBody] CommentEditDTO comment)
        {
            var record = await _bll.Comments.GetForUpdateAsync(id);

            if (record == null)
            {
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorNotFound));
            }

            if (comment.Id != id)
            {
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorIdMatch));
            }

            if (record.ProfileId != User.UserId())
            {
                return BadRequest(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorAccessDenied));
            }

            if (TryValidateModel(comment))
            {
                record.CommentValue = comment.CommentValue;

                await _bll.Comments.UpdateAsync(record);
                await _bll.SaveChangesAsync();

                return NoContent();
            }

            return BadRequest(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorBadData));
        }

        /// <summary>
        /// Deletes a comment
        /// </summary>
        /// <param name="id">Comments id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            var record = await _bll.Comments.GetForUpdateAsync(id);

            if (record == null)
            {
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorNotFound));
            }
            
            var post = await _bll.Posts.GetForUpdateAsync(record.PostId);

            if (!ValidateUserAccess(record) && !(post != null && post.ProfileId == User.UserId()))
            {
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorAccessDenied));
            }
            
            _bll.Comments.Remove(record.Id);
            await _bll.SaveChangesAsync();

            return Ok(new OkResponseDTO() {Status = Resourses.BLL.App.DTO.Common.SuccessDeleted});
        }
        
        private bool ValidateUserAccess(Comment? record)
        {
            return record != null && record.ProfileId == User.UserId();
        }
    }
}