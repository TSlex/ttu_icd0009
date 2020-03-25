using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.DAL.App;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;
using Extension;

namespace WebApp.Controllers
{
    public class ProfileRanksController : Controller
    {
        private readonly IAppUnitOfWork _uow;

        public ProfileRanksController(IAppUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: ProfileRanks
        public async Task<IActionResult> Index()
        {
            return View(await _uow.ProfileRanks.AllAsync());
        }

        // GET: ProfileRanks/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profileRank = await _uow.ProfileRanks.FindAsync(id);

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
            ProfileRank profileRank)
        {
            ModelState.Clear();
            profileRank.ProfileId = User.UserId();
            profileRank.ChangedAt = DateTime.Now;
            profileRank.CreatedAt = DateTime.Now;

            if (TryValidateModel(profileRank))
            {
                profileRank.Id = Guid.NewGuid();
                _uow.ProfileRanks.Add(profileRank);
                await _uow.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(profileRank);
        }

        // GET: ProfileRanks/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profileRank = await _uow.ProfileRanks.FindAsync(id);

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
            ProfileRank profileRank)
        {
            if (id != profileRank.Id || User.UserId() != profileRank.ProfileId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _uow.ProfileRanks.Update(profileRank);
                await _uow.SaveChangesAsync();


                return RedirectToAction(nameof(Index));
            }

            return View(profileRank);
        }

        // GET: ProfileRanks/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profileRank = await _uow.ProfileRanks.FindAsync(id);

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
            _uow.ProfileRanks.Remove(id);
            await _uow.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
