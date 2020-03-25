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
    public class GiftsController : Controller
    {
        private readonly IAppUnitOfWork _uow;

        public GiftsController(IAppUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: Gifts
        public async Task<IActionResult> Index()
        {
            return View(await _uow.Gifts.AllAsync());
        }

        // GET: Gifts/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gift = await _uow.Gifts.FindAsync(id);

            if (gift == null)
            {
                return NotFound();
            }

            return View(gift);
        }

        // GET: Gifts/Create
        public IActionResult Create()
        {
//            ViewData["ProfileId"] = new SelectList(_context.Profiles, "Id", "Id");
            return View();
        }

        // POST: Gifts/Create
        // To protect from overgifting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            Gift gift)
        {
            ModelState.Clear();
            gift.ChangedAt = DateTime.Now;
            gift.CreatedAt = DateTime.Now;

            if (TryValidateModel(gift))
            {
                gift.Id = Guid.NewGuid();
                _uow.Gifts.Add(gift);
                await _uow.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(gift);
        }

        // GET: Gifts/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gift = await _uow.Gifts.FindAsync(id);

            if (gift == null)
            {
                return NotFound();
            }

//            ViewData["ProfileId"] = new SelectList(_context.Profiles, "Id", "Id", gift.ProfileId);
            return View(gift);
        }

        // POST: Gifts/Edit/5
        // To protect from overgifting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            Gift gift)
        {
            if (id != gift.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _uow.Gifts.Update(gift);
                await _uow.SaveChangesAsync();


                return RedirectToAction(nameof(Index));
            }

            return View(gift);
        }

        // GET: Gifts/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gift = await _uow.Gifts.FindAsync(id);

            if (gift == null)
            {
                return NotFound();
            }

            return View(gift);
        }

        // POST: Gifts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            _uow.Gifts.Remove(id);
            await _uow.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
