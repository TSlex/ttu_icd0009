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
    /// <summary>
    /// Home
    /// </summary>
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
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
        /// Get admin panel
        /// </summary>
        /// <returns></returns>
        [Route("admin/panel")]
        public IActionResult Index()
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
    }
}