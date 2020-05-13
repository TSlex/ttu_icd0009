using System;
using System.Threading.Tasks;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Controllers
{
    /// <summary>
    /// Chat roles
    /// </summary>
    [Authorize]
    public class ChatRolesController : Controller
    {
        private readonly IAppBLL _bll;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bll"></param>
        public ChatRolesController(IAppBLL bll)
        {
            _bll = bll;
        }

        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            return View(await _bll.ChatRoles.AllAsync());
        }

        /// <summary>
        /// Get record details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Details(Guid id)
        {
            var chatRole = await _bll.ChatRoles.FindAsync(id);

            if (chatRole == null)
            {
                return NotFound();
            }

            return View(chatRole);
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
        /// <param name="chatRole"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            BLL.App.DTO.ChatRole chatRole)
        {
            ModelState.Clear();
            chatRole.ChangedAt = DateTime.Now;
            chatRole.CreatedAt = DateTime.Now;

            if (TryValidateModel(chatRole))
            {
                chatRole.Id = Guid.NewGuid();
                _bll.ChatRoles.Add(chatRole);
                await _bll.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(chatRole);
        }

        /// <summary>
        /// Get record editing page
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Edit(Guid id)
        {
            var chatRole = await _bll.ChatRoles.FindAsync(id);

            if (chatRole == null)
            {
                return NotFound();
            }
            
            return View(chatRole);
        }

        /// <summary>
        /// Updates a record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="chatRole"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            BLL.App.DTO.ChatRole chatRole)
        {
            if (id != chatRole.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _bll.ChatRoles.UpdateAsync(chatRole);
                await _bll.SaveChangesAsync();


                return RedirectToAction(nameof(Index));
            }

            return View(chatRole);
        }

        /// <summary>
        /// Get delete confirmation page
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(Guid id)
        {
            var chatRole = await _bll.ChatRoles.FindAsync(id);

            if (chatRole == null)
            {
                return NotFound();
            }

            return View(chatRole);
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
            _bll.ChatRoles.Remove(id);
            await _bll.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
