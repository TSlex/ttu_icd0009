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
    public class FollowersController : Controller
    {
        private readonly IAppUnitOfWork _uow;

        public FollowersController(IAppUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: Followers
        public async Task<IActionResult> Index()
        {
            return View(await _uow.Followers.AllAsync());
        }

        // GET: Followers/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var follower = await _uow.Followers.FindAsync(id);

            if (follower == null)
            {
                return NotFound();
            }

            return View(follower);
        }

        // GET: Followers/Create
        public IActionResult Create()
        {
//            ViewData["ProfileId"] = new SelectList(_context.Profiles, "Id", "Id");
            return View();
        }

        // POST: Followers/Create
        // To protect from overfollowering attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            Follower follower)
        {
            ModelState.Clear();
            follower.ProfileId = User.UserId();
            follower.ChangedAt = DateTime.Now;
            follower.CreatedAt = DateTime.Now;

            if (TryValidateModel(follower))
            {
                follower.Id = Guid.NewGuid();
                _uow.Followers.Add(follower);
                await _uow.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(follower);
        }

        // GET: Followers/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var follower = await _uow.Followers.FindAsync(id);

            if (follower == null)
            {
                return NotFound();
            }

//            ViewData["ProfileId"] = new SelectList(_context.Profiles, "Id", "Id", follower.ProfileId);
            return View(follower);
        }

        // POST: Followers/Edit/5
        // To protect from overfollowering attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            Follower follower)
        {
            if (id != follower.Id || User.UserId() != follower.ProfileId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _uow.Followers.Update(follower);
                await _uow.SaveChangesAsync();


                return RedirectToAction(nameof(Index));
            }

            return View(follower);
        }

        // GET: Followers/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var follower = await _uow.Followers.FindAsync(id);

            if (follower == null)
            {
                return NotFound();
            }

            return View(follower);
        }

        // POST: Followers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            _uow.Followers.Remove(id);
            await _uow.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
