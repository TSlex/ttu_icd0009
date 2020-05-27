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
using Profile = BLL.App.DTO.Profile;

namespace WebApp.Controllers
{
    /// <summary>
    /// Profiles
    /// </summary>
    [Route("/{username}/{action=Index}")]
    public class ProfilesController : Controller
    {
        private readonly UserManager<Domain.Profile> _userManager;
        private readonly SignInManager<Domain.Profile> _signInManager;
        private readonly IAppBLL _bll;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="bll"></param>
        /// <param name="signInManager"></param>
        public ProfilesController(UserManager<Domain.Profile> userManager, IAppBLL bll, SignInManager<Domain.Profile> signInManager)
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

            if (user == null || user.DeletedAt != null)
            {
                return NotFound();
            }

            Profile record;

            if (isAuthorized)
            {
                record = await _bll.Profiles.GetProfileAsync(user.Id, User.UserId());
            }
            else
            {
                record = await _bll.Profiles.GetProfileAsync(user.Id, null);
            }

            if (record.IsUserBlocked)
            {
                var profileLimited = new ProfileLimited()
                {
                    UserName = record.UserName,
                    Experience = record.Experience,
                    Rank = record.ProfileRanks.Take(1).FirstOrDefault(),
                    FollowedCount = record.FollowedCount,
                    FollowersCount = record.FollowersCount,
                    PostsCount = record.PostsCount,
                    ProfileAvatarId = record.ProfileAvatarId
                };
                
                return View("IndexLimited", profileLimited);
            }

            return View(record);
        }

        /// <summary>
        /// Subscribe to profile
        /// </summary>
        /// <param name="profileModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> FollowProfile(Profile profileModel)
        {
            var userId = User.UserId();
            var profile = await _userManager.FindByNameAsync(profileModel.UserName);

            if (profile == null || profile.Id == userId)
            {
                return RedirectToAction(nameof(Index), new {profileModel.UserName});
            }

            var subscription = await _bll.Followers.FindAsync(userId, profile.Id);
            var property = await _bll.BlockedProfiles.FindAsync(userId, profile.Id);

            if (property == null && subscription == null)
            {
                _bll.Followers.AddSubscription(userId, profile.Id);
                await _bll.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index), new {profileModel.UserName});
        }

        /// <summary>
        /// Unsubscribe from profile
        /// </summary>
        /// <param name="profileModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UnfollowProfile(Profile profileModel)
        {
            var userId = User.UserId();
            var profile = await _userManager.FindByNameAsync(profileModel.UserName);

            if (profile == null || profile.Id == userId)
            {
                return RedirectToAction(nameof(Index), new {profileModel.UserName});
            }

            var subscription = await _bll.Followers.FindAsync(userId, profile.Id);

            if (subscription != null)
            {
                _bll.Followers.Remove(subscription);
                await _bll.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index), new {profileModel.UserName});
        }
        
        /// <summary>
        /// Add profile to black list
        /// </summary>
        /// <param name="profileModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> BlockProfile(Profile profileModel)
        {
            var userId = User.UserId();
            var profile = await _userManager.FindByNameAsync(profileModel.UserName);

            if (profile == null || profile.Id == userId)
            {
                return RedirectToAction(nameof(Index), new {profileModel.UserName});
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

            return RedirectToAction(nameof(Index), new {profileModel.UserName});
        }
        
        /// <summary>
        /// Remove profile from black list
        /// </summary>
        /// <param name="profileModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UnblockProfile(Profile profileModel)
        {
            var userId = User.UserId();
            var profile = await _userManager.FindByNameAsync(profileModel.UserName);

            if (profile == null || profile.Id == userId)
            {
                return RedirectToAction(nameof(Index), new {profileModel.UserName});
            }

            var property = await _bll.BlockedProfiles.FindAsync(userId, profile.Id);

            if (property != null)
            {
                _bll.BlockedProfiles.Remove(property);
                await _bll.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index), new {profileModel.UserName});
        }
    }
}