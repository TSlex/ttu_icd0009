using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.BLL.App;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;
using Microsoft.AspNetCore.Identity;

namespace WebApp.Controllers
{
    [Route("/{username}")]
    public class ProfilesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Profile> _userManager;
        private readonly IAppBLL _bll;

        public ProfilesController(ApplicationDbContext context, UserManager<Profile> userManager, IAppBLL bll)
        {
            _context = context;
            _userManager = userManager;
            _bll = bll;
        }
        
        public async Task<IActionResult> Index(string? username)
        {
            if (username == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByNameAsync(username);
            
            if (user == null)
            {
                return NotFound();
            }

            var userModel = await _bll.Profiles.GetProfileFull(user.Id);
            
            if (userModel == null)
            {
                return NotFound();
            }
            
            return View(userModel);
        }
    }
}
