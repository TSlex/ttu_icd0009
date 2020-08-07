using System;
using System.Linq;
using System.Threading.Tasks;
using Contracts.BLL.App;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Extension;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Controllers
{
    /// <summary>
    /// chat rooms
    /// </summary>
    [Authorize]
    [Route("/{controller}/{action=Index}/{id?}")]
    public class ChatRoomsController : Controller
    {
        private readonly IAppBLL _bll;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bll"></param>
        public ChatRoomsController(IAppBLL bll)
        {
            _bll = bll;
        }

        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            return View(await _bll.ChatRooms.AllAsync(User.UserId()));
        }

        /// <summary>
        /// Get record details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("/{controller}/{id}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var chatRoom = await _bll.ChatRooms.FindAsync(id);

            if (!chatRoom.ChatMembers.Select(member => member.ProfileId).Contains(User.UserId()))
            {
                return RedirectToAction("PageNotFound", "Home", new {area = ""});
            }

            return View(chatRoom);
        }

        /// <summary>
        /// Opens chat room with user (if not exist - creates a new one)
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<IActionResult> OpenOrCreate(string username)
        {
            var chatRoomId = await _bll.ChatRooms.OpenOrCreateAsync(username);

            return RedirectToAction("Details", new
            {
                Id = chatRoomId
            });
        }

        /// <summary>
        /// Leaves room (member deletes, messages not)
        /// </summary>
        /// <param name="chatRoomId"></param>
        /// <returns></returns>
        public async Task<IActionResult> LeaveChat(Guid chatRoomId)
        {
            var chatMember = await _bll.ChatMembers.FindByUserAndRoomAsync(User.UserId(), chatRoomId);

            if (chatMember == null)
            {
                return RedirectToAction("PageNotFound", "Home", new {area = ""});
            }

            _bll.ChatMembers.Remove(chatMember);
            await _bll.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Get record editing page
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Edit(Guid id)
        {
            var chatRoom = await _bll.ChatRooms.FindAsync(id);
            var member = await _bll.ChatMembers.FindByUserAndRoomAsync(User.UserId(), id);

            if (chatRoom == null || member == null || !member.ChatRole!.CanRenameRoom)
            {
                return RedirectToAction("PageNotFound", "Home", new {area = ""});
            }

            return View(chatRoom);
        }

        /// <summary>
        /// Updates a record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="chatRoom"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, BLL.App.DTO.ChatRoom chatRoom)
        {
            var record = await _bll.ChatRooms.GetForUpdateAsync(id);
            var member = await _bll.ChatMembers.FindByUserAndRoomAsync(User.UserId(), chatRoom.Id);

            if (record == null || id != chatRoom.Id || member == null || !member.ChatRole!.CanRenameRoom)
            {
                ModelState.AddModelError(string.Empty, Resourses.BLL.App.DTO.Common.ErrorBadData);
                return View(chatRoom);
            }

            if (ModelState.IsValid)
            {
                record.ChatRoomTitle = chatRoom.ChatRoomTitle;

                await _bll.ChatRooms.UpdateAsync(record);

                await _bll.SaveChangesAsync();

                return RedirectToAction("Details", new {id} );
            }

            return View(record);
        }
    }
}