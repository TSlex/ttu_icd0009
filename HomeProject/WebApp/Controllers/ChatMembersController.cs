using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.BLL.App;
using Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Controllers
{
    /// <summary>
    /// Chat members
    /// </summary>
    [Authorize]
    public class ChatMembersController : Controller
    {
        private readonly IAppBLL _bll;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bll"></param>
        public ChatMembersController(IAppBLL bll)
        {
            _bll = bll;
        }

        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index(Guid chatRoomId)
        {
            return View(await _bll.ChatMembers.RoomAllAsync(chatRoomId));
        }
        
        public class ChatMemberRoleModel
        {
            public IEnumerable<ChatRole>? ChatRoles { get; set; }
            public ChatMember ChatMember { get; set; } = default!;
        }

        /// <summary>
        /// Get record editing page
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Edit(Guid id)
        {
            var chatMember = await _bll.ChatMembers.FindAsync(id);
            var chatRoles = await _bll.ChatRoles.AllAsync();

            if (chatMember == null)
            {
                return NotFound();
            }

            var model = new ChatMemberRoleModel()
            {
                ChatRoles = chatRoles,
                ChatMember = chatMember
            };

            return View(model);
        }

        /// <summary>
        /// Updates a record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            ChatMemberRoleModel model)
        {
            var member = await _bll.ChatMembers.FindAsync(id);
            var chatRole = await _bll.ChatRoles.FindAsync(model.ChatMember.ChatRoleId);
            
            if (member == null || chatRole == null || id != model.ChatMember.Id)
            {
                return NotFound();
            }

            if (member.ChatRole.RoleTitle.ToLower().Contains("creator") || member.ChatRole.RoleTitle.ToLower().Contains("admin"))
            {
                ModelState.AddModelError(string.Empty, "You cannot change creator role!");
            }
            
            if (chatRole.RoleTitle.ToLower().Contains("creator") || chatRole.RoleTitle.ToLower().Contains("admin"))
            {
                ModelState.AddModelError(string.Empty, "You cannot assign creator role!");
            }

            if (!(await _bll.ChatRooms.IsRoomAdministratorAsync(member.ChatRoomId, User.UserId())))
            {
                ModelState.AddModelError(string.Empty, "Only room creator can change roles");
            }

            if (ModelState.IsValid)
            {
                model.ChatMember.ChatRoomId = member.ChatRoomId;
                model.ChatMember.ProfileId = member.ProfileId;
                
                await _bll.ChatMembers.UpdateAsync(model.ChatMember);
                await _bll.SaveChangesAsync();


                return RedirectToAction(nameof(Index), new { chatRoomId = member.ChatRoomId});
            }
            
            var chatRoles = await _bll.ChatRoles.AllAsync();

            model.ChatRoles = chatRoles;
            
            return View(model);
        }

        /// <summary>
        /// Deletes a record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var chatMember = await _bll.ChatMembers.GetForUpdateAsync(id);
            _bll.ChatMembers.Remove(chatMember);
            await _bll.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new {chatRoomId = chatMember.ChatRoomId});
        }
    }
}
