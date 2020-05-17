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
    [Route("{area}/{id}/{action=Index}")]
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
            return View(await _bll.Profiles.AllAsync());
        }
        
        /// <summary>
        /// Get record details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Details(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            
            var isAuthorized = _signInManager.IsSignedIn(User);

            if (user == null)
            {
                return NotFound();
            }

            // ReSharper disable EF1001
            if (!(await _bll.ProfileRanks.AllUserAsync(user.Id)).Any())
            {
                _bll.ProfileRanks.Add(new BLL.App.DTO.ProfileRank()
                {
                    ProfileId = user.Id,
                    RankId = (await _bll.Ranks.FindByCodeAsync("X_00")).Id
                });
                
                await _bll.SaveChangesAsync();
            };

            var profileModel = await _bll.Profiles.GetProfileAsync(user.Id, User.UserId());

            if (profileModel == null)
            {
                return NotFound();
            }

            if (isAuthorized)
            {
                profileModel.IsUserFollows = await _bll.Followers.FindAsync(User.UserId(), user.Id) != null;
                profileModel.IsUserBlocks = await _bll.BlockedProfiles.FindAsync(User.UserId(), user.Id) != null;   
            }

            return View(profileModel);
        }
        
        /// <summary>
        /// Get record editing page
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Edit(Guid id)
        {
            var profile = await _bll.Profiles.FindAsync(id);

            if (profile == null)
            {
                return NotFound();
            }

            BLL.App.DTO.Image? avatar = null;

            if (profile.ProfileAvatarId != null)
            {
                avatar = await _bll.Images.FindAsync((Guid) profile.ProfileAvatarId);
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
//                profile.ProfileAvatar = null;

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
        /// Get delete confirmation page
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<IActionResult> Delete(Guid id)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Deletes a record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> DeleteConfirmed(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}