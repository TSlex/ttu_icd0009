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
using Favorite = DAL.App.DTO.Favorite;

namespace WebApp.Controllers
{
    public class FavoritesController : Controller
    {
        private readonly IAppUnitOfWork _uow;

        public FavoritesController(IAppUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: Favorites
        public async Task<IActionResult> Index()
        {
            return View(await _uow.Favorites.AllAsync());
        }

        // GET: Favorites/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var favorite = await _uow.Favorites.FindAsync(id);

            if (favorite == null)
            {
                return NotFound();
            }

            return View(favorite);
        }

        // GET: Favorites/Create
        public IActionResult Create()
        {
//            ViewData["ProfileId"] = new SelectList(_context.Profiles, "Id", "Id");
            return View();
        }

        // POST: Favorites/Create
        // To protect from overfavoriteing attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            DAL.App.DTO.Favorite favorite)
        {
            ModelState.Clear();
            favorite.ProfileId = User.UserId();
            favorite.ChangedAt = DateTime.Now;
            favorite.CreatedAt = DateTime.Now;

            if (TryValidateModel(favorite))
            {
                favorite.Id = Guid.NewGuid();
                _uow.Favorites.Add(favorite);
                await _uow.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(favorite);
        }

        // GET: Favorites/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var favorite = await _uow.Favorites.FindAsync(id);

            if (favorite == null)
            {
                return NotFound();
            }

//            ViewData["ProfileId"] = new SelectList(_context.Profiles, "Id", "Id", favorite.ProfileId);
            return View(favorite);
        }

        // POST: Favorites/Edit/5
        // To protect from overfavoriteing attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            Favorite favorite)
        {
            if (id != favorite.Id || User.UserId() != favorite.ProfileId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _uow.Favorites.Update(favorite);
                await _uow.SaveChangesAsync();


                return RedirectToAction(nameof(Index));
            }

            return View(favorite);
        }

        // GET: Favorites/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var favorite = await _uow.Favorites.FindAsync(id);

            if (favorite == null)
            {
                return NotFound();
            }

            return View(favorite);
        }

        // POST: Favorites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            _uow.Favorites.Remove(id);
            await _uow.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
