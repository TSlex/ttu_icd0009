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
        public async Task<IActionResult> Followers()
        {
            return View(await _bll.Followers.AllByIdPageAsync(User.UserId(), false, 1, int.MaxValue));
        }
        
        /// <summary>
        /// Get user followed
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Followed()
        {
            return View(await _bll.Followers.AllByIdPageAsync(User.UserId(), true, 1, int.MaxValue));
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
                return NotFound();
            }
            
            _bll.Followers.Remove(id);
            await _bll.SaveChangesAsync();

            return RedirectToAction(nameof(Followed));
        }
    }
}
