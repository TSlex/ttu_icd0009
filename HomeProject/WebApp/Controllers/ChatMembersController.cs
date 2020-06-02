using System;
using System.Collections.Generic;
using System.Linq;
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
            var members = await _bll.ChatMembers.RoomAllAsync(chatRoomId);

            if (!members.Select(member => member.Profile.UserName).Contains(User.Identity.Name))
            {
                return NotFound();
            }
            
            return View(members);
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
            var chatRoles = (await _bll.ChatRoles.AllAsync());

            if (chatMember == null)
            {
                return NotFound();
            }
            
            var currentMember = await _bll.ChatMembers.FindByUserAndRoomAsync(User.UserId(), chatMember.ChatRoomId);
            
            if (currentMember == null || !currentMember.ChatRole.CanEditMembers)
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

            var currentMember = await _bll.ChatMembers.FindByUserAndRoomAsync(User.UserId(), member.ChatRoomId);
            
            if (currentMember == null || !currentMember.ChatRole.CanEditMembers)
            {
                return NotFound();
            }

            if (member.ChatRole.CanEditMembers)
            {
                ModelState.AddModelError(string.Empty, Resourses.BLL.App.DTO.ChatMembers.ChatMembers.ErrorCreatorDemote);
            }

            if (chatRole.CanEditMembers)
            {
                ModelState.AddModelError(string.Empty, Resourses.BLL.App.DTO.ChatMembers.ChatMembers.ErrorCreatorAssign);
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

            model.ChatMember = member;
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

            if (chatMember == null)
            {
                return NotFound();
            }

            var currentMember = await _bll.ChatMembers.FindByUserAndRoomAsync(User.UserId(), chatMember.ChatRoomId);

            if (!currentMember.ChatRole.CanEditMembers)
            {
                return NotFound();
            }
            
            _bll.ChatMembers.Remove(chatMember);
            await _bll.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new {chatRoomId = chatMember.ChatRoomId});
        }
    }
}
