using System;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Mvc;
using Extension;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Controllers
{
    /// <summary>
    /// Messages
    /// </summary>
    [Authorize]
    public class MessagesController : Controller
    {
        private readonly IAppBLL _bll;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bll"></param>
        public MessagesController(IAppBLL bll)
        {
            _bll = bll;
        }

        /// <summary>
        /// Get record creating page
        /// </summary>
        /// <returns></returns>
        public IActionResult Create(Guid chatRoomId)
        {
            var message = new Message()
            {
                ChatRoomId = chatRoomId,
            };

            return View(message);
        }

        /// <summary>
        /// Creates a new record
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Message message)
        {
            ModelState.Clear();

            message.ProfileId = User.UserId();

            var currentMember = await _bll.ChatMembers.FindByUserAndRoomAsync(User.UserId(), message.ChatRoomId);

            if (!(currentMember != null &&
                  (message.ProfileId == User.UserId() && currentMember.ChatRole.CanWriteMessages ||
                   currentMember.ChatRole.CanEditAllMessages)))
            {
                ModelState.AddModelError(string.Empty, Resourses.BLL.App.DTO.Common.ErrorAccessDenied);
                return View(message);
            }

            if (TryValidateModel(message))
            {
                message.Id = Guid.NewGuid();
                _bll.Messages.Add(message);
                await _bll.SaveChangesAsync();

                return RedirectToAction("Details", "ChatRooms", new {id = message.ChatRoomId});
            }

            return View(message);
        }

        /// <summary>
        /// Get record editing page
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Edit(Guid id)
        {
            var message = await _bll.Messages.FindAsync(id);

            if (message == null)
            {
                return NotFound();
            }

            var currentMember = await _bll.ChatMembers.FindByUserAndRoomAsync(User.UserId(), message.ChatRoomId);

            if (!(currentMember != null &&
                  (message.ProfileId == User.UserId() && currentMember.ChatRole.CanEditMessages ||
                   currentMember.ChatRole.CanEditAllMessages)))
            {
                return NotFound();
            }

            return View(message);
        }

        /// <summary>
        /// Updates a record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Message message)
        {
            var record = await _bll.Messages.GetForUpdateAsync(id);

            if (record == null || id != message.Id)
            {
                ModelState.AddModelError(string.Empty, Resourses.BLL.App.DTO.Common.ErrorIdMatch);
                return View(message);
            }

            var currentMember = await _bll.ChatMembers.FindByUserAndRoomAsync(User.UserId(), message.ChatRoomId);

            if (!(currentMember != null &&
                  (record.ProfileId == User.UserId() && currentMember.ChatRole.CanEditMessages ||
                   currentMember.ChatRole.CanEditAllMessages)))
            {
                ModelState.AddModelError(string.Empty, Resourses.BLL.App.DTO.Common.ErrorAccessDenied);
                return View(message);
            }

            if (ModelState.IsValid)
            {
                record.MessageValue = message.MessageValue;

                await _bll.Messages.UpdateAsync(record);
                await _bll.SaveChangesAsync();

                return RedirectToAction("Details", "ChatRooms", new {id = record.ChatRoomId});
            }

            return View(record);
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
            var record = await _bll.Messages.GetForUpdateAsync(id);

            var currentMember = await _bll.ChatMembers.FindByUserAndRoomAsync(User.UserId(), record.ChatRoomId);

            if (!(currentMember != null &&
                  (record.ProfileId == User.UserId() && currentMember.ChatRole.CanEditMessages ||
                   currentMember.ChatRole.CanEditAllMessages)))
            {
                return NotFound();
            }

            _bll.Messages.Remove(id);
            await _bll.SaveChangesAsync();

            return RedirectToAction("Details", "ChatRooms", new {id = record.ChatRoomId});
        }
    }
}