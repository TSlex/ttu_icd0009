using System;
using System.Threading.Tasks;
using Contracts.BLL.App;
using Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class ProfileRanksController : Controller
    {
        private readonly IAppBLL _bll;

        public ProfileRanksController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: ProfileRanks
        public async Task<IActionResult> Index()
        {
            return View(await _bll.ProfileRanks.AllAsync());
        }

        // GET: ProfileRanks/Details/5
        public async Task<IActionResult> Details(Guid id)
        {


            var profileRank = await _bll.ProfileRanks.FindAsync(id);

            if (profileRank == null)
            {
                return NotFound();
            }

            return View(profileRank);
        }

        // GET: ProfileRanks/Create
        public IActionResult Create()
        {
//            ViewData["ProfileId"] = new SelectList(_context.Profiles, "Id", "Id");
            return View();
        }

        // POST: ProfileRanks/Create
        // To protect from overprofileRanking attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            BLL.App.DTO.ProfileRank profileRank)
        {
            ModelState.Clear();
            profileRank.ProfileId = User.UserId();
            profileRank.ChangedAt = DateTime.Now;
            profileRank.CreatedAt = DateTime.Now;

            if (TryValidateModel(profileRank))
            {
                profileRank.Id = Guid.NewGuid();
                _bll.ProfileRanks.Add(profileRank);
                await _bll.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(profileRank);
        }

        // GET: ProfileRanks/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {


            var profileRank = await _bll.ProfileRanks.FindAsync(id);

            if (profileRank == null)
            {
                return NotFound();
            }

//            ViewData["ProfileId"] = new SelectList(_context.Profiles, "Id", "Id", profileRank.ProfileId);
            return View(profileRank);
        }

        // POST: ProfileRanks/Edit/5
        // To protect from overprofileRanking attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            BLL.App.DTO.ProfileRank profileRank)
        {
            if (id != profileRank.Id || User.UserId() != profileRank.ProfileId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _bll.ProfileRanks.UpdateAsync(profileRank);
                await _bll.SaveChangesAsync();


                return RedirectToAction(nameof(Index));
            }

            return View(profileRank);
        }

        // GET: ProfileRanks/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {


            var profileRank = await _bll.ProfileRanks.FindAsync(id);

            if (profileRank == null)
            {
                return NotFound();
            }

            return View(profileRank);
        }

        // POST: ProfileRanks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            _bll.ProfileRanks.Remove(id);
            await _bll.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
