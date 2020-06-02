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
    /// <summary>
    /// Gifts and profile gifts
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GiftsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bll">Application Bll</param>
        /// <param name="configuration">appsettings.json</param>
        public GiftsController(IAppBLL bll, IConfiguration configuration)
        {
            _bll = bll;
            _configuration = configuration;
        }

        /// <summary>
        /// Get all gifts than are available
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
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
                    Price = gift.Price,
                }));
        }

        /// <summary>
        /// Get all giftt count than are available
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Get profile gifts
        /// </summary>
        /// <param name="username"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{username}/{pageNumber}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProfileGiftDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> GetUserGifts(string username, int pageNumber)
        {
            var user = await _bll.Profiles.FindByUsernameAsync(username);

            if (user == null)
            {
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorUserNotFound));
            }

            return Ok((await _bll.ProfileGifts.GetUser10ByPageAsync(user.Id, pageNumber)).Take(5)
                .Select(
                    gift => new ProfileGiftDTO()
                    {
                        Id = gift.Id,
                        GiftName = gift.Gift.GiftName,
                        Username = gift.Profile.UserName,
                        FromUsername = gift.FromProfile?.UserName,
                        Message = gift.Message,
                        Price = gift.Price,
                        GiftDateTime = gift.GiftDateTime,
                        ImageId = gift.Gift.GiftImageId
                    }));
        }

        /// <summary>
        /// Get profile gifts count
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
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
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorNotFound));
            }

            return Ok(new GiftsCountDTO()
            {
                Count = (await _bll.ProfileGifts.GetUserCountAsync(user.Id)).Count
            });
        }

        /// <summary>
        /// Send the gift by giftcode to a profile with specified username
        /// </summary>
        /// <param name="username"></param>
        /// <param name="profileGift"></param>
        /// <returns></returns>
        [HttpPost("{username}/send")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> SendGiftToUser(string username, ProfileGiftCreateDTO profileGift)
        {
            var user = await _bll.Profiles.FindByUsernameAsync(username);
            var fromUser = await _bll.Profiles.FindByUsernameAsync(profileGift.FromUsername);

            if (user == null || user.UserName != profileGift.Username)
            {
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorUserNotFound));
            }

            if (user.Id == User.UserId())
            {
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Gifts.Gifts.ErrorGiftToYourself));
            }

            var gift = await _bll.Gifts.FindByCodeAsync(profileGift.GiftCode);

            if (gift == null)
            {
                return BadRequest(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorBadData));
            }

            _bll.ProfileGifts.Add(new ProfileGift()
            {
                ProfileId = user.Id,
                FromProfileId = fromUser?.Id,
                GiftId = gift.Id,
                Price = gift.Price,
                GiftDateTime = DateTime.Now,
                Message = profileGift.Message,
            });

            await _bll.Ranks.IncreaseUserExperience(User.UserId(),
                _configuration.GetValue<int>("Rank:GiftSendExperience"));

            return Ok(new OkResponseDTO
            {
                Status = "OK"
            });
        }

        /// <summary>
        /// Get profile gift by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("profileGift/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProfileGiftDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> GetProfileGift(Guid id)
        {
            var profileGift = await _bll.ProfileGifts.FindAsync(id);

            if (profileGift == null)
            {
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorNotFound));
            }

            return Ok(new ProfileGiftDTO
            {
                Id = profileGift.Id,
                GiftName = profileGift.Gift.GiftName,
                Username = profileGift.Profile.UserName,
                FromUsername = profileGift.FromProfile.UserName,
                Message = profileGift.Message,
                Price = profileGift.Price,
                GiftDateTime = profileGift.GiftDateTime,
                ImageId = profileGift.Gift.GiftImageId
            });
        }

        /// <summary>
        /// Deletes a gift
        /// </summary>
        /// <param name="id">Profile gift id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            var record = await _bll.ProfileGifts.GetForUpdateAsync(id);

            if (record == null || record.ProfileId != User.UserId())
            {
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorNotFound));
            }

            _bll.ProfileGifts.Remove(id);
            await _bll.SaveChangesAsync();

            return Ok(new OkResponseDTO() {Status = Resourses.BLL.App.DTO.Common.SuccessDeleted});
        }
    }
}