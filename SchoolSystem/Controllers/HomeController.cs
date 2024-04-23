using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using SchoolSystem.Data;
using SchoolSystem.Models;
using SchoolSystem.ViewModels;
using System.Diagnostics;

namespace SchoolSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult StudentsProgramming1()
        {
            var studentsProgramming1 = (from course in _context.Course
                                        join enrollment in _context.Enrollment on course.CourseId equals enrollment.FkCourseId
                                        join teacher in _context.Teacher on enrollment.FkTeacherId equals teacher.TeacherId
                                        join student in _context.Student on enrollment.FkStudentId equals student.StudentId
                                        where course.CourseId == 1
                                        select new
                                        {
                                            CourseId = course.CourseId,
                                            CourseName = course.CourseName,
                                            TeacherId = teacher.TeacherId,
                                            TeacherName = teacher.TeacherName,
                                            StudentId = student.StudentId,
                                            StudentName = student.StudentName
                                        }).ToList();

            ViewBag.StudentsProgramming1List = studentsProgramming1;

            return View(studentsProgramming1);
        }

        public IActionResult TeachersProgramming1()
        {
            var teachersProgramming1 = _context.Enrollment
                .Where(x => x.FkCourseId == 1)
                .Include(x => x.Teacher)
                .Include(x => x.Course)
                .Select(e => new TeacherCourseVM_
                {
                    CourseName = e.Course.CourseName,
                    TeacherName = e.Teacher.TeacherName
                })
                .ToList();

            return View(teachersProgramming1);
        }

        public IActionResult StudentsWithTeachers()
        {
            var studentsWithTeachers = _context.Enrollment
                .Include(x => x.Teacher)
                .Include(x => x.Student)
                .Select(e => new TeacherStudentVM
                {
                    StudentName = e.Student.StudentName,
                    TeacherName = e.Teacher.TeacherName
                })
                .ToList();

            return View(studentsWithTeachers);
        }

        [HttpGet]
        public async Task <IActionResult> EditCourseName()
        {
            var courses = await _context.Course.ToListAsync();
            ViewBag.CourseList = new SelectList(courses, "CourseId", "CourseName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditCourseName(int CourseId, string CourseName)
        {
            var course = await _context.Course.FindAsync(CourseId);

            if (course == null)
            {
                return NotFound("Error 404: Course not found");
            }

            course.CourseName = CourseName;

            _context.Course.Update(course);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index"); 
        }

    }
}
