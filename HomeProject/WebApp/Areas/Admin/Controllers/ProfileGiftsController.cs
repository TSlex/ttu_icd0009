using System;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.BLL.App;
using Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Areas.Admin.Controllers
{
    /// <summary>
    /// Profile gifts
    /// </summary>
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    [Route("{area}/{controller}/{action=Index}")]
    public class ProfileGiftsController : Controller
    {
        private readonly IAppBLL _bll;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bll"></param>
        public ProfileGiftsController(IAppBLL bll)
        {
            _bll = bll;
        }

        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            return View(await _bll.ProfileGifts.AllAdminAsync());
        }

        /// <summary>
        /// Get record details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Details(Guid id)
        {
            var profileGift = await _bll.ProfileGifts.FindAdminAsync(id);

            if (profileGift == null)
            {
                return NotFound();
            }

            return View(profileGift);
        }

        /// <summary>
        /// Get record creating page
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Create(string username, string? returnUrl)
        {
            var gifts = await _bll.Gifts.AllAsync();
            return View(new ProfileGiftCreate()
            {
                Profile = new Profile {UserName = username},
                GiftGallery = gifts,
                ReturnUrl = returnUrl
            });
        }

        /// <summary>
        /// Get record creating confirmation page
        /// </summary>
        /// <param name="username"></param>
        /// <param name="giftId"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public async Task<IActionResult> CreateConfirm(string username, Guid giftId, string? returnUrl)
        {
            var gift = await _bll.Gifts.FindAdminAsync(giftId);

            await _bll.Ranks.IncreaseUserExperience(User.UserId(), 10);

            return View(new ProfileGift
            {
                Profile = new Profile {UserName = username},
                Gift = gift,
                GiftId = giftId,
                ReturnUrl = returnUrl
            });
        }

        /// <summary>
        /// Creates a new record
        /// </summary>
        /// <param name="profileGift"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateConfirm(ProfileGift profileGift)
        {
            var user = await _bll.Profiles.FindByUsernameAsync(profileGift.Profile!.UserName);

            if (user == null)
            {
                if (profileGift.ReturnUrl != null)
                {
                    return Redirect(profileGift.ReturnUrl);
                }

                return RedirectToAction("Index");
            }

            ModelState.Clear();
            profileGift.ProfileId = user.Id;

            if (TryValidateModel(profileGift))
            {
                profileGift.Id = Guid.NewGuid();
                profileGift.Profile = null;

                _bll.ProfileGifts.Add(profileGift);
                await _bll.SaveChangesAsync();

                if (profileGift.ReturnUrl != null)
                {
                    return Redirect(profileGift.ReturnUrl);
                }

                return RedirectToAction("Index");
            }

            return View(profileGift);
        }

        /// <summary>
        /// Get record editing page
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Edit(Guid id)
        {
            var profileGift = await _bll.ProfileGifts.FindAdminAsync(id);

            if (profileGift == null)
            {
                return NotFound();
            }


            return View(profileGift);
        }

        /// <summary>
        /// Updates a record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="profileGift"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProfileGift profileGift)
        {
            if (id != profileGift.Id)
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

        /// <summary>
        /// Deletes a record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            _bll.ProfileGifts.Remove(id);
            await _bll.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        
        /// <summary>
        /// Restores a record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(Guid id)
        {
            var record = await _bll.ProfileGifts.GetForUpdateAsync(id);
            _bll.ProfileGifts.Restore(record);
            await _bll.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}