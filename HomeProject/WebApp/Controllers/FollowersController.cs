using System;
using System.Threading.Tasks;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Mvc;
using Extension;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Controllers
{
    [Authorize]
    public class FollowersController : Controller
    {
        private readonly IAppBLL _bll;

        public FollowersController(IAppBLL bll)
        {
            _bll = bll;
        }

        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            return View(await _bll.Followers.AllAsync());
        }

        /// <summary>
        /// Get record details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Details(Guid id)
        {
            var follower = await _bll.Followers.FindAsync(id);

            if (follower == null)
            {
                return NotFound();
            }

            return View(follower);
        }

        /// <summary>
        /// Get record creating page
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Get record editing page
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Get delete confirmation page
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(Guid id)
        {

            var follower = await _bll.Followers.FindAsync(id);

            if (follower == null)
            {
                return NotFound();
            }

            return View(follower);
        }

        /// <summary>
        /// Deletes a record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            _bll.Followers.Remove(id);
            await _bll.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
