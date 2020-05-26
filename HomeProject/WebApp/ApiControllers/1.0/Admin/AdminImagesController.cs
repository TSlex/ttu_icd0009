using BLL.App.DTO;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PublicApi.DTO.v1;
using PublicApi.DTO.v1.Mappers;

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
    }
}