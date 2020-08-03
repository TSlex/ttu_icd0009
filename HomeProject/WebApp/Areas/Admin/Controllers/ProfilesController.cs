using System;
using System.Linq;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.BLL.App;
using Domain;
using Domain.Enums;
using Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Image = Domain.Image;
using Profile = Domain.Profile;

namespace WebApp.Areas.Admin.Controllers
{
    /// <summary>
    /// Profiles
    /// </summary>
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    [Route("{area}/{controller}/{action=Index}")]
    public class ProfilesController : Controller
    {
        private readonly UserManager<Profile> _userManager;
        private readonly SignInManager<Profile> _signInManager;
        private readonly IAppBLL _bll;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="bll"></param>
        /// <param name="signInManager"></param>
        /// <param name="hostEnvironment"></param>
        public ProfilesController(UserManager<Profile> userManager, 
            IAppBLL bll, SignInManager<Profile> signInManager, 
            IWebHostEnvironment hostEnvironment)
        {
            _userManager = userManager;
            _bll = bll;
            _bll.Images.RootPath = hostEnvironment.WebRootPath;
            _signInManager = signInManager;
        }
        
        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            return View(await _bll.Profiles.AllAdminAsync());
        }
        
        /// <summary>
        /// Get record details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Details(Guid id)
        {
            var user = await _bll.Profiles.FindAdminAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        
        /// <summary>
        /// Get record editing page
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Edit(Guid id)
        {
            var profile = await _bll.Profiles.GetAdminEditModel(id);

            if (profile == null)
            {
                return NotFound();
            }

            return View(profile);
        }
        
        /// <summary>
        /// Updates a record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="profile"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProfileEdit profile)
        {
            if (id != profile.Id)
            {
                ModelState.AddModelError(string.Empty, Resourses.BLL.App.DTO.Common.ErrorIdMatch);
                return View(profile);
            }
            
            if (profile.ProfileAvatar!.ImageFile == null && profile.ProfileAvatarId == null)
            {
                ModelState.AddModelError(string.Empty, Resourses.BLL.App.DTO.Images.Images.ImageRequired);
                return View(profile);
            }
            
            ModelState.Clear();

            var imageModel = profile.ProfileAvatar;

            if (profile.ProfileAvatarId == null)
            {
                imageModel.Id = Guid.NewGuid();
                imageModel.ImageType = ImageType.ProfileAvatar;
                imageModel.ImageFor = profile.Id;
            }

            if (TryValidateModel(profile))
            {
                var record = await _userManager.FindByIdAsync(id.ToString());

                if (record == null)
                {
                    ModelState.AddModelError(string.Empty, Resourses.BLL.App.DTO.Common.ErrorUserNotFound);
                    return View(profile);
                }
                
                if (profile.ProfileAvatarId == null)
                {
                    await _bll.Images.AddProfileAsync(profile.Id, imageModel);
                }
                else
                {
                    await _bll.Images.UpdateProfileAsync(profile.Id, imageModel);
                }
                
                profile.ProfileAvatarId = imageModel.Id;

                var (result, errors) = await _bll.Profiles.UpdateProfileAdminAsync(profile);
                
                if (errors.Length > 0)
                {
                    foreach (var error in errors)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }

                    return View(result);
                }

                return RedirectToAction(nameof(Index));
            }

            return View(profile);
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
            var record = await _bll.Profiles.GetForUpdateAsync(id);
            _bll.Profiles.Remove(record);
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
            var record = await _bll.Profiles.GetForUpdateAsync(id);
            _bll.Profiles.Restore(record);
            await _bll.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}