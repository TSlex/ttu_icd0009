using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.BLL.App;
using DAL;
using Domain;
using Extension;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PublicApi.DTO.v1;
using PublicApi.DTO.v1.Mappers;
using PublicApi.DTO.v1.Response;
using Message = BLL.App.DTO.Message;

namespace WebApp.ApiControllers._1._0
{
    /// <summary>
    /// Messages
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MessagesController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly DTOMapper<Message, MessageGetDTO> _mapper;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bll">Application Bll</param>
        public MessagesController(IAppBLL bll)
        {
            _bll = bll;
            _mapper = new DTOMapper<Message, MessageGetDTO>();
        }

        /// <summary>
        /// Create a message
        /// </summary>
        /// <param name="message">Message body</param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(MessageGetDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> PostMessage([FromBody] MessageCreateDTO message)
        {
            var room = await _bll.ChatRooms.FindAsync(message.ChatRoomId);

            if (room == null)
            {
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorNotFound));
            }
            
            var currentMember = await _bll.ChatMembers.FindByUserAndRoomAsync(User.UserId(), message.ChatRoomId);
            
            if (!(currentMember != null &&
                  (currentMember.ChatRole.CanWriteMessages ||
                   currentMember.ChatRole.CanEditAllMessages)))
            {
                return BadRequest(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorAccessDenied));
            }
            
            if (TryValidateModel(message))
            {
                var result = _bll.Messages.Add(new Message()
                {
                    ProfileId = User.UserId(),
                    MessageValue = message.MessageValue,
                    ChatRoomId = message.ChatRoomId
                });

                await _bll.SaveChangesAsync();

                return CreatedAtAction("Create", _mapper.Map(result));
            }

            return BadRequest(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorBadData));
        }
        
        /// <summary>
        /// Updates a message
        /// </summary>
        /// <param name="id">Message id</param>
        /// <param name="message">Message body</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> PutMessage(Guid id, [FromBody] MessageEditDTO message)
        {
            var record = await _bll.Messages.GetForUpdateAsync(id);

            if (record == null)
            {
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorNotFound));
            }

            if (message.Id != id)
            {
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorIdMatch));
            }
            
            var currentMember = await _bll.ChatMembers.FindByUserAndRoomAsync(User.UserId(), record.ChatRoomId);
            
            if (!(currentMember != null &&
                  (record.ProfileId == User.UserId() && currentMember.ChatRole.CanEditMessages ||
                   currentMember.ChatRole.CanEditAllMessages)))
            {
                return BadRequest(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorAccessDenied));
            }

            if (TryValidateModel(record))
            {
                record.MessageValue = message.MessageValue;

                await _bll.Messages.UpdateAsync(record);
                await _bll.SaveChangesAsync();

                return NoContent();
            }

            return BadRequest(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorBadData));
        }

        /// <summary>
        /// Deletes message
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> DeleteMessage(Guid id)
        {
            var record = await _bll.Messages.GetForUpdateAsync(id);
            
            var currentMember = await _bll.ChatMembers.FindByUserAndRoomAsync(User.UserId(), record.ChatRoomId);
            
            if (!(currentMember != null &&
                  (record.ProfileId == User.UserId() && currentMember.ChatRole.CanEditMessages ||
                   currentMember.ChatRole.CanEditAllMessages)))
            {
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorNotFound));
            }
            
            _bll.Messages.Remove(id);
            await _bll.SaveChangesAsync();

            return Ok(new OkResponseDTO() {Status = Resourses.BLL.App.DTO.Common.SuccessDeleted});
        }
    }
}
