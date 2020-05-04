using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.BLL.App;
using Extension;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class ProfilesController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly DTOMapper<ProfileFull, ProfileDTO> _mapper;

        public ProfilesController(IAppBLL bll)
        {
            _bll = bll;
            _mapper = new DTOMapper<ProfileFull, ProfileDTO>();
        }
        
        [AllowAnonymous]
        [HttpGet("{username}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProfileDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> GetProfile(string username)
        {
            var user = await _bll.Profiles.FindByUsernameAsync(username);
            
            if (user == null)
            {
                return NotFound();
            }

            if (User.Identity.IsAuthenticated && User.UserId() != user.Id)
            {
                user.IsUserBlocked = await _bll.BlockedProfiles.FindAsync(user.Id, User.UserId()) != null;
                user.IsUserFollows = await _bll.Followers.FindAsync(User.UserId(), user.Id) != null;
                user.IsUserBlocks = await _bll.BlockedProfiles.FindAsync(User.UserId(), user.Id) != null;   
            }
            
            return Ok(_mapper.Map(user));
        }

        [HttpPost("{username}/follow")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> FollowProfile(string username)
        {
            var user = await _bll.Profiles.FindByUsernameAsync(username);
            
            if (user == null)
            {
                return NotFound(new ErrorResponseDTO("User was not found!"));
            }

            if (user.Id == User.UserId())
            {
                return BadRequest(new ErrorResponseDTO("You cannot follow yourself!"));
            }
            
            var property = await _bll.BlockedProfiles.FindAsync(user.Id, User.UserId());

            if (property != null)
            {
                return BadRequest(new ErrorResponseDTO("You cannot follow a user that blocks you!"));
            }
            
            var subscription = await _bll.Followers.FindAsync(User.UserId(), user.Id);

            if (subscription == null)
            {
                _bll.Followers.AddSubscription(User.UserId(), user.Id);
                await _bll.SaveChangesAsync();
                
                return Ok(new OkResponseDTO() {Status = $"You are subscribed to {username}"});
            }
            
            return Ok(new OkResponseDTO() {Status = $"You are already subscribed to {username}"});
        }

        [HttpPost("{username}/unfollow")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> UnfollowProfile(string username)
        {
            var user = await _bll.Profiles.FindByUsernameAsync(username);
            
            if (user == null)
            {
                return NotFound(new ErrorResponseDTO("User was not found!"));
            }

            if (user.Id == User.UserId())
            {
                return BadRequest(new ErrorResponseDTO("You cannot unfollow yourself!"));
            }

            var subscription = await _bll.Followers.FindAsync(User.UserId(), user.Id);

            if (subscription != null)
            {
                _bll.Followers.Remove(subscription);
                await _bll.SaveChangesAsync();
                return Ok(new OkResponseDTO() {Status = $"You are no longer subscribed to {username}"});
            }
            
            return Ok(new OkResponseDTO() {Status = $"You are not subscribed to {username}"});
        }

        [HttpPost("{username}/block")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> BlockProfile(string username)
        {
            var user = await _bll.Profiles.FindByUsernameAsync(username);
            
            if (user == null)
            {
                return NotFound(new ErrorResponseDTO("User was not found!"));
            }

            if (user.Id == User.UserId())
            {
                return BadRequest(new ErrorResponseDTO("You cannot block yourself!"));
            }

            var property = await _bll.BlockedProfiles.FindAsync(User.UserId(), user.Id);
            var subscription = await _bll.Followers.FindAsync(User.UserId(), user.Id);

            if (property == null)
            {
                if (subscription != null)
                {
                    _bll.Followers.Remove(subscription);
                }
                _bll.BlockedProfiles.AddBlockProperty(User.UserId(), user.Id);
                await _bll.SaveChangesAsync();
                return Ok(new OkResponseDTO() {Status = $"{username} was blocked"});
            }
            
            return Ok(new OkResponseDTO() {Status = $"{username} is already blocked"});
        }

        [HttpPost("{username}/unblock")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> UnblockProfile(string username)
        {
            var user = await _bll.Profiles.FindByUsernameAsync(username);
            
            if (user == null)
            {
                return NotFound(new ErrorResponseDTO("User was not found!"));
            }

            if (user.Id == User.UserId())
            {
                return BadRequest(new ErrorResponseDTO("You cannot unblock yourself!"));
            }
            
            var property = await _bll.BlockedProfiles.FindAsync(User.UserId(), user.Id);

            if (property != null)
            {
                _bll.BlockedProfiles.Remove(property);
                await _bll.SaveChangesAsync();
                return Ok(new OkResponseDTO() {Status = $"{username} was unblocked"});
            }
            
            return Ok(new OkResponseDTO() {Status = $"{username} is not blocked"});
        }
    }
}