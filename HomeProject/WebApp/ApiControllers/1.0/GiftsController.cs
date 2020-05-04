using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.BLL.App;
using Extension;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PublicApi.DTO.v1;
using PublicApi.DTO.v1.Response;

namespace WebApp.ApiControllers._1._0
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GiftsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly IConfiguration _configuration;

        public GiftsController(IAppBLL bll, IConfiguration configuration)
        {
            _bll = bll;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpGet("{pageNumber}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GiftDTO>))]
        public async Task<IActionResult> GetAllGifts(int pageNumber)
        {
            return Ok((await _bll.Gifts.Get10ByPageAsync(pageNumber)).Select(
                gift => new GiftDTO()
                {
                    GiftName = gift.GiftName,
                    GiftCode = gift.GiftCode,
                    GiftImageId = gift.GiftImageId,
                    GiftImageUrl = gift.GiftImageUrl,
                    Price = gift.Price,
                }));
        }

        [AllowAnonymous]
        [HttpGet("count")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GiftsCountDTO>))]
        public async Task<IActionResult> GetAllGiftsCount()
        {
            return Ok(new GiftsCountDTO()
            {
                Count = (await _bll.Gifts.GetCountAsync()).Count
            });
        }

        [AllowAnonymous]
        [HttpGet("{username}/{pageNumber}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GiftDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> GetUserGifts(string username, int pageNumber)
        {
            var user = await _bll.Profiles.FindByUsernameAsync(username);

            if (user == null)
            {
                return NotFound(new ErrorResponseDTO("User is not found!"));
            }

            return Ok((await _bll.ProfileGifts.GetUser10ByPageAsync(user.Id, pageNumber)).Select(gift => gift.Gift).Select(
                gift => new GiftDTO()
                {
                    GiftName = gift.GiftName,
                    GiftCode = gift.GiftCode,
                    GiftImageId = gift.GiftImageId,
                    GiftImageUrl = gift.GiftImageUrl,
                    Price = gift.Price,
                }));
        }

        [AllowAnonymous]
        [HttpGet("{username}/count")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GiftsCountDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> GetUserGiftsCount(string username)
        {
            var user = await _bll.Profiles.FindByUsernameAsync(username);

            if (user == null)
            {
                return NotFound(new ErrorResponseDTO("User is not found!"));
            }

            return Ok(new GiftsCountDTO()
            {
                Count = (await _bll.ProfileGifts.GetUserCountAsync(user.Id)).Count
            });
        }

        [HttpPost("{username}/send")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> SendGiftToUser(string username, [Required] string giftCode)
        {
            var user = await _bll.Profiles.FindByUsernameAsync(username);

            if (user == null)
            {
                return NotFound(new ErrorResponseDTO("User is not found!"));
            }
            
            if (user.Id == User.UserId())
            {
                return NotFound(new ErrorResponseDTO("You cannot send gift to yourself!"));
            }
            
            var gift = await _bll.Gifts.FindByCodeAsync(giftCode);
            
            if (gift == null)
            {
                return BadRequest(new ErrorResponseDTO("Gift code is invalid!"));
            }

            _bll.ProfileGifts.Add(new ProfileGift()
            {
                ProfileId = user.Id,
                GiftId = gift.Id,
                Price = gift.Price,
                GiftDateTime = DateTime.Now
            });
            
            await _bll.Ranks.IncreaseUserExperience(User.UserId(), 
                _configuration.GetValue<int>("Rank:GiftSendExperience"));

            return Ok(new OkResponseDTO
            {
                Status = "Gift was send C:"
            });
        }
    }
}