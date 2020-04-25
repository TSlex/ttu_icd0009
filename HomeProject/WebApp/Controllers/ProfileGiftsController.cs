using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.BLL.App;
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
        private readonly IAppBLL _bll;

        public ProfileGiftsController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: ProfileGifts
        public async Task<IActionResult> Index()
        {
            return View(await _bll.ProfileGifts.AllAsync());
        }

        // GET: ProfileGifts/Details/5
        public async Task<IActionResult> Details(Guid id)
        {


            var profileGift = await _bll.ProfileGifts.FindAsync(id);

            if (profileGift == null)
            {
                return NotFound();
            }

            return View(profileGift);
        }

        // GET: ProfileGifts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProfileGifts/Create
        // To protect from overprofileGifting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            BLL.App.DTO.ProfileGift profileGift)
        {
            ModelState.Clear();
            profileGift.ProfileId = User.UserId();
            profileGift.ChangedAt = DateTime.Now;
            profileGift.CreatedAt = DateTime.Now;

            if (TryValidateModel(profileGift))
            {
                profileGift.Id = Guid.NewGuid();
                _bll.ProfileGifts.Add(profileGift);
                await _bll.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(profileGift);
        }

        // GET: ProfileGifts/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {


            var profileGift = await _bll.ProfileGifts.FindAsync(id);

            if (profileGift == null)
            {
                return NotFound();
            }


            return View(profileGift);
        }

        // POST: ProfileGifts/Edit/5
        // To protect from overprofileGifting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            BLL.App.DTO.ProfileGift profileGift)
        {
            if (id != profileGift.Id || User.UserId() != profileGift.ProfileId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _bll.ProfileGifts.UpdateAsync(profileGift);
                await _bll.SaveChangesAsync();


                return RedirectToAction(nameof(Index));
            }

            return View(profileGift);
        }

        // GET: ProfileGifts/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {

            var profileGift = await _bll.ProfileGifts.FindAsync(id);

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
            await _bll.ProfileGifts.RemoveAsync(id);
            await _bll.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}