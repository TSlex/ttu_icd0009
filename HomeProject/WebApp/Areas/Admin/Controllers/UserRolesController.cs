using System;
using System.Threading.Tasks;
using Contracts.BLL.App;
using Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Admin.Controllers
{
    /// <summary>
    /// User roles controller
    /// </summary>
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    [Route("{area}/{controller}/{action=Index}")]
    public class UserRolesController : Controller
    {
        private readonly RoleManager<MRole> _roleManager;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bll"></param>
        public UserRolesController(RoleManager<MRole> roleManager)
        {
            _roleManager = roleManager;
        }
        
        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            return View(await _roleManager.Roles.ToListAsync());
        }
    }
}
