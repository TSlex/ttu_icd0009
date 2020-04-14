using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public ProfilesController(ApplicationDbContext context, UserManager<Profile> userManager)
        {
            _context = context;
            _userManager = userManager;
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
            
//            return View(await _context.Profiles.ToListAsync());
            return View(user);
        }
    }
}
