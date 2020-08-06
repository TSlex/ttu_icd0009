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
using Microsoft.AspNetCore.Hosting;
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
        private readonly IWebHostEnvironment _hostEnvironment;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bll">Application Bll</param>
        /// <param name="hostEnvironment"></param>
        public ImagesController(IAppBLL bll, IWebHostEnvironment hostEnvironment)
        {
            _bll = bll;
            _hostEnvironment = hostEnvironment;
            _bll.Images.RootPath = hostEnvironment.WebRootPath;
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
            var undefined = base.File(_bll.Images.GetImageUndefinedPath(), "image/jpeg");

            if (id == null)
            {
                return undefined;
            }

            Image image;

            try
            {
                image = await _bll.Images.FindAsync(new Guid(id));

                if (image == null)
                {
                    return undefined;
                }
            }
            catch (Exception)
            {
                return undefined;
            }

            Request.Headers.Add("imageId", id.ToString());
            return base.File(_bll.Images.GetImagePath(image.ImageUrl), "image/jpeg");
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
            var undefined = base.File(_bll.Images.GetImageUndefinedPath(), "image/jpeg");

            if (id == null)
            {
                return undefined;
            }

            Image image;

            try
            {
                image = await _bll.Images.FindAsync(new Guid(id));

                if (image == null)
                {
                    return undefined;
                }
            }
            catch (Exception)
            {
                return undefined;
            }

            Request.Headers.Add("imageId", id.ToString());
            return base.File(_bll.Images.GetImagePath(image.OriginalImageUrl), "image/jpeg");
        }

        /// <summary>
        /// Get image model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/model")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = (typeof(ImageDTO)))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = (typeof(ErrorResponseDTO)))]
        public async Task<IActionResult> GetImageModel(Guid id)
        {
            var record = await _bll.Images.GetForUpdateAsync(id);

            if (record == null)
            {
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorNotFound));
            }

            return Ok(_mapper.Map(record));
        }

        /// <summary>
        /// Create a new image and returns Id
        /// </summary>
        /// <param name="imageDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ImageDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> PostImage([FromForm] ImagePostDTO imageDTO)
        {
            if (imageDTO.ImageType != ImageType.Post && imageDTO.ImageType != ImageType.ProfileAvatar)
            {
                return BadRequest(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorAccessDenied));
            }

            if (imageDTO.ImageFile == null)
            {
                return BadRequest(new ErrorResponseDTO(Resourses.BLL.App.DTO.Images.Images.ImageRequired));
            }

            if (ModelState.IsValid)
            {
                var image = new Image()
                {
                    Id = Guid.NewGuid(),
                    HeightPx = imageDTO.HeightPx,
                    WidthPx = imageDTO.WidthPx,
                    PaddingTop = imageDTO.PaddingTop,
                    PaddingRight = imageDTO.PaddingRight,
                    PaddingBottom = imageDTO.PaddingBottom,
                    PaddingLeft = imageDTO.PaddingLeft,
                    ImageType = imageDTO.ImageType,
                    ImageFor = imageDTO.ImageFor ?? Guid.NewGuid(),
                    ImageFile = imageDTO.ImageFile
                };

                var (_, errors) = _bll.Images.ValidateImage(image);

                if (errors.Length > 0)
                {
                    return BadRequest(new ErrorResponseDTO(errors));
                }

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
                        return BadRequest(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorAccessDenied));
                }

                await _bll.SaveChangesAsync();
                return Ok(_mapper.Map(result));
            }

            return BadRequest(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorBadData));
        }

        /// <summary>
        /// Updates an image
        /// </summary>
        /// <param name="id"></param>
        /// <param name="imageDTO"></param>
        /// <returns></returns>
        [HttpPut("{id?}")]
        [Produces("application/json")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> PutImage(Guid id, [FromForm] ImagePutDTO imageDTO)
        {
            var record = await _bll.Images.GetForUpdateAsync(id);

            if (record == null || id != imageDTO.Id)
            {
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorNotFound));
            }

            if (record.ImageType != ImageType.Post && record.ImageType != ImageType.ProfileAvatar
                || record.ImageType == ImageType.ProfileAvatar && record.ImageFor != User.UserId())
            {
                return BadRequest(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorAccessDenied));
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

                if (record.ImageFile != null)
                {
                    var (_, errors) = _bll.Images.ValidateImage(record);

                    if (errors.Length > 0)
                    {
                        return BadRequest(new ErrorResponseDTO(errors));
                    }
                }

                switch (record.ImageType)
                {
                    case ImageType.ProfileAvatar:
                        await _bll.Images.UpdateProfileAsync(User.UserId(), record);
                        break;
                    case ImageType.Post:
                        await _bll.Images.UpdatePostAsync((record.ImageFor ?? Guid.Empty), record);
                        break;
                    default:
                        return BadRequest(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorAccessDenied));
                }

                await _bll.SaveChangesAsync();
                return NoContent();
            }

            return BadRequest(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorBadData));
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
            var undefined = base.File(_bll.Images.GetImageUndefinedPath(), "image/jpeg");

            var user = await _bll.Profiles.FindByUsernameAsync(username);

            if (user == null)
            {
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorUserNotFound));
            }

            if (user.ProfileAvatarId == null)
            {
                return undefined;
            }

            var image = await _bll.Images.FindAsync((Guid) user.ProfileAvatarId);

            if (image == null)
            {
                return undefined;
            }

            return base.File(_bll.Images.GetImagePath(image.ImageUrl), "image/jpeg");
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
            var undefined = base.File(_bll.Images.GetImageUndefinedPath(), "image/jpeg");

            var post = await _bll.Posts.GetForUpdateAsync(postId);

            if (post == null)
            {
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorNotFound));
            }

            if (post.PostImageId == null)
            {
                return undefined;
            }

            var image = await _bll.Images.FindAsync((Guid) post.PostImageId);

            if (image == null)
            {
                return undefined;
            }

            return base.File(_bll.Images.GetImagePath(image.ImageUrl), "image/jpeg");
        }

        /// <summary>
        /// Get gift image
        /// </summary>
        /// <param name="giftId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("gift/{giftId}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> GetGiftImage(Guid giftId)
        {
            var undefined = base.File(_bll.Images.GetImageUndefinedPath(), "image/jpeg");

            var gift = await _bll.Gifts.GetForUpdateAsync(giftId);

            if (gift == null)
            {
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorNotFound));
            }

            if (gift.GiftImageId == null)
            {
                return undefined;
            }

            var image = await _bll.Images.FindAsync((Guid) gift.GiftImageId);

            if (image == null)
            {
                return undefined;
            }

            return base.File(_bll.Images.GetImagePath(image.ImageUrl), "image/jpeg");
        }
    }
}