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
using ProfileGift = DAL.App.DTO.ProfileGift;

namespace WebApp.Controllers
{
    public class ProfileGiftsController : Controller
    {
        private readonly IAppUnitOfWork _uow;

        public ProfileGiftsController(IAppUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: ProfileGifts
        public async Task<IActionResult> Index()
        {
            return View(await _uow.ProfileGifts.AllAsync());
        }

        // GET: ProfileGifts/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profileGift = await _uow.ProfileGifts.FindAsync(id);

            if (profileGift == null)
            {
                return NotFound();
            }

            return View(profileGift);
        }

        // GET: ProfileGifts/Create
        public IActionResult Create()
        {
//            ViewData["ProfileId"] = new SelectList(_context.Profiles, "Id", "Id");
            return View();
        }

        // POST: ProfileGifts/Create
        // To protect from overprofileGifting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            DAL.App.DTO.ProfileGift profileGift)
        {
            ModelState.Clear();
            profileGift.ProfileId = User.UserId();
            profileGift.ChangedAt = DateTime.Now;
            profileGift.CreatedAt = DateTime.Now;

            if (TryValidateModel(profileGift))
            {
                profileGift.Id = Guid.NewGuid();
                _uow.ProfileGifts.Add(profileGift);
                await _uow.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(profileGift);
        }

        // GET: ProfileGifts/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profileGift = await _uow.ProfileGifts.FindAsync(id);

            if (profileGift == null)
            {
                return NotFound();
            }

//            ViewData["ProfileId"] = new SelectList(_context.Profiles, "Id", "Id", profileGift.ProfileId);
            return View(profileGift);
        }

        // POST: ProfileGifts/Edit/5
        // To protect from overprofileGifting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            ProfileGift profileGift)
        {
            if (id != profileGift.Id || User.UserId() != profileGift.ProfileId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _uow.ProfileGifts.Update(profileGift);
                await _uow.SaveChangesAsync();


                return RedirectToAction(nameof(Index));
            }

            return View(profileGift);
        }

        // GET: ProfileGifts/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profileGift = await _uow.ProfileGifts.FindAsync(id);

            if (profileGift == null)
            {
                return NotFound();
            }

            return View(profileGift);
        }

        // POST: ProfileGifts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            _uow.ProfileGifts.Remove(id);
            await _uow.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}