using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.BLL.App;
using DAL;
using Domain.Enums;
using Extension;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using PublicApi.DTO.v1;
using PublicApi.DTO.v1.Mappers;
using PublicApi.DTO.v1.Response;

namespace WebApp.ApiControllers._1._0
{
    /// <summary>
    /// Images - for avatars, posts, gifts and etc...
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ImagesController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly DTOMapper<Image, ImageDTO> _mapper;
            
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bll">Application Bll</param>
        public ImagesController(IAppBLL bll)
        {
            _bll = bll;
            _mapper = new DTOMapper<Image, ImageDTO>();
        }

        /// <summary>
        /// Get image by it's id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        
        /// <summary>
        /// Get original image version by it's id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{id?}/original")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOriginalImage(string? id)
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
            return base.File("~/localstorage" + image.OriginalImageUrl, "image/jpeg");
        }
        
        /// <summary>
        /// Create a new image and returns Id
        /// </summary>
        /// <param name="imageDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ImageDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> PostImage([FromBody] ImagePostDTO imageDTO)
        {
            if (imageDTO.ImageType != ImageType.Post && imageDTO.ImageType != ImageType.ProfileAvatar)
            {
                return BadRequest(new ErrorResponseDTO("Access denied!"));
            }

            if (imageDTO.ImageFile == null)
            {
                return BadRequest(new ErrorResponseDTO("Image should be specified!"));
            }

            if (imageDTO.ImageFor == null)
            {
                ModelState.AddModelError(string.Empty, "Missing ImageFor");
            }

            var modelId = Guid.NewGuid();

            if (ModelState.IsValid)
            {
                var image = new Image()
                {
                    Id = modelId,
                    HeightPx = imageDTO.HeightPx,
                    WidthPx = imageDTO.WidthPx,
                    PaddingTop = imageDTO.PaddingTop,
                    PaddingRight = imageDTO.PaddingRight,
                    PaddingBottom = imageDTO.PaddingBottom,
                    PaddingLeft = imageDTO.PaddingLeft,
                    ImageType = imageDTO.ImageType,
                    ImageFor = imageDTO.ImageFor ?? Guid.NewGuid(),
                };

                if (image.ImageType == ImageType.ProfileAvatar)
                {
                    image.ImageFor = User.UserId();
                }

                Image? result;

                switch (image.ImageType)
                {
                    case ImageType.ProfileAvatar:
                        result = await _bll.Images.AddProfileAsync(User.UserId(), image);
                        break;
                    case ImageType.Post:
                        result = await _bll.Images.AddPostAsync((Guid) image.ImageFor, image);
                        break;
                    default:
                        return BadRequest(new ErrorResponseDTO("Access denied!"));
                }

                await _bll.SaveChangesAsync();
                return Ok(_mapper.Map(result));
            }
            
            return BadRequest(new ErrorResponseDTO("Image is invalid!"));
        }

        /// <summary>
        /// Updates an image
        /// </summary>
        /// <param name="id"></param>
        /// <param name="imageDTO"></param>
        /// <returns></returns>
        [HttpPut("{id?}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> PutImage(Guid id, [FromBody] ImagePutDTO imageDTO)
        {
            var record = await _bll.Images.GetForUpdateAsync(id);

            if (record == null || id != imageDTO.Id)
            {
                return NotFound(new ErrorResponseDTO("Image was not found!"));
            }
            
            if (record.ImageType != ImageType.Post && record.ImageType != ImageType.ProfileAvatar 
                || record.ImageType == ImageType.ProfileAvatar && record.ImageFor != User.UserId())
            {
                return BadRequest(new ErrorResponseDTO("Access denied!"));
            }

            if (ModelState.IsValid)
            {
                record.HeightPx = imageDTO.HeightPx;
                record.WidthPx = imageDTO.WidthPx;
                record.PaddingTop = imageDTO.PaddingTop;
                record.PaddingRight = imageDTO.PaddingRight;
                record.PaddingBottom = imageDTO.PaddingBottom;
                record.PaddingLeft = imageDTO.PaddingLeft;
                record.ImageFile = imageDTO.ImageFile;
                
                switch (record.ImageType)
                {
                    case ImageType.ProfileAvatar:
                        await _bll.Images.UpdateProfileAsync(User.UserId(), record);
                        break;
                    case ImageType.Post:
                        Debug.Assert(record.ImageFor != null, "record.ImageFor != null");
                        await _bll.Images.UpdatePostAsync((Guid) record.ImageFor, record);
                        break;
                    default:
                        return BadRequest(new ErrorResponseDTO("Access denied!"));
                }

                await _bll.SaveChangesAsync();
                return NoContent();
            }
            
            return BadRequest(new ErrorResponseDTO("Image is invalid!"));
        }

        /// <summary>
        /// Get profile avatar
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get posts image
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get gift image
        /// </summary>
        /// <param name="giftCode"></param>
        /// <returns></returns>
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