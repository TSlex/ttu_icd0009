using System;
using System.Linq;
using System.Threading.Tasks;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Areas.Admin.Controllers
{
    /// <summary>
    /// Chat members
    /// </summary>
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
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
        public async Task<IActionResult> Index()
        {
            return View(await _bll.ChatMembers.AllAdminAsync());
        }
        
        /// <summary>
        /// Get record history
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> History(Guid id)
        {
            var history = (await _bll.ChatMembers.GetRecordHistoryAsync(id)).ToList()
                .OrderByDescending(record => record.CreatedAt);
            
            return View(nameof(Index), history);
        }

        /// <summary>
        /// Get record details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Details(Guid id)
        {
            var chatMember = await _bll.ChatMembers.FindAsync(id);

            if (chatMember == null)
            {
                return NotFound();
            }

            return View(chatMember);
        }

        /// <summary>
        /// Get record creating page
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Creates a new record
        /// </summary>
        /// <param name="chatMember"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            BLL.App.DTO.ChatMember chatMember)
        {
            ModelState.Clear();

            if (TryValidateModel(chatMember))
            {
                chatMember.Id = Guid.NewGuid();
                _bll.ChatMembers.Add(chatMember);
                await _bll.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(chatMember);
        }

        /// <summary>
        /// Get record editing page
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Edit(Guid id)
        {
            var chatMember = await _bll.ChatMembers.FindAsync(id);

            if (chatMember == null)
            {
                return NotFound();
            }
            
            return View(chatMember);
        }

        /// <summary>
        /// Updates a record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="chatMember"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            BLL.App.DTO.ChatMember chatMember)
        {
            if (id != chatMember.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _bll.ChatMembers.UpdateAsync(chatMember);
                await _bll.SaveChangesAsync();


                return RedirectToAction(nameof(Index));
            }

            return View(chatMember);
        }

        /// <summary>
        /// Get delete confirmation page
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(Guid id)
        {

            var chatMember = await _bll.ChatMembers.FindAsync(id);

            if (chatMember == null)
            {
                return NotFound();
            }

            return View(chatMember);
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
            _bll.ChatMembers.Remove(id);
            await _bll.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        
        /// <summary>
        /// Restores a record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(Guid id)
        {
            var record = await _bll.ChatMembers.GetForUpdateAsync(id);
            _bll.ChatMembers.Restore(record);
            await _bll.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
