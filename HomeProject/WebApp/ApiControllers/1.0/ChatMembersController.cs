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
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorNotFound));
            }
            
            var canAccess = await _bll.ChatRooms.IsRoomMemberAsync(chatRoomId, User.UserId());
            
            if (!canAccess)
            {
                return BadRequest(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorAccessDenied));
            }

            return Ok((await _bll.ChatMembers.RoomAllAsync(chatRoomId)).Select(member => new ChatMemberDTO
            {
                Id = member.Id,
                UserName = member.Profile!.UserName,
                ChatRole = member.ChatRole!.RoleTitle,
                ChatRoleValue = member.ChatRole!.RoleTitleValue,
                ProfileAvatarId = member.Profile!.ProfileAvatarId,
                CanEditMembers = member.ChatRole!.CanEditMembers,
                CanEditMessages = member.ChatRole!.CanEditMessages,
                CanRenameRoom = member.ChatRole!.CanRenameRoom,
                CanWriteMessages = member.ChatRole!.CanWriteMessages,
                CanEditAllMessages = member.ChatRole!. CanEditAllMessages
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
                return BadRequest(new ErrorResponseDTO(Resourses.BLL.App.DTO.ChatMembers.ChatMembers.ErrorCreatorAssign));
            }
            
            var role = await _bll.ChatRoles.FindAsync(roleTitle);

            if (role == null)
            {
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.ChatMembers.ChatMembers.ErrorRoleNotFound));
            }

            var member = await _bll.ChatMembers.FindAsync(id);
            
            if (member == null)
            {
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.ChatMembers.ChatMembers.ErrorMemberNotFound));
            }

            if (member.ChatRole!.RoleTitle.Contains("Creator"))
            {
                return BadRequest(new ErrorResponseDTO(Resourses.BLL.App.DTO.ChatMembers.ChatMembers.ErrorCreatorDemote));
            }

            var isRoomAdministrator = await _bll.ChatRooms.IsRoomAdministratorAsync(member.ChatRoomId, User.UserId());

            if (isRoomAdministrator)
            {
                member.ChatRole = null;
                member.ChatRoleId = role.Id;
                await _bll.ChatMembers.UpdateAsync(member);
                await _bll.SaveChangesAsync();
                return NoContent();
            }
            
            return BadRequest(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorAccessDenied));
        }
        
        /// <summary>
        /// Deletes a member
        /// </summary>
        /// <param name="id">Member id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OkResponseDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponseDTO))]
        public async Task<IActionResult> DeleteMember(Guid id)
        {
            var chatMember = await _bll.ChatMembers.GetForUpdateAsync(id);

            if (chatMember == null)
            {
                return NotFound(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorNotFound));
            }

            var currentMember = await _bll.ChatMembers.FindByUserAndRoomAsync(User.UserId(), chatMember.ChatRoomId);

            if (!currentMember.ChatRole.CanEditMembers)
            {
                return BadRequest(new ErrorResponseDTO(Resourses.BLL.App.DTO.Common.ErrorAccessDenied));
            }
            
            _bll.ChatMembers.Remove(chatMember);
            await _bll.SaveChangesAsync();

            return Ok(new OkResponseDTO(){Status = Resourses.BLL.App.DTO.Common.SuccessDeleted});
        }
    }
}
