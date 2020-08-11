using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL.App.EF;
using Domain;

namespace WebApp.Controllers
{
    public class SemestersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SemestersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Semesters
        public async Task<IActionResult> Index()
        {
            return View(await _context.Semesters
                .Include(semester => semester.Subjects)
                .ThenInclude(subject => subject.Teacher)
                .Include(semester => semester.Subjects)
                .ThenInclude(subject => subject.StudentSubjects)
                .ToListAsync());
        }
    }
}