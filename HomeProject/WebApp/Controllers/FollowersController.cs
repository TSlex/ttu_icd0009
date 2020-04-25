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
using Follower = DAL.App.DTO.Follower;

namespace WebApp.Controllers
{
    public class FollowersController : Controller
    {
        private readonly IAppBLL _bll;

        public FollowersController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: Followers
        public async Task<IActionResult> Index()
        {
            return View(await _bll.Followers.AllAsync());
        }

        // GET: Followers/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var follower = await _bll.Followers.FindAsync(id);

            if (follower == null)
            {
                return NotFound();
            }

            return View(follower);
        }

        // GET: Followers/Create
        public IActionResult Create()
        {

            return View();
        }

        // POST: Followers/Create
        // To protect from overfollowering attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            BLL.App.DTO.Follower follower)
        {
            ModelState.Clear();
            follower.ProfileId = User.UserId();
            follower.ChangedAt = DateTime.Now;
            follower.CreatedAt = DateTime.Now;

            if (TryValidateModel(follower))
            {
                follower.Id = Guid.NewGuid();
                _bll.Followers.Add(follower);
                await _bll.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(follower);
        }

        // GET: Followers/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {

            var follower = await _bll.Followers.FindAsync(id);

            if (follower == null)
            {
                return NotFound();
            }


            return View(follower);
        }

        // POST: Followers/Edit/5
        // To protect from overfollowering attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            BLL.App.DTO.Follower follower)
        {
            if (id != follower.Id || User.UserId() != follower.ProfileId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _bll.Followers.UpdateAsync(follower);
                await _bll.SaveChangesAsync();


                return RedirectToAction(nameof(Index));
            }

            return View(follower);
        }

        // GET: Followers/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {

            var follower = await _bll.Followers.FindAsync(id);

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
            await _bll.Followers.RemoveAsync(id);
            await _bll.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
