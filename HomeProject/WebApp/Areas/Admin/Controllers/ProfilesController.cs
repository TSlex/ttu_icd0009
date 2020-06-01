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
            var profile = await _bll.Profiles.FindAdminAsync(id);

            if (profile == null)
            {
                return NotFound();
            }

            BLL.App.DTO.Image? avatar = null;

            if (profile.ProfileAvatarId != null)
            {
                avatar = await _bll.Images.FindAdminAsync((Guid) profile.ProfileAvatarId);
            }
            
            return View(new ProfileEdit()
            {
                Email = profile.Email,
                Id = profile.Id,
                UserName = profile.UserName,
                ProfileFullName = profile.ProfileFullName,
                ProfileWorkPlace = profile.ProfileWorkPlace,
                Experience = profile.Experience,
                ProfileAbout = profile.ProfileAbout,
                ProfileAvatarId = profile.ProfileAvatarId,
                ProfileAvatar = avatar,
                ProfileGender = profile.ProfileGender,
                ProfileGenderOwn = profile.ProfileGenderOwn,
                ProfileStatus = profile.ProfileStatus,
                PhoneNumber = profile.PhoneNumber,
                PhoneNumberConfirmed = profile.PhoneNumberConfirmed,
                LockoutEnabled = profile.LockoutEnabled,
                EmailConfirmed = profile.EmailConfirmed,
                AccessFailedCount = profile.AccessFailedCount
            });
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
                return NotFound();
            }
            
            if (profile.ProfileAvatar!.ImageFile == null && profile.ProfileAvatarId == null)
            {
                ModelState.AddModelError(string.Empty, "Image should be specified");
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
                    ModelState.AddModelError(string.Empty, "Profile does not exist");
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

                if (record.UserName != profile.UserName)
                {
                    var result = await _userManager.SetUserNameAsync(record, profile.UserName);
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError(string.Empty, "Cannot change username");
                        return View(profile);
                    }
                }
                if (record.Email != profile.Email)
                {
                    var result = await _userManager.SetEmailAsync(record, profile.Email);
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError(string.Empty, "Cannot change email");
                        return View(profile);
                    }
                }
                if (profile.Password != null)
                {
                    var resetToken = await _userManager.GeneratePasswordResetTokenAsync(record);
                    var changePasswordResult = await _userManager.ResetPasswordAsync(record, resetToken,profile.Password);
                    if (!changePasswordResult.Succeeded)
                    {
                        foreach (var error in changePasswordResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return View(profile);
                    }
                }
                
                record.ProfileFullName = profile.ProfileFullName;
                record.ProfileWorkPlace = profile.ProfileWorkPlace;
                record.Experience = profile.Experience;
                record.ProfileAbout = profile.ProfileAbout;
                record.ProfileGender = profile.ProfileGender;
                record.ProfileGenderOwn = profile.ProfileGenderOwn;
                record.ProfileStatus = profile.ProfileStatus;
                record.PhoneNumber = profile.PhoneNumber;
                record.PhoneNumberConfirmed = profile.PhoneNumberConfirmed;
                record.LockoutEnabled = profile.LockoutEnabled;
                record.EmailConfirmed = profile.EmailConfirmed;
                record.AccessFailedCount = profile.AccessFailedCount;

                await _userManager.UpdateAsync(record);

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