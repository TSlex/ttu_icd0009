using System;
using System.Threading.Tasks;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class BlockedProfilesController : Controller
    {
        private readonly IAppBLL _bll;

        public BlockedProfilesController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: BlockedProfiles
        public async Task<IActionResult> Index()
        {
            return View(await _bll.BlockedProfiles.AllAsync());
        }

        // GET: BlockedProfiles/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var blockedProfile = await _bll.BlockedProfiles.FindAsync(id);

            if (blockedProfile == null)
            {
                return NotFound();
            }

            return View(blockedProfile);
        }

        // GET: BlockedProfiles/Create
        public IActionResult Create()
        {
//            ViewData["ProfileId"] = new SelectList(_context.Profiles, "Id", "Id");
            return View();
        }

        // POST: BlockedProfiles/Create
        // To protect from overblockedProfileing attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            BLL.App.DTO.BlockedProfile blockedProfile)
        {
            ModelState.Clear();

            if (TryValidateModel(blockedProfile))
            {
                blockedProfile.Id = Guid.NewGuid();
                _bll.BlockedProfiles.Add(blockedProfile);
                await _bll.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(blockedProfile);
        }

        // GET: BlockedProfiles/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            var blockedProfile = await _bll.BlockedProfiles.FindAsync(id);

            if (blockedProfile == null)
            {
                return NotFound();
            }
            
            return View(blockedProfile);
        }

        // POST: BlockedProfiles/Edit/5
        // To protect from overblockedProfileing attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            BLL.App.DTO.BlockedProfile blockedProfile)
        {
            if (id != blockedProfile.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _bll.BlockedProfiles.UpdateAsync(blockedProfile);
                await _bll.SaveChangesAsync();


                return RedirectToAction(nameof(Index));
            }

            return View(blockedProfile);
        }

        // GET: BlockedProfiles/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {

            var blockedProfile = await _bll.BlockedProfiles.FindAsync(id);

            if (blockedProfile == null)
            {
                return NotFound();
            }

            return View(blockedProfile);
        }

        // POST: BlockedProfiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            _bll.BlockedProfiles.Remove(id);
            await _bll.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
