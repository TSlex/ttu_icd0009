using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.App.EF;
using Domain;
using Extensions;

namespace WebApp.Controllers
{
    public class StudentSubjectsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentSubjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterToSubject(Subject model)
        {
            var studentSubject =
                await _context.StudentSubjects
                    .FirstOrDefaultAsync(s =>
                        s.StudentId == User.UserId() &&
                        s.SubjectId == model.Id);

            if (studentSubject == null)
            {
                _context.StudentSubjects.Add(new StudentSubject
                {
                    StudentId = User.UserId(),
                    SubjectId = model.Id,
                });

                await _context.SaveChangesAsync();
            }
            else if (studentSubject.DeletedAt != null)
            {
                studentSubject.DeletedAt = null;
                _context.Update(studentSubject);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", "Subjects", new {id = model.Id});
        }

        [HttpPost]
        public async Task<IActionResult> CancelRegistration(Subject model)
        {
            var subject = await _context.StudentSubjects
                .FirstOrDefaultAsync(s =>
                    s.DeletedAt == null &&
                    s.StudentId == User.UserId() &&
                    s.SubjectId == model.Id);

            if (subject == null)
            {
                return RedirectToAction("Subjects", "Subjects");
            }

            _context.StudentSubjects.Remove(subject);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Subjects", new {id = model.Id});
        }
    }
}