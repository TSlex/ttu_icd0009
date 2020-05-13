using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Contracts.BLL.App;
using Domain;
using Extension;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApp.Models;

namespace WebApp.Controllers
{
    /// <summary>
    /// Home
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<Profile> _userManager;
        private readonly SignInManager<Profile> _signInManager;
        private readonly IAppBLL _bll;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="userManager"></param>
        /// <param name="bll"></param>
        /// <param name="signInManager"></param>
        public HomeController(ILogger<HomeController> logger, UserManager<Profile> userManager, IAppBLL bll,
            SignInManager<Profile> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _bll = bll;
            _signInManager = signInManager;
        }
        
        
        /// <summary>
        /// Get posts for specific user subscriptions (including own posts) or all posts of all users
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            if (_signInManager.IsSignedIn(User))
            {
                var result = await _bll.Feeds.GetUserFeedAsync(User.UserId());
                return View(result);
            }

            return View(await _bll.Feeds.GetCommonFeedAsync());
        }

        /// <summary>
        /// Get privacy page
        /// </summary>
        /// <returns></returns>
        public IActionResult Privacy()
        {
            return View();
        }
        
        /// <summary>
        /// Search user by username
        /// </summary>
        /// <param name="username"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SearchUser(string? username, string? returnUrl)
        {
            if (username != null)
            {
                var user = await _userManager.FindByNameAsync(username);

                if (user != null)
                {
                    return RedirectToAction("Index", "Profiles", new
                    {
                        username = username
                    });
                }
            }

            if (returnUrl != null)
            {
                return Redirect(returnUrl);
            }

            return View("Index");
        }

        /// <summary>
        /// Set current culture to user selected one
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions()
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1)
                }
            );
            return LocalRedirect(returnUrl);
        }


        /// <summary>
        /// Some error occured
        /// </summary>
        /// <returns></returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}