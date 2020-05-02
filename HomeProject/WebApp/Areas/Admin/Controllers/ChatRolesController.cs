using System;
using System.Threading.Tasks;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class ChatRolesController : Controller
    {
        private readonly IAppBLL _bll;

        public ChatRolesController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: ChatRoles
        public async Task<IActionResult> Index()
        {
            return View(await _bll.ChatRoles.AllAsync());
        }

        // GET: ChatRoles/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var chatRole = await _bll.ChatRoles.FindAsync(id);

            if (chatRole == null)
            {
                return NotFound();
            }

            return View(chatRole);
        }

        // GET: ChatRoles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ChatRoles/Create
        // To protect from overchatRoleing attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            BLL.App.DTO.ChatRole chatRole)
        {
            ModelState.Clear();

            if (TryValidateModel(chatRole))
            {
                chatRole.Id = Guid.NewGuid();
                _bll.ChatRoles.Add(chatRole);
                await _bll.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(chatRole);
        }

        // GET: ChatRoles/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            var chatRole = await _bll.ChatRoles.FindAsync(id);

            if (chatRole == null)
            {
                return NotFound();
            }
            
            return View(chatRole);
        }

        // POST: ChatRoles/Edit/5
        // To protect from overchatRoleing attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: ChatRoles/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            var chatRole = await _bll.ChatRoles.FindAsync(id);

            if (chatRole == null)
            {
                return NotFound();
            }

            return View(chatRole);
        }

        // POST: ChatRoles/Delete/5
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
