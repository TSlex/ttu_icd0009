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

namespace WebApp.ApiControllers._1._0
{
    /// <summary>
    /// Chat roles
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChatRolesController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly DTOMapper<ChatRole, ChatRoleDTO> _mapper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bll">Application Bll</param>
        public ChatRolesController(IAppBLL bll)
        {
            _bll = bll;
            _mapper = new DTOMapper<ChatRole, ChatRoleDTO>();
        }
        
        /// <summary>
        /// Get chat roles
        /// </summary>
        /// <returns>List of chat roles</returns>
        [HttpGet]
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ChatRoleDTO>))]
        public async Task<IActionResult> All()
        {
            return Ok((await _bll.ChatRoles.AllAsync()).Select(role => _mapper.Map(role)));
        }
    }
}
