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
    /// Ranks admin controller
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Area("Admin")]
    [Route("api/v{version:apiVersion}/[area]/[controller]")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public class AdminRanksController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly DTOMapper<Rank, RankAdminDTO> _mapper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bll">Application Bll</param>
        public AdminRanksController(IAppBLL bll)
        {
            _bll = bll;
            _mapper = new DTOMapper<Rank, RankAdminDTO>();
        }

        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<RankAdminDTO>))]
        public async Task<IActionResult> Index()
        {
            return Ok((await _bll.Ranks.AllAdminAsync()).Select(record => _mapper.Map(record)));
        }

        /// <summary>
        /// Get record history
        /// </summary>
        /// <returns></returns>
        [HttpGet("{history}/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<RankAdminDTO>))]
        public async Task<IActionResult> History(Guid id)
        {
            var history = (await _bll.Ranks.GetRecordHistoryAsync(id)).ToList()
                .Select(record => _mapper.Map(record));

            return Ok(history);
        }

        /// <summary>
        /// Get record details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RankAdminDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> Details(Guid id)
        {
            var record = await _bll.Ranks.FindAdminAsync(id);

            if (record == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map(record));
        }

        /// <summary>
        /// Creates a new record
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(RankAdminDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> Create([FromBody] RankAdminDTO model)
        {
            if (model.PreviousRankId != null &&
                (!await _bll.Ranks.ExistsAsync((Guid) model.PreviousRankId) ||
                 await _bll.Ranks.PreviousRankExists((Guid) model.PreviousRankId,
                     null)) ||
                model.NextRankId != null &&
                (!await _bll.Ranks.ExistsAsync((Guid) model.NextRankId) ||
                 await _bll.Ranks.NextRankExists((Guid) model.NextRankId, null)) ||
                model.PreviousRankId == null || model.NextRankId == null
            )
            {
                return BadRequest(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorBadData));
            }

            if (TryValidateModel(model))
            {
                model.Id = Guid.NewGuid();
                _bll.Ranks.Add(_mapper.MapReverse(model));
                await _bll.SaveChangesAsync();

                return CreatedAtAction("Create", model);
            }

            return BadRequest(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorBadData));
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
        public async Task<IActionResult> Edit(Guid id, [FromBody] RankAdminDTO model)
        {
            if (id != model.Id)
            {
                return BadRequest(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorIdMatch));
            }

            if (model.PreviousRankId != null &&
                (!await _bll.Ranks.ExistsAsync((Guid) model.PreviousRankId) ||
                 await _bll.Ranks.PreviousRankExists((Guid) model.PreviousRankId,
                     id)) ||
                model.NextRankId != null &&
                (!await _bll.Ranks.ExistsAsync((Guid) model.NextRankId) ||
                 await _bll.Ranks.NextRankExists((Guid) model.NextRankId, id)) ||
                model.PreviousRankId == id || model.NextRankId == id
            )
            {
                return BadRequest(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorBadData));
            }

            var record = await _bll.Ranks.GetForUpdateAsync(id);

            if (record == null)
            {
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorNotFound));
            }

            if (ModelState.IsValid)
            {
                model.RankTitleId = record.RankTitleId;
                model.RankDescriptionId = record.RankDescriptionId;
                await _bll.Ranks.UpdateAsync(_mapper.MapReverse(model));
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
            _bll.Ranks.Remove(id);
            await _bll.SaveChangesAsync();

            return Ok(new OkResponseDTO() {Status = Resourses.BLL.App.DTO.Common.SuccessDeleted});
        }

        /// <summary>
        /// Restores a record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("{restore}/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkResponseDTO))]
        public async Task<IActionResult> Restore(Guid id)
        {
            var record = await _bll.Ranks.GetForUpdateAsync(id);
            _bll.Ranks.Restore(record);
            await _bll.SaveChangesAsync();

            return Ok(new OkResponseDTO() {Status = Resourses.BLL.App.DTO.Common.SuccessRestored});
        }
    }
}