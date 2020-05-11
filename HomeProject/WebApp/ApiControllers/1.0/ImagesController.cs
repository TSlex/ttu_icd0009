using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.BLL.App;
using DAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PublicApi.DTO.v1;
using PublicApi.DTO.v1.Response;

namespace WebApp.ApiControllers._1._0
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ImagesController : ControllerBase
    {
        private readonly IAppBLL _bll;

        public ImagesController(IAppBLL bll)
        {
            _bll = bll;
        }

        [AllowAnonymous]
        [HttpGet("{id?}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetImage(string? id)
        {
            if (id == null)
            {
                return base.File("~/localstorage/images/misc/404.png", "image/jpeg");
            }

            Image image;

            try
            {
                image = await _bll.Images.FindAsync(new Guid(id));

                if (image == null)
                {
                    return base.File("~/localstorage/images/misc/404.png", "image/jpeg");
                }
            }
            catch (Exception)
            {
                return base.File("~/localstorage/images/misc/404.png", "image/jpeg");
            }

            Request.Headers.Add("imageId", id.ToString());
            return base.File("~/localstorage" + image.ImageUrl, "image/jpeg");
        }

        [AllowAnonymous]
        [HttpGet("profile/{username}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> GetProfileImage(string username)
        {
            var user = await _bll.Profiles.FindByUsernameAsync(username);

            if (user == null)
            {
                return NotFound(new ErrorResponseDTO("User was not found!"));
            }

            if (user.ProfileAvatarId == null)
            {
                return base.File("~/localstorage/images/misc/404.png", "image/jpeg");
            }

            var image = await _bll.Images.FindAsync((Guid) user.ProfileAvatarId);

            if (image == null)
            {
                return base.File("~/localstorage/images/misc/404.png", "image/jpeg");
            }

            return base.File("~/localstorage" + image.ImageUrl, "image/jpeg");
        }

        [AllowAnonymous]
        [HttpGet("post/{postId}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> GetPostImage(Guid postId)
        {
            var post = await _bll.Posts.GetNoIncludes(postId, null);

            if (post == null)
            {
                return NotFound(new ErrorResponseDTO("Post was not found!"));
            }

            if (post.PostImageId == null)
            {
                return base.File("~/localstorage/images/misc/404.png", "image/jpeg");
            }

            var image = await _bll.Images.FindAsync((Guid) post.PostImageId);

            if (image == null)
            {
                return base.File("~/localstorage/images/misc/404.png", "image/jpeg");
            }

            return base.File("~/localstorage" + image.ImageUrl, "image/jpeg");
        }

        [AllowAnonymous]
        [HttpGet("gift/{giftCode}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> GetGiftImage(string giftCode)
        {
            var gift = await _bll.Gifts.FindByCodeAsync(giftCode);

            if (gift == null)
            {
                return NotFound(new ErrorResponseDTO("Gift was not found!"));
            }

            if (gift.GiftImageId == null)
            {
                return base.File("~/localstorage/images/misc/404.png", "image/jpeg");
            }

            var image = await _bll.Images.FindAsync((Guid) gift.GiftImageId);

            if (image == null)
            {
                return base.File("~/localstorage/images/misc/404.png", "image/jpeg");
            }

            return base.File("~/localstorage" + image.ImageUrl, "image/jpeg");
        }
    }
}