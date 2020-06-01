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
        /// <summary>
        /// Get admin panel
        /// </summary>
        /// <returns></returns>
        [Route("admin/panel")]
        public IActionResult Index()
        {
            return View();
        }
    }
}