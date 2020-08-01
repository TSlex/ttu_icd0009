using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL.App.DTO;
using Contracts.BLL.App;
using Domain.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PublicApi.DTO.v1;
using PublicApi.DTO.v1.Mappers;
using PublicApi.DTO.v1.Response;

namespace WebApp.ApiControllers._1._0.Admin
{
    /// <summary>
    /// Images admin controller
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Area("Admin")]
    [Route("api/v{version:apiVersion}/[area]/[controller]")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public class AdminImagesController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly DTOMapper<Image, ImageAdminDTO> _mapper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bll">Application Bll</param>
        /// <param name="hostEnvironment"></param>
        public AdminImagesController(IAppBLL bll, IWebHostEnvironment hostEnvironment)
        {
            _bll = bll;
            _bll.Images.RootPath = hostEnvironment.WebRootPath;
            _mapper = new DTOMapper<Image, ImageAdminDTO>();
        }
        
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ImageAdminDTO>))]
        public async Task<IActionResult> Index()
        {
            return Ok((await _bll.Images.AllAdminAsync()).Select(record => _mapper.Map(record)));
        }
        
        [HttpGet("{history}/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ImageAdminDTO>))]
        public async Task<IActionResult> History(Guid id)
        {
            var history = (await _bll.Images.GetRecordHistoryAsync(id)).ToList()
                .OrderByDescending(record => record.CreatedAt).Select(record => _mapper.Map(record));
            
            return Ok(history);
        }
        
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ImageAdminDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> Details(Guid id)
        {
            var record = await _bll.Images.FindAdminAsync(id);

            if (record == null)
            {
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorNotFound));
            }

            return Ok(_mapper.Map(record));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [RequestSizeLimit(104857600)] 
        [Produces("application/json")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ImageAdminDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> Create([FromForm] ImageAdminDTO model)
        {
            if (model.ImageFile == null)
            {
                return BadRequest(Resourses.BLL.App.DTO.Images.Images.ImageRequired);
            }
            
            if (model.ImageType != ImageType.Undefined && model.ImageFor == null)
            {
                return BadRequest(Resourses.BLL.App.DTO.Images.Images.ErrorForIdRequired);
            }
            
            ModelState.Clear();
            
            var result = ValidateImage(_mapper.MapReverse(model));
            
            if (result != null && ModelState.IsValid)
            {
                result.Id = Guid.NewGuid();
                switch (result.ImageType)
                {
                    case ImageType.ProfileAvatar:
                        result = await _bll.Images.AddProfileAsync(result.ImageFor?? Guid.Empty, result);
                        break;
                    case ImageType.Post:
                        result = await _bll.Images.AddPostAsync(result.ImageFor?? Guid.Empty, result);
                        break;
                    case ImageType.Gift:
                        result = await _bll.Images.AddGiftAsync(result.ImageFor?? Guid.Empty, result);
                        break;
                    default:
                        result = await _bll.Images.AddUndefinedAsync(result);
                        break;
                }
                await _bll.SaveChangesAsync();

                return CreatedAtAction("Create", result);
            }

            return BadRequest(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorBadData));
        }

        [HttpPut("{id}")]
        [RequestSizeLimit(104857600)] 
        [Consumes("multipart/form-data")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> Edit(Guid id, [FromForm] ImageAdminDTO model)
        {
            if (id != model.Id)
            {
                return BadRequest(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorIdMatch));
            }
            
            Image? result;
            
            if (model.ImageType != ImageType.Undefined && model.ImageFor == null)
            {
                return BadRequest(Resourses.BLL.App.DTO.Images.Images.ErrorForIdRequired);
            }
            
            var record = await _bll.Images.GetForUpdateAsync(id);

            if (record == null)
            {
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorNotFound));
            }
            
            if (model.ImageFile != null)
            {
                result = ValidateImage(_mapper.MapReverse(model));
            }
            else
            {
                result = _mapper.MapReverse(model);
            }
            
            if (result != null && ModelState.IsValid)
            {
                switch (result.ImageType)
                {
                    case ImageType.ProfileAvatar:
                        await _bll.Images.UpdateProfileAsync(result.ImageFor?? Guid.Empty, result);
                        break;
                    case ImageType.Post:
                        await _bll.Images.UpdatePostAsync(result.ImageFor?? Guid.Empty, result);
                        break;
                    case ImageType.Gift:
                        await _bll.Images.UpdateGiftAsync(result.ImageFor?? Guid.Empty, result);
                        break;
                    default:
                        await _bll.Images.UpdateUndefinedAsync(result);
                        break;
                }
                await _bll.SaveChangesAsync();

                return NoContent();
            }

            return BadRequest(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorBadData));
        }

        private Image? ValidateImage(Image imageModel)
        {
            var extension = Path.GetExtension(imageModel.ImageFile!.FileName).ToLower();

            if (!(extension == ".png" || extension == ".jpg" || extension == ".jpeg"))
            {
                ModelState.AddModelError(string.Empty, Resourses.BLL.App.DTO.Images.Images.ExtensionsSupported);
                return null;
            }

            using (var image = System.Drawing.Image.FromStream(imageModel.ImageFile.OpenReadStream()))
            {
                if (image.Height > 4000 || image.Width > 4000)
                {
                    ModelState.AddModelError(string.Empty, Resourses.BLL.App.DTO.Images.Images.ErrorMaxImageResolution);
                    return null;
                }

                var ratio = image.Height * 1.0 / image.Width;
                if (ratio < 0.1 || 10 < ratio)
                {
                    ModelState.AddModelError(string.Empty, Resourses.BLL.App.DTO.Images.Images.ErrorImageSupportedRatio);
                    return null;
                }

                imageModel.HeightPx = image.Height;
                imageModel.WidthPx = image.Width;
            }

            return imageModel;
        }
        
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkResponseDTO))]
        public async Task<IActionResult> Delete(Guid id)
        {
            _bll.Images.Remove(id);
            await _bll.SaveChangesAsync();

            return Ok(new OkResponseDTO() {Status = Resourses.BLL.App.DTO.Common.SuccessDeleted});
        }
        
        [HttpPost("{restore}/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkResponseDTO))]
        public async Task<IActionResult> Restore(Guid id)
        {
            var record = await _bll.Images.GetForUpdateAsync(id);
            _bll.Images.Restore(record);
            await _bll.SaveChangesAsync();

            return Ok(new OkResponseDTO() {Status = Resourses.BLL.App.DTO.Common.SuccessRestored});
        }
    }
}