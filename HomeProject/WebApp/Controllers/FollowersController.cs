using System;
using System.Threading.Tasks;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Mvc;
using Extension;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Controllers
{
    /// <summary>
    /// Followers and followed
    /// </summary>
    [Authorize]
    public class FollowersController : Controller
    {
        private readonly IAppBLL _bll;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bll"></param>
        public FollowersController(IAppBLL bll)
        {
            _bll = bll;
        }

        /// <summary>
        /// Get user followers
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Followers(string username)
        {
            var user = await _bll.Profiles.FindByUsernameWithFollowersAsync(username);

            if (user == null)
            {
                return RedirectToAction("PageNotFound", "Home", new {area = ""});
            }

            return View(user.Followers);
        }

        /// <summary>
        /// Get user followed
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Followed(string username)
        {
            var user = await _bll.Profiles.FindByUsernameWithFollowersAsync(username);

            if (user == null)
            {
                return RedirectToAction("PageNotFound", "Home", new {area = ""});
            }

            return View(user.Followed);
        }

        /// <summary>
        /// Deletes a record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var record = await _bll.Followers.GetForUpdateAsync(id);

            if (record == null || record.FollowerProfileId != User.UserId())
            {
                return RedirectToAction("PageNotFound", "Home", new {area = ""});
            }

            _bll.Followers.Remove(id);
            await _bll.SaveChangesAsync();

            return RedirectToAction(nameof(Followed), new {username = User.Identity.Name});
        }
    }
}