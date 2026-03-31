using Microsoft.AspNetCore.Mvc;
using NguyenTienPhat_Final_1638.Models;
using NguyenTienPhat_Final_1638.Repositories;

namespace NguyenTienPhat_Final_1638.Controllers
{
    // Public ProductController - Dành cho Customer xem và mua hàng
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        // Hiển thị danh sách sản phẩm cho khách hàng
        public async Task<IActionResult> Index(int? categoryId)
        {
            IEnumerable<Product> products;

            if (categoryId.HasValue)
            {
                products = (await _productRepository.GetAllAsync())
                    .Where(p => p.CategoryId == categoryId.Value);
                ViewBag.SelectedCategoryId = categoryId.Value;
            }
            else
            {
                products = await _productRepository.GetAllAsync();
            }

            // Load categories cho filter
            ViewBag.Categories = await _categoryRepository.GetAllAsync();
            
            return View(products);
        }

        // Hiển thị chi tiết sản phẩm
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
    }
}
