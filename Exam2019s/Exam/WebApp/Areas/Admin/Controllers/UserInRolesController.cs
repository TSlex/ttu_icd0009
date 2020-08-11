using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.Admin.Models;

namespace WebApp.Areas.Admin.Controllers
{
    /// <summary>
    /// User roles controller
    /// </summary>
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [Route("{area}/{controller}/{action=Index}")]
    public class UserInRolesController : Controller
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="roleManager"></param>
        /// <param name="userManager"></param>
        public UserInRolesController(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var result = new Dictionary<AppRole, ICollection<AppUser>>();

            var roles = await _roleManager.Roles.ToListAsync();

            foreach (var role in roles)
            {
                result.Add(role, await _userManager.GetUsersInRoleAsync(role.Name));
            }

            return View(result);
        }

        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(UserRoleModel model)
        {
            ModelState.Clear();

            model.OldRoleId = Guid.Empty;

            if (TryValidateModel(model))
            {
                var user = await _userManager.FindByIdAsync(model.UserId.ToString());
                var role = await _roleManager.FindByIdAsync(model.NewRoleId.ToString());

                if (user == null || role == null)
                {
                    ModelState.AddModelError(string.Empty, Resources.Domain.Common.ErrorBadData);
                    return View(model);
                }

                await _userManager.AddToRoleAsync(user, role.Name);

                return RedirectToAction("Index");
            }

            return View(model);
        }

        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Edit(Guid userId, Guid roleId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var role = await _roleManager.FindByIdAsync(roleId.ToString());

            if (user == null || role == null)
            {
                return RedirectToAction("PageNotFound", "Home", new {area = ""});
            }

            if (user.UserName.ToLower().Contains("admin") && role.Name.ToLower().Contains("admin"))
            {
                return RedirectToAction("PageNotFound", "Home", new {area = ""});
            }

            if (user.UserName.ToLower().Contains("root"))
            {
                return RedirectToAction("PageNotFound", "Home", new {area = ""});
            }

            if (!(await _userManager.GetRolesAsync(user)).Contains(role.Name))
            {
                return RedirectToAction("PageNotFound", "Home", new {area = ""});
            }

            return View(new UserRoleModel {OldRoleId = role.Id, UserId = user.Id});
        }

        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Edit(Guid userId, Guid roleId, UserRoleModel model)
        {
            if (userId != model.UserId || roleId != model.OldRoleId)
            {
                ModelState.AddModelError(string.Empty, Resources.Domain.Common.ErrorIdMatch);
            }

            if (model.NewRoleId == model.OldRoleId)
            {
                return RedirectToAction("Index");
            }

            if (TryValidateModel(model))
            {
                var user = await _userManager.FindByIdAsync(model.UserId.ToString());

                var oldRole = await _roleManager.FindByIdAsync(model.OldRoleId.ToString());
                var newRole = await _roleManager.FindByIdAsync(model.NewRoleId.ToString());

                if (user == null || oldRole == null || newRole == null)
                {
                    ModelState.AddModelError(string.Empty, Resources.Domain.Common.ErrorBadData);
                    return View(model);
                }

                if (user.UserName.ToLower().Contains("admin") && oldRole.Name.ToLower().Contains("admin"))
                {
                    ModelState.AddModelError(string.Empty, Resources.Domain.Common.ErrorBadData);
                    return View(model);
                }

                if (user.UserName.ToLower().Contains("root"))
                {
                    ModelState.AddModelError(string.Empty, Resources.Domain.Common.ErrorBadData);
                    return View(model);
                }

                if ((await _userManager.GetRolesAsync(user)).Contains(oldRole.Name))
                {
                    await _userManager.RemoveFromRoleAsync(user, oldRole.Name);
                    await _userManager.AddToRoleAsync(user, newRole.Name);
                }

                return RedirectToAction("Index");
            }

            return View(model);
        }

        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Delete(Guid userId, Guid roleId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var role = await _roleManager.FindByIdAsync(roleId.ToString());

            if (user == null || role == null)
            {
                return RedirectToAction("PageNotFound", "Home", new {area = ""});
            }

            if (user.UserName.ToLower().Contains("admin") && role.Name.ToLower().Contains("admin"))
            {
                return RedirectToAction("PageNotFound", "Home", new {area = ""});
            }

            if (user.UserName.ToLower().Contains("root"))
            {
                return RedirectToAction("PageNotFound", "Home", new {area = ""});
            }

            if ((await _userManager.GetRolesAsync(user)).Contains(role.Name))
            {
                await _userManager.RemoveFromRoleAsync(user, role.Name);
            }

            return RedirectToAction("Index");
        }
    }
}