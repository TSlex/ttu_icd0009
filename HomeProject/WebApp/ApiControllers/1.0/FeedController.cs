using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.BLL.App;
using Domain;
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

namespace WebApp.ApiControllers._1._0
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FeedController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly SignInManager<Profile> _signInManager;
        private readonly DTOMapper<Post, PostGetDTO> _postGetMapper;

        public FeedController(IAppBLL bll, SignInManager<Profile> signInManager)
        {
            _bll = bll;
            _signInManager = signInManager;
            _postGetMapper = new DTOMapper<Post, PostGetDTO>();
        }

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

        [AllowAnonymous]
        [HttpGet("{pageNumber}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PostGetDTO>))]
        public async Task<IActionResult> GetPosts(int pageNumber)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Ok((await _bll.Feeds.GetUser10ByPage(User.UserId(), pageNumber))
                    .Posts.Select(post => _postGetMapper.Map(post)));
            }

            return Ok((await _bll.Feeds.Get10ByPage(pageNumber))
                .Posts.Select(post => _postGetMapper.Map(post)));
        }
    }
}