using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.BLL.App;
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
    /// Favorites admin controller
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Area("Admin")]
    [Route("api/v{version:apiVersion}/[area]/[controller]")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public class AdminFavoritesController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly DTOMapper<Favorite, FavoriteAdminDTO> _mapper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bll">Application Bll</param>
        public AdminFavoritesController(IAppBLL bll)
        {
            _bll = bll;
            _mapper = new DTOMapper<Favorite, FavoriteAdminDTO>();
        }
        
        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<FavoriteAdminDTO>))]
        public async Task<IActionResult> Index()
        {
            return Ok((await _bll.Favorites.AllAdminAsync()).Select(record => _mapper.Map(record)));
        }

        /// <summary>
        /// Get record details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FavoriteAdminDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> Details(Guid id)
        {
            var record = await _bll.Favorites.FindAdminAsync(id);

            if (record == null)
            {
                return RedirectToAction("PageNotFound", "Home", new {area = ""});
            }

            return Ok(_mapper.Map(record));
        }
        
        /// <summary>
        /// Updates a record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> Edit(Guid id, [FromBody] FavoriteAdminDTO model)
        {
            if (id != model.Id)
            {
                return BadRequest(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorIdMatch));
            }
            
            if (!await _bll.Profiles.ExistsAsync(model.ProfileId) ||
                !await _bll.Posts.ExistsAsync(model.PostId))
            {
                return BadRequest(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorBadData));
            }
            
            var record = await _bll.Favorites.GetForUpdateAsync(id);
            

            if (record == null)
            {
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorNotFound));
            }

            if (ModelState.IsValid)
            {
                await _bll.Favorites.UpdateAsync(_mapper.MapReverse(model));
                await _bll.SaveChangesAsync();


                return NoContent();
            }

            return BadRequest(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorBadData));
        }
        
        /// <summary>
        /// Deletes a record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkResponseDTO))]
        public async Task<IActionResult> Delete(Guid id)
        {
            _bll.Favorites.Remove(id);
            await _bll.SaveChangesAsync();

            return Ok(new OkResponseDTO() {Status = Resourses.BLL.App.DTO.Common.SuccessDeleted});
        }
    }
}