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
using Microsoft.AspNetCore.Authorization;
using Favorite = DAL.App.DTO.Favorite;

namespace WebApp.Controllers
{
    [Authorize]
    public class FavoritesController : Controller
    {
        private readonly IAppBLL _bll;

        public FavoritesController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: Favorites
        public async Task<IActionResult> Index()
        {
            return View(await _bll.Favorites.AllAsync());
        }

        // GET: Favorites/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var favorite = await _bll.Favorites.FindAsync(id);

            if (favorite == null)
            {
                return NotFound();
            }

            return View(favorite);
        }

        // GET: Favorites/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Favorites/Create
        // To protect from overfavoriteing attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            BLL.App.DTO.Favorite favorite)
        {
            ModelState.Clear();
            favorite.ProfileId = User.UserId();
            favorite.ChangedAt = DateTime.Now;
            favorite.CreatedAt = DateTime.Now;

            if (TryValidateModel(favorite))
            {
                favorite.Id = Guid.NewGuid();
                _bll.Favorites.Add(favorite);
                await _bll.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(favorite);
        }

        // GET: Favorites/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {


            var favorite = await _bll.Favorites.FindAsync(id);

            if (favorite == null)
            {
                return NotFound();
            }
            
            return View(favorite);
        }

        // POST: Favorites/Edit/5
        // To protect from overfavoriteing attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            BLL.App.DTO.Favorite favorite)
        {
            if (id != favorite.Id || User.UserId() != favorite.ProfileId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _bll.Favorites.UpdateAsync(favorite);
                await _bll.SaveChangesAsync();


                return RedirectToAction(nameof(Index));
            }

            return View(favorite);
        }

        // GET: Favorites/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {


            var favorite = await _bll.Favorites.FindAsync(id);

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
            await _bll.Favorites.RemoveAsync(id);
            await _bll.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}