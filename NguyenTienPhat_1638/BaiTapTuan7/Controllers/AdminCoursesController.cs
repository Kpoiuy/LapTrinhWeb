using BaiTapTuan7.Data;
using BaiTapTuan7.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BaiTapTuan7.Controllers;

[Authorize(Roles = RoleNames.Admin)]
[Route("admin/courses")]
public class AdminCoursesController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<AdminCoursesController> _logger;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IHttpClientFactory _httpClientFactory;
    private static readonly string[] AllowedImageExtensions =
    [
        ".jpg", ".jpeg", ".png", ".webp", ".gif", ".bmp", ".svg"
    ];

    public AdminCoursesController(
        ApplicationDbContext context,
        ILogger<AdminCoursesController> logger,
        IWebHostEnvironment webHostEnvironment,
        IHttpClientFactory httpClientFactory)
    {
        _context = context;
        _logger = logger;
        _webHostEnvironment = webHostEnvironment;
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var courses = await _context.Courses
            .Include(c => c.Category)
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .ToListAsync();

        return View(courses);
    }

    [HttpGet("create")]
    public async Task<IActionResult> Create()
    {
        await LoadCategoriesAsync();
        return View(new Course());
    }

    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Course course, IFormFile? uploadedImage)
    {
        if (!ModelState.IsValid)
        {
            await LoadCategoriesAsync(course.CategoryId);
            return View(course);
        }

        try
        {
            course.Image = await ResolveAndPersistImageAsync(course.Image, uploadedImage);
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Admin created course {CourseId} - {CourseName}.", course.Id, course.Name);
            TempData["Success"] = "Thêm học phần thành công.";
        }
        catch (FileNotFoundException ex)
        {
            _logger.LogWarning(ex, "Image file not found while creating course.");
            ModelState.AddModelError(nameof(Course.Image), ex.Message);
            await LoadCategoriesAsync(course.CategoryId);
            return View(course);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Invalid image input while creating course.");
            ModelState.AddModelError(nameof(Course.Image), ex.Message);
            await LoadCategoriesAsync(course.CategoryId);
            return View(course);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database update failed while creating course.");
            ModelState.AddModelError(string.Empty, "Không thể lưu học phần. Vui lòng kiểm tra độ dài dữ liệu và thử lại.");
            await LoadCategoriesAsync(course.CategoryId);
            return View(course);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("edit/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        var course = await _context.Courses.FindAsync(id);
        if (course is null)
        {
            return NotFound();
        }

        await LoadCategoriesAsync(course.CategoryId);
        return View(course);
    }

    [HttpPost("edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Course course, IFormFile? uploadedImage)
    {
        if (id != course.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            await LoadCategoriesAsync(course.CategoryId);
            return View(course);
        }

        try
        {
            course.Image = await ResolveAndPersistImageAsync(course.Image, uploadedImage);
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Admin updated course {CourseId} - {CourseName}.", course.Id, course.Name);
            TempData["Success"] = "Cập nhật học phần thành công.";
        }
        catch (FileNotFoundException ex)
        {
            _logger.LogWarning(ex, "Image file not found while updating course {CourseId}.", course.Id);
            ModelState.AddModelError(nameof(Course.Image), ex.Message);
            await LoadCategoriesAsync(course.CategoryId);
            return View(course);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Invalid image input while updating course {CourseId}.", course.Id);
            ModelState.AddModelError(nameof(Course.Image), ex.Message);
            await LoadCategoriesAsync(course.CategoryId);
            return View(course);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database update failed while updating course {CourseId}.", course.Id);
            ModelState.AddModelError(string.Empty, "Không thể cập nhật học phần. Vui lòng kiểm tra dữ liệu và thử lại.");
            await LoadCategoriesAsync(course.CategoryId);
            return View(course);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var course = await _context.Courses
            .Include(c => c.Category)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

        if (course is null)
        {
            return NotFound();
        }

        return View(course);
    }

    [HttpPost("delete/{id:int}")]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var course = await _context.Courses.FindAsync(id);
        if (course is null)
        {
            return NotFound();
        }

        var enrollments = _context.Enrollments.Where(e => e.CourseId == id);
        _context.Enrollments.RemoveRange(enrollments);
        _context.Courses.Remove(course);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Admin deleted course {CourseId} - {CourseName}.", course.Id, course.Name);

        TempData["Success"] = "Xóa học phần thành công.";
        return RedirectToAction(nameof(Index));
    }

    private async Task LoadCategoriesAsync(int? selectedCategory = null)
    {
        var categories = await _context.Categories
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .ToListAsync();

        ViewBag.CategoryId = new SelectList(categories, nameof(Category.Id), nameof(Category.Name), selectedCategory);
    }

    private async Task<string?> ResolveAndPersistImageAsync(string? imageInput, IFormFile? uploadedImage)
    {
        if (uploadedImage is not null && uploadedImage.Length > 0)
        {
            return await SaveUploadedImageAsync(uploadedImage);
        }

        if (string.IsNullOrWhiteSpace(imageInput))
        {
            return null;
        }

        var value = imageInput.Trim();
        if (value.StartsWith("data:image/", StringComparison.OrdinalIgnoreCase))
        {
            return await SaveDataUriImageAsync(value);
        }

        if (Uri.TryCreate(value, UriKind.Absolute, out var uri)
            && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
        {
            return await DownloadExternalImageAsync(uri);
        }

        if (value.StartsWith("~/", StringComparison.Ordinal))
        {
            return value[1..];
        }

        if (value.StartsWith("/", StringComparison.Ordinal))
        {
            return value;
        }

        if (value.StartsWith("images/", StringComparison.OrdinalIgnoreCase))
        {
            return "/" + value.TrimStart('/');
        }

        if (Path.IsPathRooted(value))
        {
            if (!System.IO.File.Exists(value))
            {
                throw new FileNotFoundException("Không tìm thấy file ảnh trên máy chủ. Vui lòng kiểm tra lại đường dẫn.", value);
            }

            var extension = Path.GetExtension(value);
            if (!AllowedImageExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("Định dạng ảnh không hợp lệ. Chỉ chấp nhận: .jpg, .jpeg, .png, .webp, .gif, .bmp, .svg.");
            }

            var targetDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "images", "uploads");
            Directory.CreateDirectory(targetDirectory);

            var safeFileName = $"{Path.GetFileNameWithoutExtension(value)}-{Guid.NewGuid():N}{extension.ToLowerInvariant()}";
            var destinationPath = Path.Combine(targetDirectory, safeFileName);

            await using var source = System.IO.File.OpenRead(value);
            await using var destination = System.IO.File.Create(destinationPath);
            await source.CopyToAsync(destination);

            return $"/images/uploads/{safeFileName}";
        }

        var relativePath = value.Replace('\\', '/').TrimStart('/');
        if (AllowedImageExtensions.Contains(Path.GetExtension(relativePath), StringComparer.OrdinalIgnoreCase))
        {
            return "/" + relativePath;
        }

        throw new InvalidOperationException("Đường dẫn ảnh không hợp lệ. Vui lòng nhập URL ảnh trực tiếp hoặc đường dẫn file hợp lệ.");
    }

    private async Task<string> SaveUploadedImageAsync(IFormFile uploadedImage)
    {
        var extension = Path.GetExtension(uploadedImage.FileName);
        if (string.IsNullOrWhiteSpace(extension)
            || !AllowedImageExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("File anh upload khong hop le. Chi chap nhan: .jpg, .jpeg, .png, .webp, .gif, .bmp, .svg.");
        }

        if (uploadedImage.Length > 8_000_000)
        {
            throw new InvalidOperationException("File anh upload qua lon (toi da 8MB).");
        }

        var uploadsDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "images", "uploads");
        Directory.CreateDirectory(uploadsDirectory);

        var fileName = $"upload-{Guid.NewGuid():N}{extension.ToLowerInvariant()}";
        var destinationPath = Path.Combine(uploadsDirectory, fileName);

        await using var outputStream = System.IO.File.Create(destinationPath);
        await uploadedImage.CopyToAsync(outputStream);

        return $"/images/uploads/{fileName}";
    }

    private async Task<string> DownloadExternalImageAsync(Uri uri)
    {
        var httpClient = _httpClientFactory.CreateClient();
        httpClient.Timeout = TimeSpan.FromSeconds(25);

        using var request = new HttpRequestMessage(HttpMethod.Get, uri);
        request.Headers.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome Safari");
        request.Headers.TryAddWithoutValidation("Accept", "image/*,*/*;q=0.8");

        using var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException($"Không tải được ảnh từ URL. Mã phản hồi: {(int)response.StatusCode}.");
        }

        if (response.Content.Headers.ContentLength is > 8_000_000)
        {
            throw new InvalidOperationException("Ảnh từ URL quá lớn (tối đa 8MB).");
        }

        var mediaType = response.Content.Headers.ContentType?.MediaType;
        if (string.IsNullOrWhiteSpace(mediaType) || !mediaType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("URL không trả về tệp ảnh trực tiếp. Vui lòng dùng link ảnh trực tiếp (.jpg/.png/.webp/...).");
        }

        var extension = ResolveImageExtension(mediaType, uri.AbsolutePath);
        var uploadsDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "images", "uploads");
        Directory.CreateDirectory(uploadsDirectory);

        var fileName = $"remote-{Guid.NewGuid():N}{extension}";
        var destinationPath = Path.Combine(uploadsDirectory, fileName);

        await using var sourceStream = await response.Content.ReadAsStreamAsync();
        await using var destinationStream = System.IO.File.Create(destinationPath);
        await sourceStream.CopyToAsync(destinationStream);

        return $"/images/uploads/{fileName}";
    }

    private async Task<string> SaveDataUriImageAsync(string dataUri)
    {
        var separatorIndex = dataUri.IndexOf(',', StringComparison.Ordinal);
        if (separatorIndex <= 0)
        {
            throw new InvalidOperationException("Data URI ảnh không hợp lệ.");
        }

        var metadata = dataUri[..separatorIndex];
        if (!metadata.Contains(";base64", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Data URI phải ở định dạng base64.");
        }

        var slashIndex = metadata.IndexOf('/');
        var semicolonIndex = metadata.IndexOf(';');
        var imageType = (slashIndex >= 0 && semicolonIndex > slashIndex)
            ? metadata[(slashIndex + 1)..semicolonIndex]
            : "jpeg";

        byte[] bytes;
        try
        {
            bytes = Convert.FromBase64String(dataUri[(separatorIndex + 1)..]);
        }
        catch (FormatException)
        {
            throw new InvalidOperationException("Data URI ảnh không đúng định dạng base64.");
        }

        if (bytes.Length == 0)
        {
            throw new InvalidOperationException("Data URI ảnh không chứa dữ liệu.");
        }

        if (bytes.Length > 8_000_000)
        {
            throw new InvalidOperationException("Ảnh base64 quá lớn (tối đa 8MB).");
        }

        var extension = ResolveImageExtension($"image/{imageType}", string.Empty);
        var uploadsDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "images", "uploads");
        Directory.CreateDirectory(uploadsDirectory);

        var fileName = $"inline-{Guid.NewGuid():N}{extension}";
        var destinationPath = Path.Combine(uploadsDirectory, fileName);
        await System.IO.File.WriteAllBytesAsync(destinationPath, bytes);

        return $"/images/uploads/{fileName}";
    }

    private static string ResolveImageExtension(string mediaType, string urlPath)
    {
        var normalizedType = mediaType.Trim().ToLowerInvariant();
        var extension = normalizedType switch
        {
            "image/jpeg" => ".jpg",
            "image/jpg" => ".jpg",
            "image/png" => ".png",
            "image/webp" => ".webp",
            "image/gif" => ".gif",
            "image/bmp" => ".bmp",
            "image/svg+xml" => ".svg",
            _ => Path.GetExtension(urlPath)
        };

        if (string.IsNullOrWhiteSpace(extension))
        {
            extension = ".jpg";
        }

        if (!extension.StartsWith(".", StringComparison.Ordinal))
        {
            extension = "." + extension;
        }

        if (!AllowedImageExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
        {
            extension = ".jpg";
        }

        return extension.ToLowerInvariant();
    }
}
