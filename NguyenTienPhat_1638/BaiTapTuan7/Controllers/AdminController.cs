using BaiTapTuan7.Data;
using BaiTapTuan7.Models;
using BaiTapTuan7.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BaiTapTuan7.Controllers;

[Authorize(Roles = RoleNames.Admin)]
[Route("admin")]
public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdminController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var topCourses = await _context.Courses
            .AsNoTracking()
            .Select(c => new TopCourseItem
            {
                CourseName = c.Name,
                Enrollments = c.Enrollments.Count
            })
            .OrderByDescending(c => c.Enrollments)
            .ThenBy(c => c.CourseName)
            .Take(5)
            .ToListAsync();

        var vm = new AdminDashboardViewModel
        {
            TotalCourses = await _context.Courses.CountAsync(),
            TotalCategories = await _context.Categories.CountAsync(),
            TotalEnrollments = await _context.Enrollments.CountAsync(),
            TopCourses = topCourses
        };

        return View(vm);
    }
}
