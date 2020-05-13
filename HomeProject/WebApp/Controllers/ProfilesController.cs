using System;
using System.Linq;
using System.Threading.Tasks;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Mvc;
using BLL.App.DTO;
using Domain;
using Extension;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;

namespace WebApp.Controllers
{
    /// <summary>
    /// Profiles
    /// </summary>
    [Route("/{username}/{action=Index}")]
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
        public ProfilesController(UserManager<Profile> userManager, IAppBLL bll, SignInManager<Profile> signInManager)
        {
            _userManager = userManager;
            _bll = bll;
            _signInManager = signInManager;
        }

        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            var isAuthorized = _signInManager.IsSignedIn(User);

            if (user == null)
            {
                return NotFound();
            }

            var isUserBlocked = isAuthorized &&
                                user.Id != User.UserId() &&
                                await _bll.BlockedProfiles.FindAsync(user.Id, User.UserId()) != null;
            
            // ReSharper disable EF1001
            if (!(await _bll.ProfileRanks.AllUserAsync(user.Id)).Any())
            {
                _bll.ProfileRanks.Add(new BLL.App.DTO.ProfileRank()
                {
                    ProfileId = user.Id,
                    RankId = (await _bll.Ranks.FindByCodeAsync("X_00")).Id
                });

                await _bll.SaveChangesAsync();
            }

            ;

            if (isUserBlocked)
            {
                var profileLimited = await _bll.Profiles.GetProfileLimited(user.Id);

                return View("IndexLimited", profileLimited);
            }

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

        /// <summary>
        /// Subscribe to profile
        /// </summary>
        /// <param name="profileFullModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> FollowProfile(ProfileFull profileFullModel)
        {
            var userId = User.UserId();
            var profile = await _userManager.FindByNameAsync(profileFullModel.UserName);

            if (profile == null || profile.Id == userId)
            {
                return RedirectToAction(nameof(Index), new {profileFullModel.UserName});
            }

            var subscription = await _bll.Followers.FindAsync(userId, profile.Id);
            var property = await _bll.BlockedProfiles.FindAsync(userId, profile.Id);

            if (property == null && subscription == null)
            {
                _bll.Followers.AddSubscription(userId, profile.Id);
                await _bll.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index), new {profileFullModel.UserName});
        }

        /// <summary>
        /// Unsubscribe from profile
        /// </summary>
        /// <param name="profileFullModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UnfollowProfile(ProfileFull profileFullModel)
        {
            var userId = User.UserId();
            var profile = await _userManager.FindByNameAsync(profileFullModel.UserName);

            if (profile == null || profile.Id == userId)
            {
                return RedirectToAction(nameof(Index), new {profileFullModel.UserName});
            }

            var subscription = await _bll.Followers.FindAsync(userId, profile.Id);

            if (subscription != null)
            {
                _bll.Followers.Remove(subscription);
                await _bll.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index), new {profileFullModel.UserName});
        }
        
        /// <summary>
        /// Add profile to black list
        /// </summary>
        /// <param name="profileFullModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> BlockProfile(ProfileFull profileFullModel)
        {
            var userId = User.UserId();
            var profile = await _userManager.FindByNameAsync(profileFullModel.UserName);

            if (profile == null || profile.Id == userId)
            {
                return RedirectToAction(nameof(Index), new {profileFullModel.UserName});
            }

            var property = await _bll.BlockedProfiles.FindAsync(userId, profile.Id);
            var subscription = await _bll.Followers.FindAsync(profile.Id, userId);

            if (property == null)
            {
                if (subscription != null)
                {
                    _bll.Followers.Remove(subscription);
                }

                _bll.BlockedProfiles.AddBlockProperty(userId, profile.Id);
                await _bll.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index), new {profileFullModel.UserName});
        }
        
        /// <summary>
        /// Remove profile from black list
        /// </summary>
        /// <param name="profileFullModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UnblockProfile(ProfileFull profileFullModel)
        {
            var userId = User.UserId();
            var profile = await _userManager.FindByNameAsync(profileFullModel.UserName);

            if (profile == null || profile.Id == userId)
            {
                return RedirectToAction(nameof(Index), new {profileFullModel.UserName});
            }

            var property = await _bll.BlockedProfiles.FindAsync(userId, profile.Id);

            if (property != null)
            {
                _bll.BlockedProfiles.Remove(property);
                await _bll.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index), new {profileFullModel.UserName});
        }
    }
}