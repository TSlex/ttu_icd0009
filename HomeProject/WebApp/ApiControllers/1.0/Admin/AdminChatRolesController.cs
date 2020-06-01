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
    /// Chat roles admin controller
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Area("Admin")]
    [Route("api/v{version:apiVersion}/[area]/[controller]")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public class AdminChatRolesController : Controller
    {
        private readonly IAppBLL _bll;
        private readonly DTOMapper<ChatRole, ChatRoleAdminDTO> _mapper;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bll"></param>
        public AdminChatRolesController(IAppBLL bll)
        {
            _bll = bll;
            _mapper = new DTOMapper<ChatRole, ChatRoleAdminDTO>();
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ChatRoleAdminDTO>))]
        public async Task<IActionResult> Index()
        {
            return Ok((await _bll.ChatRoles.AllAdminAsync()).Select(record => _mapper.Map(record)));
        }
        
        [HttpGet("{history}/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ChatRoleAdminDTO>))]
        public async Task<IActionResult> History(Guid id)
        {
            var history = (await _bll.ChatRoles.GetRecordHistoryAsync(id)).ToList()
                .OrderByDescending(record => record.CreatedAt).Select(record => _mapper.Map(record));
            
            return Ok(history);
        }
        
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ChatRoleAdminDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> Details(Guid id)
        {
            var record = await _bll.ChatRoles.FindAsync(id);

            if (record == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map(record));
        }
        
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ChatRoleAdminDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> Create([FromBody] ChatRoleAdminDTO model)
        {
            if (TryValidateModel(model))
            {
                model.Id = Guid.NewGuid();
                _bll.ChatRoles.Add(_mapper.MapReverse(model));
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
        public async Task<IActionResult> Edit(Guid id, [FromBody] ChatRoleAdminDTO model)
        {
            if (id != model.Id)
            {
                return BadRequest("Id's should match!");
            }
            
            var record = await _bll.ChatRoles.GetForUpdateAsync(id);

            if (record == null)
            {
                return NotFound("Record was not found!");
            }

            if (ModelState.IsValid)
            {
                model.RoleTitleValueId = record.RoleTitleValueId;
                await _bll.ChatRoles.UpdateAsync(_mapper.MapReverse(model));
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
            _bll.ChatRoles.Remove(id);
            await _bll.SaveChangesAsync();

            return Ok(new OkResponseDTO() {Status = "Record was deleted"});
        }
        
        [HttpPost("{restore}/{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkResponseDTO))]
        public async Task<IActionResult> Restore(Guid id)
        {
            var record = await _bll.ChatRoles.GetForUpdateAsync(id);
            _bll.ChatRoles.Restore(record);
            await _bll.SaveChangesAsync();

            return Ok(new OkResponseDTO() {Status = "Record was deleted"});
        }
    }
}
