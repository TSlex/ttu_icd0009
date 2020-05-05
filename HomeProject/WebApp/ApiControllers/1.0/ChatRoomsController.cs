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
        public async Task<IActionResult> GetRooms()
        {
            return Ok((await _bll.ChatRooms.AllAsync(User.UserId()))
                .Select(room => new ChatRoomDTO()
            {
                ChatRoomTitle = room.ChatRoomTitle,
                
            }));
        }

        [HttpGet("{id}/last")]
        public async Task<IActionResult> GetLastMessage(Guid chatRoomId)
        {
            var exist = await _bll.ChatRooms.Exist(chatRoomId);

            if (!exist)
            {
                return NotFound();
            }

            var result = await _bll.Messages.GetLastMessage(chatRoomId);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("{id}/last")]
        public async Task<IActionResult> GetMessagesCount(Guid chatRoomId)
        {
            var exist = await _bll.ChatRooms.Exist(chatRoomId);

            if (!exist)
            {
                return NotFound();
            }

            return Ok(_bll.Messages.CountByRoomAsync(chatRoomId));
        }

        [HttpGet("{id}/{pageNumber}")]
        public async Task<IActionResult> GetRoom(Guid chatRoomId, int pageNumber)
        {
            var exist = await _bll.ChatRooms.Exist(chatRoomId);

            if (!exist)
            {
                return NotFound();
            }

            return Ok(await _bll.Messages.AllByIdPageAsync(chatRoomId, pageNumber, 10));
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetRoomWithUser(string username)
        {
            var chatRoomId = await _bll.ChatRooms.OpenOrCreateAsync(username);

            return Ok(chatRoomId);
        }

        [HttpDelete("{id}/delete")]
        public Task<IActionResult> DeleteRoom(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}