using BaiTapTuan7.Data;
using BaiTapTuan7.Models;
using BaiTapTuan7.Models.ViewModels;
using BaiTapTuan7.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BaiTapTuan7.Controllers
{
    public class HomeController : Controller
    {
        private const int PageSize = 5;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            ILogger<HomeController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string? keyword, int page = 1)
        {
            _logger.LogInformation("Home index requested. Keyword={Keyword}, Page={Page}, Authenticated={IsAuthenticated}",
                keyword, page, User.Identity?.IsAuthenticated == true);
            var vm = await BuildCourseListAsync(keyword, page);
            return View("Landing", vm);
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

        private async Task<CourseListViewModel> BuildCourseListAsync(string? keyword, int page)
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

            return new CourseListViewModel
            {
                Courses = courses,
                CurrentPage = page,
                TotalPages = totalPages,
                TotalItems = totalItems,
                Keyword = keyword
            };
        }
    }
}
