using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.App.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.App.EF;
using Microsoft.AspNetCore.Authorization;
using HomeWork = Domain.HomeWork;
using StudentHomeWork = Domain.StudentHomeWork;

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
                .Where(work => work.DeletedAt == null && work.Id == id)
                .Select(work => new HomeWorkDetailsDTO
                {
                    Id = work.Id,
                    SubjectId = work.SubjectId,
                    SubjectCode = work.Subject.SubjectCode,
                    SubjectTitle = work.Subject.SubjectTitle,
                    Deadline = work.Deadline,
                    Title = work.Title,
                    Description = work.Description,
                    StudentHomeWorks = work.Subject.StudentSubjects
                        .Where(ssb => ssb.DeletedAt == null && ssb.IsAccepted)
                        .Select(ssb => new StudentHomeWorkDTO()
                        {
                            StudentSubjectId = ssb.Id,
                            SubjectId = ssb.SubjectId,
                            HomeWorkId = work.Id,
                            IsAccepted = ssb.StudentHomeWorks.FirstOrDefault(w => w.HomeWorkId == work.Id).IsAccepted,
                            IsChecked = ssb.StudentHomeWorks.FirstOrDefault(w => w.HomeWorkId == work.Id).IsChecked,
                            StudentCode = ssb.Student.UserName,
                            StudentName = ssb.Student.FirstName + " " + ssb.Student.LastName,
                            Grade = ssb.StudentHomeWorks.FirstOrDefault(w => w.HomeWorkId == work.Id).Grade
                        }).ToList()
                }).FirstOrDefaultAsync();

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
            
            //remove related student homeworks
            _context.RemoveRange(await _context.StudentHomeWorks.Where(shw => shw.HomeWorkId == homeWork.Id)
                .ToListAsync());

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Subjects", new {id = homeWork.SubjectId});
        }
    }
}