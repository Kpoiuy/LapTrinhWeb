using Microsoft.AspNetCore.Mvc;
using Models;

namespace Controllers
{
    public class StudentController : Controller
    {
        // GET: /Student or /Student/Index - Hiển thị form đăng ký
        public IActionResult Index()
        {
            return View();
        }

        // POST: /Student/ShowKQ - Xử lý form submit và hiển thị kết quả
        [HttpPost]
        public IActionResult ShowKQ(StudentViewModel model)
        {
            // Tính số lượng sinh viên đã đăng ký cùng ngành (giả lập)
            var soLuong = new Random().Next(30, 100);
            ViewBag.SoLuong = soLuong;
            
            return View(model);
        }
    }
}
