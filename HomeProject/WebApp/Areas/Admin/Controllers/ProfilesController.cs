using System;
using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.BLL.App;
using Domain;
using Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;

namespace WebApp.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    [Route("{area}/{id}/{action=Index}")]
    public class ProfilesController : Controller
    {
        private readonly UserManager<Profile> _userManager;
        private readonly SignInManager<Profile> _signInManager;
        private readonly IAppBLL _bll;

        public ProfilesController(UserManager<Profile> userManager, IAppBLL bll, SignInManager<Profile> signInManager)
        {
            _userManager = userManager;
            _bll = bll;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _bll.Profiles.AllAsync());
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            
            var isAuthorized = _signInManager.IsSignedIn(User);

            if (user == null)
            {
                return NotFound();
            }

            if (!(await _bll.ProfileRanks.AllUserAsync(user.Id)).Any())
            {
                _bll.ProfileRanks.Add(new BLL.App.DTO.ProfileRank()
                {
                    ProfileId = user.Id,
                    RankId = (await _bll.Ranks.FindByCodeAsync("X_00")).Id
                });
                
                await _bll.SaveChangesAsync();
            };

            var profileModel = await _bll.Profiles.GetProfileFull(user.Id);

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
        
        
        public async Task<IActionResult> Edit(Guid id)
        {
            var profile = await _bll.Profiles.FindAsync(id);

            if (profile == null)
            {
                return NotFound();
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
                ProfileAvatarUrl = profile.ProfileAvatarUrl,
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
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProfileEdit profile)
        {
            if (id != profile.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                var record = await _userManager.FindByIdAsync(id.ToString());

                if (record == null)
                {
                    ModelState.AddModelError(string.Empty, "Profile does not exist");
                    return View(profile);
                }

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
                record.ProfileAvatarUrl = profile.ProfileAvatarUrl;
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
        
        public Task<IActionResult> Delete(Guid id)
        {
            throw new NotImplementedException();
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> DeleteConfirmed(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}