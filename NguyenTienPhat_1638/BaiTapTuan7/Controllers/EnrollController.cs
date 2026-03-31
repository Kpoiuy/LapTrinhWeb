using BaiTapTuan7.Data;
using BaiTapTuan7.Models;
using BaiTapTuan7.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BaiTapTuan7.Controllers;

[Authorize(Roles = RoleNames.Student)]
[Route("enroll")]
public class EnrollController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ILogger<EnrollController> _logger;

    public EnrollController(
        ApplicationDbContext context,
        UserManager<IdentityUser> userManager,
        ILogger<EnrollController> logger)
    {
        _context = context;
        _userManager = userManager;
        _logger = logger;
    }

    [HttpPost("{courseId:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Enroll(int courseId, string? returnUrl = null)
    {
        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Forbid();
        }

        var courseExists = await _context.Courses.AnyAsync(c => c.Id == courseId);
        if (!courseExists)
        {
            _logger.LogWarning("Enroll failed: course {CourseId} not found.", courseId);
            return NotFound();
        }

        var alreadyEnrolled = await _context.Enrollments
            .AnyAsync(e => e.UserId == userId && e.CourseId == courseId);

        if (!alreadyEnrolled)
        {
            _context.Enrollments.Add(new Enrollment
            {
                CourseId = courseId,
                UserId = userId,
                EnrollDate = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            _logger.LogInformation("User {UserId} enrolled course {CourseId}.", userId, courseId);
            TempData["Success"] = "Đăng ký học phần thành công.";
        }
        else
        {
            _logger.LogWarning("User {UserId} attempted duplicate enrollment for course {CourseId}.", userId, courseId);
            TempData["Error"] = "Bạn đã đăng ký học phần này rồi.";
        }

        return RedirectToLocal(returnUrl);
    }

    [HttpPost("cancel/{courseId:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int courseId, string? returnUrl = null)
    {
        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Forbid();
        }

        var enrollment = await _context.Enrollments
            .FirstOrDefaultAsync(e => e.UserId == userId && e.CourseId == courseId);

        if (enrollment is not null)
        {
            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();
            _logger.LogInformation("User {UserId} canceled enrollment for course {CourseId}.", userId, courseId);
            TempData["Success"] = "Hủy đăng ký học phần thành công.";
        }
        else
        {
            _logger.LogWarning("Cancel ignored: enrollment not found for user {UserId}, course {CourseId}.", userId, courseId);
        }

        return RedirectToLocal(returnUrl);
    }

    [HttpGet("my-courses")]
    public async Task<IActionResult> MyCourses()
    {
        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Forbid();
        }

        var enrollments = await _context.Enrollments
            .Where(e => e.UserId == userId)
            .Include(e => e.Course)
            .ThenInclude(c => c!.Category)
            .AsNoTracking()
            .OrderByDescending(e => e.EnrollDate)
            .ToListAsync();

        var vm = new MyCoursesViewModel
        {
            Enrollments = enrollments,
            TotalCourses = enrollments.Count,
            TotalCredits = enrollments.Sum(e => e.Course?.Credits ?? 0),
            LatestEnrollDate = enrollments.Select(e => (DateTime?)e.EnrollDate).FirstOrDefault()
        };

        _logger.LogInformation("Loaded My Courses for user {UserId}. TotalCourses={TotalCourses}, TotalCredits={TotalCredits}.",
            userId, vm.TotalCourses, vm.TotalCredits);

        return View("Dashboard", vm);
    }

    private IActionResult RedirectToLocal(string? returnUrl)
    {
        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction("Index", "Home");
    }
}
