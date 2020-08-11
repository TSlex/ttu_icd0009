using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PublicApi.v1;
using PublicApi.v1.Mappers;
using PublicApi.v1.Response;

namespace WebApp.ApiControllers._1._0
{
    /// <summary>
    /// Template
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TemplatesController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly UniversalAPIMapper<BLL.App.DTO.Template, Template> _mapper;

        /// <inheritdoc />
        public TemplatesController(IAppBLL bll)
        {
            _bll = bll;
            _mapper = new UniversalAPIMapper<BLL.App.DTO.Template, Template>();
        }

        /// <summary>
        /// Return all templates
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Template>))]
        public async Task<IActionResult> Index()
        {
            return Ok((await _bll.Templates.AllAsync()).Select(record => _mapper.Map(record)));
        }

        /// <summary>
        /// Return specific template
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Template))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseDTO))]
        public async Task<IActionResult> Details(Guid id)
        {
            var template = await _bll.Templates.FindAsync(id);
            
            if (template == null)
            {
                return NotFound(new ResponseDTO(Resources.Domain.Common.ErrorNotFound));
            }

            return Ok(_mapper.Map(template));
        }

        /// <summary>
        /// Creates a new template
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Template))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseDTO))]
        public async Task<IActionResult> Create( Template template)
        {
            if (ModelState.IsValid)
            {
                template.Id = Guid.NewGuid();

                _bll.Templates.Add(_mapper.MapReverse(template));

                await _bll.SaveChangesAsync();
                return CreatedAtAction("Create", template);
            }

            return BadRequest(new ResponseDTO(Resources.Domain.Common.ErrorBadData));
        }

        /// <summary>
        /// Updates a template
        /// </summary>
        /// <param name="id"></param>
        /// <param name="template"></param>
        /// <returns></returns>
        [HttpPut]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResponseDTO))]
        public async Task<IActionResult> Edit(Guid id, Template template)
        {
            if (id != template.Id)
            {
                return NotFound(new ResponseDTO(Resources.Domain.Common.ErrorNotFound));
            }

            if (ModelState.IsValid)
            {
                await _bll.Templates.UpdateAsync(_mapper.MapReverse(template));
                await _bll.SaveChangesAsync();

                return NoContent();
            }

            return BadRequest(new ResponseDTO(Resources.Domain.Common.ErrorBadData));
        }

        /// <summary>
        /// Deletes a template
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Template))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResponseDTO))]
        public async Task<IActionResult> Delete(Guid id)
        {
            var template = await _bll.Templates.FindAsync(id);

            if (template == null)
            {
                return BadRequest(new ResponseDTO(Resources.Domain.Common.ErrorBadData));
            }

            _bll.Templates.Remove(template);
            
            await _bll.SaveChangesAsync();
            return Ok(_mapper.Map(template));
        }
    }
}