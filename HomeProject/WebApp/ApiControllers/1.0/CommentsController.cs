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
    public class CommentsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly DTOMapper<Comment, CommentGetDTO> _mapper;

        public CommentsController(IAppBLL bll)
        {
            _bll = bll;
            _mapper = new DTOMapper<Comment, CommentGetDTO>();
        }

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
                return NotFound(new ErrorResponseDTO("Post was not found!"));
            }

            return Ok((await _bll.Comments.AllByIdPageAsync(post.Id, pageNumber, 10)).Select(comment =>
                new CommentGetDTO
                {
                    UserName = comment.Profile.UserName,
                    ProfileAvatarUrl = comment.Profile.ProfileAvatarUrl,
                    CommentValue = comment.CommentValue,
                    CommentDateTime = comment.CommentDateTime
                }));
        }

        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CommentGetDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> PostComment([FromBody] CommentCreateDTO comment)
        {
            if (TryValidateModel(comment))
            {
                var result = _bll.Comments.Add(new Comment()
                {
                    ProfileId = User.UserId(),
                    CommentValue = comment.CommentValue,
                    PostId = comment.PostId
                });

                await _bll.SaveChangesAsync();

                return CreatedAtAction("GetPostsComments", _mapper.Map(result));
            }

            return BadRequest(new ErrorResponseDTO("Comment is invalid"));
        }
        
        [HttpPut("{id}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> PutComment(Guid id, [FromBody] CommentEditDTO comment)
        {
            var record = await _bll.Comments.FindAsync(id);

            if (record == null)
            {
                return NotFound(new ErrorResponseDTO("Comment was not found!"));
            }

            var post = await _bll.Posts.FindAsync(comment.PostId);
            
            if (post == null)
            {
                return NotFound(new ErrorResponseDTO("Post was not found!"));
            }

            if (comment.Id != id)
            {
                return NotFound(new ErrorResponseDTO("Ids should math!"));
            }

            if (record.ProfileId != User.UserId())
            {
                return BadRequest(new ErrorResponseDTO("Access denied!"));
            }

            if (TryValidateModel(comment))
            {
                record.CommentValue = comment.CommentValue;

                await _bll.Comments.UpdateAsync(record);
                await _bll.SaveChangesAsync();

                return NoContent();
            }

            return BadRequest(new ErrorResponseDTO("Comment is invalid"));
        }

        [HttpDelete("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommentGetDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            var comment = await _bll.Comments.FindAsync(id);

            if (comment == null)
            {
                return NotFound(new ErrorResponseDTO("Comment was not found!"));
            }

            throw new NotImplementedException();
        }
    }
}