using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.App.EF;
using Domain;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Controllers
{
    [Authorize(Roles = "Student, Teacher, Admin")]
    public class HomeWorksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeWorksController(ApplicationDbContext context)
        {
            _context = context;
        }

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

        public async Task<IActionResult> Create(Guid subjectId)
        {
            var subject = await _context.Subjects.FirstOrDefaultAsync(s => s.Id == subjectId);
            
            return View(new HomeWork
            {
                Subject = subject
            });
        }

        [HttpPost]
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
        [ValidateAntiForgeryToken]
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var homeWork = await _context.HomeWorks.FindAsync(id);
            
            _context.HomeWorks.Remove(homeWork);
            
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Subjects", new {id = homeWork.SubjectId});
        }
    }
}