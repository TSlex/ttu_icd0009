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
    /// Comments admin controller
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Area("Admin")]
    [Route("api/v{version:apiVersion}/[area]/[controller]")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public class AdminCommentsController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly DTOMapper<Comment, CommentAdminDTO> _mapper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bll">Application Bll</param>
        public AdminCommentsController(IAppBLL bll)
        {
            _bll = bll;
            _mapper = new DTOMapper<Comment, CommentAdminDTO>();
        }
        
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CommentAdminDTO>))]
        public async Task<IActionResult> Index()
        {
            return Ok((await _bll.Comments.AllAdminAsync()).Select(record => _mapper.Map(record)));
        }
        
        [HttpGet("{history}/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CommentAdminDTO>))]
        public async Task<IActionResult> History(Guid id)
        {
            var history = (await _bll.Comments.GetRecordHistoryAsync(id)).ToList()
                .OrderByDescending(record => record.CreatedAt).Select(record => _mapper.Map(record));
            
            return Ok(history);
        }
        
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommentAdminDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> Details(Guid id)
        {
            var record = await _bll.Comments.FindAsync(id);

            if (record == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map(record));
        }
        
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CommentAdminDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> Create([FromBody] CommentAdminDTO model)
        {
            if (TryValidateModel(model))
            {
                model.Id = Guid.NewGuid();
                _bll.Comments.Add(_mapper.MapReverse(model));
                await _bll.SaveChangesAsync();

                return CreatedAtAction("Create", model);
            }

            return BadRequest("Record data is invalid!");
        }
        
        [HttpPut("{id}")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> Edit(Guid id, [FromBody] CommentAdminDTO model)
        {
            if (id != model.Id)
            {
                return BadRequest("Id's should match!");
            }
            
            var record = await _bll.Comments.GetForUpdateAsync(id);

            if (record == null)
            {
                return NotFound("Record was not found!");
            }

            if (ModelState.IsValid)
            {
                await _bll.Comments.UpdateAsync(_mapper.MapReverse(model));
                await _bll.SaveChangesAsync();


                return NoContent();
            }

            return BadRequest("Record data is invalid!");
        }
        
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkResponseDTO))]
        public async Task<IActionResult> Delete(Guid id)
        {
            _bll.Comments.Remove(id);
            await _bll.SaveChangesAsync();

            return Ok(new OkResponseDTO() {Status = "Record was deleted"});
        }
        
        [HttpPost("{restore}/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkResponseDTO))]
        public async Task<IActionResult> Restore(Guid id)
        {
            var record = await _bll.Comments.GetForUpdateAsync(id);
            _bll.Comments.Restore(record);
            await _bll.SaveChangesAsync();

            return Ok(new OkResponseDTO() {Status = "Record was deleted"});
        }
    }
}