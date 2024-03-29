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
using WebApp.Helpers;

namespace WebApp.ApiControllers._1._0
{
    /// <summary>
    /// Chat rooms
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChatRoomsController : ControllerBase
    {
        private readonly IAppBLL _bll;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bll">Application Bll</param>
        public ChatRoomsController(IAppBLL bll)
        {
            _bll = bll;
        }

        /// <summary>
        /// Get chat rooms where current User is a member
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ChatRoomGetDTO>))]
        public async Task<IActionResult> GetRooms()
        {
            return Ok((await _bll.ChatRooms.AllAsync(User.UserId())).Select(room => new ChatRoomGetDTO()
            {
                Id = room.Id,
                ChatRoomTitle = room.ChatRoomTitle,
                LastMessageDateTime = room.Messages!.Count > 0 ? room.Messages.First()?.MessageDateTime : null,
                LastMessageValue = room.Messages!.Count > 0 ? room.Messages.First()?.MessageValue : null,
                ChatRoomImageId = room.Messages!.Count > 0 ? room.Messages.First()?.Profile?.ProfileAvatarId : null,
            }).OrderByDescending(room => room.LastMessageDateTime));
        }

        /// <summary>
        /// Get last message of this room
        /// </summary>
        /// <param name="id">Chat room id</param>
        /// <returns></returns>
        [HttpGet("{id}/last")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MessageGetDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> GetLastMessage(Guid id)
        {
            var exist = await _bll.ChatRooms.ExistsAsync(id);

            if (!exist)
            {
                return RedirectToAction("PageNotFound", "Home", new {area = ""});
            }

            var canAccess = await _bll.ChatRooms.IsRoomMemberAsync(id, User.UserId());

            if (!canAccess)
            {
                return BadRequest(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorAccessDenied));
            }

            var result = await _bll.Messages.GetLastMessage(id);

            if (result == null)
            {
                return RedirectToAction("PageNotFound", "Home", new {area = ""});
            }

            return Ok(new MessageGetDTO()
            {
                Id = result.Id,
                ChatRoomId = result.ChatRoomId,
                MessageValue = result.MessageValue,
                MessageDateTime = result.MessageDateTime,
                UserName = result.Profile!.UserName,
                ProfileAvatarId = result.Profile!.ProfileAvatarId,
            });
        }

        /// <summary>
        /// Check if room exist
        /// </summary>
        /// <param name="id">Chat room id</param>
        /// <returns></returns>
        [HttpGet("{id}/exists")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        public async Task<IActionResult> RoomExists(Guid id)
        {
            return Ok(await _bll.ChatRooms.ExistsAsync(id));
        }

        /// <summary>
        /// Get room messages count
        /// </summary>
        /// <param name="id">Chat room id</param>
        /// <returns></returns>
        [HttpGet("{id}/count")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CountResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> GetMessagesCount(Guid id)
        {
            var exist = await _bll.ChatRooms.ExistsAsync(id);

            if (!exist)
            {
                return RedirectToAction("PageNotFound", "Home", new {area = ""});
            }

            var canAccess = await _bll.ChatRooms.IsRoomMemberAsync(id, User.UserId());

            if (!canAccess)
            {
                return BadRequest(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorAccessDenied));
            }

            return Ok(new CountResponseDTO {Count = await _bll.Messages.CountByRoomAsync(id)});
        }

        /// <summary>
        /// Get room's messages by page
        /// </summary>
        /// <param name="id">Chat room id</param>
        /// <param name="pageNumber">Number of page</param>
        /// <returns></returns>
        [HttpGet("{id}/{pageNumber}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<MessageGetDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> GetRoom(Guid id, int pageNumber)
        {
            var exist = await _bll.ChatRooms.ExistsAsync(id);

            if (!exist)
            {
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorNotFound));
            }

            var canAccess = await _bll.ChatRooms.IsRoomMemberAsync(id, User.UserId());

            if (!canAccess)
            {
                return BadRequest(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorAccessDenied));
            }

            return Ok((await _bll.Messages.AllByIdPageAsync(id, pageNumber, 10)).Select(message =>
                new MessageGetDTO()
                {
                    Id = message.Id,
                    ChatRoomId = message.ChatRoomId,
                    ProfileAvatarId = message.Profile!.ProfileAvatarId,
                    MessageValue = message.MessageValue,
                    UserName = message.Profile!.UserName,
                    MessageDateTime = message.MessageDateTime
                }));
        }

        /// <summary>
        /// Get room id (if room is not exist, create)
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns></returns>
        [HttpGet("{username}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> GetRoomWithUser(string username)
        {
            var chatRoomId = await _bll.ChatRooms.OpenOrCreateAsync(username);

            if (chatRoomId == null)
            {
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorUserNotFound));
            }

            return Ok(chatRoomId);
        }

        /// <summary>
        /// Room title update
        /// </summary>
        /// <param name="id"></param>
        /// <param name="chatRoom"></param>
        /// <returns></returns>
        [HttpPut("{id}/rename")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> RenameRoom(Guid id, [FromBody] ChatRoomEditDTO chatRoom)
        {
            var record = await _bll.ChatRooms.GetForUpdateAsync(id);

            if (record == null)
            {
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorNotFound));
            }

            if (chatRoom.Id != id)
            {
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorIdMatch));
            }

            var canAccess = await _bll.ChatRooms.IsRoomMemberAsync(id, User.UserId());

            if (!canAccess)
            {
                return BadRequest(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorAccessDenied));
            }

            if (TryValidateModel(chatRoom))
            {
                record.ChatRoomTitle = chatRoom.ChatRoomTitle;

                await _bll.ChatRooms.UpdateAsync(record);
                await _bll.SaveChangesAsync();

                return NoContent();
            }

            return BadRequest(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorBadData));
        }
    }
}