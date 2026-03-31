using Microsoft.AspNetCore.Mvc;
using NguyenTienPhat_Final_1638.Models;
using NguyenTienPhat_Final_1638.Repositories;

namespace NguyenTienPhat_Final_1638.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        // Bỏ bớt IProductRepository ở đây vì Controller của Category thường chỉ cần thao tác với Category
        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        // Danh sách Danh mục
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return View(categories);
        }

        // Xem chi tiết (Optional)
        public async Task<IActionResult> Display(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // GET: Thêm mới
        public IActionResult Add()
        {
            return View();
        }

        // POST: Xử lý Thêm mới
        [HttpPost]
        public async Task<IActionResult> Add(Category category)
        {
            // Bỏ qua validate list Products vì ta không nhập list này từ form
            ModelState.Remove("Products");

            if (ModelState.IsValid)
            {
                // FIX LỖI SÁCH: Thêm await
                await _categoryRepository.AddAsync(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Sửa thông tin
        public async Task<IActionResult> Update(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Xử lý Sửa thông tin
        [HttpPost]
        public async Task<IActionResult> Update(int id, Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            ModelState.Remove("Products");

            if (ModelState.IsValid)
            {
                // FIX LỖI SÁCH: Thêm await
                await _categoryRepository.UpdateAsync(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Cảnh báo Xóa
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Xác nhận Xóa
        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // FIX LỖI SÁCH: Thêm await
            await _categoryRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}