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
        public AdminImagesController(IAppBLL bll)
        {
            _bll = bll;
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
            var record = await _bll.Images.FindAsync(id);

            if (record == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map(record));
        }

        [HttpPost]
        [RequestSizeLimit(104857600)] 
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ImageAdminDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> Create([FromBody] ImageAdminDTO model)
        {
            if (model.ImageFile == null)
            {
                return BadRequest("Image file is missing!");
            }
            
            if (model.ImageType != ImageType.Undefined && model.ImageFor == null)
            {
                return BadRequest("Id should be specified if not misc image type!");
            }
            
            ModelState.Clear();
            
            var result = ValidateImage(_mapper.MapReverse(model));
            
            if (result != null && ModelState.IsValid)
            {
                result.Id = Guid.NewGuid();
                switch (result.ImageType)
                {
                    case ImageType.ProfileAvatar:
                        await _bll.Images.AddProfileAsync(result.ImageFor?? Guid.Empty, result);
                        break;
                    case ImageType.Post:
                        await _bll.Images.AddPostAsync(result.ImageFor?? Guid.Empty, result);
                        break;
                    case ImageType.Gift:
                        await _bll.Images.AddGiftAsync(result.ImageFor?? Guid.Empty, result);
                        break;
                    default:
                        await _bll.Images.AddUndefinedAsync(result);
                        break;
                }
                await _bll.SaveChangesAsync();

                return CreatedAtAction("Create", model);
            }

            return BadRequest("Record data is invalid!");
        }

        [HttpPut("{id}")]
        [RequestSizeLimit(104857600)] 
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> Edit(Guid id, [FromBody] ImageAdminDTO model)
        {
            if (id != model.Id)
            {
                return BadRequest("Id's should match!");
            }
            
            Image? result;
            
            if (model.ImageType != ImageType.Undefined && model.ImageFor == null)
            {
                return BadRequest("Id should be specified if not misc image type!");
            }
            
            var record = await _bll.Images.GetForUpdateAsync(id);

            if (record == null)
            {
                return NotFound("Record was not found!");
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

            return BadRequest("Record data is invalid!");
        }

        private Image? ValidateImage(Image imageModel)
        {
            var extension = Path.GetExtension(imageModel.ImageFile!.FileName);

            if (!(extension == ".png" || extension == ".jpg" || extension == ".jpeg"))
            {
                ModelState.AddModelError(string.Empty, "Extension supported only: [.png, .jpg, .jpeg]");
                return null;
            }

            using (var image = System.Drawing.Image.FromStream(imageModel.ImageFile.OpenReadStream()))
            {
                if (image.Height > 4000 || image.Width > 4000)
                {
                    ModelState.AddModelError(string.Empty, "Image should be not bigger that 4000x4000");
                    return null;
                }

                var ratio = image.Height * 1.0 / image.Width;
                if (ratio < 0.1 || 10 < ratio)
                {
                    ModelState.AddModelError(string.Empty, "Image ration should be between 0.1 and 10");
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

            return Ok(new OkResponseDTO() {Status = "Record was deleted"});
        }
        
        [HttpPost("{restore}/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkResponseDTO))]
        public async Task<IActionResult> Restore(Guid id)
        {
            var record = await _bll.Images.GetForUpdateAsync(id);
            _bll.Images.Restore(record);
            await _bll.SaveChangesAsync();

            return Ok(new OkResponseDTO() {Status = "Record was deleted"});
        }
    }
}