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
    public class StudentHomeWorksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentHomeWorksController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var query = _context.HomeWorks
                .Include(hw => hw.Subject)
                .ThenInclude(hw => hw.StudentSubjects)
                .ThenInclude(sb => sb.Student)
                .Include(hw => hw.Subject)
                .ThenInclude(hw => hw.StudentSubjects)
                .ThenInclude(hw => hw.StudentHomeWorks)
                .Where(hw => hw.Subject.StudentSubjects.Select(ssb => ssb.StudentId).Contains(User.UserId()));

            return View(await query.ToListAsync());
        }

        // GET: StudentHomeWorks/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var studentHomeWork = await _context.StudentHomeWorks
                .Include(s => s.HomeWork)
                .Include(s => s.StudentSubject)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studentHomeWork == null)
            {
                return NotFound();
            }

            return View(studentHomeWork);
        }

        // GET: StudentHomeWorks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StudentHomeWorks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentHomeWork studentHomeWork)
        {
            if (ModelState.IsValid)
            {
                studentHomeWork.Id = Guid.NewGuid();
                _context.Add(studentHomeWork);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }


            return View(studentHomeWork);
        }

        // GET: StudentHomeWorks/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentHomeWork = await _context.StudentHomeWorks.FindAsync(id);
            if (studentHomeWork == null)
            {
                return NotFound();
            }

            return View(studentHomeWork);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, StudentHomeWork studentHomeWork)
        {
            if (id != studentHomeWork.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(studentHomeWork);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(studentHomeWork);
        }
    }
}