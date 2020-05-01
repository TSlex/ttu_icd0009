using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Contracts.BLL.App;
using Domain;
using Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApp.Areas.Admin.Models;

namespace WebApp.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<Profile> _userManager;
        private readonly SignInManager<Profile> _signInManager;
        private readonly IAppBLL _bll;

        public HomeController(ILogger<HomeController> logger, UserManager<Profile> userManager, IAppBLL bll,
            SignInManager<Profile> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _bll = bll;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

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


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}