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
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MessagesController : ControllerBase
    {
        private readonly IAppBLL _bll;
        private readonly DTOMapper<Message, MessageGetDTO> _mapper;

        public MessagesController(IAppBLL bll)
        {
            _bll = bll;
            _mapper = new DTOMapper<Message, MessageGetDTO>();
        }

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
                return NotFound(new ErrorResponseDTO("Chat room was not found!"));
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

            return BadRequest(new ErrorResponseDTO("Message is invalid"));
        }
        
        [HttpPut("{id}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> PutMessage(Guid id, [FromBody] MessageEditDTO message)
        {
            var record = await _bll.Messages.FindAsync(id);

            if (record == null)
            {
                return NotFound(new ErrorResponseDTO("Message was not found!"));
            }

            if (message.Id != id)
            {
                return NotFound(new ErrorResponseDTO("Ids should math!"));
            }

            if (record.ProfileId != User.UserId())
            {
                return BadRequest(new ErrorResponseDTO("Access denied!"));
            }

            if (TryValidateModel(record))
            {
                record.MessageValue = message.MessageValue;

                await _bll.Messages.UpdateAsync(record);
                await _bll.SaveChangesAsync();

                return NoContent();
            }

            return BadRequest(new ErrorResponseDTO("Message is invalid"));
        }

        [HttpDelete("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MessageGetDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> DeleteMessage(Guid id)
        {
            var message = await _bll.Messages.FindAsync(id);

            if (message == null)
            {
                return NotFound(new ErrorResponseDTO("Comment was not found!"));
            }

            throw new NotImplementedException();
        }
    }
}
