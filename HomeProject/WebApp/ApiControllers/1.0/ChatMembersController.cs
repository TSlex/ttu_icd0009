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
using PublicApi.DTO.v1.Response;

namespace WebApp.ApiControllers._1._0
{
    /// <summary>
    /// Chat members
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChatMembersController : ControllerBase
    {
        private readonly IAppBLL _bll;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bll">Application Bll</param>
        public ChatMembersController(IAppBLL bll)
        {
            _bll = bll;
        }
        
        /// <summary>
        /// Get chat room members
        /// </summary>
        /// <param name="chatRoomId"></param>
        /// <returns>List of members</returns>
        [HttpGet("{chatRoomId}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ChatMemberDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> GetRoomMembers(Guid chatRoomId)
        {
            var exist = await _bll.ChatRooms.ExistAsync(chatRoomId);

            if (!exist)
            {
                return NotFound(new ErrorResponseDTO("Chat room was not found!"));
            }
            
            var canAccess = await _bll.ChatRooms.IsRoomMemberAsync(chatRoomId, User.UserId());
            
            if (!canAccess)
            {
                return BadRequest(new ErrorResponseDTO("Access denied!"));
            }

            return Ok((await _bll.ChatMembers.RoomAllAsync(chatRoomId)).Select(member => new ChatMemberDTO
            {
                Id = member.Id,
                UserName = member.Profile!.UserName,
                ProfileAvatarUrl = member.Profile!.ProfileAvatarUrl,
                ChatRole = member.ChatRole!.RoleTitle
            }));
        }
        
        /// <summary>
        /// Changes member role if possible
        /// </summary>
        /// <param name="id">Member's id</param>
        /// <param name="roleTitle">Role name that will be set</param>
        /// <returns></returns>
        [HttpPost("{id}/{roleTitle}/set")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> SetMemberRole(Guid id, string roleTitle)
        {
            if (roleTitle.Contains("Creator"))
            {
                return BadRequest(new ErrorResponseDTO("Creator role is not assignable!"));
            }
            
            var role = await _bll.ChatRoles.FindAsync(roleTitle);

            if (role == null)
            {
                return NotFound(new ErrorResponseDTO("Role was not found!"));
            }

            var member = await _bll.ChatMembers.FindAsync(id);
            
            if (member == null)
            {
                return NotFound(new ErrorResponseDTO("Member was not found!"));
            }

            if (member.ChatRole!.RoleTitle.Contains("Creator"))
            {
                return BadRequest(new ErrorResponseDTO("Creator cannot be demoted!"));
            }

            var isRoomAdministrator = await _bll.ChatRooms.IsRoomAdministratorAsync(member.ChatRoomId, User.UserId());

            if (isRoomAdministrator)
            {
                member.ChatRole = role;
                await _bll.ChatMembers.UpdateAsync(member);
                await _bll.SaveChangesAsync();
                return NoContent();
            }
            
            return BadRequest(new ErrorResponseDTO("Only room administrator can assign roles!"));
        }
    }
}
