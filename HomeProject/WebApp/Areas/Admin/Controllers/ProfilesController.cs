using System.Threading.Tasks;
using BLL.App.DTO;
using Contracts.BLL.App;
using Domain;
using Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    [Route("{area}/{username}/{action=Index}")]
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

            if (subscription == null)
            {
                _bll.Followers.AddSubscription(userId, profile.Id);
                await _bll.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index), new {profileFullModel.UserName});
        }

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
//                await _bll.Followers.RemoveSubscriptionAsync(userId, profile.Id);
                _bll.Followers.Remove(subscription);
                await _bll.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index), new {profileFullModel.UserName});
        }

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
            var subscription = await _bll.Followers.FindAsync(profile.Id,userId);

            if (property == null)
            {
                if (subscription != null)
                {
//                    await _bll.Followers.RemoveSubscriptionAsync(profile.Id, userId);
                    _bll.Followers.Remove(subscription);
                }
//                _bll.BlockedProfiles.AddBlockProperty(userId, profile.Id);
                _bll.BlockedProfiles.AddBlockProperty(userId, profile.Id);
                await _bll.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index), new {profileFullModel.UserName});
        }

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
//                await _bll.BlockedProfiles.RemoveBlockPropertyAsync(userId, profile.Id);
                _bll.BlockedProfiles.Remove(property);
                await _bll.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index), new {profileFullModel.UserName});
        }
    }
}