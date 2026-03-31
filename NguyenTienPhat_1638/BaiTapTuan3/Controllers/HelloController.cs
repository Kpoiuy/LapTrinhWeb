using Microsoft.AspNetCore.Mvc;
using Models;

namespace Controllers
{
    public class HelloController : Controller
    {
        // GET: /Hello or /Hello/Index
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Hello/BaiTap2
        public IActionResult BaiTap2()
        {
            var sanPham = new SanPhamViewModel
            {
                TenSanPham = "tai nghe không dây",
                GiaBan = 100000,
                MoTa =" sản phẩm chính hãng 100% giá trị thật không lùa gà",
                AnhMoTa = "https://images.unsplash.com/photo-1505740420928-5e560c06d30e?w=400"
            };
            return View(sanPham);
        }
    }
}
