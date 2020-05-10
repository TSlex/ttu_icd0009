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

namespace WebApp.ApiControllers._1._0
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChatRoomsController : ControllerBase
    {
        private readonly IAppBLL _bll;

        public ChatRoomsController(IAppBLL bll)
        {
            _bll = bll;
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ChatRoomGetDTO>))]
        public async Task<IActionResult> GetRooms()
        {
            return Ok((await _bll.ChatRooms.AllAsync(User.UserId())).Select(room => new ChatRoomGetDTO()
            {
                Id = room.Id,
                ChatRoomTitle = room.ChatRoomTitle,
                LastMessageDateTime = room.Messages.Count > 0 ? room.Messages.First()?.MessageDateTime : null,
                LastMessageValue = room.Messages.Count > 0 ? room.Messages.First()?.MessageValue : null,
            }).OrderByDescending(room => room.LastMessageDateTime));
        }

        [HttpGet("{id}/last")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MessageGetDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> GetLastMessage(Guid id)
        {
            var exist = await _bll.ChatRooms.ExistAsync(id);

            if (!exist)
            {
                return NotFound();
            }

            var canAccess = await _bll.ChatRooms.IsRoomMemberAsync(id, User.UserId());

            if (!canAccess)
            {
                return BadRequest(new ErrorResponseDTO("Access denied!"));
            }

            var result = await _bll.Messages.GetLastMessage(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(new MessageGetDTO()
            {
                Id = result.Id,
                MessageValue = result.MessageValue,
                UserName = result.Profile.UserName,
                MessageDateTime = result.MessageDateTime
            });
        }

        [HttpGet("{id}/count")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CountResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> GetMessagesCount(Guid id)
        {
            var exist = await _bll.ChatRooms.ExistAsync(id);

            if (!exist)
            {
                return NotFound();
            }

            var canAccess = await _bll.ChatRooms.IsRoomMemberAsync(id, User.UserId());

            if (!canAccess)
            {
                return BadRequest(new ErrorResponseDTO("Access denied!"));
            }

            return Ok(_bll.Messages.CountByRoomAsync(id));
        }

        [HttpGet("{id}/{pageNumber}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<MessageGetDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> GetRoom(Guid id, int pageNumber)
        {
            var exist = await _bll.ChatRooms.ExistAsync(id);

            if (!exist)
            {
                return NotFound(new ErrorResponseDTO("Chat room was not found!"));
            }

            var canAccess = await _bll.ChatRooms.IsRoomMemberAsync(id, User.UserId());

            if (!canAccess)
            {
                return BadRequest(new ErrorResponseDTO("Access denied!"));
            }

            return Ok((await _bll.Messages.AllByIdPageAsync(id, pageNumber, 10)).Select(message =>
                new MessageGetDTO()
                {
                    Id = message.Id,
                    MessageValue = message.MessageValue,
                    UserName = message.Profile.UserName,
                    MessageDateTime = message.MessageDateTime
                }));
        }

        [HttpGet("{username}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> GetRoomWithUser(string username)
        {
            var chatRoomId = await _bll.ChatRooms.OpenOrCreateAsync(username);

            if (chatRoomId == null)
            {
                return NotFound(new ErrorResponseDTO("User was not found!"));
            }

            return Ok(chatRoomId);
        }

        [HttpPut("{id}/rename")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> PutRoom(Guid id, [FromBody] ChatRoomEditDTO chatRoom)
        {
            var record = await _bll.ChatRooms.FindAsync(id);

            if (record == null)
            {
                return NotFound(new ErrorResponseDTO("Chat room was not found!"));
            }

            if (chatRoom.Id != id)
            {
                return NotFound(new ErrorResponseDTO("Ids should math!"));
            }

            var canAccess = await _bll.ChatRooms.IsRoomMemberAsync(id, User.UserId());

            if (!canAccess)
            {
                return BadRequest(new ErrorResponseDTO("Access denied!"));
            }

            if (TryValidateModel(chatRoom))
            {
                record.ChatRoomTitle = chatRoom.ChatRoomTitle;

                await _bll.ChatRooms.UpdateAsync(record);
                await _bll.SaveChangesAsync();

                return NoContent();
            }

            return BadRequest(new ErrorResponseDTO("Chat room is invalid"));
        }

        [HttpDelete("{id}/delete")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> DeleteRoom(Guid id)
        {
            var exist = await _bll.ChatRooms.ExistAsync(id);

            if (!exist)
            {
                return NotFound(new ErrorResponseDTO("Chat room was not found!"));
            }

            throw new NotImplementedException();
        }
    }
}