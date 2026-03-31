using BaiTapTuan7.Data;
using BaiTapTuan7.Models;
using BaiTapTuan7.Models.ViewModels;
using BaiTapTuan7.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BaiTapTuan7.Controllers;

[Route("courses")]
public class CoursesController : Controller
{
    private const int PageSize = 5;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public CoursesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet("")]
    [HttpGet("index")]
    public async Task<IActionResult> Index(string? keyword, int page = 1)
    {
        if (page < 1)
        {
            page = 1;
        }

        var query = _context.Courses
            .AsNoTracking()
            .Include(c => c.Category)
            .OrderBy(c => c.Name)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(c => c.Name.Contains(keyword));
        }

        var enrolledCourseIds = new HashSet<int>();
        if (User.IsInRole(RoleNames.Student))
        {
            var userId = _userManager.GetUserId(User);
            if (!string.IsNullOrWhiteSpace(userId))
            {
                var enrolledCourseIdList = await _context.Enrollments
                    .Where(e => e.UserId == userId)
                    .Select(e => e.CourseId)
                    .ToListAsync();
                enrolledCourseIds = enrolledCourseIdList.ToHashSet();
            }
        }

        var totalItems = await query.CountAsync();
        var totalPages = Math.Max(1, (int)Math.Ceiling(totalItems / (double)PageSize));
        page = Math.Min(page, totalPages);

        var courses = await query
            .Skip((page - 1) * PageSize)
            .Take(PageSize)
            .Select(c => new CourseCardViewModel
            {
                Id = c.Id,
                Name = c.Name,
                Credits = c.Credits,
                Lecturer = c.Lecturer,
                Image = ImageUrlResolver.Resolve(c.Image),
                CategoryName = c.Category != null ? c.Category.Name : "Chưa phân loại",
                IsEnrolled = enrolledCourseIds.Contains(c.Id)
            })
            .ToListAsync();

        var vm = new CourseListViewModel
        {
            Courses = courses,
            CurrentPage = page,
            TotalPages = totalPages,
            TotalItems = totalItems,
            Keyword = keyword
        };

        return View("Catalog", vm);
    }
}
