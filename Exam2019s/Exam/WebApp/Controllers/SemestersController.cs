using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.App.DTO;
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
            var semesters = await _context.Semesters
                .Where(semester => semester.DeletedAt == null)
                .Select(semester => new SemesterDTO
                {
                    Id = semester.Id,
                    Title = semester.Title,
                    Subjects = semester.Subjects
                        .Where(subject => subject.DeletedAt == null)
                        .SelectMany(subject => subject.StudentSubjects)
                        .Where(subject => subject.DeletedAt == null && subject.StudentId == User.UserId())
                        .Select(subject => new SemesterSubjectDTO()
                        {
                            Id = subject.SubjectId,
                            Grade = subject.Grade,
                            SubjectCode = subject.Subject.SubjectCode,
                            SubjectTitle = subject.Subject.SubjectTitle,
                            TeacherName = subject.Subject.Teacher.FirstName + " " + subject.Subject.Teacher.LastName,
                            IsAccepted = subject.IsAccepted
                        }).ToList()
                }).ToListAsync();

            return View(semesters);
        }
    }
}