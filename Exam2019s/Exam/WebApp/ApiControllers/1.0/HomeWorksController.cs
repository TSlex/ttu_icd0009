using System;
using System.Threading.Tasks;
using DAL.App.EF;
using Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers._1._0
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Student, Teacher, Admin")]
    public class HomeWorksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeWorksController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        [Consumes(("application/json"))]
        [Produces("application/json")]
        public async Task<IActionResult> Details(Guid id)
        {
            var homeWork = await _context.HomeWorks
                .Include(hw => hw.Subject)
                .ThenInclude(hw => hw.StudentSubjects)
                .ThenInclude(sb => sb.Student)
                .Include(hw => hw.Subject)
                .ThenInclude(hw => hw.StudentSubjects)
                .ThenInclude(hw => hw.StudentHomeWorks)
                .FirstOrDefaultAsync(hw => hw.Id == id);
            
            if (homeWork == null)
            {
                return NotFound();
            }

            return View(homeWork);
        }

        [HttpGet("{subjectId}")]
        [Consumes(("application/json"))]
        [Produces("application/json")]
        public async Task<IActionResult> Create(Guid subjectId)
        {
            var subject = await _context.Subjects.FirstOrDefaultAsync(s => s.Id == subjectId);
            
            return View(new HomeWork
            {
                Subject = subject
            });
        }

        [HttpPost]
        [Consumes(("application/json"))]
        [Produces("application/json")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HomeWork homeWork)
        {
            if (ModelState.IsValid)
            {
                homeWork.Id = Guid.NewGuid();

                _context.Add(homeWork);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Subjects", new {id = homeWork.SubjectId});
            }

            return View(homeWork);
        }

        [Consumes(("application/json"))]
        [Produces("application/json")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homeWork = await _context.HomeWorks
                .Include(h => h.Subject)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (homeWork == null)
            {
                return NotFound();
            }

            return View(homeWork);
        }


        [HttpPost]
        [Consumes(("application/json"))]
        [Produces("application/json")]
        public async Task<IActionResult> Edit(Guid id, HomeWork homeWork)
        {
            if (id != homeWork.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(homeWork);
                await _context.SaveChangesAsync();

                return RedirectToAction("Details", "Subjects", new {id = homeWork.SubjectId});
            }

            return View(homeWork);
        }

        [HttpPost]
        [Consumes(("application/json"))]
        [Produces("application/json")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var homeWork = await _context.HomeWorks.FindAsync(id);
            
            _context.HomeWorks.Remove(homeWork);
            
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Subjects", new {id = homeWork.SubjectId});
        }
    }
}