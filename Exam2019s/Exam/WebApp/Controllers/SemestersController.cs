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
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Controllers
{
    [Authorize(Roles = "Student, Teacher, Admin")]
    public class SemestersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SemestersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Semesters
                .Include(semester => semester.Subjects)
                .ThenInclude(subject => subject.Teacher)
                .Include(semester => semester.Subjects)
                .ThenInclude(subject => subject.StudentSubjects)
                .Where(semester => 
                    semester.Subjects
                        .SelectMany(subject => subject.StudentSubjects)
                        .Select(ssb => ssb.StudentId).Contains(User.UserId())
                    )
                .ToListAsync());
        }
    }
}