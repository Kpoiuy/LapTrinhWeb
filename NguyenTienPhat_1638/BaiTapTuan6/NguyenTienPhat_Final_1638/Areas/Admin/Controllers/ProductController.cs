using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NguyenTienPhat_Final_1638.DataAccess;
using NguyenTienPhat_Final_1638.Models;
using NguyenTienPhat_Final_1638.Repositories;

namespace NguyenTienPhat_Final_1638.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(
            ApplicationDbContext context,
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllAsync();
            return View(products);
        }

        public async Task<IActionResult> Add()
        {
            await LoadCategoriesAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Product product, IFormFile? imageFile)
        {
            // Kiểm tra category có tồn tại không (chỉ khi CategoryId > 0)
            if (product.CategoryId > 0 && !await CategoryExistsAsync(product.CategoryId))
            {
                ModelState.AddModelError(nameof(product.CategoryId), "Danh mục không tồn tại.");
            }

            if (!ModelState.IsValid)
            {
                await LoadCategoriesAsync();
                return View(product);
            }

            if (imageFile != null && imageFile.Length > 0)
            {
                var imageValidationError = ValidateImageFile(imageFile);
                if (imageValidationError != null)
                {
                    ModelState.AddModelError("imageFile", imageValidationError);
                    await LoadCategoriesAsync();
                    return View(product);
                }

                product.ImageUrl = await SaveImageAsync(imageFile);
            }

            try
            {
                await _productRepository.AddAsync(product);
                TempData["Success"] = "Thêm sản phẩm thành công.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty, "Không thể lưu sản phẩm. Vui lòng kiểm tra lại dữ liệu rồi thử lại.");
                await LoadCategoriesAsync();
                return View(product);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            await LoadCategoriesAsync();
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product product, IFormFile? imageFile)
        {
            var existingProduct = await _productRepository.GetByIdAsync(product.Id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            // Kiểm tra category có tồn tại không (chỉ khi CategoryId > 0)
            if (product.CategoryId > 0 && !await CategoryExistsAsync(product.CategoryId))
            {
                ModelState.AddModelError(nameof(product.CategoryId), "Danh mục không tồn tại.");
            }

            if (!ModelState.IsValid)
            {
                product.ImageUrl = existingProduct.ImageUrl;
                await LoadCategoriesAsync();
                return View(product);
            }

            if (imageFile != null && imageFile.Length > 0)
            {
                var imageValidationError = ValidateImageFile(imageFile);
                if (imageValidationError != null)
                {
                    ModelState.AddModelError("imageFile", imageValidationError);
                    product.ImageUrl = existingProduct.ImageUrl;
                    await LoadCategoriesAsync();
                    return View(product);
                }

                if (IsLocalImage(existingProduct.ImageUrl))
                {
                    DeleteImage(existingProduct.ImageUrl!);
                }

                product.ImageUrl = await SaveImageAsync(imageFile);
            }
            else
            {
                product.ImageUrl = existingProduct.ImageUrl;
            }

            try
            {
                await _productRepository.UpdateAsync(product);
                TempData["Success"] = "Cập nhật sản phẩm thành công.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty, "Không thể cập nhật sản phẩm. Vui lòng thử lại.");
                product.ImageUrl = existingProduct.ImageUrl;
                await LoadCategoriesAsync();
                return View(product);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.OrderItemCount = await GetOrderItemCountAsync(id);
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var orderItemCount = await GetOrderItemCountAsync(id);
            if (orderItemCount > 0)
            {
                TempData["Error"] = $"Không thể xóa sản phẩm này vì đang xuất hiện trong {orderItemCount} dòng chi tiết đơn hàng.";
                return RedirectToAction(nameof(Index));
            }

            if (IsLocalImage(product.ImageUrl))
            {
                DeleteImage(product.ImageUrl!);
            }

            try
            {
                await _productRepository.DeleteAsync(id);
                TempData["Success"] = "Xóa sản phẩm thành công.";
            }
            catch (DbUpdateException)
            {
                TempData["Error"] = "Không thể xóa sản phẩm do đang được sử dụng ở dữ liệu khác.";
            }

            return RedirectToAction(nameof(Index));
        }

        private string? ValidateImageFile(IFormFile imageFile)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
            {
                return "Chỉ cho phép tệp ảnh (.jpg, .jpeg, .png, .gif).";
            }

            if (imageFile.Length > 5 * 1024 * 1024)
            {
                return "Dung lượng ảnh không được vượt quá 5MB.";
            }

            return null;
        }

        private async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return "/images/products/" + uniqueFileName;
        }

        private void DeleteImage(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                return;
            }

            var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, imageUrl.TrimStart('/'));

            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
        }

        private bool IsLocalImage(string? imageUrl)
        {
            return !string.IsNullOrWhiteSpace(imageUrl)
                && imageUrl.StartsWith("/", StringComparison.Ordinal);
        }

        private async Task<bool> CategoryExistsAsync(int categoryId)
        {
            return await _context.Categories.AnyAsync(c => c.Id == categoryId);
        }

        private async Task<int> GetOrderItemCountAsync(int productId)
        {
            return await _context.OrderItems.CountAsync(oi => oi.ProductId == productId);
        }

        private async Task LoadCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            });
        }
    }
}
